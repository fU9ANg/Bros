// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionAnimator : MonoBehaviour
{
	private void Start()
	{
		this.emitter = base.GetComponent<ParticleEmitter>();
		this.groundLayer = (1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("DirtyHippie") | 1 << LayerMask.NameToLayer("Platform"));
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		this.bledTrail = false;
		this.particleCount = this.emitter.particleCount;
		if (this.particleCount > 0)
		{
			this.particleList.Clear();
			this.particles = this.emitter.particles;
			for (int i = 0; i < this.particles.Length; i++)
			{
				this.p = this.particles[i];
				if (this.DestroyParticlesOutsideOfFluid && !FluidController.IsSubmerged(this.p.position.x, this.p.position.y))
				{
					this.p.energy = 0f;
				}
				if (this.randomMotion)
				{
					this.p.velocity = new Vector3((this.p.velocity.x + this.randomMotionM * (1f - UnityEngine.Random.value * 2f)) * (1f - deltaTime * this.dampeningM), (this.p.velocity.y + this.randomMotionM * (1f - UnityEngine.Random.value * 2f)) * (1f - deltaTime * this.dampeningM) - this.gravity * deltaTime, 0f);
					if (this.splits && this.p.energy > 0.5f * this.emitter.minEnergy && UnityEngine.Random.value < deltaTime * 2f)
					{
						this.particleList.Add(this.p);
						Particle value = this.particleList[this.particleList.Count - 1];
						Vector3 velocity = this.p.velocity * 0.8f + UnityEngine.Random.onUnitSphere * this.p.velocity.magnitude * 0.4f;
						velocity.z = 0f;
						value.velocity = velocity;
						value.energy += 0.1f;
						this.particleList[this.particleList.Count - 1] = value;
					}
				}
				else
				{
					this.p.velocity = new Vector3(this.p.velocity.x * (1f - deltaTime * this.dampeningM), this.p.velocity.y * (1f - deltaTime * this.dampeningM) - this.gravity * deltaTime, 0f);
				}
				this.p.energy = this.p.energy - deltaTime;
				if (this.resetParticleAngleOnCollision)
				{
					this.p.rotation = this.p.rotation + this.p.angularVelocity * deltaTime;
				}
				this.p.size = this.p.size + this.p.size * deltaTime * this.growSizeM;
				float num = this.p.velocity.magnitude * deltaTime * 2f;
				if (this.collidesWithGround && Physics.Raycast(this.p.position, this.p.velocity, out this.raycastHit, 16f + num, this.groundLayer))
				{
					float x = this.p.velocity.x * deltaTime;
					float y = this.p.velocity.y * deltaTime;
					DirectionEnum directionEnum = DirectionEnum.Any;
					if (this.raycastHit.normal.y >= 0.5f)
					{
						directionEnum = DirectionEnum.Up;
					}
					else if (this.raycastHit.normal.y <= -0.5f)
					{
						directionEnum = DirectionEnum.Down;
					}
					else if (this.raycastHit.normal.x >= 0.5f)
					{
						directionEnum = DirectionEnum.Right;
					}
					else if (this.raycastHit.normal.x <= -0.5f)
					{
						directionEnum = DirectionEnum.Left;
					}
					if (this.ConstrainToColliders(this.p.position, this.p.size / 2f, ref x, ref y, this.raycastHit.point, directionEnum))
					{
						if (this.resetParticleAngleOnCollision)
						{
							this.p.rotation = 0f;
							this.p.angularVelocity = 30f * this.p.velocity.x;
						}
						if (this.leavesDecals && this.p.energy > this.minEnergyDecal)
						{
							this.raycastHit.collider.gameObject.SendMessage("CreateDecal", new DecalInfo(DecalType.Bloody, this.color, this.p.position, directionEnum, (float)this.decalEffectSize, this.raycastHit.normal), SendMessageOptions.DontRequireReceiver);
						}
						if (this.collisionEnergyLoss > 0f)
						{
							this.p.energy = this.p.energy - this.collisionEnergyLoss;
						}
						else
						{
							this.p.energy = this.p.energy - deltaTime * 2f;
						}
						if (this.p.energy > 0f)
						{
							this.p.position = this.p.position + new Vector3(x, y, 0f);
							this.p.velocity = Vector3.Reflect(this.p.velocity, this.raycastHit.normal) * this.collisionSpeedM;
						}
						else if (this.createSplashParticles && this.p.size > 1.5f)
						{
							EffectsController.CreateBloodSmallSplashEffect(this.color, this.p.position.x, this.p.position.y, this.p.velocity.x, this.p.velocity.y);
						}
						if (this.createSplashParticles && directionEnum == DirectionEnum.Up)
						{
							EffectsController.CreateBloodSplashEffect(this.color, this.p.position.x, this.p.position.y, this.p.velocity.x, this.p.velocity.y);
							this.p.energy = 0f;
						}
						if (this.p.energy > 0f)
						{
							this.particleList.Add(this.p);
						}
					}
					else if (this.p.size > 0.01f && this.p.energy > 0f)
					{
						this.RunParticle(this.p, deltaTime);
					}
				}
				else if (this.p.size > 0.01f && this.p.energy > 0f)
				{
					this.RunParticle(this.p, deltaTime);
				}
			}
			this.emitter.particles = this.particleList.ToArray();
		}
		if (this.bledTrail)
		{
			this.lastBloodTrailTime = Time.time;
		}
	}

	protected bool ConstrainToColliders(Vector3 position, float size, ref float xIT, ref float yIT, Vector3 collisionPoint, DirectionEnum hitDirection)
	{
		bool result = false;
		if (hitDirection == DirectionEnum.Right)
		{
			if (position.x + xIT - size < collisionPoint.x)
			{
				result = true;
				xIT = collisionPoint.x - position.x + size;
			}
		}
		else if (hitDirection == DirectionEnum.Left && position.x + xIT + size > collisionPoint.x)
		{
			result = true;
			xIT = collisionPoint.x - position.x - size;
		}
		if (hitDirection == DirectionEnum.Up)
		{
			if (position.y + yIT - size < collisionPoint.y)
			{
				result = true;
				yIT = collisionPoint.y - position.y + size;
			}
		}
		else if (hitDirection == DirectionEnum.Down && position.y + yIT + size > collisionPoint.y)
		{
			result = true;
			yIT = collisionPoint.y - position.y - size;
		}
		return result;
	}

	protected void RunParticle(Particle p, float t)
	{
		p.position += p.velocity * t;
		if (this.fadeColor && p.energy < p.startEnergy * 0.25f)
		{
			p.color = new Color(p.color.r, p.color.g, p.color.b, p.energy / (p.startEnergy * 0.25f));
		}
		if (this.fadeSize && p.energy < p.startEnergy * 0.25f)
		{
			p.size -= p.startEnergy * 4f * t;
			if (p.size < 0.3f)
			{
				p.energy = 0f;
			}
		}
		this.particleList.Add(p);
		if (this.trailBlood)
		{
			int num = (int)((Time.time - this.lastBloodTrailTime) / this.bloodTrailFrequency * (p.energy * 0.4f - 1f));
			if (num > 0)
			{
				if (num > 3)
				{
					num = 3;
				}
				this.bledTrail = true;
			}
			for (int i = 0; i < num; i++)
			{
				float num2 = 1f - (float)i / (float)num;
				if (UnityEngine.Random.value < p.startEnergy * 0.3f)
				{
					EffectsController.CreateBloodParticlesDots(this.color, p.position.x - p.velocity.x * num2 * t, p.position.y - p.velocity.y * num2 * t, p.position.z + 0.1f, 1, 0f, 0f, 3f, p.velocity.x * 0.6f, p.velocity.y * 0.6f, 1.5f);
				}
				else
				{
					EffectsController.CreateBloodParticlesDots(this.color, p.position.x - p.velocity.x * num2 * t, p.position.y - p.velocity.y * num2 * t, p.position.z + 0.1f, 1, 0f, 0f, 3f, p.velocity.x * 0.6f, p.velocity.y * 0.6f, 1f);
				}
			}
		}
		if (this.trailDust && p.energy > 2f)
		{
			int num3 = (int)((Time.time - this.lastBloodTrailTime) / 0.0316f * (p.energy * 0.4f - 1f));
			if (num3 > 0)
			{
				if (num3 > 3)
				{
					num3 = 3;
				}
				this.bledTrail = true;
			}
			for (int j = 0; j < num3; j++)
			{
				float num4 = 1f - (float)j / (float)num3;
				EffectsController.CreateDustParticles(p.position.x - p.velocity.x * num4 * t, p.position.y - p.velocity.y * num4 * t, 1, 0f, 6f * p.energy, p.velocity.x * 0.6f, p.velocity.y * 0.6f);
			}
		}
	}

	private ParticleEmitter emitter;

	private Particle[] particles;

	private List<Particle> particleList = new List<Particle>(300);

	private int particleCount;

	private LayerMask groundLayer;

	private RaycastHit raycastHit;

	public bool createSplashParticles = true;

	public bool DestroyParticlesOutsideOfFluid;

	public bool collidesWithGround = true;

	public bool leavesDecals = true;

	public float minEnergyDecal = 0.4f;

	public float collisionEnergyLoss = 0.1f;

	public float collisionSpeedM = 0.35f;

	public float gravity = 20f;

	public int decalEffectSize = 4;

	public float growSizeM = -0.5f;

	public float dampeningM = 2f;

	public bool randomMotion;

	public float randomMotionM = 1f;

	public bool splits;

	public bool fadeColor;

	public bool fadeSize;

	public bool trailBlood;

	public bool trailDust;

	protected float lastBloodTrailTime;

	public float bloodTrailFrequency = 0.0316f;

	protected bool bledTrail;

	public bool resetParticleAngleOnCollision;

	public BloodColor color;

	private Particle p;
}
