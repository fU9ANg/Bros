// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PetrolTankCompartment : DoodadDestroyable
{
	private new void Start()
	{
		base.Start();
		this.tankBase = this.GetComponentInHeirarchy<DoodadPetrolTank>();
	}

	private new void Update()
	{
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		if (this.Launched)
		{
			Vector3 a = UnityEngine.Random.insideUnitCircle;
			a.x *= 6f;
			a.y *= 9f;
			a += 12f * this.flameDirection;
			a.y += 16f;
			Vector3 velocity = this.flameDirection * 100f;
			switch (UnityEngine.Random.Range(0, 3))
			{
			case 0:
				EffectsController.CreateEffect(this.fire1, this.x + a.x, this.y + a.y - 2f, UnityEngine.Random.value * 0.0434f, velocity);
				break;
			case 1:
				EffectsController.CreateEffect(this.fire2, this.x + a.x, this.y + a.y - 2f, UnityEngine.Random.value * 0.0434f, velocity);
				break;
			case 2:
				EffectsController.CreateEffect(this.fire3, this.x + a.x, this.y + a.y - 2f, UnityEngine.Random.value * 0.0434f, velocity);
				break;
			}
		}
		if (this.xI != 0f)
		{
			MapController.DamageGround(this, 15, DamageType.Crush, 16f, 48f, base.centerX, base.centerY, true, this.tankBase.collidersToIgnore);
		}
		this.CrushTerain();
	}

	public void Launch(Vector3 direction)
	{
		this.Launched = true;
		this.tankBase.Launch(direction);
		this.flameDirection = -direction;
		if (this.blastSound == null)
		{
			this.blastSound = base.gameObject.AddComponent<AudioSource>();
			this.blastSound.clip = this.soundHolder.specialSounds[UnityEngine.Random.Range(0, this.soundHolder.specialSounds.Length)];
			this.blastSound.loop = true;
			this.blastSound.volume = 0.01f;
			this.blastSound.rolloffMode = AudioRolloffMode.Linear;
			this.blastSound.maxDistance = 200f;
			this.blastSound.dopplerLevel = 0.12f;
			this.blastSound.pitch = 0.5f;
			this.blastSound.Play();
		}
	}

	protected virtual void CrushTerain()
	{
		float x = base.GetComponent<Collider>().bounds.extents.x;
		float y = base.GetComponent<Collider>().bounds.extents.y;
		float x2 = base.GetComponent<Collider>().bounds.center.x;
		float y2 = base.GetComponent<Collider>().bounds.center.y;
		if (!Map.HitUnits(this, this, 15, 18, DamageType.Crush, x / 2f, y / 2f, x2, y2, this.tankBase.xI, this.tankBase.yI, true, false, true))
		{
			if (Map.HitUnits(this, this, -1, 24, DamageType.Crush, x / 2f, y / 2f, x2, y2, this.tankBase.xI, this.tankBase.yI, true, false, true))
			{
			}
		}
	}

	protected override void CreateExplosion()
	{
		Vector3 bottonLeft = base.transform.position - Vector3.up * 16f;
		EffectsController.CreateExplosionInRectangle(bottonLeft, 3, 5, 4f, 0f, 1f, this.xI, this.yI, 1f, 0.5f, false, true);
		MapController.DamageGround(this, 15, DamageType.Crush, 48f, 80f, base.centerX, base.centerY + 80f, false, this.tankBase.collidersToIgnore);
		MapController.BurnGround_Local(48f, base.centerX, base.centerY, Map.groundLayer);
		RaycastHit raycastHit;
		if (Extensions.Raycast(base.transform.position, Vector3.right, out raycastHit, 20f, Map.groundLayer, true, Color.magenta, 2f) && raycastHit.collider.GetComponent<PetrolTankCompartment>() != null)
		{
			raycastHit.collider.GetComponent<PetrolTankCompartment>().TryLaunch();
		}
		if (Extensions.Raycast(base.transform.position, Vector3.left, out raycastHit, 20f, Map.groundLayer, true, Color.magenta, 2f) && raycastHit.collider.GetComponent<PetrolTankCompartment>() != null)
		{
			raycastHit.collider.GetComponent<PetrolTankCompartment>().TryLaunch();
		}
	}

	public override bool Damage(DamageObject damageObject)
	{
		base.Damage(damageObject);
		this.TryLaunch();
		return false;
	}

	public void TryLaunch()
	{
		if (this.Launched)
		{
			return;
		}
		Vector3 vector = Vector3.zero;
		RaycastHit raycastHit;
		if (Extensions.Raycast(base.transform.position, Vector3.right, out raycastHit, 16f, Map.groundLayer, true, Color.magenta, 2f))
		{
			if (raycastHit.collider.GetComponent<PetrolTankCompartment>() == null)
			{
				vector = Vector3.left;
			}
		}
		else
		{
			vector = Vector3.left;
		}
		if (Extensions.Raycast(base.transform.position, Vector3.left, out raycastHit, 16f, Map.groundLayer, true, Color.magenta, 2f))
		{
			if (raycastHit.collider.GetComponent<PetrolTankCompartment>() == null)
			{
				vector = Vector3.right;
			}
		}
		else
		{
			vector = Vector3.right;
		}
		if (vector != Vector3.zero)
		{
			this.Launch(vector);
		}
	}

	protected AudioSource blastSound;

	public FlickerFader fire1;

	public FlickerFader fire2;

	public FlickerFader fire3;

	public Puff smoke1;

	public Puff smoke2;

	public Puff explosion;

	public Puff explosionBig;

	private bool Launched;

	private Vector3 flameDirection = Vector3.zero;

	private DoodadPetrolTank tankBase;
}
