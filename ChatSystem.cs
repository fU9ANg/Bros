// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatSystem : SingletonMono<ChatSystem>
{
	public static char MapToGermanKeyboard(char Char)
	{
		switch (Char)
		{
		case '#':
			return '§';
		default:
			switch (Char)
			{
			case '<':
				return ';';
			case '=':
				return '+';
			case '>':
				return ':';
			case '?':
				return '_';
			case '@':
				return '"';
			default:
				switch (Char)
				{
				case '[':
					return 'ß';
				case '\\':
					return '<';
				case ']':
					return '´';
				case '^':
					return '&';
				default:
					switch (Char)
					{
					case '{':
						return '?';
					case '|':
						return '>';
					case '}':
						return '`';
					default:
						return Char;
					}
					break;
				}
				break;
			}
			break;
		case '&':
			return '/';
		case '(':
			return ')';
		case ')':
			return '=';
		case '*':
			return '(';
		case '+':
			return '*';
		case '/':
			return '#';
		}
	}

	public static bool IsFocused
	{
		get
		{
			return ChatSystem.display;
		}
	}

	private void OnLevelWasLoaded()
	{
		this.Hide();
	}

	private IEnumerator FlashCaret()
	{
		for (;;)
		{
			if (!ChatSystem.display)
			{
				this.displayCaret = false;
				yield return null;
			}
			else
			{
				this.displayCaret = !this.displayCaret;
				yield return new WaitForSeconds(0.7f);
			}
		}
		yield break;
	}

	private void Start()
	{
		if (Map.MapData != null && Map.MapData.theme != LevelTheme.Jungle)
		{
			this.defaultColour = this.CityTextCol;
		}
		base.StartCoroutine(this.FlashCaret());
	}

	private void Update()
	{
		if (ChatSystem.display)
		{
			ChatSystem.fadeTimer = ChatSystem.fadeInterval + ChatSystem.waitToFade;
		}
		else if (ChatSystem.fadeTimer > 0f)
		{
			ChatSystem.fadeTimer -= Time.deltaTime;
			ChatSystem.fadeTimer = Mathf.Max(ChatSystem.fadeTimer, 0f);
		}
		ChatSystem.typedMessage = ChatSystem.updatedTypedMessage;
		if (Connect.IsOffline || MainMenu.instance != null || !LevelSelectionController.IsCampaignScene)
		{
			this.Hide();
			return;
		}
		if (!PauseController.Paused)
		{
			if (ChatSystem.display)
			{
				foreach (KeyCode keyCode in (KeyCode[])Enum.GetValues(typeof(KeyCode)))
				{
					if (ChatSystem.typedMessage.Length < 100)
					{
						if (keyCode != KeyCode.None)
						{
							if (Input.GetKeyDown(keyCode))
							{
								string text = string.Empty;
								if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
								{
									if (ChatSystem.shftKeyMappings.ContainsKey(keyCode))
									{
										text += ChatSystem.shftKeyMappings[keyCode];
									}
								}
								else if (ChatSystem.keyMappings.ContainsKey(keyCode))
								{
									text += ChatSystem.keyMappings[keyCode];
								}
								if (text != string.Empty && ChatSystem.UseGermanLayout)
								{
									text = string.Empty + ChatSystem.MapToGermanKeyboard(text[0]);
								}
								ChatSystem.updatedTypedMessage += text;
							}
						}
					}
				}
			}
			if (Input.GetKeyDown(KeyCode.Backspace) && ChatSystem.updatedTypedMessage.Length > 0)
			{
				ChatSystem.updatedTypedMessage = ChatSystem.updatedTypedMessage.Substring(0, ChatSystem.updatedTypedMessage.Length - 1);
			}
			if (Input.GetKeyDown(KeyCode.Return))
			{
				ChatSystem.typedMessage = ChatSystem.typedMessage.Trim();
				if (ChatSystem.display)
				{
					if (ChatSystem.typedMessage != string.Empty)
					{
						Networking.RPC<string, PID, float>(PID.TargetAll, new RpcSignature<string, PID, float>(ChatSystem.AddMessage), ChatSystem.typedMessage, PID.MyID, Connect.Timef, false);
						ChatSystem.typedMessage = string.Empty;
						ChatSystem.updatedTypedMessage = string.Empty;
					}
					this.Hide();
				}
				else
				{
					this.Show();
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape) && ChatSystem.display)
		{
			this.Hide();
		}
	}

	public static void AddMessage(string text, PID sender, float timeStamp)
	{
		ChatMessage chatMessage = new ChatMessage();
		chatMessage.text = text;
		chatMessage.text = chatMessage.text.Replace("\n", string.Empty);
		chatMessage.senderID = sender;
		chatMessage.timeStamp = timeStamp;
		ChatSystem.messages.Add(chatMessage);
		ChatSystem.messages.Sort(new Comparison<ChatMessage>(ChatSystem.CompareMessages));
		ChatSystem.fadeTimer = ChatSystem.fadeInterval + ChatSystem.waitToFade;
		UnityEngine.Debug.Log("Chat Message: " + text);
	}

	private static int CompareMessages(ChatMessage A, ChatMessage B)
	{
		if (A.timeStamp > B.timeStamp)
		{
			return 1;
		}
		if (A.timeStamp < B.timeStamp)
		{
			return -1;
		}
		return 0;
	}

	private void Hide()
	{
		ChatSystem.display = false;
	}

	private void Show()
	{
		ChatElipsis.CreateOnAllLocallyOwnedHeros();
		ChatSystem.display = true;
		if (Map.MapData != null && Map.MapData.theme == LevelTheme.City)
		{
			this.defaultColour = this.CityTextCol;
		}
	}

	private void DrawChat(bool DropShadow)
	{
		if (DropShadow)
		{
			GUILayout.BeginArea(new Rect(1f, (float)(1 + Screen.height / 6), 300f, (float)(Screen.height * 4 / 6)));
		}
		else
		{
			GUILayout.BeginArea(new Rect(0f, (float)(0 + Screen.height / 6), 300f, (float)(Screen.height * 4 / 6)));
		}
		float alpha = Mathf.Clamp01(ChatSystem.fadeTimer / ChatSystem.fadeInterval);
		GUI.skin = this.skin;
		GUI.SetNextControlName("ChatWindow");
		this.SetColor(this.defaultColour, alpha, DropShadow);
		if (ChatSystem.display)
		{
			string text = ChatSystem.typedMessage;
			if (this.displayCaret)
			{
				text += "|";
			}
			GUILayout.Label(text, new GUILayoutOption[]
			{
				GUILayout.Height(60f)
			});
		}
		else
		{
			GUILayout.Label("Press Enter to Chat...", new GUILayoutOption[]
			{
				GUILayout.Height(60f)
			});
		}
		GUILayout.Label("_________________________", new GUILayoutOption[0]);
		for (int i = ChatSystem.messages.Count - 1; i >= 0; i--)
		{
			this.SetColor(this.defaultColour, alpha, DropShadow);
			if (ChatSystem.messages[i].senderID != PID.NoID)
			{
				int playerNum = HeroController.GetPlayerNum(ChatSystem.messages[i].senderID);
				if (playerNum >= 0 && playerNum < 5)
				{
					Color heroColor = HeroController.GetHeroColor(playerNum);
					this.SetColor(heroColor, alpha, DropShadow);
				}
			}
			GUI.skin.GetStyle("label").contentOffset = new Vector2(0f, 8f);
			GUILayout.Label(ChatSystem.messages[i].senderID.PlayerName + string.Empty, new GUILayoutOption[0]);
			GUI.skin.GetStyle("label").contentOffset = new Vector2(0f, 0f);
			this.SetColor(this.defaultColour, alpha, DropShadow);
			GUILayout.Label(ChatSystem.messages[i].text, new GUILayoutOption[0]);
		}
		GUILayout.EndArea();
	}

	private void OnGUI()
	{
		this.DrawChat(true);
		this.DrawChat(false);
	}

	private void SetColor(Color col, float alpha, bool isDropShadow)
	{
		if (isDropShadow)
		{
			col = this.shadowColor;
		}
		col.a = alpha;
		GUI.color = col;
	}

	private const string ControlID = "ChatWindow";

	public static Dictionary<KeyCode, char> keyMappings = new Dictionary<KeyCode, char>
	{
		{
			KeyCode.A,
			'a'
		},
		{
			KeyCode.B,
			'b'
		},
		{
			KeyCode.C,
			'c'
		},
		{
			KeyCode.D,
			'd'
		},
		{
			KeyCode.E,
			'e'
		},
		{
			KeyCode.F,
			'f'
		},
		{
			KeyCode.G,
			'g'
		},
		{
			KeyCode.H,
			'h'
		},
		{
			KeyCode.I,
			'i'
		},
		{
			KeyCode.J,
			'j'
		},
		{
			KeyCode.K,
			'k'
		},
		{
			KeyCode.L,
			'l'
		},
		{
			KeyCode.M,
			'm'
		},
		{
			KeyCode.N,
			'n'
		},
		{
			KeyCode.O,
			'o'
		},
		{
			KeyCode.P,
			'p'
		},
		{
			KeyCode.Q,
			'q'
		},
		{
			KeyCode.R,
			'r'
		},
		{
			KeyCode.S,
			's'
		},
		{
			KeyCode.T,
			't'
		},
		{
			KeyCode.V,
			'v'
		},
		{
			KeyCode.U,
			'u'
		},
		{
			KeyCode.W,
			'w'
		},
		{
			KeyCode.X,
			'x'
		},
		{
			KeyCode.Y,
			'y'
		},
		{
			KeyCode.Z,
			'z'
		},
		{
			KeyCode.Alpha0,
			'0'
		},
		{
			KeyCode.Alpha1,
			'1'
		},
		{
			KeyCode.Alpha2,
			'2'
		},
		{
			KeyCode.Alpha3,
			'3'
		},
		{
			KeyCode.Alpha4,
			'4'
		},
		{
			KeyCode.Alpha5,
			'5'
		},
		{
			KeyCode.Alpha6,
			'6'
		},
		{
			KeyCode.Alpha7,
			'7'
		},
		{
			KeyCode.Alpha8,
			'8'
		},
		{
			KeyCode.Alpha9,
			'9'
		},
		{
			KeyCode.Comma,
			','
		},
		{
			KeyCode.Period,
			'.'
		},
		{
			KeyCode.Exclaim,
			'!'
		},
		{
			KeyCode.Equals,
			'='
		},
		{
			KeyCode.Minus,
			'-'
		},
		{
			KeyCode.Backslash,
			'\\'
		},
		{
			KeyCode.Slash,
			'/'
		},
		{
			KeyCode.Quote,
			'\''
		},
		{
			KeyCode.RightBracket,
			']'
		},
		{
			KeyCode.LeftBracket,
			'['
		},
		{
			KeyCode.Space,
			' '
		}
	};

	public static Dictionary<KeyCode, char> shftKeyMappings = new Dictionary<KeyCode, char>
	{
		{
			KeyCode.A,
			'A'
		},
		{
			KeyCode.B,
			'B'
		},
		{
			KeyCode.C,
			'C'
		},
		{
			KeyCode.D,
			'D'
		},
		{
			KeyCode.E,
			'E'
		},
		{
			KeyCode.F,
			'F'
		},
		{
			KeyCode.G,
			'G'
		},
		{
			KeyCode.H,
			'H'
		},
		{
			KeyCode.I,
			'I'
		},
		{
			KeyCode.J,
			'J'
		},
		{
			KeyCode.K,
			'K'
		},
		{
			KeyCode.L,
			'L'
		},
		{
			KeyCode.M,
			'M'
		},
		{
			KeyCode.N,
			'N'
		},
		{
			KeyCode.O,
			'O'
		},
		{
			KeyCode.P,
			'P'
		},
		{
			KeyCode.Q,
			'Q'
		},
		{
			KeyCode.R,
			'R'
		},
		{
			KeyCode.S,
			'S'
		},
		{
			KeyCode.T,
			'T'
		},
		{
			KeyCode.V,
			'V'
		},
		{
			KeyCode.U,
			'U'
		},
		{
			KeyCode.W,
			'W'
		},
		{
			KeyCode.X,
			'X'
		},
		{
			KeyCode.Y,
			'Y'
		},
		{
			KeyCode.Z,
			'Z'
		},
		{
			KeyCode.Alpha0,
			')'
		},
		{
			KeyCode.Alpha1,
			'!'
		},
		{
			KeyCode.Alpha2,
			'@'
		},
		{
			KeyCode.Alpha3,
			'#'
		},
		{
			KeyCode.Alpha4,
			'$'
		},
		{
			KeyCode.Alpha5,
			'%'
		},
		{
			KeyCode.Alpha6,
			'^'
		},
		{
			KeyCode.Alpha7,
			'&'
		},
		{
			KeyCode.Alpha8,
			'*'
		},
		{
			KeyCode.Alpha9,
			'('
		},
		{
			KeyCode.Comma,
			'<'
		},
		{
			KeyCode.Period,
			'>'
		},
		{
			KeyCode.Equals,
			'+'
		},
		{
			KeyCode.Minus,
			'_'
		},
		{
			KeyCode.Backslash,
			'|'
		},
		{
			KeyCode.Slash,
			'?'
		},
		{
			KeyCode.Quote,
			'"'
		},
		{
			KeyCode.RightBracket,
			'}'
		},
		{
			KeyCode.LeftBracket,
			'{'
		},
		{
			KeyCode.Space,
			' '
		}
	};

	private static List<ChatMessage> messages = new List<ChatMessage>();

	private static string typedMessage = string.Empty;

	private static string updatedTypedMessage = string.Empty;

	private static bool display = false;

	private static float fadeTimer = 0f;

	private static float fadeInterval = 3f;

	private static float waitToFade = 4f;

	public GUISkin skin;

	public static bool UseGermanLayout = false;

	private bool displayCaret;

	private Color defaultColour = Color.white;

	private Color shadowColor = Color.black;

	private bool enter;

	public Color CityTextCol;

	private string prevControl = string.Empty;
}
