// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CustomCampaignVictoryScoreDisplay : MonoBehaviour
{
	private bool ShowBrotality
	{
		get
		{
			return LevelSelectionController.currentCampaign.header.hasBrotalityScoreboard;
		}
	}

	private bool ShowTime
	{
		get
		{
			return LevelSelectionController.currentCampaign.header.hasTimeScoreBoard;
		}
	}

	private bool ShowBothScores
	{
		get
		{
			return this.ShowBrotality && this.ShowTime;
		}
	}

	private bool ShowNoScores
	{
		get
		{
			return !this.ShowBrotality && !this.ShowTime;
		}
	}

	private void Start()
	{
		foreach (TextMesh textMesh in this.speedNames)
		{
			textMesh.text = string.Empty;
		}
		foreach (TextMesh textMesh2 in this.speedScores)
		{
			textMesh2.text = string.Empty;
		}
		foreach (TextMesh textMesh3 in this.brotalityNames)
		{
			textMesh3.text = string.Empty;
		}
		foreach (TextMesh textMesh4 in this.brotalityScores)
		{
			textMesh4.text = string.Empty;
		}
		if (!this.ShowBothScores)
		{
			if (this.ShowTime)
			{
				this.timeHolder.transform.localPosition = new Vector3(0f, this.timeHolder.transform.localPosition.y, this.timeHolder.transform.localPosition.z);
			}
			else
			{
				this.timeHolder.gameObject.SetActive(false);
			}
			if (this.ShowBrotality)
			{
				this.brotalityHolder.transform.localPosition = new Vector3(0f, this.brotalityHolder.transform.localPosition.y, this.brotalityHolder.transform.localPosition.z);
			}
			else
			{
				this.brotalityHolder.gameObject.SetActive(false);
			}
		}
		float num = 0f;
		this.campaignStats = StatisticsController.GetCampaignStats();
		foreach (KeyValuePair<int, LevelStats> keyValuePair in this.campaignStats)
		{
			num += keyValuePair.Value.totalTime;
		}
	}

	private void Update()
	{
		if (!this.hasSetSpeedScores && PlaytomicController.speedList != null)
		{
			this.hasSetSpeedScores = true;
			for (int i = 0; i < PlaytomicController.speedList.Count; i++)
			{
				this.speedNames[i].text = string.Concat(new object[]
				{
					string.Empty,
					PlaytomicController.speedList[i].rank,
					". ",
					PlaytomicController.speedList[i].playername
				});
				this.speedScores[i].text = StatisticsController.GetTimeString((float)PlaytomicController.speedList[i].points / 1000f);
				if (PlaytomicController.speedList[i].submitted)
				{
					this.submittedSpeedScoreRank = i;
				}
			}
		}
		if (this.hasSetSpeedScores && this.submittedSpeedScoreRank > -1)
		{
			TextMesh textMesh = this.speedNames[this.submittedSpeedScoreRank];
			Color color = Color.Lerp(Color.white, Color.blue, Mathf.PingPong(Time.time * 3f, 1f));
			this.speedScores[this.submittedSpeedScoreRank].color = color;
			textMesh.color = color;
		}
		if (!this.hasSetBrotalityScores && PlaytomicController.brotalityList != null)
		{
			this.hasSetBrotalityScores = true;
			for (int j = 0; j < PlaytomicController.brotalityList.Count; j++)
			{
				this.brotalityNames[j].text = string.Concat(new object[]
				{
					string.Empty,
					PlaytomicController.brotalityList[j].rank,
					". ",
					PlaytomicController.brotalityList[j].playername
				});
				this.brotalityScores[j].text = PlaytomicController.brotalityList[j].points.ToString();
				if (PlaytomicController.brotalityList[j].submitted)
				{
					this.submittedBrotalityScoreRank = j;
				}
			}
		}
		if (this.hasSetBrotalityScores && this.submittedBrotalityScoreRank > -1)
		{
			TextMesh textMesh2 = this.brotalityScores[this.submittedBrotalityScoreRank];
			Color color = Color.Lerp(Color.white, Color.blue, Mathf.PingPong(Time.time * 3f, 1f));
			this.brotalityNames[this.submittedBrotalityScoreRank].color = color;
			textMesh2.color = color;
		}
		if ((this.ShowTime && !this.hasSetSpeedScores) || (this.ShowBrotality && !this.hasSetBrotalityScores))
		{
			this.statusText.text = "Submitting Scores...";
			this.statusText.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * 5f, 1f));
		}
		else
		{
			this.statusText.text = string.Empty;
		}
	}

	private void NotOnGUI()
	{
		if (this.campaignStats == null)
		{
			this.campaignStats = StatisticsController.GetCampaignStats();
		}
		GUILayout.BeginArea(new Rect((float)Screen.width * 0.125f, (float)Screen.height * 0.125f, (float)(Screen.width * 3) / 4f, (float)(Screen.height * 3) / 4f));
		long num = 0L;
		float num2 = 0f;
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<int, LevelStats> keyValuePair in this.campaignStats)
		{
			num2 += keyValuePair.Value.totalTime;
			num += StatisticsController.CalculateBrotality(keyValuePair.Value);
			stringBuilder.Append("\n\nTotal Brotality: " + num);
			stringBuilder.Append("\nTotal Time: " + StatisticsController.GetTimeString(num2));
		}
		stringBuilder.Append("\n");
		stringBuilder.Append("\n");
		GUILayout.Label(stringBuilder.ToString(), new GUILayoutOption[0]);
		GUILayout.EndArea();
	}

	private Dictionary<int, LevelStats> campaignStats;

	public TextMesh[] speedNames;

	public TextMesh[] speedScores;

	public TextMesh[] brotalityNames;

	public TextMesh[] brotalityScores;

	public TextMesh statusText;

	public TextMesh yourScoreText;

	public Transform timeHolder;

	public Transform brotalityHolder;

	private bool hasSetSpeedScores;

	private bool hasSetBrotalityScores;

	private int submittedSpeedScoreRank = -1;

	private int submittedBrotalityScoreRank = -1;
}
