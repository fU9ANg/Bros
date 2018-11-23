// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaserExampleScene : MonoBehaviour
{
	protected virtual void Awake()
	{
		Easer.Initialize();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			base.StopCoroutine("ease_cr");
			base.StartCoroutine("ease_cr");
		}
	}

	private IEnumerator ease_cr()
	{
		this._t = 0f;
		while (this._t < 1f)
		{
			for (int i = 0; i < this._cubes.Count; i++)
			{
				Vector3 initPos = this._cubes[i].initPos;
				Vector3 targetPos = this._cubes[i].initPos + this._cubes[i].transform.forward;
				this._cubes[i].transform.position = Easer.EaseVector3(this._ease, initPos, targetPos, this._t);
			}
			this._t += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	[SerializeField]
	private EaserEase _ease;

	[SerializeField]
	private List<EaserExampleSceneCube> _cubes;

	private float _t;
}
