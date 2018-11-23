// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Button : MonoBehaviour
{
	public virtual void Hilight()
	{
		this.hilighted = true;
		this.hilightSprite.gameObject.SetActive(true);
	}

	public virtual void Unhilight()
	{
		this.hilighted = false;
		this.hilightSprite.gameObject.SetActive(false);
	}

	protected virtual void Awake()
	{
		this.hilightSprite.gameObject.SetActive(false);
	}

	public virtual void Use()
	{
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}

	protected SpriteSM sprite;

	public SpriteSM hilightSprite;

	protected bool hilighted;
}
