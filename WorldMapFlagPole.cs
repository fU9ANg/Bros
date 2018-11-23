// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapFlagPole : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Start()
	{
	}

	public void RaiseFlag()
	{
		this.flag.localPosition = new Vector3(this.flagStartHeight.x, this.flagStartHeight.y - 30f, this.flagStartHeight.z);
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.033f);
		if (this.flag.localPosition.y < this.flagStartHeight.y)
		{
			this.flag.transform.localPosition = new Vector3(this.flagStartHeight.x, Mathf.Clamp(this.flag.transform.localPosition.y + 20f * num, -1000f, this.flagStartHeight.y), this.flagStartHeight.z);
		}
	}

	public Transform flag;

	public Vector3 flagStartHeight;
}
