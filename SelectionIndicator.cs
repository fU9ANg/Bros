// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SelectionIndicator : MonoBehaviour
{
	private void Update()
	{
		if (this.fade)
		{
			Color color = base.GetComponent<SpriteSM>().color;
			color.a -= Time.deltaTime;
			base.GetComponent<SpriteSM>().SetColor(color);
			if (color.a <= 0f)
			{
				this.associatedObject = null;
				base.enabled = false;
			}
		}
	}

	public void HighlightSquare(int top, int bot, int left, int right, Color color, bool fade)
	{
		if (!base.enabled)
		{
			base.enabled = true;
		}
		this.fade = fade;
		base.transform.position = new Vector3(Map.GetBlocksX(left), Map.GetBlocksY(top + 1), 0f);
		base.GetComponent<SpriteSM>().SetSize((float)(right - left + 1) * 16f, (float)(top - bot + 1) * 16f);
		color.a = this.defaultAlpha;
		base.GetComponent<SpriteSM>().SetColor(color);
	}

	public void UnHighlightSquare()
	{
		this.fade = true;
	}

	public float defaultAlpha = 0.4f;

	public object associatedObject;

	private bool fade;
}
