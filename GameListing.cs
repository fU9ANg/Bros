// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using Badumna.Match;
using UnityEngine;

public class GameListing : MonoBehaviour
{
	private void Awake()
	{
		this.defaultAnchorWorldPos = this.listingAnchor.transform.position;
		this.defaultAnchorPos = this.listingAnchor.transform.localPosition;
		this.Setup();
	}

	private void Start()
	{
		this.highlight.SetTargetSize(new Vector3(this.s, 20f, 0f));
		this.Refresh();
	}

	private GameListingEntry CreateEntry(MatchmakingResult match, int index)
	{
		GameListingEntry gameListingEntry = UnityEngine.Object.Instantiate(this.listingPrefab) as GameListingEntry;
		gameListingEntry.transform.parent = this.listingAnchor;
		gameListingEntry.transform.localPosition = new Vector3(0f, (float)(-(float)index) * this.offset, 0f);
		gameListingEntry.SetupRandom(index);
		return gameListingEntry;
	}

	private void Setup()
	{
		this.entries.Clear();
		for (int i = 0; i < 113; i++)
		{
			GameListingEntry item = this.CreateEntry(null, i);
			this.entries.Add(item);
		}
	}

	private void Update()
	{
		this.inputTimer -= Time.deltaTime;
		bool flag = false;
		if (Input.GetKey(KeyCode.W))
		{
			if (this.inputTimer < 0f)
			{
				if (this.highlightIndex > 0)
				{
					this.highlightIndex--;
				}
				else
				{
					this.scrollPos--;
				}
				flag = true;
			}
		}
		else if (Input.GetKey(KeyCode.S))
		{
			if (this.inputTimer < 0f)
			{
				if (this.highlightIndex < this.maxDisplay - 1)
				{
					this.highlightIndex++;
				}
				else
				{
					this.scrollPos++;
				}
				flag = true;
			}
		}
		else
		{
			this.inputTimer = 0f;
		}
		this.highlightIndex = Mathf.Clamp(this.highlightIndex, 0, this.maxDisplay);
		this.scrollPos = Mathf.Clamp(this.scrollPos, 0, this.entries.Count - this.maxDisplay);
		if (flag)
		{
			this.inputTimer = 0.1f;
			this.Refresh();
		}
		this.listingAnchor.transform.localPosition = Vector3.Lerp(this.listingAnchor.transform.localPosition, this.defaultAnchorPos + Vector3.up * (float)this.scrollPos * this.offset, Time.deltaTime * 10f);
	}

	private void Refresh()
	{
		this.indexDisplay.text = this.scrollPos + this.highlightIndex + "/" + this.entries.Count;
		this.highlight.SetTargetPos(this.defaultAnchorWorldPos - (float)this.highlightIndex * this.offset * Vector3.up + Vector3.right * this.o + UnityEngine.Random.onUnitSphere * 1E-05f - Vector3.forward, false);
		for (int i = 0; i < this.entries.Count; i++)
		{
			Color white = Color.white;
			int num = i - this.scrollPos;
			if (num < 0 || num > this.maxDisplay - 1)
			{
				white.a = 0.1f;
			}
			if (num < -1 || num > this.maxDisplay)
			{
				white.a = 0f;
			}
			if (num == this.highlightIndex)
			{
				white = new Color(0.2f, 0.2f, 1f);
			}
			this.entries[i].SetColor(white);
		}
	}

	public GameListingEntry listingPrefab;

	public Transform listingAnchor;

	public TextMesh indexDisplay;

	private Vector3 defaultAnchorPos;

	private Vector3 defaultAnchorWorldPos;

	private List<GameListingEntry> entries = new List<GameListingEntry>();

	public MenuHighlightTween highlight;

	private float offset = 14f;

	private int highlightIndex;

	private int maxDisplay = 17;

	private int scrollPos;

	public float o = 250f;

	public float s = 250f;

	private float inputTimer;
}
