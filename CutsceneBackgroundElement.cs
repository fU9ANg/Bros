// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CutsceneBackgroundElement : MonoBehaviour
{
	private void Start()
	{
		base.transform.position = new Vector3(0f, 0f, 100f);
		this.sprite = base.GetComponent<SpriteSM>();
		this.sprite.width = CutsceneController.MaxX - CutsceneController.MinX;
		this.sprite.height = CutsceneController.MaxY - CutsceneController.MinY;
		this.sprite.CalcSize();
	}

	private void Update()
	{
		base.GetComponent<Renderer>().material.mainTextureOffset = base.GetComponent<Renderer>().material.mainTextureOffset + Vector2.right * Time.deltaTime * 2f;
	}

	public CutsceneBackgroundType type;

	private SpriteSM sprite;

	[HideInInspector]
	public float timeLeft;
}
