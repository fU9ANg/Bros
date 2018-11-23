// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class Rambro : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		EffectsController.CreateShrapnel(this.bulletShell, x + base.transform.localScale.x * -15f, y + 3f, 1f, 30f, 1f, -base.transform.localScale.x * 80f, 170f);
		base.FireWeapon(x, y, xSpeed, ySpeed);
	}

	public Shrapnel bulletShell;
}
