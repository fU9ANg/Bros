// dnSpy decompiler from Assembly-CSharp.dll
using System;

[Serializable]
public class CutsceneScreen
{
	public CutsceneScreen(CutsceneForegroundType foreground, CutsceneBackgroundType background, float time, string text)
	{
		this.foreground = foreground;
		this.background = background;
		this.displayTime = time;
		this.text = text;
	}

	public CutsceneForegroundType foreground;

	public CutsceneBackgroundType background;

	public string text;

	public float displayTime = 4f;
}
