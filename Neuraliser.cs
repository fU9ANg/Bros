// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Neuraliser : MonoBehaviour
{
	private void Awake()
	{
		this.line = base.GetComponent<LineRenderer>();
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
	}

	private void Start()
	{
		this.grownToPos = base.transform.position; this.startPos = (this.grownToPos );
		this.grownToPos.z = -15f; this.startPos.z = (this.grownToPos.z );
		this.directionVect = new Vector3((float)this.direction, 0f, 0f);
		Map.BlindUnits(this.playerNum, this.startPos.x, this.startPos.y, 16f);
		this.growDelay = 0.02f;
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.033334f);
		if (this.growing)
		{
			this.growDelay += num;
			int num2 = (int)(this.growDelay / 0.016f);
			if (num2 > 0)
			{
				this.growDelay -= (float)num2 * 0.02f;
				this.colorCounter++;
				this.line.SetColors((this.colorCounter % 2 != 0) ? Color.black : Color.white, (this.colorCounter % 2 != 0) ? Color.black : Color.white);
				int num3 = 0;
				while (this.growing && num3 < num2)
				{
					this.totalGrowCount++;
					RaycastHit raycastHit;
					if (Physics.Raycast(this.grownToPos, this.directionVect, out raycastHit, 16f, this.groundLayer))
					{
						this.grownToPos = raycastHit.point;
						this.growing = false;
					}
					else if (this.totalGrowCount > 32)
					{
						this.growing = false;
					}
					else
					{
						this.grownToPos.x = this.grownToPos.x + (float)this.direction * 16f;
						if (Mathf.Abs(this.grownToPos.x - this.startPos.x) > 240f)
						{
							this.growing = false;
						}
					}
					Map.BlindUnits(this.playerNum, this.grownToPos.x, this.grownToPos.y, 16f);
					num3++;
				}
				this.line.SetPosition(0, this.startPos);
				this.line.SetPosition(1, this.grownToPos);
			}
		}
		else if (this.flashing)
		{
			this.colorCounter++;
			this.line.SetColors((this.colorCounter % 2 != 0) ? Color.black : Color.white, (this.colorCounter % 2 != 0) ? Color.black : Color.white);
			this.flashTime -= num;
			this.line.SetWidth(16f * this.flashTime / 0.11f, 16f * this.flashTime / 0.11f);
			if (this.flashTime <= 0f)
			{
				this.flashing = false;
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		this.line.material.SetTextureScale("_MainTex", new Vector2(Mathf.Clamp((float)this.totalGrowCount * 0.5f, 1f, 100f), 1f));
	}

	public int direction;

	public int playerNum;

	protected int totalGrowCount;

	private LayerMask groundLayer;

	protected int col;

	protected int row;

	private float growDelay;

	private Vector3 startPos;

	private Vector3 grownToPos;

	private Vector3 directionVect;

	private bool growing = true;

	private bool flashing = true;

	private float flashTime = 0.11f;

	private int colorCounter;

	private LineRenderer line;
}
