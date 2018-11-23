// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class OKButton : Button
{
	public override void Use()
	{
		Fader.nextScene = this.nextScene;
		Fader.FadeSolid();
	}

	public string nextScene = string.Empty;
}
