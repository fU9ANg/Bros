// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class FinalCinematics : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(2f);
		CutsceneSound.PlaySoundEffectAt(this.siren, 0.7f + UnityEngine.Random.value * 0.2f, base.transform.position, 1f);
		yield return new WaitForSeconds(1f);
		this.foreground.SetLowerLeftPixel_X(this.foreground.width * 0f);
		this.foreground.GetComponent<AnimatedTexture>().enabled = false;
		yield return new WaitForSeconds(1f);
		this.foreground.SetLowerLeftPixel_X(this.foreground.width * 7f);
		base.StartCoroutine(this.Siren());
		yield return new WaitForSeconds(3f);
		MonoBehaviour.print("Set is finished");
		base.gameObject.GetComponentInHeirarchy<Cutscene>().isFinished = true;
		yield break;
	}

	private IEnumerator Siren()
	{
		AmplifyColorEffect colorGrading = base.GetComponentInChildren<AmplifyColorEffect>();
		float t = 0f;
		float lerp = 0f;
		for (;;)
		{
			t += Time.deltaTime;
			float rate = 1f;
			lerp += Time.deltaTime * rate;
			colorGrading.BlendAmount = 1f - this.PingPong(lerp);
			yield return null;
		}
		yield break;
	}

	private void Update()
	{
	}

	private float PingPong(float f)
	{
		int num = Mathf.FloorToInt(f);
		float num2 = f - (float)num;
		if (num2 % 2f == 1f)
		{
			num2 = -num2;
		}
		return num2;
	}

	public SpriteSM foreground;

	public AudioClip siren;
}
