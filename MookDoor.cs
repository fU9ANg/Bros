// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookDoor : Doodad
{
	protected override void Start()
	{
		base.Start();
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f);
		Map.RegisterMookDoor(this);
		this.normalColor = base.GetComponent<Renderer>().material.GetColor("_TintColor");
	}

	public override void Collapse()
	{
		this.destroyedDelay = 1.5f + UnityEngine.Random.value * 1.5f;
		this.isDestroyed = true;
	}

	protected virtual void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (Map.isEditing || CutsceneController.isInCutscene)
		{
			return;
		}
		if (GameModeController.GameMode == GameMode.SuicideHorde)
		{
			if (this.flashTime > 0f)
			{
				if (this.flashTime % 0.15f < 0.075f)
				{
					base.GetComponent<Renderer>().material.SetColor("_TintColor", this.flashColor);
				}
				else
				{
					base.GetComponent<Renderer>().material.SetColor("_TintColor", this.normalColor);
				}
				this.flashTime -= Time.deltaTime;
				if (this.flashTime <= 0f)
				{
					base.GetComponent<Renderer>().material.SetColor("_TintColor", this.normalColor);
				}
			}
			return;
		}
		if (this.spawning)
		{
			this.spawningCounter += this.t;
			if (this.spawningCounter >= this.spawningRate && Connect.IsHost)
			{
				if (this.destroyedDelay <= 0f)
				{
					if (this.alarmedMookCounter > 0)
					{
						this.spawningCounter -= this.spawningRate * (0.3f + UnityEngine.Random.value * 0.5f);
						this.SpawnMook();
						this.alarmedMookCounter--;
					}
					else
					{
						this.spawningCounter -= this.spawningRate * (0.9f + UnityEngine.Random.value * 0.2f);
						if (this.mookCount < this.maxMookCount && SetResolutionCamera.IsItVisible(base.transform.position))
						{
							this.SpawnMook();
						}
					}
				}
				else
				{
					this.spawningCounter -= this.spawningRate * (0.9f + UnityEngine.Random.value * 0.2f + UnityEngine.Random.value * 0.5f);
					if (this.mookCount < this.maxMookCount)
					{
						this.SpawnMook();
					}
					float x = base.transform.position.x;
					float y = base.transform.position.y;
					Vector3 a = UnityEngine.Random.insideUnitCircle;
					float num = 16f;
					int num2 = UnityEngine.Random.Range(0, 3);
					if (num2 != 0)
					{
						if (num2 != 1)
						{
							EffectsController.CreateEffect(this.fire3, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.6f + UnityEngine.Random.value * 0.2f, a * num * 0.5f * 3f);
						}
						else
						{
							EffectsController.CreateEffect(this.fire2, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.6f + UnityEngine.Random.value * 0.2f, a * num * 0.5f * 3f);
						}
					}
					else
					{
						EffectsController.CreateEffect(this.fire1, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.6f + UnityEngine.Random.value * 0.2f, a * num * 0.5f * 3f);
					}
				}
			}
			if (this.destroyedDelay > 0f)
			{
				this.spawningCounter += this.t * 3f;
				this.maxMookCount = 8;
				this.destroyedDelay -= this.t;
				if (this.destroyedDelay <= 0f)
				{
					this.DestroyDoor();
				}
			}
		}
	}

	public void AlarmMooks(GridPoint targetPoint)
	{
		this.alarmedMookCounter = 5;
		this.path = PathfindingController.FindPath(Map.GetGridPoint(base.transform.position), targetPoint, this.mookPrefab.GetComponent<PathAgent>().capabilities, 100f);
	}

	protected virtual void DestroyDoor()
	{
		this.spawning = false;
		this.isDestroyed = true;
		this.backgroundRenderer.sharedMaterial = this.backgroundDestroyedMaterial;
		if (this.destroyedMaterial != null && base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.destroyedMaterial;
		}
		float x = base.transform.position.x;
		float y = base.transform.position.y;
		float num = 16f;
		EffectsController.CreateShrapnel(this.shrapnel, x, y, 3f, 200f, 40f, 0f, 250f);
		Vector3 a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke1, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.9f + UnityEngine.Random.value * 0.5f, a * num * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke1, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.9f + UnityEngine.Random.value * 0.5f, a * num * 3f);
		EffectsController.CreateEffect(this.smoke1, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.8f + UnityEngine.Random.value * 0.2f, UnityEngine.Random.insideUnitSphere * num * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.7f + UnityEngine.Random.value * 0.5f, a * num * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.5f + UnityEngine.Random.value * 0.5f, a * num * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.7f + UnityEngine.Random.value * 0.2f, a * num * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.explosion, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.3f + UnityEngine.Random.value * 0.5f, a * num * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.explosionBig, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.3f + UnityEngine.Random.value * 0.5f, a * num * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.explosion, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, UnityEngine.Random.value * 0.2f, a * num * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.explosionBig, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0f, a * num * 0.3f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire1, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.6f + UnityEngine.Random.value * 0.2f, a * num * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire2, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.6f + UnityEngine.Random.value * 0.2f, a * num * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire3, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.6f + UnityEngine.Random.value * 0.2f, a * num * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire1, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.6f + UnityEngine.Random.value * 0.2f, a * num * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire2, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.6f + UnityEngine.Random.value * 0.2f, a * num * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire3, x + a.x * num * 0.25f, y + a.y * num * 0.15f, 10f, 0.6f + UnityEngine.Random.value * 0.2f, a * num * 0.5f * 3f);
	}

	protected void SpawnMook()
	{
		bool flag = UnityEngine.Random.value >= 0.66f;
		bool flag2 = this.destroyedDelay > 0f;
		if (!Connect.IsHost)
		{
			MonoBehaviour.print("Only master client should be responsible for spawning new mooks");
		}
		string str = "ZMook";
		if (flag)
		{
			str = "ZMookSuicide";
		}
		Vector3 position = base.transform.position;
		GameObject gameObject = Resources.Load("Mooks/" + str) as GameObject;
		Mook component = gameObject.GetComponent<Mook>();
		Mook arg = MapController.SpawnMook_Networked(component, position.x, position.y, 0f, 0f, false, false, false, flag2, false);
		Networking.RPC<Mook, float, float, bool>(PID.TargetAll, new RpcSignature<Mook, float, float, bool>(this.ReleaseMook), arg, 0f, 0f, flag2, false);
	}

	public void ReleaseMook(Mook newMook, float xI, float yI, bool onFire)
	{
		if (newMook != null)
		{
			if (yI != 0f)
			{
				newMook.yI = yI;
				newMook.xI = xI;
				if (yI > 0f)
				{
					newMook.SetCanParachute(true);
				}
			}
			else if (this.alarmedMookCounter > 0)
			{
				newMook.enemyAI.FollowPath(this.path);
			}
			else
			{
				this.mookCount++;
				newMook.RegisterOriginDoor(this);
			}
			if (onFire)
			{
				Map.KnockAndDamageUnit(this, newMook, 3, DamageType.Fire, 0f, 0f, 0, false);
			}
		}
	}

	public void RemoveMook(Mook mook)
	{
		this.mookCount--;
	}

	internal void Flash(Color color)
	{
		this.flashTime = 1f;
		this.flashColor = color;
	}

	protected bool spawning = true;

	protected float spawningCounter;

	protected float spawningRate = 1f;

	public FlickerFader fire1;

	public FlickerFader fire2;

	public FlickerFader fire3;

	public Puff smoke1;

	public Puff smoke2;

	public Puff explosion;

	public Puff explosionBig;

	public Shrapnel shrapnel;

	public Renderer backgroundRenderer;

	public Material backgroundDestroyedMaterial;

	public Material destroyedMaterial;

	public Mook mookPrefab;

	public Mook mookSuicidePrefab;

	protected int mookCount;

	protected int maxMookCount = 3;

	protected float destroyedDelay;

	public bool isDestroyed;

	protected float t;

	private Color normalColor;

	protected GridPoint targetPoint;

	protected int alarmedMookCounter;

	protected NavPath path;

	private float flashTime;

	private Color flashColor = Color.grey;
}
