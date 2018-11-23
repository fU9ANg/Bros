// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BambooFence : Doodad
{
	protected override void Start()
	{
		base.Start();
		SpriteSM component = base.GetComponent<SpriteSM>();
		int collumn = Map.GetCollumn(this.x);
		component.SetLowerLeftPixel(new Vector2((float)(collumn % 4 * 16), 64f));
	}

	protected override void DropGibs()
	{
		base.DropGibs();
		SpriteSM component = this.gibs[0].GetComponent<SpriteSM>();
		if (component != null)
		{
			component.SetLowerLeftPixel(base.gameObject.GetComponent<SpriteSM>().lowerLeftPixel);
		}
	}
}
