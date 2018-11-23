// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class TweenLetter : MonoBehaviour
{
	private void Awake()
	{
		this.endScale = base.transform.localScale;
		this.endPos = base.transform.position;
		this.startScale = this.endScale * 10f;
		this.startPos = this.endPos - Vector3.forward - Vector3.up * 200f;
		base.gameObject.SetActive(false);
	}

	public void Init(float delay, AnimationCurve Curve, Transform flarePrefab)
	{
		base.gameObject.SetActive(true);
		this.curve = Curve;
		this.flare = (UnityEngine.Object.Instantiate(flarePrefab) as Transform);
		this.flare.parent = base.transform;
		this.flare.localPosition = -Vector3.forward;
		base.StartCoroutine(this.InitRoutine(delay));
	}

	private IEnumerator InitRoutine(float delay)
	{
		Renderer renderer = this.flare.GetComponent<Renderer>();
		bool enabled = false;
		base.GetComponent<Renderer>().enabled = enabled;
		renderer.enabled = enabled;
		yield return new WaitForSeconds(delay);
		Sound.GetInstance().PlaySoundEffect(this.soundEffect, 0.5f);
		float duration = 0.2f;
		float timer = duration;
		Renderer renderer2 = this.flare.GetComponent<Renderer>();
		enabled = true;
		base.GetComponent<Renderer>().enabled = enabled;
		renderer2.enabled = enabled;
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			float lerp = 1f - Mathf.Clamp01(timer / duration);
			float e = this.curve.Evaluate(lerp);
			this.flare.GetComponent<Renderer>().material.SetAlpha((1f - lerp) * 0.7f);
			base.transform.transform.position = Vector3.Lerp(this.startPos, this.endPos, lerp) + Vector3.right * e;
			base.transform.transform.localScale = Vector3.Lerp(this.startScale, this.endScale, lerp);
			yield return null;
		}
		Camera.main.GetComponent<SpringShake>().Shake(UnityEngine.Random.onUnitSphere * 4f);
		SpringShake shake = base.gameObject.AddComponent<SpringShake>();
		shake.damping = 80f;
		shake.stiffness = 4000f;
		shake.Shake(UnityEngine.Random.insideUnitCircle.normalized * 30f);
		yield break;
	}

	private Vector3 startScale;

	private Vector3 endScale;

	private Vector3 startPos;

	private Vector3 endPos;

	private AnimationCurve curve;

	public AudioClip[] soundEffect;

	private Transform flare;
}
