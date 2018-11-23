// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class CompetitivePunchingFace : MonoBehaviour
{
	private void Awake()
	{
		this.faceSprite = base.GetComponent<SpriteSM>();
		this.spriteWidth = this.faceSprite.pixelDimensions.x; this.spritePixelWidth = (this.spriteWidth );
		this.maxHealth = this.health;
		this.bloodFountainAnim.gameObject.SetActive(false);
		for (int i = 0; i < this.bloodStreams.Length; i++)
		{
			this.bloodStreams[i].gameObject.SetActive(false);
		}
	}

	private void Start()
	{
		this.goreNeckBack.gameObject.SetActive(false);
	}

	public float GetProgress()
	{
		return 1f - (float)this.health / (float)this.maxHealth;
	}

	public bool IsDead
	{
		get
		{
			return this.isDead;
		}
	}

	public bool FaceSmashedOff
	{
		get
		{
			return this.faceSmashedOff;
		}
	}

	protected Vector3 GetXOffset(DirectionEnum direction)
	{
		if (direction == DirectionEnum.Right)
		{
			return Vector3.left * 40f;
		}
		if (direction == DirectionEnum.Left)
		{
			return Vector3.right * 40f;
		}
		return Vector3.zero;
	}

	protected Vector3 GetForce(DirectionEnum direction)
	{
		if (direction == DirectionEnum.Right)
		{
			return Vector3.right * 340f;
		}
		if (direction == DirectionEnum.Left)
		{
			return Vector3.left * 340f;
		}
		if (direction == DirectionEnum.Up)
		{
			return Vector3.up * 340f;
		}
		if (direction == DirectionEnum.Down)
		{
			return Vector3.down * 340f;
		}
		return Vector3.zero;
	}

	public bool HeadMustExplode(int Health)
	{
		return (float)Health < (float)(-(float)this.maxHealth) * 0.4f;
	}

	public bool HeadExplodesOnNextBlow()
	{
		return this.HeadMustExplode(this.health - this.damagePerPunch);
	}

	public void Punch(DirectionEnum direction, float x, float y)
	{
		if (!this.isDead)
		{
			Vector3 force = this.GetForce(direction);
			this.health -= this.damagePerPunch;
			if (this.HeadMustExplode(this.health))
			{
				this.decapitatedM = 1f;
				this.isDead = true;
				base.GetComponent<Renderer>().sharedMaterial = this.materialDeadState;
				CutsceneEffectsController.CreateBloodParticles(base.transform.position.x, base.transform.position.y, 520, 80f, 160f, 640f, force.x * 0.3f, force.y * 0.3f + 350f);
				EffectsController.CreateGibs(this.deathGibs, base.transform.position.x, base.transform.position.y, 170f, 120f, force.x * 0.4f, force.y * 0.4f + 240f);
				CutsceneSound.PlaySoundEffectAt(this.soundHolder.deathSounds, 0.7f, base.transform.position, 1f);
				CutsceneSound.PlaySoundEffectAt(this.ElectroExplode, 0.7f, base.transform.position, 1f);
				CutsceneSound.PlaySoundEffectAt(this.sparks, 0.7f, base.transform.position, 1f);
				this.bloodFountainAnim.Restart();
				this.frame = 0;
				this.goreNeckBack.gameObject.SetActive(true);
				for (int i = 0; i < this.bloodStreams.Length; i++)
				{
					this.bloodStreams[i].gameObject.SetActive(false);
				}
				UnityEngine.Debug.Log("Flip Gore Neck punch " + direction);
				this.goreNeckBack.SetSize(this.spriteWidth, this.faceSprite.height);
				this.faceSprite.SetLowerLeftPixel(0f, (float)((int)this.faceSprite.lowerLeftPixel.y));
			}
			else if ((float)this.health <= 0f)
			{
				if (!this.faceSmashedOff)
				{
					this.faceSmashedOff = true;
					GibHolder gibs = this.smashFaceOffAboveBelowGibs;
					if (direction == DirectionEnum.Right)
					{
						gibs = this.smashFaceOffToTheRightGibs;
					}
					else if (direction == DirectionEnum.Left)
					{
						gibs = this.smashFaceOffToTheLeftGibs;
					}
					float num = Mathf.Sign(force.x);
					float num2 = Mathf.Sign(force.y);
					EffectsController.CreateGibs(gibs, base.transform.position.x, base.transform.position.y, 120f, 40f, force.x * 0.4f, force.y * 0.4f);
					CutsceneSound.PlaySoundEffectAt(this.soundHolder.deathSounds, 0.7f, base.transform.position, 1f);
				}
				CutsceneEffectsController.CreateBloodParticles(x, y, 20, 30f, 30f, 80f, force.x, force.y);
				EffectsController.CreateGibs(this.hurtGibs, 2, base.transform.position.x, base.transform.position.y, 100f, 100f, force.x, force.y + 140f, (int)Mathf.Sign(force.x));
				base.GetComponent<Renderer>().sharedMaterial = this.materialCriticalState;
				CutsceneSound.PlaySoundEffectAt(this.soundHolder.confused, 0.5f + UnityEngine.Random.value * 0.2f, base.transform.position + this.GetXOffset(direction), 0.9f + UnityEngine.Random.value * 0.2f);
			}
			else if ((float)this.health < (float)this.maxHealth * 0.4f)
			{
				CutsceneEffectsController.CreateBloodParticles(x, y, 20, 30f, 30f, 80f, force.x, force.y);
				EffectsController.CreateGibs(this.hurtGibs, 2, base.transform.position.x, base.transform.position.y, 100f, 100f, force.x, force.y + 140f, (int)Mathf.Sign(force.x));
				base.GetComponent<Renderer>().sharedMaterial = this.materialHurtState;
				CutsceneSound.PlaySoundEffectAt(this.soundHolder.effortSounds, 0.5f + UnityEngine.Random.value * 0.2f, base.transform.position + this.GetXOffset(direction), 0.9f + UnityEngine.Random.value * 0.2f);
			}
			else if ((float)this.health < (float)this.maxHealth * 0.85f)
			{
				CutsceneEffectsController.CreateBloodParticles(x, y, 20, 30f, 30f, 80f, force.x, force.y);
				EffectsController.CreateGibs(this.hurtGibs, 1, base.transform.position.x, base.transform.position.y, 100f, 100f, force.x, force.y + 140f, (int)Mathf.Sign(force.x));
				base.GetComponent<Renderer>().sharedMaterial = this.materialRuffledState;
				CutsceneSound.PlaySoundEffectAt(this.soundHolder.effortSounds, 0.5f + UnityEngine.Random.value * 0.2f, base.transform.position + this.GetXOffset(direction), 0.9f + UnityEngine.Random.value * 0.2f);
			}
			else
			{
				CutsceneEffectsController.CreateBloodParticles(x, y, 20, 30f, 30f, 80f, force.x, force.y);
				EffectsController.CreateGibs(this.hurtGibs, 1, base.transform.position.x, base.transform.position.y, 100f, 100f, force.x, force.y + 140f, (int)Mathf.Sign(force.x));
				CutsceneSound.PlaySoundEffectAt(this.soundHolder.confused, 0.5f + UnityEngine.Random.value * 0.2f, base.transform.position + this.GetXOffset(direction), 0.9f + UnityEngine.Random.value * 0.2f);
			}
			this.hurtGlowCounter = 1f;
			this.frameCounter = 0f;
			if (!this.isDead)
			{
				this.punchedDirection = direction;
				this.frame = 1;
			}
			else
			{
				this.punchedDirection = direction;
				this.frame = 0;
			}
			if (!this.isDead)
			{
				this.SetHurtFrame(this.punchedDirection);
			}
		}
	}

	private IEnumerator SlowRoutine()
	{
		Time.timeScale = 0.1f;
		float stamp = Time.realtimeSinceStartup;
		while (stamp + 2f > Time.realtimeSinceStartup)
		{
			yield return null;
		}
		Time.timeScale = 1f;
		yield break;
	}

	protected void RunBloodFountain(float t)
	{
		if (this.isDead)
		{
			this.decapitatedM -= 0.1f * t;
			if (this.decapitatedM > 0f)
			{
				this.bloodSpurtCounter -= t;
				if (this.bloodSpurtCounter < 0f)
				{
					this.veinNozzleTransformIndex++;
					if (UnityEngine.Random.value > 0.5f && this.veinNozzleTransforms.Length > 2)
					{
						this.veinNozzleTransformIndex++;
					}
					if (UnityEngine.Random.value > 0.5f && this.veinNozzleTransforms.Length > 4)
					{
						this.veinNozzleTransformIndex++;
					}
					if (this.bloodStreams[this.veinNozzleTransformIndex % this.bloodStreams.Length].gameObject.activeInHierarchy)
					{
						this.veinNozzleTransformIndex++;
					}
					if (this.bloodStreams[this.veinNozzleTransformIndex % this.bloodStreams.Length].gameObject.activeInHierarchy)
					{
						this.veinNozzleTransformIndex++;
					}
					this.bloodSpurtCounter += (UnityEngine.Random.value * 1f + 1.1f) * (1.23f - this.decapitatedM);
					Vector3 position = this.veinNozzleTransforms[this.veinNozzleTransformIndex % this.veinNozzleTransforms.Length].transform.position;
					this.bloodSpurtX = position.x;
					this.bloodSpurtY = position.y;
					this.bloodSpurtXI = this.bloodSpurtX * 1.3f;
					this.bloodSpurtTime = (UnityEngine.Random.value + 0.5f) * this.decapitatedM;
					if (!this.bloodStreams[this.veinNozzleTransformIndex % this.bloodStreams.Length].gameObject.activeInHierarchy)
					{
						this.bloodStreams[this.veinNozzleTransformIndex % this.bloodStreams.Length].Restart(this.bloodSpurtTime + 0.5f, 220f * (0.5f + this.decapitatedM * 0.84f), 11f);
					}
					this.bloodSpurtTime = -1f;
				}
			}
			if (this.bloodSpurtTime > 0f)
			{
				this.bloodSpurtTime -= t;
				this.bloodSpurtSprayCounter += t;
				if (this.bloodSpurtSprayCounter > 0.033f)
				{
					this.bloodSpurtSprayCounter -= 0.033f;
				}
			}
		}
	}

	protected void SetHurtFrame(DirectionEnum direction)
	{
		int num = 0;
		switch (this.frame)
		{
		case 1:
			num = 0;
			break;
		case 2:
			num = 1;
			break;
		case 3:
			num = 0;
			break;
		}
		if (!this.finalFrameSet)
		{
			switch (direction)
			{
			case DirectionEnum.None:
				this.faceSprite.SetLowerLeftPixel(0f, (float)((int)this.faceSprite.lowerLeftPixel.y));
				break;
			case DirectionEnum.Up:
				this.faceSprite.SetLowerLeftPixel((float)((int)(this.spriteWidth * (float)(4 + num))), (float)((int)this.faceSprite.lowerLeftPixel.y));
				break;
			case DirectionEnum.Down:
				this.faceSprite.SetLowerLeftPixel((float)((int)(this.spriteWidth * (float)(6 + num))), (float)((int)this.faceSprite.lowerLeftPixel.y));
				break;
			case DirectionEnum.Left:
				this.faceSprite.SetLowerLeftPixel((float)((int)(this.spriteWidth * (float)(8 + num))), (float)((int)this.faceSprite.lowerLeftPixel.y));
				break;
			case DirectionEnum.Right:
				this.faceSprite.SetLowerLeftPixel((float)((int)(this.spriteWidth * (float)(2 + num))), (float)((int)this.faceSprite.lowerLeftPixel.y));
				break;
			}
			if (this.isDead)
			{
				this.faceSprite.SetLowerLeftPixel(0f, (float)((int)this.faceSprite.lowerLeftPixel.y));
				UnityEngine.Debug.Log("faceSprite" + this.faceSprite.lowerLeftPixel);
				this.finalFrameSet = true;
			}
		}
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		if (this.punchedDirection != DirectionEnum.None)
		{
			this.frameCounter += deltaTime;
			if (this.frameCounter > 0.067f)
			{
				this.frame++;
				if (this.frame > 3)
				{
					this.punchedDirection = DirectionEnum.None;
					this.frame = 0;
				}
				this.frameCounter -= 0.067f;
				this.SetHurtFrame(this.punchedDirection);
			}
		}
		this.RunBloodFountain(deltaTime);
		if (this.hurtGlowCounter > 0f)
		{
			this.faceSprite.SetColor(new Color(1f, 1f - this.hurtGlowCounter, 1f - this.hurtGlowCounter, 1f));
			this.hurtGlowCounter -= deltaTime * 8f;
			if (this.hurtGlowCounter <= 0f)
			{
				this.hurtGlowCounter = 0f;
				this.faceSprite.SetColor(new Color(1f, 1f - this.hurtGlowCounter, 1f - this.hurtGlowCounter, 1f));
			}
		}
	}

	protected SpriteSM faceSprite;

	protected float spritePixelWidth = 128f;

	protected float spriteWidth = 128f;

	protected float frameCounter;

	protected int frame;

	protected float hurtGlowCounter;

	protected DirectionEnum punchedDirection;

	public SoundHolder soundHolder;

	public Material materialRuffledState;

	public Material materialHurtState;

	public Material materialCriticalState;

	public Material materialDeadState;

	protected bool isDead;

	protected bool faceSmashedOff;

	protected bool finalFrameSet;

	public int health = 50;

	protected int maxHealth = 50;

	public GibHolder hurtGibs;

	public GibHolder deathGibs;

	public GibHolder smashFaceOffToTheRightGibs;

	public GibHolder smashFaceOffToTheLeftGibs;

	public GibHolder smashFaceOffAboveBelowGibs;

	protected float decapitatedM;

	protected float bloodSpurtCounter;

	protected float bloodSpurtX;

	protected float bloodSpurtY;

	protected float bloodSpurtTime;

	protected float bloodSpurtXI;

	protected float bloodSpurtSprayCounter;

	public Transform[] sparkTransforms;

	public Transform[] veinNozzleTransforms;

	protected int veinNozzleTransformIndex = -1;

	public BloodStream[] bloodStreams;

	public SpriteSM goreNeckBack;

	public AudioClip ElectroExplode;

	public AudioClip sparks;

	private int damagePerPunch = 4;

	public AnimatedTextureComplicated bloodFountainAnim;

	protected enum HealthState
	{
		Healthy,
		Ruffled,
		Hurt,
		CriticallyWounded,
		Dead,
		Exploded
	}
}
