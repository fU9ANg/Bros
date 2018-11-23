// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CheckPoint : BroforceObject
{
	protected virtual void Awake()
	{
		Map.RegisterCheckPoint(this);
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
	}

	protected virtual void Start()
	{
		LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
		RaycastHit raycastHit = default(RaycastHit);
		if (Physics.Raycast(new Vector3(this.x, this.y + 5f, 0f), Vector3.down, out raycastHit, 16f, mask) && raycastHit.collider.GetComponent<Block>().groundType != GroundType.Cage)
		{
			raycastHit.collider.GetComponent<Block>().replaceOnCollapse = true;
			raycastHit.collider.GetComponent<Block>().replacementBlockType = GroundType.Steel;
		}
		if (Physics.Raycast(new Vector3(this.x - 16f, this.y + 5f, 0f), Vector3.down, out raycastHit, 16f, mask) && raycastHit.collider.GetComponent<Block>().groundType != GroundType.Cage)
		{
			raycastHit.collider.GetComponent<Block>().replaceOnCollapse = true;
			raycastHit.collider.GetComponent<Block>().replacementBlockType = GroundType.Steel;
		}
		if (Physics.Raycast(new Vector3(this.x + 16f, this.y + 5f, 0f), Vector3.down, out raycastHit, 16f, mask) && raycastHit.collider.GetComponent<Block>().groundType != GroundType.Cage)
		{
			raycastHit.collider.GetComponent<Block>().replaceOnCollapse = true;
			raycastHit.collider.GetComponent<Block>().replacementBlockType = GroundType.Steel;
		}
		this.flag.gameObject.SetActive(false);
		this.blockingUnit = Map.GetNearestUnitWithXBias(-1, 64, this.x, this.y, false);
		if (this.blockingUnit != null && this.blockingUnit.IsEvil())
		{
			this.isFinal = true;
		}
	}

	protected virtual void Update()
	{
		if (this.deactivateDelay > 0f)
		{
			this.deactivateDelay -= Time.deltaTime;
			if (this.deactivateDelay <= 0f)
			{
				this.activated = false;
			}
		}
	}

	public void DelayDeactivate(float time)
	{
		this.deactivateDelay = time;
	}

	public bool IsBlockedByUnit()
	{
		return !(this.blockingUnit == null) && (this.blockingUnit.IsEvil() && this.blockingUnit.health > 0 && Map.IsUnitInRange(this.blockingUnit, 96, this.x, this.y));
	}

	protected override void OnDestroy()
	{
		LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
		RaycastHit raycastHit = default(RaycastHit);
		if (Physics.Raycast(new Vector3(this.x, this.y + 5f, 0f), Vector3.down, out raycastHit, 16f, mask))
		{
			raycastHit.collider.GetComponent<Block>().replaceOnCollapse = false;
		}
		if (Physics.Raycast(new Vector3(this.x - 16f, this.y + 5f, 0f), Vector3.down, out raycastHit, 16f, mask))
		{
			raycastHit.collider.GetComponent<Block>().replaceOnCollapse = false;
		}
		if (Physics.Raycast(new Vector3(this.x + 16f, this.y + 5f, 0f), Vector3.down, out raycastHit, 16f, mask))
		{
			raycastHit.collider.GetComponent<Block>().replaceOnCollapse = false;
		}
	}

	public virtual void ActivateInternal()
	{
		if (this.activated)
		{
			return;
		}
		StatisticsController.NotifyCaptureCheckPoint();
		this.activated = true;
		this.flag.gameObject.SetActive(true);
		Sound instance = Sound.GetInstance();
		if (instance != null)
		{
			instance.PlaySoundEffectAt(this.yeahClips, this.yeahVolume, base.transform.position, 1f, false);
		}
		HeroController.SetCheckPoint(new Vector2(base.transform.position.x, base.transform.position.y + 16f));
		if (this.isFinal)
		{
			Networking.RPC<Vector2, float>(PID.TargetAll, new RpcSignature<Vector2, float>(Map.newestHelicopter.Enter), new Vector2(this.x, this.y), 0f, false);
		}
	}

	public override UnityStream PackState(UnityStream stream)
	{
		stream.Serialize<bool>(this.activated);
		return base.PackState(stream);
	}

	public override UnityStream UnpackState(UnityStream stream)
	{
		this.activated = (bool)stream.DeserializeNext();
		if (this.activated)
		{
			this.flag.gameObject.SetActive(true);
			if (this.isFinal)
			{
				Map.newestHelicopter.Enter(new Vector2(this.x, this.y), 0f);
			}
		}
		return base.UnpackState(stream);
	}

	public FlagFlap flag;

	public bool activated;

	protected Unit blockingUnit;

	public float yeahVolume = 0.5f;

	public AudioClip[] yeahClips;

	protected bool isFinal;

	protected float deactivateDelay;
}
