// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SuperCheckpoint : CheckPoint
{
	public override void ActivateInternal()
	{
		if (this.activated)
		{
			return;
		}
		HeroController.GiveAllLifelessPlayersALife();
		base.ActivateInternal();
		if (!LevelEditorGUI.levelEditorActive)
		{
			Map.startFromSuperCheckPoint = true;
			if (this.horizontal)
			{
				Map.nextXLoadOffset = Map.GetCollumn(this.x) + Map.lastXLoadOffset - 6;
				if (Map.nextXLoadOffset < 0)
				{
					Map.nextXLoadOffset = 0;
				}
			}
			if (this.vertical)
			{
				Map.nextYLoadOffset = Map.GetRow(this.y) + Map.lastYLoadOffset - 5;
				if (Map.nextYLoadOffset < 0)
				{
					Map.nextYLoadOffset = 0;
				}
			}
			Map.superCheckpointStartPos.c = Map.GetCollumn(this.x) + Map.lastXLoadOffset - Map.nextXLoadOffset;
			Map.superCheckpointStartPos.r = Map.GetCollumn(this.y) + Map.lastYLoadOffset - Map.nextYLoadOffset;
			StatisticsController.CacheStats();
		}
	}

	protected override void Start()
	{
		base.Start();
		LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
		RaycastHit raycastHit = default(RaycastHit);
		if (Physics.Raycast(new Vector3(this.x - 32f, this.y + 5f, 0f), Vector3.down, out raycastHit, 16f, mask))
		{
			raycastHit.collider.GetComponent<Block>().replaceOnCollapse = true;
			raycastHit.collider.GetComponent<Block>().replacementBlockType = GroundType.Steel;
		}
		if (Physics.Raycast(new Vector3(this.x + 32f, this.y + 5f, 0f), Vector3.down, out raycastHit, 16f, mask))
		{
			raycastHit.collider.GetComponent<Block>().replaceOnCollapse = true;
			raycastHit.collider.GetComponent<Block>().replacementBlockType = GroundType.Steel;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.activated && (this.lightblinkDelay -= Time.deltaTime) < 0f)
		{
			if (base.GetComponent<SpriteSM>().lowerLeftPixel.x == 0f)
			{
				base.GetComponent<SpriteSM>().SetLowerLeftPixel(64f, 143f);
			}
			else
			{
				base.GetComponent<SpriteSM>().SetLowerLeftPixel(0f, 143f);
			}
			this.lightblinkDelay = 0.5f;
		}
	}

	public bool horizontal;

	public bool vertical;

	private float lightblinkDelay;
}
