// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DoorOfGuns : MonoBehaviour
{
	private void Awake()
	{
		WallOfGuns[] componentsInChildren = base.GetComponentsInChildren<WallOfGuns>();
		if (componentsInChildren.Length > 1)
		{
			foreach (WallOfGuns wallOfGuns in componentsInChildren)
			{
				if (wallOfGuns.openYOffset < 0f)
				{
					this.wallTop = wallOfGuns;
					wallOfGuns.autonomous = false;
					UnityEngine.Debug.Log(" --------------- Found Top Wall ! " + wallOfGuns.name);
				}
				else if (wallOfGuns.openYOffset > 0f)
				{
					this.wallBottom = wallOfGuns;
					wallOfGuns.autonomous = false;
					UnityEngine.Debug.Log(" --------------- Found Bottom Wall   " + wallOfGuns.name);
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogError("THERE AREN'T ENOUGH WALLS ON THE DOOR!");
		}
	}

	protected virtual void Update()
	{
		if (Map.isEditing)
		{
			return;
		}
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.thinkCounter += num;
		if (this.thinkCounter > 1f)
		{
			this.thinkCounter -= 1f;
			this.Think();
		}
	}

	protected virtual void Think()
	{
		if (!this.opened && SortOfFollow.IsItSortOfVisible(base.transform.position.x, base.transform.position.y + this.lookForPlayerYOffset, 10f, 20f) && HeroController.IsPlayerNearby(base.transform.position.x + this.lookForPlayerXOffset, base.transform.position.y + this.lookForPlayerYOffset, this.lookForPlayerXDirection, this.lookForPlayerXRange, this.lookForPlayerYRange))
		{
			this.wallBottom.Activate();
			this.wallTop.Activate();
		}
	}

	protected WallOfGuns wallBottom;

	protected WallOfGuns wallTop;

	private float thinkCounter;

	public float lookForPlayerXOffset = -64f;

	public float lookForPlayerYOffset = 64f;

	public float lookForPlayerXRange = 128f;

	public float lookForPlayerYRange = 128f;

	public int lookForPlayerXDirection = -1;

	protected bool opened;
}
