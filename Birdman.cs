// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class Birdman : MonoBehaviour
{
	private void Awake()
	{
		this.endScale = base.transform.localScale;
		base.transform.localScale = this.startScale;
	}

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.6f);
		if (this.soundEffect != null)
		{
			Sound.GetInstance().PlaySoundEffect(this.soundEffect, 0.5f);
		}
		float duration = 0.15f;
		float timer = duration;
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			float lerp = timer / duration;
			lerp = Mathf.Clamp01(1f - lerp);
			if ((double)lerp > 0.8)
			{
				lerp = 1f;
			}
			base.transform.localScale = Vector3.Lerp(this.startScale, this.endScale, lerp);
			this.finished = true;
			yield return null;
		}
		Camera.main.GetComponent<SpringShake>().Shake(UnityEngine.Random.onUnitSphere * 10f);
		yield break;
	}

	private Vector3 startScale = Vector3.zero;

	private Vector3 endScale;

	public AudioClip soundEffect;

	public bool finished;
}
