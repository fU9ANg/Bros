// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class BirdmanWing : MonoBehaviour
{
	private void Awake()
	{
		this.endScale = base.transform.localScale;
		base.transform.localScale = this.startScale;
		if (this.direction == 1)
		{
			this.startRotation = Quaternion.AngleAxis(90f, Vector3.forward);
		}
		else
		{
			this.startRotation = Quaternion.AngleAxis(-90f, Vector3.forward);
		}
	}

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(1.2f);
		float duration = 0.2f;
		float timer = duration;
		if (!BirdmanWing.havePlayedSound && this.soundEffect != null)
		{
			BirdmanWing.havePlayedSound = true;
			Sound.GetInstance().PlaySoundEffect(this.soundEffect, 0.4f);
		}
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			float lerp = timer / duration;
			lerp = Mathf.Clamp01(1f - lerp);
			if ((double)lerp > 0.9)
			{
				lerp = 1f;
			}
			base.transform.localScale = Vector3.Lerp(this.startScale, this.endScale, lerp);
			base.transform.rotation = Quaternion.Lerp(this.startRotation, this.EndRotation, lerp);
			this.finished = true;
			yield return null;
		}
		yield return new WaitForSeconds(0.05f);
		BirdmanWing.havePlayedSound = false;
		Camera.main.GetComponent<SpringShake>().Shake(UnityEngine.Random.onUnitSphere * 10f);
		yield break;
	}

	private Vector3 startScale = Vector3.zero;

	private Vector3 endScale;

	private Quaternion startRotation;

	private Quaternion EndRotation = Quaternion.identity;

	public int direction = 1;

	public AudioClip soundEffect;

	private static bool havePlayedSound;

	public bool finished;
}
