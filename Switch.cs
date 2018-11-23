// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Switch : Doodad
{
	protected override void Start()
	{
		base.Start();
		LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
		RaycastHit raycastHit = default(RaycastHit);
		if (Physics.Raycast(new Vector3(this.x, this.y + 5f, 0f), Vector3.down, out raycastHit, 16f, mask))
		{
			raycastHit.collider.GetComponent<Block>().replaceOnCollapse = true;
			raycastHit.collider.GetComponent<Block>().replacementBlockType = GroundType.Steel;
		}
		this.triggerInfo.type = TriggerType.Entity;
		this.triggerInfo.name = string.Concat(new object[]
		{
			"Switch at ",
			this.collumn,
			", ",
			this.row
		});
		this.triggerInfo.bottomLeft = (this.triggerInfo.upperRight = new GridPoint(this.collumn, this.row));
		Map.MapData.entityTriggers.Add(this.triggerInfo);
		this.ResetTrigger();
		Map.RegisterSwitch(this);
	}

	public void ResetTrigger()
	{
		this.trigger = TriggerManager.RegisterEntityTrigger(this.triggerInfo);
	}

	private void Update()
	{
		if (this.hasBeenActivated && (this.resetDelay -= Time.deltaTime) < 0f)
		{
			this.hasBeenActivated = false;
			base.GetComponent<SpriteSM>().SetLowerLeftPixel(0f, 16f);
			this.trigger.Reset();
		}
	}

	public virtual void Activate(Unit activatingUnit)
	{
		if (!this.hasBeenActivated)
		{
			this.trigger.triggerNextFrame = true;
			Sound.GetInstance().PlaySoundEffectAt(this.soundEffect, 0.8f, base.transform.position);
			base.GetComponent<SpriteSM>().SetLowerLeftPixel(16f, 16f);
			base.GetComponent<SpriteSM>().UpdateUVs();
			this.hasBeenActivated = true;
			this.resetDelay = 0.5f;
		}
	}

	protected override void AttachDoodad()
	{
	}

	public override void Collapse()
	{
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Map.MapData != null && Map.MapData.entityTriggers != null)
		{
			Map.MapData.entityTriggers.Remove(this.triggerInfo);
		}
	}

	[HideInInspector]
	public TriggerInfo triggerInfo;

	protected Trigger trigger;

	public AudioClip soundEffect;

	public bool hasBeenActivated;

	public bool repeatable;

	private float resetDelay;
}
