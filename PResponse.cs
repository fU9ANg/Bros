// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class PResponse
{
	public static PResponse GeneralError(string message)
	{
		return new PResponse
		{
			errorcode = 1,
			overridemessage = message
		};
	}

	public static PResponse GeneralError(int nodataerror)
	{
		return new PResponse
		{
			errorcode = -1
		};
	}

	public static PResponse Error(int errorcode)
	{
		return new PResponse
		{
			errorcode = errorcode
		};
	}

	public string errormessage
	{
		get
		{
			if (!string.IsNullOrEmpty(this.overridemessage))
			{
				return this.overridemessage;
			}
			if (this.errorcode == 0)
			{
				return "Nothing went wrong!";
			}
			int num = this.errorcode;
			switch (num)
			{
			case 200:
				return "Leaderboard API has been disabled for this game";
			case 201:
				return "The player's name wasn't provided";
			default:
				switch (num)
				{
				case 500:
					return "Achievements API has been disabled for this game";
				case 501:
					return "Missing playerid";
				case 502:
					return "Missing player name";
				case 503:
					return "Missing achievementid";
				case 504:
					return "Invalid achievementid or achievement key";
				case 505:
					return "Player already had the achievement, you can overwrite old achievements with overwrite=true or save each time the player is awarded with allowduplicates=true";
				case 506:
					return "Player already had the achievement and it was overwritten or a duplicate was saved successfully";
				default:
					switch (num)
					{
					case 400:
						return "Level sharing API has been disabled for this game";
					case 401:
						return "Invalid rating (must be 1 - 10)";
					case 402:
						return "Player has already rated that level";
					case 403:
						return "Missing level name";
					case 404:
						return "Missing level id";
					case 405:
						return "Level already exists";
					default:
						switch (num)
						{
						case 1:
							return "General error, this typically means the player is unable to connect to the server";
						case 2:
							return "Invalid game credentials. Make sure you use the right public and private keys";
						case 3:
							return "Request timed out";
						case 4:
							return "Invalid request";
						default:
							switch (num)
							{
							case 600:
								return "Newsletter API has been disabled for this game";
							case 601:
								return "MailChimp API Key has not been configured";
							case 602:
								return "MailChimp API returned an error";
							default:
								if (num == 100)
								{
									return "GeoIP API has been disabled for this game";
								}
								if (num != 300)
								{
									return "Unknown error...";
								}
								return "GameVars API has been disabled for this game";
							}
							break;
						}
						break;
					}
					break;
				}
				break;
			case 203:
				return "Player is banned from submitting scores in this game";
			case 204:
				return "Score was not saved because it was not the player's best, you can allow players to have more than one score by specifying allowduplicates=true in your save options";
			case 207:
				return "The leaderboard table wasn't provided";
			}
		}
	}

	public bool success;

	public int errorcode;

	public string overridemessage;

	internal Dictionary<string, object> json;
}
