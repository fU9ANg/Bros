// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapBird : MonoBehaviour
{
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.targetPoint = this.GetRandomTargetPoint();
		this.currentTargetPoint = this.targetPoint;
		base.transform.position = this.flockTransform.position + new Vector3(this.targetPoint.x, 0f, this.targetPoint.z);
		this.lastFlockPosition = this.flockTransform.position;
	}

	protected Vector3 GetRandomTargetPoint()
	{
		return new Vector3(-0.3f + UnityEngine.Random.value * 0.6f, this.flyHeight, -0.3f + UnityEngine.Random.value * 0.6f);
	}

	private void Update()
	{
		Vector3 position = base.transform.position;
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.navigateCounter += Time.deltaTime;
		if (this.navigateCounter > 0.2f)
		{
			this.targetPoint = this.flockTransform.position + (this.flockTransform.position - this.lastFlockPosition).normalized * 1.5f;
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < this.otherSeagulls.Length; i++)
			{
				if (this.otherSeagulls[i] != null && this.otherSeagulls[i] != base.transform)
				{
					Vector3 a = this.otherSeagulls[i].position - base.transform.position;
					float magnitude = a.magnitude;
					if (magnitude > 0f && magnitude < this.repelRange)
					{
						float d = 1f - magnitude / this.repelRange;
						vector -= a / magnitude * this.repelM * d * d;
					}
				}
			}
			Vector3 diff = new Vector3(this.avoidTransform.position.x, 0f, this.avoidTransform.position.z) - new Vector3(base.transform.position.x, 0f, base.transform.position.z);
			float magnitude2 = diff.magnitude;
			if (magnitude2 < this.avoidRange)
			{
				if (this.panicExtraSpeed < 0.01f)
				{
					if (UnityEngine.Random.value > 0.7f)
					{
						Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.effortSounds, 0.65f, base.transform.position);
					}
					if (this.panicTime <= 0f)
					{
						Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.defendSounds, 0.11f + 0.04f * UnityEngine.Random.value, base.transform.position, 0.9f + UnityEngine.Random.value * 0.2f, false, UnityEngine.Random.value * 0.5f);
						this.panicTime = 1f + UnityEngine.Random.value * 3f;
					}
				}
				if (magnitude2 < this.avoidKillRange)
				{
					this.Gib(diff);
				}
				else
				{
					float num2 = magnitude2 / this.avoidRange;
					num2 = 1f - num2 * num2;
					vector -= num2 * diff.normalized * this.repelM * 8f;
					this.panicExtraSpeed = Mathf.Clamp(this.panicExtraSpeed + num2 * this.flySpeed * 2f, 0f, this.flySpeed * 4f);
				}
			}
			this.targetPoint += vector;
			this.targetPoint = new Vector3(this.targetPoint.x, this.flyHeight, this.targetPoint.z);
			this.navigateCounter = -UnityEngine.Random.value * 0.5f;
			this.lastFlockPosition = this.flockTransform.position;
		}
		else
		{
			Vector3 diff2 = new Vector3(this.avoidTransform.position.x, 0f, this.avoidTransform.position.z) - new Vector3(base.transform.position.x, 0f, base.transform.position.z);
			float magnitude3 = diff2.magnitude;
			if (magnitude3 < this.avoidRange && magnitude3 < this.avoidKillRange)
			{
				this.Gib(diff2);
			}
		}
		float num3 = this.flySpeed + this.panicExtraSpeed;
		base.transform.position += base.transform.forward * (this.flySpeed + this.panicExtraSpeed) * Time.deltaTime;
		this.flapCounter += num;
		if (this.flapCounter >= 1f / num3 * 0.05f)
		{
			this.flapCounter -= 1f / num3 * 0.05f;
			this.flapCount++;
			this.sprite.SetLowerLeftPixel((float)(this.flapCount % 4 * 16), 16f);
		}
		this.panicExtraSpeed *= 1f - num * 1f;
		this.panicTime -= num;
		this.dummyTransform.LookAt(base.transform.position + (this.targetPoint - base.transform.position), Vector3.up);
		Vector3 b = Vector3.RotateTowards(base.transform.forward, this.dummyTransform.forward, Time.deltaTime * (this.turnSpeed + this.panicExtraSpeed * 4f), Time.deltaTime * 8f);
		base.transform.LookAt(base.transform.position + b, Vector3.up);
	}

	protected void Gib(Vector3 diff)
	{
		if (UnityEngine.Random.value > 0.6f)
		{
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.deathSounds, 0.2f, base.transform.position);
		}
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.dismemberSounds, 0.33f, base.transform.position);
		diff = diff.normalized;
		WorldMapEffectsController.EmitBlood(26, base.transform.position, Color.white, 0.05f, new Vector3(diff.x * -0.5f, 1f, diff.z * -0.5f), 2f, 0.2f, 1f);
		WorldMapEffectsController.EmitFeathers(12, base.transform.position, Color.white, 0.05f, new Vector3(diff.x * -0.5f, 1f, diff.z * -0.5f), 2f, 0.2f, 1f);
		base.gameObject.SetActive(false);
	}

	public float flyHeight = 5.4f;

	public float flySpeed = 0.5f;

	protected float navigateCounter;

	protected Vector3 targetPoint = Vector3.up * 5f;

	protected Vector3 currentTargetPoint = Vector3.up * 0.5f;

	public Transform dummyTransform;

	public Transform flockTransform;

	public Transform[] otherSeagulls;

	public float repelRange = 0.25f;

	public float repelM = 0.4f;

	protected Vector3 lastFlockPosition;

	public float turnSpeed = 1.5f;

	protected SpriteSM sprite;

	protected float flapCounter;

	protected int flapCount;

	public Transform avoidTransform;

	protected float avoidRange = 2f;

	protected float avoidKillRange = 0.2f;

	protected float panicExtraSpeed;

	public SoundHolder soundHolder;

	protected float panicTime;
}
