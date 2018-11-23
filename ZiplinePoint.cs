// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ZiplinePoint : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (this.firstFrame && !Map.isEditing)
		{
			this.firstFrame = false;
			LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
			if (!this.isTelephonePole)
			{
				RaycastHit raycastHit = default(RaycastHit);
				if (Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y + 1f, 0f), Vector3.down, out raycastHit, 32f, mask))
				{
					raycastHit.collider.GetComponent<Block>().replaceOnCollapse = true;
					raycastHit.collider.GetComponent<Block>().replacementBlockType = GroundType.Steel;
				}
			}
		}
	}

	public void SetupZipline()
	{
		if (this.otherPoint == null)
		{
			return;
		}
		if (this.otherPoint.transform.position.y > base.transform.position.y)
		{
			this.isStartPoint = false;
		}
		else if (this.otherPoint.transform.position.y < base.transform.position.y)
		{
			this.isStartPoint = true;
		}
		else if (this.otherPoint.transform.position.x > base.transform.position.x)
		{
			this.isStartPoint = true;
		}
		else
		{
			this.isStartPoint = false;
		}
		this.zipLine = (UnityEngine.Object.Instantiate(this.zipLinePrefab) as ZipLine);
		this.otherPoint.zipLine = this.zipLine;
		this.otherPoint.otherPoint = this;
		this.zipLine.startExtended = true;
		this.zipLine.isTelephoneLine = this.isTelephonePole;
		if (this.isStartPoint)
		{
			this.otherPoint.isStartPoint = false;
			this.zipLine.fromAnchorPoint = base.transform.position;
			this.zipLine.toAnchorPoint = this.otherPoint.transform.position;
		}
		else
		{
			this.otherPoint.isStartPoint = true;
			this.zipLine.fromAnchorPoint = this.otherPoint.transform.position;
			this.zipLine.toAnchorPoint = base.transform.position;
		}
		this.zipLine.fromAnchorPoint += Vector3.forward + Vector3.up * this.anchorYOffet;
		this.zipLine.toAnchorPoint += Vector3.forward + Vector3.up * this.anchorYOffet;
		if (this.zipLine.fromAnchorPoint.x < this.zipLine.toAnchorPoint.x)
		{
			this.zipLine.fromAnchorPoint += Vector3.right * this.anchorXOffet;
			this.zipLine.toAnchorPoint -= Vector3.right * this.anchorXOffet;
		}
		else
		{
			this.zipLine.fromAnchorPoint -= Vector3.right * this.anchorXOffet;
			this.zipLine.toAnchorPoint += Vector3.right * this.anchorXOffet;
		}
	}

	private void OnDestroy()
	{
		if (this.otherPoint != null)
		{
			this.otherPoint.zipLine = null;
			this.otherPoint.otherPoint = null;
		}
		if (this.zipLine != null)
		{
			UnityEngine.Object.Destroy(this.zipLine.gameObject);
		}
	}

	private void HoverDoodadBeingDestroyed()
	{
		if (this.otherPoint != null)
		{
			this.otherPoint.zipLine = null;
			this.otherPoint.otherPoint = null;
		}
		if (this.zipLine != null)
		{
			UnityEngine.Object.Destroy(this.zipLine.gameObject);
		}
	}

	internal void ResetZipline()
	{
		if (this.otherPoint != null)
		{
			this.otherPoint.otherPoint = null;
			this.isStartPoint = false;
			this.otherPoint.isStartPoint = false;
			this.otherPoint = null;
		}
		if (this.zipLine != null)
		{
			UnityEngine.Object.Destroy(this.zipLine.gameObject);
		}
	}

	public ZiplinePoint otherPoint;

	public ZipLine zipLinePrefab;

	public ZipLine zipLine;

	public bool isTelephonePole;

	public bool isStartPoint;

	public float anchorXOffet;

	public float anchorYOffet;

	protected bool firstFrame = true;
}
