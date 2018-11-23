// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public static class FileIO
{
	private static string FileExtension
	{
		get
		{
			if (Application.isEditor)
			{
				return ".bytes";
			}
			return ".bfc";
		}
	}

	public static string path
	{
		get
		{
			if (Application.isWebPlayer || Application.isEditor)
			{
				return "Assets/Resources/LevelsXML/";
			}
			if (!Directory.Exists("Levels/"))
			{
				Directory.CreateDirectory("Levels");
			}
			return "Levels/";
		}
	}

	public static void SaveToXML(MapData mapData, string fileName)
	{
		string text = FileIO.path + fileName;
		if (!fileName.EndsWith(".xml") && !fileName.EndsWith(".XML"))
		{
			text += ".xml";
		}
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(MapData));
		StreamWriter streamWriter = new StreamWriter(text);
		xmlSerializer.Serialize(streamWriter, mapData);
		streamWriter.Close();
	}

	public static void SaveOptions(PlayerOptions options)
	{
		string path = "Saves/Options.xml";
		FileIO.CheckSavesFolder();
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(PlayerOptions));
		StreamWriter streamWriter = new StreamWriter(path);
		xmlSerializer.Serialize(streamWriter, options);
		streamWriter.Close();
	}

	public static void SaveToBFLCompressed(MapData mapData, string fileName)
	{
		string text = FileIO.path + fileName;
		if (!fileName.EndsWith(".bytes") && !fileName.EndsWith(".bytes"))
		{
			text += ".bytes";
		}
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(MapData));
		MemoryStream memoryStream = new MemoryStream();
		StreamWriter textWriter = new StreamWriter(memoryStream);
		xmlSerializer.Serialize(textWriter, mapData);
		byte[] array = CLZF2.Compress(memoryStream.ToArray());
		FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.Write);
		fileStream.Write(array, 0, array.Length);
		fileStream.Close();
	}

	public static MapData LoadFromXML(string fileName)
	{
		TextAsset textAsset = (TextAsset)Resources.Load("LevelsXML/" + fileName, typeof(TextAsset));
		XmlTextReader xmlReader = new XmlTextReader(new StringReader(textAsset.text));
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(MapData));
		return (MapData)xmlSerializer.Deserialize(xmlReader);
	}

	public static MapData LoadLevelFromResources(string fileName)
	{
		TextAsset textAsset = (TextAsset)Resources.Load("LevelsXML/" + fileName, typeof(TextAsset));
		byte[] buffer = CLZF2.Decompress(textAsset.bytes);
		XmlTextReader xmlReader = new XmlTextReader(new MemoryStream(buffer));
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(MapData));
		MapData mapData = (MapData)xmlSerializer.Deserialize(xmlReader);
		mapData.levelDescription = fileName;
		return mapData;
	}

	public static Campaign LoadCampaignFromResources(string fileName)
	{
		TextAsset textAsset = (TextAsset)Resources.Load("LevelsXML/" + fileName, typeof(TextAsset));
		int num = 0;
		while (textAsset.bytes[num] != 31 && num < textAsset.bytes.Length)
		{
			num++;
		}
		byte[] array = new byte[textAsset.bytes.Length - num];
		Buffer.BlockCopy(textAsset.bytes, num, array, 0, textAsset.bytes.Length - num);
		byte[] buffer = CLZF2.Decompress(array);
		XmlTextReader xmlReader = new XmlTextReader(new MemoryStream(buffer));
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(Campaign));
		return (Campaign)xmlSerializer.Deserialize(xmlReader);
	}

	public static MapData LoadLevelFromDisk(string fileName)
	{
		FileStream input = new FileStream(FileIO.path + fileName + FileIO.FileExtension, FileMode.Open, FileAccess.Read);
		BinaryReader binaryReader = new BinaryReader(input);
		long length = new FileInfo(FileIO.path + fileName + FileIO.FileExtension).Length;
		byte[] inputBytes = binaryReader.ReadBytes((int)length);
		UnityEngine.Debug.Log("Loading file: " + fileName);
		byte[] buffer = CLZF2.Decompress(inputBytes);
		XmlTextReader xmlReader = new XmlTextReader(new MemoryStream(buffer));
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(MapData));
		return (MapData)xmlSerializer.Deserialize(xmlReader);
	}

	public static Campaign LoadCampaignFromDisk(string fileName)
	{
		UnityEngine.Debug.Log("LOAD CAMPAIGN FROM DISK! " + fileName);
		FileStream input = new FileStream(FileIO.path + fileName + FileIO.FileExtension, FileMode.Open, FileAccess.Read);
		BinaryReader binaryReader = new BinaryReader(input);
		long length = new FileInfo(FileIO.path + fileName + FileIO.FileExtension).Length;
		int num = 0;
		while (binaryReader.PeekChar() != 31)
		{
			num++;
			binaryReader.ReadByte();
		}
		byte[] inputBytes = binaryReader.ReadBytes((int)length);
		UnityEngine.Debug.Log("Loading file: " + fileName);
		byte[] buffer = CLZF2.Decompress(inputBytes);
		XmlTextReader xmlReader = new XmlTextReader(new MemoryStream(buffer));
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(Campaign));
		Campaign campaign = (Campaign)xmlSerializer.Deserialize(xmlReader);
		campaign.header = FileIO.ReadCampaignHeader(fileName, FileIO.FileExtension);
		return campaign;
	}

	public static Campaign LoadPublishedCampaignFromDisk(string fileName)
	{
		FileStream input = new FileStream(FileIO.path + fileName + ".bfg", FileMode.Open, FileAccess.Read);
		BinaryReader binaryReader = new BinaryReader(input);
		long length = new FileInfo(FileIO.path + fileName + ".bfg").Length;
		int num = 0;
		while (binaryReader.PeekChar() != 31)
		{
			num++;
			binaryReader.ReadByte();
		}
		binaryReader.ReadByte();
		num++;
		byte[] blob = binaryReader.ReadBytes((int)length - num);
		CampaignHeader campaignHeader = FileIO.ReadCampaignHeader(fileName, ".bfg");
		byte[] array = FileIO.DecryptBlob(blob, campaignHeader.md5);
		byte[] buffer = CLZF2.Decompress(array);
		XmlTextReader xmlReader = new XmlTextReader(new MemoryStream(buffer));
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(Campaign));
		Campaign campaign = (Campaign)xmlSerializer.Deserialize(xmlReader);
		campaign.header = campaignHeader;
		string text = FileIO.ComputeMD5(campaign.header, array);
		if (text.Equals(campaign.header.md5))
		{
			campaign.header.isPublished = true;
		}
		else
		{
			UnityEngine.Debug.LogError("Stored and computed md5's do not match - some fool probably tried to hack this campaign\nStored:" + campaign.header.md5 + "\nCalced:" + text);
		}
		return campaign;
	}

	public static void SaveCampaign(Campaign camp, string fileName)
	{
		string text = FileIO.path + fileName;
		if (Application.isEditor)
		{
			if (!fileName.ToUpper().EndsWith(".BYTES"))
			{
				text += ".bytes";
			}
		}
		else if (!fileName.ToUpper().EndsWith(".BFC"))
		{
			text += ".bfc";
		}
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(Campaign));
		MemoryStream memoryStream = new MemoryStream();
		StreamWriter textWriter = new StreamWriter(memoryStream);
		xmlSerializer.Serialize(textWriter, camp);
		byte[] array = CLZF2.Compress(memoryStream.ToArray());
		if (camp.header == null)
		{
			camp.header = new CampaignHeader();
		}
		camp.header.length = camp.Length;
		XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(CampaignHeader));
		MemoryStream memoryStream2 = new MemoryStream();
		StreamWriter textWriter2 = new StreamWriter(memoryStream2);
		xmlSerializer2.Serialize(textWriter2, camp.header);
		FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.Write);
		fileStream.Write(memoryStream2.ToArray(), 0, memoryStream2.ToArray().Length);
		fileStream.Write(array, 0, array.Length);
		fileStream.Close();
	}

	public static void PublishCampaign(Campaign camp, string fileName)
	{
		string text = FileIO.path + fileName;
		if (!fileName.ToUpper().EndsWith(".BFG"))
		{
			text += ".bfg";
		}
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(Campaign));
		MemoryStream memoryStream = new MemoryStream();
		StreamWriter textWriter = new StreamWriter(memoryStream);
		xmlSerializer.Serialize(textWriter, camp);
		byte[] array = CLZF2.Compress(memoryStream.ToArray());
		if (camp.header == null)
		{
			camp.header = new CampaignHeader();
		}
		camp.header.length = camp.Length;
		camp.header.md5 = FileIO.ComputeMD5(camp.header, array);
		XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(CampaignHeader));
		MemoryStream memoryStream2 = new MemoryStream();
		StreamWriter textWriter2 = new StreamWriter(memoryStream2);
		xmlSerializer2.Serialize(textWriter2, camp.header);
		FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.Write);
		fileStream.Write(memoryStream2.ToArray(), 0, memoryStream2.ToArray().Length);
		fileStream.WriteByte(31);
		byte[] array2 = FileIO.EncryptBlob(array, camp.header.md5);
		UnityEngine.Debug.Log("Encrypt Length: " + array2.Length);
		fileStream.Write(array2, 0, array2.Length);
		fileStream.Close();
	}

	public static void CompressAllXmls()
	{
		foreach (string fileName in FileIO.FindLevelFiles())
		{
			FileIO.SaveToBFLCompressed(FileIO.LoadFromXML(fileName), fileName);
		}
	}

	public static string[] FindLevelFiles()
	{
		string str = (!Application.isEditor) ? "bfl" : "bytes";
		string[] files = Directory.GetFiles(FileIO.path, "*." + str);
		for (int i = 0; i < files.Length; i++)
		{
			string text = files[i];
			text = text.Substring(text.LastIndexOf('/') + 1);
			text = text.Remove(text.LastIndexOf('.'));
			files[i] = text;
		}
		return files;
	}

	public static string[] FindCampaignFiles()
	{
		string[] files = Directory.GetFiles(FileIO.path, "*" + FileIO.FileExtension);
		for (int i = 0; i < files.Length; i++)
		{
			string text = files[i];
			text = text.Substring(text.LastIndexOf('/') + 1);
			text = text.Remove(text.LastIndexOf('.'));
			files[i] = text;
		}
		return files;
	}

	public static string[] FindCampaignFilesTest1()
	{
		string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Levels", "*" + FileIO.FileExtension);
		for (int i = 0; i < files.Length; i++)
		{
			string text = files[i];
			text = text.Substring(text.LastIndexOf('/') + 1);
			text = text.Remove(text.LastIndexOf('.'));
			files[i] = text;
		}
		return files;
	}

	public static string[] FindCampaignFilesTest2()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(FileIO.path);
		FileInfo[] files = directoryInfo.GetFiles();
		string[] array = new string[files.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string text = files[i].Name;
			text = text.Substring(text.LastIndexOf('/') + 1);
			text = text.Remove(text.LastIndexOf('.'));
			array[i] = text;
		}
		return array;
	}

	public static string[] FindCampaignFilesTest3()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory() + "/Levels");
		FileInfo[] files = directoryInfo.GetFiles();
		string[] array = new string[files.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string text = files[i].Name;
			text = text.Substring(text.LastIndexOf('/') + 1);
			text = text.Remove(text.LastIndexOf('.'));
			array[i] = text;
		}
		return array;
	}

	public static string[] FindPublishedCampaignFiles()
	{
		string[] files = Directory.GetFiles(FileIO.path, "*.bfg");
		for (int i = 0; i < files.Length; i++)
		{
			string text = files[i];
			text = text.Substring(text.LastIndexOf('/') + 1);
			text = text.Remove(text.LastIndexOf('.'));
			files[i] = text;
		}
		return files;
	}

	private static CampaignHeader ReadCampaignHeader(byte[] fullCampaignBlob)
	{
		int num = 0;
		byte[] array = null;
		CampaignHeader result = null;
		byte b;
		do
		{
			b = fullCampaignBlob[num];
			num++;
		}
		while (num < fullCampaignBlob.Length && b != 31);
		if (b == 31 && num > 1)
		{
			array = new byte[num - 1];
			for (int i = 0; i < num - 1; i++)
			{
				array[i] = fullCampaignBlob[i];
			}
		}
		try
		{
			XmlTextReader xmlReader = new XmlTextReader(new StringReader(new string(Encoding.UTF8.GetString(array).ToCharArray())));
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(CampaignHeader));
			result = (CampaignHeader)xmlSerializer.Deserialize(xmlReader);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Could not reader header from file remote file. \nError: " + ex.Message);
			result = new CampaignHeader();
		}
		return result;
	}

	public static CampaignHeader ReadCampaignHeader(string fileName, string extension)
	{
		CampaignHeader result = null;
		if (string.IsNullOrEmpty(extension))
		{
			extension = FileIO.FileExtension;
		}
		FileStream input = new FileStream(FileIO.path + fileName + extension, FileMode.Open, FileAccess.Read);
		BinaryReader binaryReader = new BinaryReader(input);
		long length = new FileInfo(FileIO.path + fileName + extension).Length;
		int num = 0;
		byte[] array = new byte[length];
		byte[] array2 = null;
		byte b;
		do
		{
			b = binaryReader.ReadByte();
			if (b != 31)
			{
				array[num] = b;
			}
			num++;
		}
		while ((long)num < length && b != 31);
		if (b == 31 && num > 1)
		{
			array2 = new byte[num - 1];
			for (int i = 0; i < num - 1; i++)
			{
				array2[i] = array[i];
			}
		}
		try
		{
			XmlTextReader xmlReader = new XmlTextReader(new StringReader(new string(Encoding.UTF8.GetString(array2).ToCharArray())));
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(CampaignHeader));
			result = (CampaignHeader)xmlSerializer.Deserialize(xmlReader);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Could not reader header from file " + fileName + "\nError: " + ex.Message);
			result = new CampaignHeader();
		}
		return result;
	}

	private static string ComputeMD5(CampaignHeader header, byte[] campaignBlob)
	{
		string s = header.author + header.name + header.length.ToString() + header.description;
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		byte[] array = new byte[campaignBlob.Length + bytes.Length];
		Buffer.BlockCopy(campaignBlob, 0, array, 0, campaignBlob.Length);
		Buffer.BlockCopy(bytes, 0, array, bytes.Length, bytes.Length);
		string result;
		using (MD5 md = MD5.Create())
		{
			byte[] array2 = md.ComputeHash(array);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array2.Length; i++)
			{
				stringBuilder.Append(array2[i].ToString("x2"));
			}
			result = stringBuilder.ToString();
		}
		return result;
	}

	private static byte[] EncryptBlob(byte[] blob, string md5)
	{
		ICryptoTransform cryptoTransform = new DESCryptoServiceProvider
		{
			Key = Encoding.ASCII.GetBytes(md5.Substring(0, 8)),
			IV = new byte[]
			{
				5,
				2,
				177,
				3,
				1,
				28,
				13,
				69
			}
		}.CreateEncryptor();
		return cryptoTransform.TransformFinalBlock(blob, 0, blob.Length);
	}

	private static byte[] DecryptBlob(byte[] blob, string md5)
	{
		ICryptoTransform cryptoTransform = new DESCryptoServiceProvider
		{
			Key = Encoding.ASCII.GetBytes(md5.Substring(0, 8)),
			IV = new byte[]
			{
				5,
				2,
				177,
				3,
				1,
				28,
				13,
				69
			}
		}.CreateDecryptor();
		return cryptoTransform.TransformFinalBlock(blob, 0, blob.Length);
	}

	internal static string GetPublishedLevelAsString(string fileName)
	{
		FileStream fileStream = new FileStream(FileIO.path + fileName + ".bfg", FileMode.Open, FileAccess.Read);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		long length = new FileInfo(FileIO.path + fileName + ".bfg").Length;
		byte[] inArray = binaryReader.ReadBytes((int)length);
		string result = Convert.ToBase64String(inArray);
		binaryReader.Close();
		fileStream.Close();
		return result;
	}

	internal static Campaign DecodeCampaign(string data)
	{
		byte[] array = Convert.FromBase64String(data);
		CampaignHeader campaignHeader = FileIO.ReadCampaignHeader(array);
		int i = 0;
		do
		{
			i++;
		}
		while (array[i] != 31);
		i++;
		byte[] array2 = new byte[array.Length - i];
		UnityEngine.Debug.Log(array2.Length);
		int num = 0;
		while (i < array.Length)
		{
			array2[num] = array[i];
			num++;
			i++;
		}
		byte[] array3 = FileIO.DecryptBlob(array2, campaignHeader.md5);
		byte[] buffer = CLZF2.Decompress(array3);
		XmlTextReader xmlReader = new XmlTextReader(new MemoryStream(buffer));
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(Campaign));
		Campaign campaign = (Campaign)xmlSerializer.Deserialize(xmlReader);
		campaign.header = campaignHeader;
		string text = FileIO.ComputeMD5(campaign.header, array3);
		if (text.Equals(campaign.header.md5))
		{
			campaign.header.isPublished = true;
		}
		else
		{
			UnityEngine.Debug.LogError("Stored and computed md5's do not match - some fool probably tried to hack this campaign\nStored:" + campaign.header.md5 + "\nCalced:" + text);
		}
		return campaign;
	}

	internal static PlayerOptions LoadOptions()
	{
		FileIO.CheckSavesFolder();
		FileStream input = File.OpenRead("Saves/Options.xml");
		XmlTextReader xmlReader = new XmlTextReader(input);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(PlayerOptions));
		return (PlayerOptions)xmlSerializer.Deserialize(xmlReader);
	}

	internal static PlayerProgress LoadProgress()
	{
		FileIO.CheckSavesFolder();
		string text;
		if (SteamController.IsSteamEnabled())
		{
			text = "Saves/" + SteamController.SteamInterface.User.GetSteamID().ToString() + ".sav";
		}
		else
		{
			text = "Saves/progress.sav";
		}
		FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		long length = new FileInfo(text).Length;
		byte[] inArray = binaryReader.ReadBytes((int)length);
		string s = Convert.ToBase64String(inArray);
		binaryReader.Close();
		fileStream.Close();
		byte[] blob = Convert.FromBase64String(s);
		string md = "brololforce";
		if (SteamController.IsSteamEnabled())
		{
			md = SteamController.SteamInterface.User.GetSteamID().ToString();
		}
		byte[] buffer = FileIO.DecryptBlob(blob, md);
		XmlTextReader xmlReader = new XmlTextReader(new MemoryStream(buffer));
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(PlayerProgress));
		return (PlayerProgress)xmlSerializer.Deserialize(xmlReader);
	}

	public static void SaveProgress()
	{
		string md = "brololforce";
		FileIO.CheckSavesFolder();
		if (SteamController.IsSteamEnabled())
		{
			md = SteamController.SteamInterface.User.GetSteamID().ToString();
		}
		string text;
		if (SteamController.IsSteamEnabled())
		{
			text = "Saves/" + SteamController.SteamInterface.User.GetSteamID().ToString();
		}
		else
		{
			text = "Saves/progress";
		}
		text += ".sav";
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(PlayerProgress));
		MemoryStream memoryStream = new MemoryStream();
		StreamWriter textWriter = new StreamWriter(memoryStream);
		xmlSerializer.Serialize(textWriter, PlayerProgress.Instance);
		memoryStream.Close();
		byte[] array = FileIO.EncryptBlob(memoryStream.GetBuffer(), md);
		FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.Write);
		fileStream.Write(array, 0, array.Length);
		fileStream.Close();
	}

	private static void CheckSavesFolder()
	{
		if (!Directory.Exists("Saves/"))
		{
			Directory.CreateDirectory("Saves");
		}
	}
}
