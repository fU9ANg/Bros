// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class IntroController : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < 10; i++)
		{
			StarGrow starGrow = UnityEngine.Object.Instantiate(this.star, this.star.transform.position, Quaternion.identity) as StarGrow;
			starGrow.delay = 0.1f + (float)i * 0.1f;
			starGrow.transform.parent = this.starsHolder.transform;
		}
		this.starsHolder.gameObject.SetActive(false);
		this.hand1Pos = this.hand1.position;
		this.hand2Pos = this.hand2.position;
		this.hand1.position = this.hand1Pos - Vector3.right * 400f;
		this.hand2.position = this.hand2Pos + Vector3.right * 400f;
		this.riserSource = base.gameObject.AddComponent<AudioSource>();
		this.riserSource.maxDistance = 20000f;
		this.riserSource.minDistance = 3000f;
		this.riserSource.loop = true;
		this.riserSource.volume = 0f;
		this.riserSource.pitch = 0.25f;
		this.riserSource.clip = this.riser;
		this.riserSource.playOnAwake = false;
		this.riserSource.Play();
		this.blurEffect.enabled = false;
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime * 0.5f, 0f, 0.033f);
		if (!this.hasHit)
		{
			if (!this.hasFaded)
			{
				this.riserSource.volume += num * 0.05f + this.riserSource.volume * num * 1.2f;
				this.riserSource.pitch += num * 0.1f + this.riserSource.pitch * num * 2f;
				this.xI += 800f * num;
				this.hand1.position += Vector3.right * this.xI * num;
				this.hand2.position -= Vector3.right * this.xI * num;
				if (this.hand1.position.x > this.hand1Pos.x)
				{
					this.hand1.position = this.hand1Pos;
					this.hand2.position = this.hand2Pos;
					InterfaceCameraShake.Shake(2f, 2f, 2f);
					this.clapSource.rolloffMode = AudioRolloffMode.Linear;
					this.clapSource.maxDistance = 20000f;
					this.clapSource.minDistance = 3000f;
					this.clapSource.volume = 1f;
					this.clapSource.Play();
					this.riserSource.Stop();
					this.hasHit = true;
					Sound instance = Sound.GetInstance();
					instance.PlayMainMusic();
					this.starsHolder.gameObject.SetActive(true);
				}
			}
			else
			{
				if (this.xI > 0f)
				{
					this.xI = 0f;
				}
				this.riserSource.volume -= num * 0.08f + this.riserSource.volume * num * 1.2f;
				this.riserSource.pitch -= num * 0.1f + this.riserSource.pitch * num * 2f;
				this.xI -= 800f * num;
				this.hand1.position += Vector3.right * this.xI * num;
				this.hand2.position -= Vector3.right * this.xI * num;
				this.filter.reverbLevel *= 1f - num * 18f;
			}
			if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(PlayerOptions.Instance.keyPlayer1Fire) || Input.GetKeyDown(PlayerOptions.Instance.keyPlayer2Fire) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
			{
				this.Continue();
			}
			if (InputReader.GetControllerPressingFire() > -1)
			{
				this.Continue();
			}
		}
		else
		{
			if (!this.hasFaded)
			{
				if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(PlayerOptions.Instance.keyPlayer1Fire) || Input.GetKeyDown(PlayerOptions.Instance.keyPlayer2Fire) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
				{
					this.Continue();
				}
				if (InputReader.GetControllerPressingFire() > -1)
				{
					this.Continue();
				}
			}
			this.endDelay -= num;
			if (this.endDelay < 3f)
			{
				this.lowPassFilter.cutoffFrequency *= 1f - num * 1.3f;
				this.lowPassFilter.lowpassResonanceQ += num * 2.5f;
			}
			if (this.endDelay < 0.5f)
			{
				this.blurEffect.enabled = true;
				this.blurEffect.blurSpread += num * 1f + this.blurEffect.blurSpread * num * 6f;
			}
			this.bloomController.bloomIntensity += num * 0.3f + this.bloomController.bloomIntensity * num * 0.15f;
			this.bloomController.sepBlurSpread += num * 0.5f + this.bloomController.sepBlurSpread * num * 0.4f;
			if (!this.hasFaded && this.endDelay <= 0f)
			{
				this.Continue();
			}
		}
	}

	protected void Continue()
	{
		Fader.nextScene = LevelSelectionController.MainMenuScene;
		if (this.hasHit)
		{
			Fader.FadeSolid(1.5f);
		}
		else
		{
			Fader.FadeSolid(0.5f);
		}
		this.hasFaded = true;
		Sound instance = Sound.GetInstance();
		instance.FadeMusic();
	}

	public Transform hand1;

	public Transform hand2;

	protected Vector3 hand1Pos;

	protected Vector3 hand2Pos;

	protected float xI;

	public AudioSource clapSource;

	public AudioClip riser;

	protected AudioSource riserSource;

	protected bool hasHit;

	protected bool hasFaded;

	public AudioReverbFilter filter;

	protected float endDelay = 4.5f;

	public GameObject starsHolder;

	public StarGrow star;

	public BloomAndLensFlares bloomController;

	public AudioLowPassFilter lowPassFilter;

	public BlurEffect blurEffect;
}
