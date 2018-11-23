// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Security.Cryptography;
using System.Text;

internal class PEncode
{
	public static string MD5(string input)
	{
		MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] array = md5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(input));
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	public static string Base64(string data)
	{
		byte[] inArray = new byte[data.Length];
		inArray = Encoding.UTF8.GetBytes(data);
		return Convert.ToBase64String(inArray);
	}
}
