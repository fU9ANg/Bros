// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public abstract class GameInfo
{
	public virtual int Capacity
	{
		get
		{
			return 4;
		}
	}

	public virtual int EmptySlots
	{
		get
		{
			return 4;
		}
	}

	public static string EncodeGameInfo()
	{
		return string.Concat(new object[]
		{
			Connect.PlayerName,
			"--%```1D232dd--",
			Connect.GameName,
			"--%```1D232dd--",
			VersionNumber.version,
			"--%```1D232dd--",
			Connect.Password,
			"--%```1D232dd--",
			Connect.Country,
			"--%```1D232dd--",
			Connect.City
		});
	}

	protected void DecodeGameInfo(string encodedDetails)
	{
		try
		{
			string[] array = encodedDetails.Split(new string[]
			{
				"--%```1D232dd--"
			}, StringSplitOptions.None);
			if (array.Length > 5)
			{
				this.HostName = array[0];
				this.GameName = array[1];
				this.Version = int.Parse(array[2]);
				this.Password = array[3];
				this.Country = array[4];
				this.City = array[5];
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
			this.invalidInfo = true;
		}
	}

	private const string DELIMITER = "--%```1D232dd--";

	public string HostName = string.Empty;

	public string GameName = string.Empty;

	public int Version;

	public string Password = string.Empty;

	public string Country = string.Empty;

	public string City = string.Empty;

	public bool invalidInfo;
}
