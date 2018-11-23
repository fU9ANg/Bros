// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FlexBackdrop : MonoBehaviour
{
	public void Setup(int playerNum)
	{
		float num = 1f / (float)FlexController.instance.playerCount;
		float num2 = num * (float)playerNum;
		SpriteSM component = base.GetComponent<SpriteSM>();
		component.transform.SetX(Camera.main.BottomLeftWorldPos().x);
		float width = component.width;
		component.width *= num;
		component.pixelDimensions.x = component.width;
		component.SetPixelDimensions(component.pixelDimensions);
		component.OffsetLowerLeftPixel(width * (0.5f - num / 2f), 0f);
		SpriteSM spriteSM = component;
		spriteSM.offset.x = spriteSM.offset.x + width * num2;
		component.RecalcTexture();
		component.UpdateUVs();
	}
}
