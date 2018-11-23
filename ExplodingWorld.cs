// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ExplodingWorld : MonoBehaviour
{
	private void Start()
	{
		this.sortOfFollow = SortOfFollow.GetInstance();
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
		this.explosionCounter = -this.delay;
		this.worldScreenWidth = SortOfFollow.GetWorldScreenWidth();
		if (this.worldScreenWidth < 50f)
		{
			UnityEngine.Debug.LogError("Bad Screen Width");
			this.worldScreenWidth = 400f;
		}
		this.explosionInterval = 1f / (float)this.explosionCount;
	}

	private void Update()
	{
		if (Map.isEditing || !this.explosions || GameModeController.IsLevelFinished())
		{
			return;
		}
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (HeroController.isCountdownFinished)
		{
			this.explosionCounter += this.t;
		}
		if (this.explosionCounter > this.explosionInterval)
		{
			this.explosionCounter -= this.explosionInterval * UnityEngine.Random.value;
			float num = 0f;
			float num2 = 0f;
			float minY = this.sortOfFollow.GetMinY();
			float num3 = 100f;
			switch (this.sortOfFollow.followMode)
			{
			case CameraFollowMode.Vertical:
			case CameraFollowMode.ForcedVertical:
			case CameraFollowMode.ForcedDescent:
			{
				SetResolutionCamera.GetScreenExtents(ref num, ref num2, ref minY, ref num3);
				this.worldScreenWidth = num2 - num;
				int num4 = this.explosionIndex % this.explosionCount;
				float num5 = num2 - num;
				if (this.sortOfFollow.followMode != CameraFollowMode.ForcedDescent)
				{
					this.MakeEffects(num + (float)num4 * this.worldScreenWidth / (float)this.explosionCount + UnityEngine.Random.value * this.worldScreenWidth / (float)this.explosionCount, minY);
					if (Map.HitLivingUnits(this, -1, 2, DamageType.Crush, num5 / 2f, 200f, this.sortOfFollow.transform.position.x, minY - 215f, 0f, 385f, true, true))
					{
					}
				}
				else
				{
					this.MakeEffects(num + (float)num4 * this.worldScreenWidth / (float)this.explosionCount + UnityEngine.Random.value * this.worldScreenWidth / (float)this.explosionCount, num3 + 24f);
					if (Map.HitLivingUnits(this, -1, 2, DamageType.Crush, num5 / 2f, 200f, this.sortOfFollow.transform.position.x, num3 + 232f, 0f, -100f, true, true))
					{
					}
				}
				this.PlayDeathSound();
				SortOfFollow.Shake(0.15f, base.transform.position);
				this.explosionIndex++;
				break;
			}
			case CameraFollowMode.Horizontal:
			case CameraFollowMode.ForcedHorizontal:
			{
				SetResolutionCamera.GetScreenExtents(ref num, ref num2, ref minY, ref num3);
				this.worldScreenHeight = num3 - minY + 48f;
				int num4 = this.explosionIndex % this.explosionCount + ((this.count % 2 != 0) ? -1 : 1);
				this.MakeEffects(num + 16f, minY + (float)num4 * this.worldScreenHeight / (float)this.explosionCount + UnityEngine.Random.value * this.worldScreenHeight / (float)this.explosionCount);
				this.PlayDeathSound();
				Map.PanicUnits(num - 170f, this.sortOfFollow.transform.position.y, 256f, this.worldScreenHeight, 1, 100f, false);
				if (Map.HitLivingUnits(this, -100, 2, DamageType.Crush, 256f, this.worldScreenHeight, num - 256f, this.sortOfFollow.transform.position.y, 0f, 300f, true, true))
				{
				}
				SortOfFollow.Shake(0.15f, Camera.main.transform.position);
				this.explosionIndex++;
				this.count++;
				break;
			}
			}
		}
	}

	protected virtual bool MakeEffects(float x, float y)
	{
		if (Physics.OverlapSphere(new Vector3(x, y), 24f).Length > 0)
		{
			EffectsController.CreateEffect(this.explosionBig, x, y + 10f, UnityEngine.Random.value * 0.2f, Vector3.zero, BloodColor.None);
			Map.DisturbWildLife(x, y, 120f, -1);
			MapController.DamageGround(this, 12, DamageType.Explosion, 45f, x, y, null);
			Map.BurnUnitsAround_Local(SingletonMono<MapController>.Instance, -1, 1, 90f, x, y, true, true);
			Map.ExplodeUnits(this, 12, DamageType.Explosion, 50f, 50f, x, y - 6f, 200f, 300f, -1, false, false);
			return true;
		}
		return false;
	}

	protected void PlayDeathSound()
	{
		Sound instance = Sound.GetInstance();
		if (instance != null)
		{
			instance.PlaySoundEffectAt(this.soundHolder.deathSounds, 0.2f, base.transform.position);
		}
	}

	private SortOfFollow sortOfFollow;

	private float explosionCounter;

	public int explosionCount = 5;

	protected float explosionInterval = 0.25f;

	private int explosionIndex;

	public bool explosions = true;

	public SoundHolder soundHolder;

	public float delay = 2.8f;

	public Puff explosionBig;

	protected LayerMask groundLayer;

	protected float worldScreenWidth = 400f;

	protected float worldScreenHeight = 272f;

	protected int count;

	private float t;
}
