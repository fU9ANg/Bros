// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class EaserExampleSceneCube : MonoBehaviour
{
	private void Awake()
	{
		this._color = base.GetComponent<Renderer>().material.color;
		this._hoverColor = (this._color + Color.white) / 2f;
		this.initPos = base.transform.position;
	}

	private void Update()
	{
		base.transform.localScale = Easer.EaseVector3(EaserEase.InOutCubic, Vector3.one, Vector3.one * 0.9f, Mathf.PingPong(Time.time, 1f));
	}

	private void OnMouseEnter()
	{
		base.GetComponent<Renderer>().material.color = this._hoverColor;
	}

	private void OnMouseExit()
	{
		base.GetComponent<Renderer>().material.color = this._color;
	}

	private void OnMouseDown()
	{
		base.StopCoroutine("ease_cr");
		base.StartCoroutine("ease_cr");
	}

	private IEnumerator ease_cr()
	{
		this._t = 0f;
		while (this._t < 1f)
		{
			Vector3 newPos = base.transform.position;
			newPos.y = Easer.Ease(this._ease, -1f, 0f, this._t);
			base.transform.position = newPos;
			this._t += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	[HideInInspector]
	public Vector3 initPos;

	[SerializeField]
	private EaserEase _ease;

	private Color _color;

	private Color _hoverColor;

	private float _t;
}
