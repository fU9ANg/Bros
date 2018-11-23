// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class ButtonMash : MonoBehaviour
{
	private void Start()
	{
	}

	public void Setup(int index)
	{
		this.playerNum = index;
		this.sprite = base.GetComponent<SpriteSM>();
		this.sprite.SetLowerLeftPixel_Y((float)((index + 2) * 128));
	}

	private void Update()
	{
	}

	public void Mash()
	{
		base.StopCoroutine(this.MashRoutine());
		base.StartCoroutine(this.MashRoutine());
	}

	private IEnumerator MashRoutine()
	{
		this.sprite.SetLowerLeftPixel_X(128f);
		yield return new WaitForSeconds(0.1f);
		this.sprite.SetLowerLeftPixel_X(0f);
		yield break;
	}

	private Coroutine routine;

	private SpriteSM sprite;

	private int playerNum;
}
