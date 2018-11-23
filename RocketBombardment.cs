// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class RocketBombardment : Rocket
{
	public override void Fire(float x, float y, float xI, float yI, int playerNum, MonoBehaviour FiredBy)
	{
		base.Fire(x, y, xI, yI, playerNum, FiredBy);
		this.line.SetVertexCount(2);
		this.lineOrigin = new Vector3(x, y, 0f);
		this.line.SetPosition(0, this.lineOrigin);
		this.line.SetPosition(1, new Vector3(x + xI * 0.001f, y + yI * 0.001f, 0f));
		this.lineTarget = new Vector3(xI * 4f, yI * 4f, 0f);
		this.cross.gameObject.SetActive(false);
	}

	public override void SetPosition()
	{
		base.transform.position = new Vector3(this.x, this.y, this.z + this.zOffset);
	}

	protected override void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime * 0.33f, 0f, 0.0334f);
		this.RunProjectile(this.t);
		this.RunProjectile(this.t);
		this.RunProjectile(this.t);
		this.RunSmokeTrail(this.t * 3f);
		this.life -= this.t * 3f;
		if (this.life <= 0f)
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
			this.DeregisterProjectile();
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.RunLine();
	}

	protected void RunLine()
	{
		if (this.runningLine)
		{
			this.lineM += this.t * 2f;
			Vector3 b = new Vector3(this.lineTarget.x * this.lineM, this.lineTarget.y * this.lineM, -20f);
			this.line.SetPosition(1, this.lineOrigin + b);
			if (this.lineM > 1.5f)
			{
				this.runningLine = false;
				this.lineM = 1f;
			}
			float magnitude = b.magnitude;
			if (Physics.Raycast(this.lineOrigin, this.lineTarget, magnitude, this.groundLayer))
			{
				this.runningLine = false;
				this.lineM = this.lineFadeTime * 3f;
				this.cross.transform.parent = null;
				this.cross.transform.position = this.lineOrigin + b;
				this.cross.transform.eulerAngles = Vector3.zero;
				this.cross.gameObject.SetActive(true);
			}
		}
		else
		{
			this.lineM = Mathf.Clamp(this.lineM - this.t * 3f, 0f, 3f);
			this.line.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, this.lineM * 0.5f));
		}
	}

	public override void Death()
	{
		base.Death();
	}

	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 point)
	{
		if (this.hasMadeEffects)
		{
			return;
		}
		this.cross.GoAway();
		base.MakeEffects(particles, x, y, useRayCast, this.raycastHit.normal, this.raycastHit.point);
		Networking.RPC<float, float, float, int>(PID.TargetAll, new RpcSignature<float, float, float, int>(MapController.BurnGround_Local), this.range * 1.25f, x, y, this.groundLayer, false);
		Map.HitUnits(this.firedBy, 1, DamageType.Fire, this.range * 1.2f, x, y, 0f, 100f, true, false);
		EffectsController.CreatePlumes(x, y, 3, 8f, 270f, 0f, 0f);
	}

	public LineRenderer line;

	public AnimatedIcon cross;

	protected Vector3 lineTarget;

	protected Vector3 lineOrigin;

	protected float lineM;

	protected bool runningLine = true;

	public float lineFadeTime = 0.3333f;
}
