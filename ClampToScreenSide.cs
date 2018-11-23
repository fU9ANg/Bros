// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ClampToScreenSide : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Update()
	{
		Vector3 position = Vector3.zero;
		switch (this.anchor)
		{
		case ClampToScreenSide.Anchor.Bottom:
			position = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
			position.x = base.transform.position.x;
			break;
		case ClampToScreenSide.Anchor.BottomRight:
			position = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
			break;
		case ClampToScreenSide.Anchor.BottomLeft:
			position = Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width, 0f, 0f));
			break;
		}
		position.z = base.transform.position.z;
		base.transform.position = position;
	}

	public ClampToScreenSide.Anchor anchor = ClampToScreenSide.Anchor.BottomLeft;

	public enum Anchor
	{
		Bottom,
		BottomRight,
		BottomLeft
	}
}
