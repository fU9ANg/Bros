// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class Boss : NetworkedUnit
{
	protected override void Awake()
	{
		base.Awake();
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		this.AddFollowObjectsToChildren(this.bossAnimation.transform);
		this.CombineBossPieceClusters();
		this.AssignAnimations();
		this.AssignBossPieceIds();
	}

	protected virtual void Start()
	{
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
	}

	public override bool IsLocalMook
	{
		get
		{
			return Connect.IsHost;
		}
	}

	protected void AssignBossPieceIds()
	{
	}

	protected virtual void AssignAnimations()
	{
		this.animationEngine = this.bossAnimation.gameObject.GetComponent<BossAnimationEngine>();
	}

	protected virtual void Update()
	{
		if (Map.isEditing || CutsceneController.isInCutscene)
		{
			return;
		}
		this.t = Time.deltaTime;
		this.thinkCounter -= this.t;
		if (this.thinkCounter <= 0f && base.IsMine)
		{
			this.Think();
			byte[] arg = this.Serialize();
			Networking.RPC<byte[]>(PID.TargetOthers, new RpcSignature<byte[]>(this.Deserialize), arg, true);
		}
	}

	protected override void LateUpdate()
	{
		base.LateUpdate();
		this.RunAnimationCues();
	}

	protected virtual void RunAnimationCues()
	{
	}

	protected virtual void Think()
	{
		int num = this.thinkState;
	}

	protected virtual void RunStanding()
	{
		this.GetGroundHeight();
		if (this.y > this.groundHeight + 1f && this.settled)
		{
			this.settled = false;
			this.shakeCounter = 0.3f;
		}
		if (!this.settled)
		{
			if (this.shakeCounter > 0f)
			{
				this.shakeCounter -= this.t;
				this.SetPosition(global::Math.Sin(this.shakeCounter * 60f) * 2f);
			}
			else
			{
				this.yI -= 800f * this.t;
				float num = this.yI * this.t;
				if (this.y + num < this.groundHeight)
				{
					if (this.yI < -200f)
					{
						this.yI = 0f;
						this.CrushGround();
						Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.hitSounds, 0.6f, base.transform.position);
					}
					else
					{
						this.y = this.groundHeight;
						this.settled = true;
						this.yI = 0f;
					}
				}
				else
				{
					this.y += num;
				}
				this.SetPosition(0f);
				this.SquashUnits();
			}
		}
		else
		{
			this.SetPosition(0f);
		}
	}

	protected virtual void RunFlying(float targetY, float targetX)
	{
		float num = targetX - this.x;
		float num2 = targetY - this.y;
		if (num2 > 128f && Mathf.Abs(this.yI) < 80f)
		{
			this.yI += num2 * this.t * 3f;
		}
		if (Mathf.Abs(this.yI) < 50f)
		{
			this.yI += num2 * this.t * 8f;
		}
		else
		{
			this.yI += num2 * this.t * 3f;
		}
		this.yI *= 1f - this.t * 4f;
		if (Mathf.Abs(num) > 128f && Mathf.Abs(this.xI) < 120f)
		{
			this.xI += num * this.t * 3f;
		}
		if (Mathf.Abs(this.xI) < 60f)
		{
			this.xI += num * this.t * 4f;
		}
		else
		{
			this.xI += num * this.t * 2f;
		}
		this.xI *= 1f - this.t * 3f;
		this.y += this.yI * this.t;
		this.x += this.xI * this.t;
		this.shakeCounter -= this.t;
		this.SetPosition(global::Math.Sin(this.shakeCounter * 20f) * 2f);
		if (Mathf.Abs(num) < 25f && Mathf.Abs(num2) < 25f && this.thinkCounter > 0.6f)
		{
			this.thinkCounter = 0.6f;
		}
	}

	protected virtual bool CheckGroundHeight(float xOffset, float yOffset, ref float groundHeight, ref RaycastHit rayCastHit)
	{
		if (Physics.Raycast(new Vector3(this.x + xOffset, this.y + yOffset, 0f), Vector3.down, out rayCastHit, 64f, this.groundLayer))
		{
			if (rayCastHit.point.y > groundHeight)
			{
				groundHeight = rayCastHit.point.y;
			}
			if (rayCastHit.point.y > this.y - 9f)
			{
				return true;
			}
		}
		return false;
	}

	protected virtual void SquashUnits()
	{
		if (Map.HitUnits(this, 20, DamageType.Crush, this.width / 2f, 2f, this.x, this.y - 8f, 0f, this.yI, true, false))
		{
		}
	}

	protected virtual void GetGroundHeight()
	{
		this.groundHeight = -200f;
		this.groundChecks = new bool[this.rayCastHits.Length];
		for (int i = 0; i < this.groundChecks.Length; i++)
		{
			this.groundChecks[i] = this.CheckGroundHeight((float)(this.groundChecks.Length - 1) * 0.5f * -16f + (float)(i * 16), 6f, ref this.groundHeight, ref this.rayCastHits[i]);
		}
		this.CrushGround(this.groundChecks, this.rayCastHits);
	}

	protected virtual void CrushGround(bool[] groundChecks, RaycastHit[] hits)
	{
		if (!this.IsAcceptableGround(groundChecks))
		{
			for (int i = 0; i < groundChecks.Length; i++)
			{
				if (groundChecks[i])
				{
					hits[i].collider.gameObject.SendMessage("Damage", new DamageObject(5, DamageType.Crush, 0f, 0f, null), SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	protected virtual void CrushGround()
	{
		for (int i = 0; i < this.groundChecks.Length; i++)
		{
			if (this.groundChecks[i])
			{
				this.rayCastHits[i].collider.gameObject.SendMessage("Damage", new DamageObject(5, DamageType.Crush, 0f, 0f, null), SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	protected virtual bool IsAcceptableGround(bool[] groundChecks)
	{
		return groundChecks[2] && groundChecks[3];
	}

	protected virtual void SetPosition(float xOffset)
	{
		base.transform.position = new Vector3(this.x + xOffset, this.y, 0f);
	}

	protected virtual void CombineBossPieceClusters()
	{
		for (int i = 0; i < this.bossPieceClusters.Length; i++)
		{
		}
	}

	protected virtual void AddFollowObjectsToChildren(Transform baseTransform)
	{
		Transform[] componentsInChildren = base.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].GetComponent<Renderer>() != null)
			{
				GameObject gameObject = new GameObject(componentsInChildren[i].name + " Boss Piece");
				MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
				meshFilter.sharedMesh = componentsInChildren[i].GetComponent<MeshFilter>().sharedMesh;
				MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
				meshRenderer.sharedMaterials = componentsInChildren[i].GetComponent<Renderer>().sharedMaterials;
				gameObject.transform.parent = base.transform;
				gameObject.layer = componentsInChildren[i].gameObject.layer;
				gameObject.tag = componentsInChildren[i].gameObject.tag;
				BossPiece bossPiece = gameObject.AddComponent<BossPiece>();
				if (componentsInChildren[i].GetComponent<Renderer>().sharedMaterial == this.mainMaterialReference)
				{
					bossPiece.hurtMaterial = this.hurtMainMaterial;
					bossPiece.damagedMaterial = this.damagedMainMaterial;
					bossPiece.damagedHurtMaterial = this.damagedHurtMainMaterial;
					bossPiece.deadMaterial = this.deadMainMaterial;
				}
				else if (componentsInChildren[i].GetComponent<Renderer>().sharedMaterial == this.otherMaterialReference)
				{
					bossPiece.hurtMaterial = this.hurtOtherMaterial;
					bossPiece.damagedMaterial = this.damagedOtherMaterial;
					bossPiece.damagedHurtMaterial = this.damagedHurtOtherMaterial;
					bossPiece.deadMaterial = this.deadOtherMaterial;
				}
				else
				{
					UnityEngine.Debug.LogWarning("Cannot determine Hurt Material... guessing)");
					bossPiece.hurtMaterial = this.hurtMainMaterial;
					bossPiece.damagedMaterial = this.damagedMainMaterial;
					bossPiece.damagedHurtMaterial = this.damagedHurtMainMaterial;
					bossPiece.deadMaterial = this.deadMainMaterial;
				}
				bossPiece.parentTransform = componentsInChildren[i];
				componentsInChildren[i].GetComponent<Renderer>().enabled = false;
				gameObject.transform.rotation = componentsInChildren[i].rotation;
				if (componentsInChildren[i].GetComponent<Collider>() != null)
				{
					BoxCollider component = componentsInChildren[i].GetComponent<BoxCollider>();
					BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
					boxCollider.center = component.center;
					boxCollider.size = component.size;
					componentsInChildren[i].GetComponent<Collider>().enabled = false;
				}
				this.bossPieces.Add(bossPiece);
			}
		}
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (componentsInChildren[j].GetComponent<Renderer>() == null && componentsInChildren[j].GetComponent<Collider>() != null)
			{
				componentsInChildren[j].GetComponent<Collider>().enabled = false;
			}
		}
		for (int k = 0; k < this.bossPieceClusters.Length; k++)
		{
			List<BossPiece> list = new List<BossPiece>();
			for (int l = 0; l < this.bossPieces.Count; l++)
			{
				if (this.bossPieces[l].parentTransform.IsChildOf(this.bossPieceClusters[k]) || this.bossPieces[l].parentTransform == this.bossPieceClusters[k])
				{
					list.Add(this.bossPieces[l]);
				}
			}
			for (int m = 0; m < list.Count; m++)
			{
				list[m].connectedPieces = list;
			}
			UnityEngine.Debug.Log("connected pieces count " + list.Count);
		}
	}

	public virtual byte[] Serialize()
	{
		UnityStream unityStream = new UnityStream();
		return unityStream.ByteArray;
	}

	public virtual void Deserialize(byte[] byteStream)
	{
	}

	public Animation bossAnimation;

	protected float width = 80f;

	protected int thinkState;

	protected float thinkCounter = 2f;

	protected BossTankAnimationCue animationCue;

	public Material mainMaterialReference;

	public Material hurtMainMaterial;

	public Material damagedMainMaterial;

	public Material damagedHurtMainMaterial;

	public Material otherMaterialReference;

	public Material hurtOtherMaterial;

	public Material damagedOtherMaterial;

	public Material damagedHurtOtherMaterial;

	public Material deadMainMaterial;

	public Material deadOtherMaterial;

	protected BossAnimationEngine animationEngine;

	public SoundHolder soundHolder;

	public Transform[] bossPieceClusters;

	protected List<BossPiece> bossPieces = new List<BossPiece>();

	protected float t = 0.03f;

	protected float groundHeight;

	protected LayerMask groundLayer;

	protected RaycastHit[] rayCastHits;

	public bool[] groundChecks;

	protected bool settled = true;

	public float shakeCounter;
}
