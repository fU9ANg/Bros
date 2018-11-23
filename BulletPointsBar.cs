// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BulletPointsBar : MonoBehaviour
{
	private void Start()
	{
		this.bulletPoints = new BulletPoint[11];
		for (int i = 0; i < this.bulletPoints.Length; i++)
		{
			this.bulletPoints[i] = (UnityEngine.Object.Instantiate(this.bulletPointPrefab, base.transform.position + Vector3.right * (float)(-30 + 6 * i), Quaternion.identity) as BulletPoint);
			this.bulletPoints[i].Deactivate();
			this.bulletPoints[i].Setup((float)i * 0.133f);
			this.bulletPoints[i].transform.parent = base.transform;
		}
	}

	public void SetPoints(float noughtToOne)
	{
		int num = (int)(noughtToOne * (float)(this.bulletPoints.Length - 1));
		this.targetActivatedBulletPoints = num;
		this.changing = true;
	}

	public void ClearPoints()
	{
		this.targetActivatedBulletPoints = 0;
		this.activatedBulletPoints = 0f;
		for (int i = 0; i < this.bulletPoints.Length; i++)
		{
			this.bulletPoints[i].Deactivate();
		}
		this.changing = false;
	}

	private void Update()
	{
		if (this.changing)
		{
			if (this.activatedBulletPoints < (float)this.targetActivatedBulletPoints)
			{
				this.activatedBulletPoints += Time.deltaTime * 18f;
				if (this.activatedBulletPoints >= (float)this.targetActivatedBulletPoints)
				{
					this.activatedBulletPoints = (float)this.targetActivatedBulletPoints;
					this.changing = false;
				}
			}
			else if (this.activatedBulletPoints > (float)this.targetActivatedBulletPoints)
			{
				this.activatedBulletPoints -= Time.deltaTime * 18f;
				if (this.activatedBulletPoints <= (float)this.targetActivatedBulletPoints)
				{
					this.activatedBulletPoints = (float)this.targetActivatedBulletPoints;
					this.changing = false;
				}
			}
			else
			{
				this.changing = false;
			}
			for (int i = 0; i < this.bulletPoints.Length; i++)
			{
				if ((float)i <= this.activatedBulletPoints)
				{
					this.bulletPoints[i].Activate();
				}
				else
				{
					this.bulletPoints[i].Deactivate();
				}
			}
		}
	}

	public BulletPoint bulletPointPrefab;

	protected float activatedBulletPoints;

	protected int targetActivatedBulletPoints;

	protected bool changing;

	protected BulletPoint[] bulletPoints;
}
