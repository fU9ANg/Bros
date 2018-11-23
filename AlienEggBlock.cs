// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AlienEggBlock : BarrelBlock
{
	public override void Explode()
	{
		base.GetComponent<Collider>().enabled = false;
		this.zAngle = Mathf.Repeat(this.zAngle, 360f);
		UnityEngine.Debug.Log("Zangle " + this.zAngle);
		if ((this.zAngle > 135f && this.zAngle < 225f) || (this.zAngle < -135f && this.zAngle > -225f))
		{
			Map.SpawnFaceHugger(this.x, this.y, -25f + UnityEngine.Random.value * 50f, 240f);
		}
		else if (this.zAngle > 45f && this.zAngle < 136f)
		{
			Map.SpawnFaceHugger(this.x, this.y, -480f + -UnityEngine.Random.value * 50f, 200f);
		}
		else if ((this.zAngle < -45f && this.zAngle > -136f) || (this.zAngle < 315f && this.zAngle > 224f))
		{
			Map.SpawnFaceHugger(this.x, this.y, 480f + UnityEngine.Random.value * 50f, 200f);
		}
		else
		{
			Map.SpawnFaceHugger(this.x, this.y, -25f + UnityEngine.Random.value * 50f, 360f);
		}
	}
}
