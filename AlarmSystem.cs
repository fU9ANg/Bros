// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AlarmSystem : MonoBehaviour
{
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteSM>();
	}

	private void Update()
	{
		if (!Map.isEditing && !CutsceneController.isInCutscene)
		{
			this.alarmCounter += Time.deltaTime;
			if (this.alarmCounter > this.alarmRate)
			{
				this.alarmCounter -= this.alarmRate;
				FlashBangExplosion flashBangExplosion = UnityEngine.Object.Instantiate(this.flashBangExplosion, base.transform.position + Vector3.up, Quaternion.identity) as FlashBangExplosion;
				flashBangExplosion.Setup(-1, this, DirectionEnum.Any);
			}
		}
	}

	public FlashBangExplosion flashBangExplosion;

	protected float alarmCounter;

	public float alarmRate = 3f;

	protected bool alarmOn = true;

	protected SpriteSM sprite;

	protected int spriteFrame;
}
