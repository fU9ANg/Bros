// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class JoinScreenPlayerEntry : MonoBehaviour
{
	private void Awake()
	{
		this.SetParticleColor(this.defaultColor);
		this.yPos = Vector3.up * base.transform.localScale.y;
	}

	private void SetParticleColor(Color color)
	{
	}

	public void SetPlayerName(string Name)
	{
	}

	private void Update()
	{
		if (this.joined)
		{
			this.playerName.localPosition = Vector3.Lerp(this.playerName.localPosition, this.joinedtOffset * (float)this.direction + this.yPos, Time.deltaTime * 10f);
		}
		else
		{
			this.playerName.localPosition = Vector3.Lerp(this.playerName.localPosition, this.startOffset * (float)this.direction + this.yPos, Time.deltaTime * 10f);
		}
		if (this.joined)
		{
			this.SetParticleColor(this.defaultColor);
		}
		else
		{
			this.SetParticleColor(this.defaultColor);
		}
	}

	public Color defaultColor = Color.white;

	public Color playerColor = Color.red;

	public Transform playerName;

	public int direction = 1;

	public int index;

	public TextMesh playerName1;

	public TextMesh playerName2;

	public string playerNameString = string.Empty;

	public bool joined;

	private Vector3 startOffset = new Vector3(0f, 0f, 0f);

	private Vector3 joinedtOffset = new Vector3(-400f, 0f, 0f);

	private Vector3 yPos;
}
