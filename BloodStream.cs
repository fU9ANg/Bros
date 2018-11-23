// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class BloodStream : MonoBehaviour
{
	public void Restart(float time, float force, float width)
	{
		if (this.attachToParentsParent)
		{
			base.transform.parent = base.transform.parent.parent;
			if (base.transform.localScale.x < 0f)
			{
				base.transform.localScale = new Vector3(Mathf.Abs(base.transform.localScale.x), base.transform.localScale.y, base.transform.localScale.z);
				base.transform.eulerAngles = new Vector3(0f, 0f, 360f - base.transform.eulerAngles.z);
			}
		}
		this.started = true;
		this.spurtTime = time;
		this.width = width;
		this.force = force;
		base.gameObject.SetActive(true);
		this.particles = new List<BloodParticle>();
		this.sinOffset = UnityEngine.Random.value * 6f;
		this.sinSpeed = 8f + UnityEngine.Random.value * 10f;
		this.sinOffset2 = UnityEngine.Random.value * 6f;
		this.sinSpeed2 = 4f + UnityEngine.Random.value * 8f;
		this.sinSpeedISpeed = 12f + UnityEngine.Random.value * 14f;
		this.sinSpeedI = 2f + UnityEngine.Random.value * 5f;
		this.particlesMade = 0;
		this.spurtCounter = this.spurtRate;
		this.RunParticles(0.001f);
	}

	protected void RunDropBlood(float t)
	{
		if (this.particlesCount > 2)
		{
			this.dropBloodCounter += t;
			if (this.dropBloodCounter > 0.05f)
			{
				this.dropBloodCounter -= 0.05f;
				BloodParticle bloodParticle = this.particles[UnityEngine.Random.Range(0, this.particlesCount)];
				Vector3 pos = bloodParticle.GetPos();
				EffectsController.CreateBloodParticlesDots(this.color, pos.x, pos.y, pos.z + 0.1f, 1, 0f, 0f, 3f, bloodParticle.GetVelocity().x, bloodParticle.GetVelocity().y, 1f);
			}
		}
	}

	public void Start()
	{
		global::Math.SetupLookupTables();
		this.line = base.GetComponent<LineRenderer>();
		this.lineMaterial = this.line.material;
		if (!this.started)
		{
			this.Restart(this.spurtTime, 80f, this.width);
		}
	}

	protected void DrawLine(float t)
	{
		int num = this.particlesCount;
		int num2 = 0;
		if (this.spurtTime > 0f)
		{
			num++;
		}
		if (this.hitGround)
		{
			num2++;
		}
		if (num > 1)
		{
			this.line.SetVertexCount(num);
		}
		else
		{
			this.line.enabled = false;
		}
		float num3 = (0.9f + (float)num * 0.02f) * this.width;
		this.line.SetWidth(num3, 0.4f * Mathf.Clamp(this.spurtTime, 0.1f, 1f) * num3);
		if (this.spurtTime > 0f)
		{
			this.line.SetPosition(this.particlesCount, base.transform.position);
		}
		for (int i = 0; i < this.particlesCount; i++)
		{
			if (i + num2 < this.particlesCount)
			{
				this.line.SetPosition(i + num2, this.particles[i].GetPos());
			}
		}
		if (this.hitGround)
		{
			this.line.SetPosition(0, this.hitPos);
		}
		this.lineMaterial.SetTextureOffset("_MainTex", new Vector2(0f + this.materialScrolling, 0f));
	}

	protected void RunParticles(float t)
	{
		if (this.spurtTime > 0f)
		{
			this.spurtCounter += t;
			if (this.spurtCounter > this.spurtRate)
			{
				this.spurtCounter -= this.spurtRate;
				BloodParticle item = new BloodParticle(base.transform.position, base.transform.TransformDirection(this.spurtDirection) * this.force * (1f + (global::Math.Sin(this.sinOffset + global::Math.Sin(this.sinOffset2 * 2f) * 0.5f) * 0.08f + global::Math.Sin(this.sinOffset2) * 0.08f) * this.sinWobbleM));
				this.particles.Add(item);
				this.particlesCount++;
				this.particlesMade++;
			}
		}
		for (int i = 0; i < this.particles.Count; i++)
		{
			this.particles[i].Run(t);
		}
	}

	protected void RemoveParticles(float t)
	{
		Vector3 pos = this.particles[0].GetPos();
		Vector3 velocity = this.particles[0].GetVelocity();
		RaycastHit raycastHit = default(RaycastHit);
		LayerMask mask = 256;
		for (int i = 0; i < this.particlesCount; i++)
		{
			if (this.particles[i].life > 0f && this.particles[i].alive)
			{
				velocity = this.particles[i].GetVelocity();
				pos = this.particles[i].GetPos();
				if (Physics.Raycast(pos - velocity * t * 2f, velocity, out raycastHit, velocity.magnitude * 4f * t, mask))
				{
					this.newHitPos = raycastHit.point;
					this.hitNormal = raycastHit.normal;
					this.hitGround = true;
					this.particles[i].alive = false;
					this.particles[i].life = 0f;
					this.splatCount--;
					if (this.splatCount <= 0)
					{
						this.splatCount = 1;
						this.newHitPos += this.hitNormal * 0.01f;
					}
					if (i <= 2)
					{
						this.hitPos = this.newHitPos - this.hitNormal * 0.1f;
					}
					else
					{
						this.hitGround = false;
					}
				}
			}
			else if (this.particles[i].alive || i == 0)
			{
			}
		}
		if (this.particlesCount > 0 && this.particles[0].life <= -0.2f)
		{
			if (this.particlesCount > 1)
			{
				this.lostDistance += (this.particles[0].GetPos() - this.particles[1].GetPos()).magnitude;
			}
			this.particles.RemoveAt(0);
			this.particlesCount--;
			this.particlesCount = this.particles.Count;
		}
		if (this.scaleTextureWithLength && this.particlesCount > 2)
		{
			float num = 0f;
			Vector3 a = this.particles[0].GetPos();
			for (int j = 1; j < this.particlesCount; j++)
			{
				Vector3 pos2 = this.particles[j].GetPos();
				Vector3 vector = a - pos2;
				a = pos2;
				num += vector.magnitude;
			}
			base.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(num / 60f, 1f));
		}
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.spurtTime -= num;
		this.particlesCount = this.particles.Count;
		this.sinSpeedI += this.sinSpeedISpeed * num;
		this.sinOffset += num * this.sinSpeed * (1f + global::Math.Sin(this.sinSpeedI) * 0.7f);
		this.sinOffset2 += num * this.sinSpeed2;
		this.RunParticles(num);
		if (this.dropBlood)
		{
			this.RunDropBlood(num);
		}
		if (this.particlesCount > 0 && (this.particlesCount > 1 || this.spurtTime > 0f))
		{
			this.DrawLine(num);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
		if (this.particlesCount > 0)
		{
			this.RemoveParticles(num);
		}
	}

	protected List<BloodParticle> particles;

	protected LineRenderer line;

	protected float spurtRate = 0.0334f;

	public float spurtTime = 1.5f;

	protected float spurtCounter;

	public Vector3 spurtDirection = new Vector3(1f, 2f, -1f);

	protected int particlesCount;

	protected float sinOffset;

	protected float sinOffset2;

	protected float sinSpeed = 1f;

	protected float sinSpeed2 = 1f;

	protected float sinSpeedI = 1f;

	protected float sinSpeedISpeed = 1f;

	protected Vector3 hitPos = Vector3.zero;

	protected Vector3 newHitPos = Vector3.zero;

	protected Vector3 hitNormal = Vector3.zero;

	protected bool hitGround;

	protected int splatCount;

	protected Material lineMaterial;

	protected float materialScrolling;

	protected float width = 4f;

	protected float force = 20f;

	protected int particlesMade;

	public bool useTransformDirection = true;

	protected bool started;

	protected float sinWobbleM = 0.7f;

	public bool scaleTextureWithLength = true;

	protected float lostDistance;

	public bool dropBlood = true;

	protected float dropBloodCounter;

	public bool attachToParentsParent;

	private BloodColor color;
}
