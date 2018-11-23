// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class OnlineCustomCampaignBrowser : Menu
{
	private void Start()
	{
		global::Math.SetupLookupTables();
		if (!this.MenuActive)
		{
			this.statusText.gameObject.SetActive(false);
			this.MenuActive = false;
		}
	}

	protected override void SetupItems()
	{
		if (PlaytomicController.remoteLevels != null)
		{
			List<MenuBarItem> list = new List<MenuBarItem>();
			for (int i = 0; i < PlaytomicController.remoteLevels.Count; i++)
			{
				MenuBarItem menuBarItem = new MenuBarItem();
				list.Add(new MenuBarItem
				{
					color = Color.white,
					size = 14f,
					name = PlaytomicController.remoteLevels[i].name,
					invokeMethod = "LaunchLevel"
				});
			}
			if (this.pageNo > 1)
			{
				MenuBarItem menuBarItem2 = new MenuBarItem();
				list.Add(new MenuBarItem
				{
					color = Color.yellow,
					size = 14f,
					name = "PAGE " + (this.pageNo - 1),
					invokeMethod = "PrevPage"
				});
			}
			if (PlaytomicController.remoteLevels.Count >= 20)
			{
				list.Add(new MenuBarItem
				{
					color = Color.yellow,
					size = 14f,
					name = "PAGE " + (this.pageNo + 1),
					invokeMethod = "NextPage"
				});
			}
			list.Add(new MenuBarItem
			{
				color = Color.yellow,
				size = 14f,
				name = "BACK",
				invokeMethod = "BackToCustomMenu"
			});
			this.itemNames = list.ToArray();
		}
	}

	private void BackToCustomMenu()
	{
		this.offlineLevelsMenu.MenuActive = true;
		this.offlineLevelsMenu.TransitionIn();
		this.MenuActive = false;
	}

	public void LaunchLevel()
	{
		Map.ClearSuperCheckpointStatus();
		PlayerProgress.Instance.lastOnlineLevelProgress = 0;
		PlaytomicController.LoadLevel(PlaytomicController.remoteLevels[this.highlightIndex].levelid);
		this.goToLevelUponFinishLoad = true;
	}

	public void LaunchLevel(string levelid)
	{
		Map.ClearSuperCheckpointStatus();
		PlaytomicController.LoadLevel(levelid);
		this.goToLevelUponFinishLoad = true;
	}

	protected override void Update()
	{
		base.Update();
		if (this.MenuActive)
		{
			if (!this.hasSetup)
			{
				this.hasSetup = true;
				PlaytomicController.ListLevels(this.pageNo);
				this.waitingForLevelList = true;
				global::Math.SetupLookupTables();
				this.statusText.gameObject.SetActive(true);
			}
			if (PlaytomicController.listingLevels == RemoteOperationStatus.Busy || this.goToLevelUponFinishLoad)
			{
				this.statusText.text = "LOADING...";
				this.statusText.color = Color.Lerp(Color.black, Color.white, Mathf.PingPong(Time.time * 3f, 1f));
				TextMesh textMesh = this.dateText;
				string text = string.Empty;
				this.ratingNumberText.text = text;
				text = text;
				this.ratingText.text = text;
				text = text;
				this.gameModeText.text = text;
				text = text;
				this.lengthText.text = text;
				text = text;
				this.descriptionText.text = text;
				text = text;
				this.authorText.text = text;
				text = text;
				this.nameText.text = text;
				textMesh.text = text;
				this.ClearStars();
			}
			else if (this.waitingForLevelList)
			{
				this.waitingForLevelList = false;
				this.InstantiateItems();
			}
			if (PlaytomicController.listingLevels != RemoteOperationStatus.Busy && !this.goToLevelUponFinishLoad)
			{
				this.statusText.text = string.Empty;
			}
			if (PlaytomicController.listingLevels == RemoteOperationStatus.Success && this.highlightIndex >= 0 && this.highlightIndex < PlaytomicController.remoteLevels.Count)
			{
				if (PlaytomicController.listingLevels != RemoteOperationStatus.Busy && !this.goToLevelUponFinishLoad)
				{
					this.ClearStars();
					string text2 = PlaytomicController.remoteLevels[this.highlightIndex].name;
					if (text2.Length > 35)
					{
						text2 = text2.Substring(0, 32) + "...";
					}
					this.nameText.text = text2;
					this.authorText.text = string.Concat(new string[]
					{
						"AUTHOR: ",
						PlaytomicController.remoteLevels[this.highlightIndex].fields["author"].ToString(),
						"   LENGTH: ",
						PlaytomicController.remoteLevels[this.highlightIndex].fields["length"].ToString(),
						"   MODE: ",
						PlaytomicController.remoteLevels[this.highlightIndex].fields["gamemode"].ToString()
					});
					this.descriptionText.text = PlaytomicController.remoteLevels[this.highlightIndex].fields["description"].ToString();
					this.dateText.text = "UPLOADED: " + PlaytomicController.remoteLevels[this.highlightIndex].date.ToShortDateString();
					string empty = string.Empty;
					int num = 0;
					float num2 = (float)PlaytomicController.remoteLevels[this.highlightIndex].score / (float)PlaytomicController.remoteLevels[this.highlightIndex].votes;
					int num3 = 0;
					while ((float)num3 < Mathf.Floor(Mathf.Round(num2) / 2f))
					{
						num++;
						this.stars.Add(UnityEngine.Object.Instantiate(this.starPrefab, this.ratingText.transform.position + Vector3.right * 10f * (float)num, Quaternion.identity) as SpriteSM);
						num3++;
					}
					float f = num2 - (float)(num * 2);
					if (Mathf.Round(f) >= 1f)
					{
						num++;
						if (Mathf.Round(f) < 2f)
						{
							this.stars.Add(UnityEngine.Object.Instantiate(this.starHalfPrefab, this.ratingText.transform.position + Vector3.right * 10f * (float)num, Quaternion.identity) as SpriteSM);
						}
						else
						{
							this.stars.Add(UnityEngine.Object.Instantiate(this.starPrefab, this.ratingText.transform.position + Vector3.right * 10f * (float)num, Quaternion.identity) as SpriteSM);
						}
					}
					this.ratingText.text = "Rating: " + empty;
					this.ratingText.color = Color.yellow;
					this.ratingNumberText.transform.position = this.ratingText.transform.position + Vector3.right * 10f * (float)(num + 1);
					this.ratingNumberText.text = string.Empty + Mathf.Ceil((float)PlaytomicController.remoteLevels[this.highlightIndex].score / (float)PlaytomicController.remoteLevels[this.highlightIndex].votes * 50f) / 100f;
				}
			}
			else
			{
				TextMesh textMesh2 = this.dateText;
				string text = string.Empty;
				this.ratingNumberText.text = text;
				text = text;
				this.ratingText.text = text;
				text = text;
				this.gameModeText.text = text;
				text = text;
				this.lengthText.text = text;
				text = text;
				this.descriptionText.text = text;
				text = text;
				this.authorText.text = text;
				text = text;
				this.nameText.text = text;
				textMesh2.text = text;
				this.ClearStars();
			}
			if (this.goToLevelUponFinishLoad && PlaytomicController.loadedLevel != null)
			{
				this.goToLevelUponFinishLoad = false;
				LevelSelectionController.ResetLevelAndGameModeToDefault();
				LevelSelectionController.isOnlineCampaign = true;
				LevelSelectionController.loadCustomCampaign = true;
				LevelSelectionController.currentCampaign = FileIO.DecodeCampaign(PlaytomicController.loadedLevel.data);
				GameModeController.GameMode = LevelSelectionController.currentCampaign.header.gameMode;
				LevelSelectionController.loadMode = MapLoadMode.Campaign;
				LevelSelectionController.CurrentLevelNum = PlayerProgress.Instance.lastOnlineLevelProgress;
				Application.LoadLevel(LevelSelectionController.JoinScene);
			}
			if (Input.GetKeyDown(KeyCode.Escape) || InputReader.GetControllerPressingSpecial() > -1)
			{
				this.BackToCustomMenu();
			}
		}
	}

	private void ShowHighScores()
	{
		PlaytomicController.GetCampaignScores();
	}

	protected void ClearStars()
	{
		foreach (SpriteSM spriteSM in this.stars)
		{
			UnityEngine.Object.Destroy(spriteSM.gameObject);
		}
		this.stars.Clear();
	}

	private void NextPage()
	{
		this.pageNo++;
		this.hasSetup = false;
	}

	private void PrevPage()
	{
		this.pageNo--;
		this.hasSetup = false;
	}

	private bool goToLevelUponFinishLoad;

	private bool showingHighScores;

	public TextMesh statusText;

	private bool waitingForLevelList;

	public bool hasSetup;

	public Menu offlineLevelsMenu;

	public SpriteSM starPrefab;

	public SpriteSM starHalfPrefab;

	protected List<SpriteSM> stars = new List<SpriteSM>();

	public TextMesh nameText;

	public TextMesh authorText;

	public TextMesh descriptionText;

	public TextMesh lengthText;

	public TextMesh gameModeText;

	public TextMesh ratingText;

	public TextMesh ratingNumberText;

	public TextMesh dateText;

	public int pageNo;
}
