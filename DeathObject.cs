// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class DeathObject
{
	public DeathObject(DeathType deathType, MookType mookType, float xI, float yI)
	{
		this.deathType = deathType;
		this.mookType = mookType;
		this.xForce = xI;
		this.yForce = yI;
	}

	public DeathObject(DeathType deathType, HeroType heroType, float xI, float yI)
	{
		this.deathType = deathType;
		this.heroType = heroType;
		this.xForce = xI;
		this.yForce = yI;
	}

	public DeathObject(DeathType deathType, VehicleType vehicleType)
	{
		this.deathType = deathType;
		this.vehicleType = vehicleType;
	}

	public DeathType deathType = DeathType.None;

	public MookType mookType = MookType.None;

	public HeroType heroType = HeroType.None;

	public VehicleType vehicleType = VehicleType.None;

	public float xForce;

	public float yForce;
}
