// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class IntroAnim : MonoBehaviour
{
	private void Awake()
	{
	}

	private IEnumerator Start()
	{
		Renderer renderer = this.topFlare.GetComponent<Renderer>();
		bool enabled = false;
		this.topFlare.enabled = enabled;
		renderer.enabled = enabled;
		Renderer renderer2 = this.bottomFlare.GetComponent<Renderer>();
		enabled = false;
		this.bottomFlare.enabled = enabled;
		renderer2.enabled = enabled;
		Renderer renderer3 = this.topFlare1.GetComponent<Renderer>();
		enabled = false;
		this.topFlare1.enabled = enabled;
		renderer3.enabled = enabled;
		Renderer renderer4 = this.bottomFlare1.GetComponent<Renderer>();
		enabled = false;
		this.bottomFlare1.enabled = enabled;
		renderer4.enabled = enabled;
		this.particles.gameObject.SetActive(false);
		float delay = 0.1f;
		for (int i = 0; i < this.Letters.Length; i++)
		{
			this.Letters[i].gameObject.AddComponent<TweenLetter>();
			this.Letters[i].gameObject.GetComponent<TweenLetter>().soundEffect = this.letterSoundEffect;
		}
		yield return new WaitForSeconds(1.4f);
		while (!this.Logo.finished)
		{
			yield return null;
		}
		Sound.GetInstance().PlaySoundEffect(this.announcement, 0.4f);
		float totalDelay = 0f;
		for (int j = 0; j < this.Letters.Length; j++)
		{
			totalDelay = 0.2f + delay * (float)j;
			this.Letters[j].gameObject.GetComponent<TweenLetter>().Init(totalDelay, this.letterCurve, this.flarePrefab);
		}
		yield return new WaitForSeconds(totalDelay);
		Renderer renderer5 = this.topFlare.GetComponent<Renderer>();
		enabled = true;
		this.topFlare.enabled = enabled;
		renderer5.enabled = enabled;
		Renderer renderer6 = this.bottomFlare.GetComponent<Renderer>();
		enabled = true;
		this.bottomFlare.enabled = enabled;
		renderer6.enabled = enabled;
		Renderer renderer7 = this.bottomFlare1.GetComponent<Renderer>();
		enabled = true;
		this.bottomFlare1.enabled = enabled;
		renderer7.enabled = enabled;
		Renderer renderer8 = this.topFlare1.GetComponent<Renderer>();
		enabled = true;
		this.topFlare1.enabled = enabled;
		renderer8.enabled = enabled;
		yield return new WaitForSeconds(0.2f);
		this.mainMenu.MenuActive = true;
		this.menuWasShown = true;
		yield break;
	}

	private void Update()
	{
		if (Application.isEditor && Input.GetKeyDown(KeyCode.F6))
		{
			Application.LoadLevel(Application.loadedLevel);
		}
		if (!this.menuWasShown && !this.mainMenu.MenuActive && InputReader.GetControllerPressingFire() > -1)
		{
			this.mainMenu.MenuActive = true;
			this.mainMenu.lastInputTime = Time.time + 0.5f;
			this.menuWasShown = true;
		}
	}

	public Birdman Logo;

	public LightFlare topFlare;

	public LightFlare topFlare1;

	public LightFlare bottomFlare;

	public LightFlare bottomFlare1;

	public Transform flarePrefab;

	public Transform[] Letters;

	public AnimationCurve letterCurve;

	public ParticleSystem particles;

	public Menu mainMenu;

	public AudioClip announcement;

	public AudioClip[] letterSoundEffect;

	private bool menuWasShown;
}
