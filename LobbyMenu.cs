// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenu : SingletonMono<LobbyMenu>
{
	public void Close()
	{
		this.SwitchState(LobbyMenu.State.Hidden);
		base.gameObject.SetActive(false);
	}

	public void Open()
	{
		MonoBehaviour.print("Connect.PlayerName " + Connect.PlayerName);
		this.Version.text = "Ver " + VersionNumber.version;
		MonoBehaviour.print("Set Version " + VersionNumber.version);
		LobbyMenu.matchToJoin = null;
		base.gameObject.SetActive(true);
		this.joinPasswordField.Text = string.Empty;
		this.enterNameState = LobbyMenu.EnterNameState.enterName;
		this.SwitchState(LobbyMenu.State.Disclamer);
		this.lastInputTime = Time.time;
		if (Connect.PlayerName.Length < 6)
		{
			UnityEngine.Debug.LogWarning("Player name too short " + Connect.PlayerName);
			Connect.PlayerName = "Player " + UnityEngine.Random.Range(0, 99);
		}
		Connect.PlayerName = PlayerOptions.Instance.PlayerName;
		this.playerNameField.Text = Connect.PlayerName;
		this.gameName.Text = "Game " + UnityEngine.Random.Range(10, 99);
		MonoBehaviour.print("Connect.PlayerName " + Connect.PlayerName);
		this.Version.text = "Ver " + VersionNumber.version;
		MonoBehaviour.print("Set Version " + VersionNumber.version);
	}

	private string InsertSpacesBeforeCaps(string str)
	{
		string text = str;
		for (int i = 1; i < text.Length - 1; i++)
		{
			if ((char.IsLower(text[i - 1]) && char.IsUpper(text[i])) || (text[i - 1] != ' ' && char.IsUpper(text[i]) && char.IsLower(text[i + 1])))
			{
				text = text.Insert(i, " ");
			}
		}
		return text;
	}

	private string AddWordWrap(string str, int charPerLine)
	{
		string text = string.Empty;
		int num = 0;
		int startIndex = 0;
		for (int i = 0; i < str.Length; i++)
		{
			num++;
			if (i == str.Length - 1)
			{
				text += str.Substring(startIndex, num);
			}
			else if (num > charPerLine && str[i] == ' ')
			{
				text = text + str.Substring(startIndex, num) + '\n';
				num = 0;
				startIndex = i + 1;
			}
		}
		return text;
	}

	private void Update()
	{
		if (this.skipFrame)
		{
			this.skipFrame = false;
			return;
		}
		this.CheckInput();
		switch (this.state)
		{
		case LobbyMenu.State.Disclamer:
			this.menuSizeTarget = this.DisclaimerContinueButton.GetComponent<Renderer>().bounds.size;
			this.menuPosTarget = this.DisclaimerContinueButton.GetComponent<Renderer>().bounds.center;
			if (this.accept)
			{
				this.SwitchState(LobbyMenu.State.Name);
			}
			if (this.decline)
			{
				MainMenu.GoBackToMenu();
			}
			break;
		case LobbyMenu.State.Name:
		{
			LobbyMenu.EnterNameState enterNameState = this.enterNameState;
			if (enterNameState != LobbyMenu.EnterNameState.enterName)
			{
				if (enterNameState == LobbyMenu.EnterNameState.Continue)
				{
					this.menuSizeTarget = this.EnterNameContinueButton.GetComponent<Renderer>().bounds.size;
					this.menuPosTarget = this.EnterNameContinueButton.GetComponent<Renderer>().bounds.center;
					if (this.decline || this.up)
					{
						this.enterNameState = LobbyMenu.EnterNameState.enterName;
					}
					if (this.accept)
					{
						BNetwork.TryLogin();
						this.SwitchState(LobbyMenu.State.LoggingIn);
					}
				}
			}
			else
			{
				this.menuSizeTarget = this.playerNameField.GetComponent<Collider>().bounds.size;
				this.menuPosTarget = this.playerNameField.GetComponent<Collider>().bounds.center;
				this.UsingSteamName.SetActive(SteamController.IsSteamEnabled());
				if (!this.playerNameField.focused)
				{
					if (this.Enter && !SteamController.IsSteamEnabled())
					{
						this.menuSizeTarget = Vector3.zero;
						this.playerNameField.Focus();
					}
					else if (this.accept || this.down)
					{
						this.enterNameState = LobbyMenu.EnterNameState.Continue;
					}
					else if (this.Escape || this.decline)
					{
						this.SwitchState(LobbyMenu.State.Disclamer);
					}
				}
				else
				{
					if (this.Enter)
					{
						PlayerOptions instance = PlayerOptions.Instance;
						string text = this.playerNameField.Text;
						Connect.PlayerName = text;
						instance.PlayerName = text;
						PlayerOptions.Instance.Persist();
						this.playerNameField.LooseFocus();
						this.menuSizeTarget = Vector3.zero;
						this.enterNameState = LobbyMenu.EnterNameState.Continue;
					}
					if (this.Escape)
					{
						MonoBehaviour.print("Escape");
						this.menuSizeTarget = Vector3.zero;
						this.playerNameField.LooseFocus();
					}
				}
				Connect.PlayerName = this.playerNameField.Text;
			}
			break;
		}
		case LobbyMenu.State.LoggingIn:
			this.LogInName.text = "LOGGING IN AS " + Connect.PlayerName;
			this.menuSizeTarget = this.LogInBackButton.GetComponent<Renderer>().bounds.size;
			this.menuPosTarget = this.LogInBackButton.GetComponent<Renderer>().bounds.center;
			if (SingletonMono<BNetwork>.Instance.LogInSucceeded)
			{
				this.SwitchState(LobbyMenu.State.JoinOrStart);
			}
			else if (this.decline || this.accept)
			{
				this.SwitchState(LobbyMenu.State.Name);
			}
			if (SingletonMono<BNetwork>.Instance.LogInfailed)
			{
				this.FailedLogIn.gameObject.SetActive(true);
				this.LoginInInProcessDots.gameObject.SetActive(false);
			}
			else
			{
				this.LoginInInProcessDots.gameObject.SetActive(true);
				this.FailedLogIn.gameObject.SetActive(false);
			}
			break;
		case LobbyMenu.State.JoinOrStart:
			if (this.startNewGame)
			{
				this.menuSizeTarget = this.StartOption.GetComponent<Renderer>().bounds.size;
				this.menuPosTarget = this.StartOption.GetComponent<Renderer>().bounds.center;
			}
			else
			{
				this.menuSizeTarget = this.JoinOption.GetComponent<Renderer>().bounds.size;
				this.menuPosTarget = this.JoinOption.GetComponent<Renderer>().bounds.center;
			}
			if (this.decline)
			{
				this.SwitchState(LobbyMenu.State.Name);
				this.highLight.GetComponent<Renderer>().enabled = true;
			}
			if (this.down || this.up)
			{
				this.startNewGame = !this.startNewGame;
			}
			if (this.accept)
			{
				if (this.startNewGame)
				{
					this.SwitchState(LobbyMenu.State.Start);
				}
				else
				{
					Connect.Layer.FindMatch();
					this.SwitchState(LobbyMenu.State.Join);
				}
				this.highLight.GetComponent<Renderer>().enabled = true;
			}
			break;
		case LobbyMenu.State.Start:
			switch (this.startState)
			{
			case LobbyMenu.StartState.EnterName:
				this.menuSizeTarget = this.gameName.GetComponent<Collider>().bounds.size;
				this.menuPosTarget = this.gameName.GetComponent<Collider>().bounds.center;
				if (this.gameName.focused)
				{
					if (this.Enter || this.Escape)
					{
						this.gameName.LooseFocus();
						this.menuSizeTarget = Vector3.zero;
						this.startState = LobbyMenu.StartState.EnterPassword;
					}
				}
				else if (this.Enter)
				{
					this.gameName.Focus();
					this.menuSizeTarget = Vector3.zero;
				}
				else if ((this.down || this.accept) && this.gameName.Text.Length >= 6)
				{
					this.startState = LobbyMenu.StartState.EnterPassword;
				}
				else if (this.decline || this.Escape)
				{
					this.SwitchState(LobbyMenu.State.JoinOrStart);
				}
				break;
			case LobbyMenu.StartState.EnterPassword:
				this.menuSizeTarget = this.startPasswordField.GetComponent<Collider>().bounds.size;
				this.menuPosTarget = this.startPasswordField.GetComponent<Collider>().bounds.center;
				if (this.startPasswordField.focused)
				{
					if (this.Enter || this.Escape)
					{
						this.startPasswordField.LooseFocus();
						this.menuSizeTarget = Vector3.zero;
						this.startState = LobbyMenu.StartState.PressStart;
					}
				}
				else if (this.Enter)
				{
					this.startPasswordField.Focus();
					this.menuSizeTarget = Vector3.zero;
				}
				else if (this.down || this.accept)
				{
					this.startState = LobbyMenu.StartState.PressStart;
				}
				else if (this.decline || this.up)
				{
					this.startState = LobbyMenu.StartState.EnterName;
				}
				break;
			case LobbyMenu.StartState.PressStart:
				this.menuSizeTarget = this.StartButton.GetComponent<Renderer>().bounds.size;
				this.menuPosTarget = this.StartButton.GetComponent<Renderer>().bounds.center;
				if (this.right)
				{
					this.startState = LobbyMenu.StartState.PressContinue;
				}
				else if (this.accept)
				{
					this.StartButton.text = "STARTING...";
					this.highLight.gameObject.SetActive(false);
					this.highLight.gameObject.SetActive(true);
					Connect.GameName = this.gameName.Text;
					Connect.Password = this.startPasswordField.Text;
					base.gameObject.SetActive(false);
					MainMenu.instance.StartOnline(false, null);
				}
				else if (this.decline || this.up)
				{
					this.startState = LobbyMenu.StartState.EnterPassword;
				}
				break;
			case LobbyMenu.StartState.PressContinue:
				this.menuSizeTarget = this.ContinueButton.GetComponent<Renderer>().bounds.size;
				this.menuPosTarget = this.ContinueButton.GetComponent<Renderer>().bounds.center;
				if (this.left)
				{
					this.startState = LobbyMenu.StartState.PressStart;
				}
				else if (this.accept)
				{
					this.StartButton.text = "STARTING...";
					this.highLight.gameObject.SetActive(false);
					this.highLight.gameObject.SetActive(true);
					Connect.Password = this.startPasswordField.Text;
					base.gameObject.SetActive(false);
					MainMenu.instance.StartOnline(true, null);
				}
				else if (this.decline || this.up)
				{
					this.startState = LobbyMenu.StartState.EnterPassword;
				}
				break;
			}
			break;
		case LobbyMenu.State.Join:
			if (this.IsPasswordCorrect())
			{
				this.PasswordCorrect.text = "}";
			}
			else
			{
				this.PasswordCorrect.text = "{";
			}
			this.RequiresPassword.SetActive(false);
			switch (this.joinState)
			{
			case LobbyMenu.JoinState.SelectGame:
				LobbyMenu.matchToJoin = null;
				this.joinPasswordField.Text = string.Empty;
				this.menuSizeTarget = this.gameName.GetComponent<Collider>().bounds.size;
				this.menuPosTarget = this.gameName.GetComponent<Collider>().bounds.center;
				this.highLight.gameObject.SetActive(false);
				this.joinPasswordField.LooseFocus();
				this.JoinFailed.SetActive(false);
				this.joinGameConfirm.SetActive(false);
				this.RequiresPassword.SetActive(true);
				if (!ConnectionLayer.matchQueryHandled)
				{
					this.gameListMenu.gameObject.SetActive(false);
					this.SearchingForGameDots.SetActive(true);
				}
				else
				{
					this.gameListMenu.gameObject.SetActive(true);
					this.SearchingForGameDots.SetActive(false);
				}
				if (this.decline)
				{
					this.SwitchState(LobbyMenu.State.JoinOrStart);
				}
				if (this.accept)
				{
					if (this.gameListMenu.Index == 0)
					{
						Connect.Layer.FindMatch();
						this.RefreshGameList(false);
					}
					else
					{
						MonoBehaviour.print("accept");
						MenuBarItem selectedItem = this.gameListMenu.GetSelectedItem();
						GameInfo gameInfo = null;
						if (selectedItem != null)
						{
							MonoBehaviour.print("selectedGame != null");
							gameInfo = (selectedItem.storage as GameInfo);
							LobbyMenu.matchToJoin = gameInfo;
							MonoBehaviour.print(selectedItem.storage);
							MonoBehaviour.print(LobbyMenu.matchToJoin);
							if (LobbyMenu.matchToJoin == null)
							{
								MonoBehaviour.print("is null");
							}
						}
						if (LobbyMenu.matchToJoin != null)
						{
							MonoBehaviour.print("matchToJoin != null");
							if (gameInfo.Password != string.Empty)
							{
								this.joinState = LobbyMenu.JoinState.EnterPassword;
							}
							else
							{
								this.joinState = LobbyMenu.JoinState.PressJoin;
							}
							this.gameListMenu.gameObject.SetActive(false);
							this.JoinGameName.text = gameInfo.GameName.ToUpper();
							this.joinGameConfirm.SetActive(true);
						}
					}
				}
				break;
			case LobbyMenu.JoinState.EnterPassword:
				this.highLight.gameObject.SetActive(true);
				this.menuSizeTarget = this.joinPasswordField.GetComponent<Collider>().bounds.size;
				this.menuPosTarget = this.joinPasswordField.GetComponent<Collider>().bounds.center;
				if (this.joinPasswordField.focused)
				{
					if (this.Enter || this.Escape)
					{
						this.joinPasswordField.LooseFocus();
						this.menuSizeTarget = Vector3.zero;
					}
				}
				else if (this.Enter)
				{
					this.joinPasswordField.Focus();
					this.menuSizeTarget = Vector3.zero;
				}
				else if ((this.down || this.accept) && this.IsPasswordCorrect())
				{
					this.joinState = LobbyMenu.JoinState.PressJoin;
				}
				else if (this.decline)
				{
					this.joinGameConfirm.SetActive(false);
					this.joinState = LobbyMenu.JoinState.SelectGame;
					this.gameListMenu.gameObject.SetActive(true);
				}
				if (this.accept && this.joinPasswordField.Text.ToLower() == LobbyMenu.matchToJoin.GameName)
				{
					this.joinState = LobbyMenu.JoinState.PressJoin;
					Connect.Password = this.joinPasswordField.Text;
				}
				break;
			case LobbyMenu.JoinState.PressJoin:
				this.highLight.gameObject.SetActive(true);
				this.menuSizeTarget = this.JoinButton.GetComponent<Renderer>().bounds.size;
				this.menuPosTarget = this.JoinButton.GetComponent<Renderer>().bounds.center;
				this.joinPasswordField.LooseFocus();
				this.JoinButton.text = "JOIN";
				if (this.decline)
				{
					this.joinState = LobbyMenu.JoinState.SelectGame;
					this.joinGameConfirm.SetActive(false);
					this.gameListMenu.gameObject.SetActive(true);
				}
				if (this.accept)
				{
					this.JoinButton.text = "JOINING...";
					this.highLight.gameObject.SetActive(false);
					this.highLight.gameObject.SetActive(true);
					this.joinState = LobbyMenu.JoinState.Joining;
					MainMenu.instance.StartOnline(false, LobbyMenu.matchToJoin);
				}
				break;
			case LobbyMenu.JoinState.Joining:
				this.highLight.gameObject.SetActive(false);
				this.JoinButton.text = "JOINING...";
				this.joinPasswordField.LooseFocus();
				break;
			case LobbyMenu.JoinState.FailedToJoin:
				this.JoinButton.text = "BACK";
				this.joinPasswordField.LooseFocus();
				this.highLight.gameObject.SetActive(true);
				this.menuSizeTarget = this.JoinButton.GetComponent<Renderer>().bounds.size;
				this.menuPosTarget = this.JoinButton.GetComponent<Renderer>().bounds.center;
				if (this.accept || this.decline)
				{
					this.joinState = LobbyMenu.JoinState.SelectGame;
				}
				break;
			}
			break;
		}
		if (this.highLight.gameObject.activeSelf)
		{
			this.highLight.SetTargetSize(this.menuSizeTarget);
			this.highLight.SetTargetPos(this.menuPosTarget, true);
		}
	}

	private bool IsPasswordCorrect()
	{
		return LobbyMenu.matchToJoin == null || LobbyMenu.matchToJoin.Password.ToLower() == this.joinPasswordField.Text.ToLower();
	}

	public void SwitchState(LobbyMenu.State nextState)
	{
		this.PlayerNameRoot.SetActive(false);
		this.StartOrJoinRoot.SetActive(false);
		this.startRoot.SetActive(false);
		this.joinRoot.SetActive(false);
		this.joinGameConfirm.SetActive(false);
		this.DisclaimerText.SetActive(false);
		this.LogInRoot.gameObject.SetActive(false);
		this.JoinFailed.SetActive(false);
		this.highLight.gameObject.SetActive(true);
		this.playerNameField.LooseFocus();
		this.gameName.LooseFocus();
		this.joinPasswordField.LooseFocus();
		this.startPasswordField.LooseFocus();
		this.state = nextState;
		if (nextState == LobbyMenu.State.Disclamer)
		{
			this.DisclaimerText.SetActive(true);
		}
		if (nextState == LobbyMenu.State.LoggingIn)
		{
			this.LogInRoot.gameObject.SetActive(true);
		}
		if (nextState == LobbyMenu.State.Name)
		{
			this.PlayerNameRoot.SetActive(true);
		}
		if (nextState == LobbyMenu.State.JoinOrStart)
		{
			this.StartOrJoinRoot.SetActive(true);
		}
		if (nextState == LobbyMenu.State.Start)
		{
			this.startRoot.SetActive(true);
			this.JoinButton.text = "START!";
		}
		if (nextState == LobbyMenu.State.Join)
		{
			this.joinRoot.SetActive(true);
			this.JoinButton.text = "JOIN!";
		}
	}

	private void CheckInput()
	{
		this.up = false;
		this.down = false;
		this.left = false;
		this.right = false;
		this.accept = false;
		this.decline = false;
		this.Enter = false;
		this.Escape = false;
		if (Time.time - this.lastInputTime < 0.22f)
		{
			return;
		}
		for (int i = 0; i < 6; i++)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			InputReader.GetMenuInput(i, ref flag, ref flag2, ref flag3, ref flag4, ref flag5, ref flag6);
			this.up = (this.up || flag);
			this.down = (this.down || flag2);
			this.left = (this.left || flag3);
			this.right = (this.right || flag4);
			this.accept = (this.accept || flag5);
			this.decline = (this.decline || flag6);
		}
		this.up = (this.up || Input.GetKeyDown(KeyCode.UpArrow));
		this.down = (this.down || Input.GetKeyDown(KeyCode.DownArrow));
		this.left = (this.left || Input.GetKeyDown(KeyCode.LeftArrow));
		this.right = (this.right || Input.GetKeyDown(KeyCode.RightArrow));
		this.accept = (this.accept || Input.GetKeyDown(KeyCode.Return));
		this.decline = (this.decline || Input.GetKeyDown(KeyCode.Escape));
		this.Enter = Input.GetKeyDown(KeyCode.Return);
		this.Escape = Input.GetKeyDown(KeyCode.Escape);
		if (this.up || (this.left | this.right) || this.down || this.accept || this.decline)
		{
			this.lastInputTime = Time.time;
		}
	}

	public void RefreshGameList(bool listGames)
	{
		GameInfo[] array = ConnectionLayer.matchList.ToArray();
		if (listGames)
		{
			if (array.Length > 0)
			{
				int num = array.Length + 1;
			}
		}
		this.gameListMenu.DestroyAllItems();
		List<MenuBarItem> list = new List<MenuBarItem>();
		list.Add(new MenuBarItem
		{
			name = "Refresh List",
			invokeMethod = string.Empty,
			size = 10f,
			isBetaAccess = false
		});
		if (listGames && array.Length > 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				try
				{
					GameInfo gameInfo = array[i];
					if (!gameInfo.invalidInfo)
					{
						if (gameInfo.Version == VersionNumber.version)
						{
							MenuBarItem menuBarItem = new MenuBarItem();
							menuBarItem.name = i + ". ";
							if (gameInfo.Password != string.Empty)
							{
								MenuBarItem menuBarItem2 = menuBarItem;
								menuBarItem2.name += "* ";
							}
							string text = string.Empty + (array[i].Capacity - array[i].EmptySlots) + "/4";
							MenuBarItem menuBarItem3 = menuBarItem;
							string name = menuBarItem3.name;
							menuBarItem3.name = string.Concat(new string[]
							{
								name,
								gameInfo.GameName,
								"  -  ",
								gameInfo.HostName,
								" - ",
								gameInfo.Country,
								" - ",
								text
							});
							menuBarItem.invokeMethod = string.Empty;
							menuBarItem.size = 8f;
							menuBarItem.isBetaAccess = false;
							menuBarItem.storage = array[i];
							MonoBehaviour.print(menuBarItem.storage);
							list.Add(menuBarItem);
						}
					}
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception, this);
				}
			}
		}
		if (list.Count <= 1)
		{
			list.Add(new MenuBarItem
			{
				name = "No games available",
				invokeMethod = string.Empty,
				size = 8f,
				isBetaAccess = false
			});
		}
		this.gameListMenu.itemNames = list.ToArray();
		this.gameListMenu.InstantiateItems();
		this.gameListMenu.SnapItemsToFinalPosition();
		this.gameListMenu.SnapHiglightToTarget();
	}

	private void OnMatchConnectionClosed()
	{
		if (this.state == LobbyMenu.State.Join)
		{
			this.joinState = LobbyMenu.JoinState.FailedToJoin;
			this.JoinFailed.SetActive(true);
		}
	}

	public MenuHighlightTween highLight;

	private bool up;

	private bool down;

	private bool left;

	private bool right;

	private bool accept;

	private bool decline;

	private bool Enter;

	private bool Escape;

	private Vector3 menuPosTarget = Vector3.zero;

	private Vector3 menuSizeTarget = Vector3.zero;

	private LobbyMenu.State state;

	private LobbyMenu.StartState startState;

	public TextField gameName;

	public TextField startPasswordField;

	public TextMesh StartButton;

	public TextMesh ContinueButton;

	public GameObject DisclaimerText;

	public TextMesh DisclaimerContinueButton;

	public TextField playerNameField;

	public TextMesh EnterNameContinueButton;

	private LobbyMenu.EnterNameState enterNameState;

	public GameObject UsingSteamName;

	public GameObject LogInRoot;

	public TextMesh LogInBackButton;

	public TextMesh FailedLogIn;

	public TextMesh LogInName;

	public GameObject LoginInInProcessDots;

	private LobbyMenu.JoinState joinState;

	public TextField joinPasswordField;

	public TextMesh joinPassworLabel;

	public TextMesh JoinButton;

	public Menu gameListMenu;

	public GameObject joinGameConfirm;

	public TextMesh JoinGameName;

	public TextMesh PasswordCorrect;

	public static GameInfo matchToJoin;

	public GameObject SearchingForGameDots;

	public GameObject JoinFailed;

	public GameObject RequiresPassword;

	public Transform StartOption;

	public Transform JoinOption;

	private bool startNewGame = true;

	public GameObject PlayerNameRoot;

	public GameObject StartOrJoinRoot;

	public GameObject startRoot;

	public GameObject joinRoot;

	public bool skipFrame = true;

	public TextMesh Version;

	private float lastInputTime;

	public enum State
	{
		Disclamer,
		Name,
		LoggingIn,
		JoinOrStart,
		Start,
		Join,
		Hidden
	}

	private enum StartState
	{
		EnterName,
		EnterPassword,
		PressStart,
		PressContinue
	}

	private enum EnterNameState
	{
		enterName,
		Continue
	}

	private enum JoinState
	{
		SelectGame,
		EnterPassword,
		PressJoin,
		Joining,
		FailedToJoin
	}
}
