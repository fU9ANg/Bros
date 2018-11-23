// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Sound : MonoBehaviour
{
	public static Sound GetInstance()
	{
		return Sound.instance;
	}

	public static bool MusicEnabled
	{
		get
		{
			return PlayerOptions.Instance.musicVolume > 0.01f;
		}
	}

	private void Awake()
	{
		Sound.instance = this;
		Sound.fading = false;
		this.effects = (base.gameObject.AddComponent(typeof(AudioSource)) as AudioSource);
		this.effects.volume = Sound.volumeEffects;
		this.effects.rolloffMode = AudioRolloffMode.Linear;
		this.effects.maxDistance = 250f;
		this.explosions = (base.gameObject.AddComponent(typeof(AudioSource)) as AudioSource);
		this.explosions.volume = Sound.volumeEffects;
		this.explosions.rolloffMode = AudioRolloffMode.Linear;
		this.explosions.maxDistance = 250f;
		this.speech = (base.gameObject.AddComponent(typeof(AudioSource)) as AudioSource);
		this.speech.volume = Sound.volumeEffects;
		this.speech.rolloffMode = AudioRolloffMode.Linear;
		this.speech.maxDistance = 250f;
		this.speech.loop = false;
		this.charge = (base.gameObject.AddComponent(typeof(AudioSource)) as AudioSource);
		this.charge.rolloffMode = AudioRolloffMode.Linear;
		this.charge.maxDistance = 250f;
		this.charge.loop = true;
		this.music = (base.gameObject.AddComponent(typeof(AudioSource)) as AudioSource);
		this.music.clip = null;
		this.music.loop = true;
		if (this.overrideVolumeLevel > 0f)
		{
			this.music.volume = Mathf.Clamp(PlayerOptions.Instance.musicVolume * this.overrideVolumeLevel, 0f, this.overrideVolumeLevel);
		}
		else if (this.isInMenuScreen)
		{
			if (PlaytomicController.isExpendabrosBuild)
			{
				this.music.loop = false;
			}
			this.music.volume = Mathf.Clamp(PlayerOptions.Instance.musicVolume * 0.43f, 0f, 0.43f);
		}
		else
		{
			this.music.volume = Mathf.Clamp(PlayerOptions.Instance.musicVolume * 0.3f, 0f, 0.36f);
		}
		this.music.rolloffMode = AudioRolloffMode.Linear;
		this.music.maxDistance = 250f;
		this.music.playOnAwake = false;
		this.music.Stop();
		this.highIntensityMusic = (base.gameObject.AddComponent(typeof(AudioSource)) as AudioSource);
		this.highIntensityMusic.clip = this.highIntensityMusicClip;
		this.highIntensityMusic.rolloffMode = AudioRolloffMode.Linear;
		this.highIntensityMusic.maxDistance = 250f;
		this.highIntensityMusic.loop = true;
		this.highIntensityMusic.playOnAwake = false;
		this.highIntensityMusic.volume = this.intensityMusicVolume * 0.5f;
		this.highIntensityMusic.Stop();
		this.lowIntensityMusic = (base.gameObject.AddComponent(typeof(AudioSource)) as AudioSource);
		this.lowIntensityMusic.clip = this.lowIntensityMusicClip;
		this.lowIntensityMusic.rolloffMode = AudioRolloffMode.Linear;
		this.lowIntensityMusic.maxDistance = 250f;
		this.lowIntensityMusic.loop = true;
		this.lowIntensityMusic.playOnAwake = false;
		this.lowIntensityMusic.volume = this.intensityMusicVolume * 0.666f;
		this.lowIntensityMusic.Stop();
		Sound.charging = false;
	}

	public static bool IsVictoryStingPlaying()
	{
		return Sound.instance.music.clip == Sound.instance.victorySting && Sound.instance.music.isPlaying;
	}

	protected void PlayMusic()
	{
		UnityEngine.Debug.Log("Play Music " + Sound.MusicEnabled);
		if (PlayerOptions.Instance.musicVolume > 0.01f && !Map.isEditing && Sound.MusicEnabled)
		{
			this.SetMusicVolume(0.13f);
			MusicType musicType = MusicType.Default;
			if (Map.MapData != null)
			{
				musicType = Map.MapData.musicType;
				if (musicType == MusicType.Default)
				{
					switch (Map.MapData.theme)
					{
					case LevelTheme.Jungle:
						musicType = MusicType.JungleBlueSky;
						goto IL_AB;
					case LevelTheme.BurningJungle:
						musicType = MusicType.JungleRedSky;
						goto IL_AB;
					case LevelTheme.Forest:
						musicType = MusicType.Factory;
						goto IL_AB;
					}
					musicType = MusicType.Factory;
				}
			}
			IL_AB:
			switch (musicType)
			{
			case MusicType.JungleBlueSky:
				this.lowIntensityMusicClip = this.lowJungleBlueSkyClip;
				this.highIntensityMusicClip = this.highJungleBlueSkyClip;
				break;
			case MusicType.JungleRedSky:
				this.lowIntensityMusicClip = this.lowJungleRedSkyClip;
				this.highIntensityMusicClip = this.highJungleRedSkyClip;
				break;
			case MusicType.Factory:
				this.lowIntensityMusicClip = this.lowFactoryClip;
				this.highIntensityMusicClip = this.highFactoryClip;
				break;
			default:
				this.lowIntensityMusicClip = this.lowJungleBlueSkyClip;
				this.highIntensityMusicClip = this.highJungleBlueSkyClip;
				break;
			}
			UnityEngine.Debug.Log("Play intensity is not null " + (this.lowIntensityMusicClip != null));
			if (GameModeController.GameMode == GameMode.BroDown && this.brodownMusic != null)
			{
				this.useMusicIntensity = false;
				this.music.clip = this.brodownMusic;
				this.SetMusicVolume(0.24f);
			}
			else if (Map.MapData != null && Map.MapData.musicType == MusicType.Bossfight && this.bossFightClip != null)
			{
				this.useMusicIntensity = false;
				this.music.clip = this.bossFightClip;
				this.SetMusicVolume(0.3f);
			}
			else if (Map.MapData != null && this.lowIntensityMusicClip != null)
			{
				UnityEngine.Debug.Log("Play intensity music " + this.lowIntensityMusicClip.name);
				this.useMusicIntensity = true;
				this.lowIntensityMusic.clip = this.lowIntensityMusicClip;
				this.lowIntensityMusic.volume = this.intensityMusicVolume * 0.666f;
				this.highIntensityMusic.clip = this.highIntensityMusicClip;
				this.highIntensityMusic.volume = this.intensityMusicVolume;
				if (this.highIntensityTime > 0f)
				{
					this.highIntensityMusic.Play();
				}
				else
				{
					this.lowIntensityMusic.Play();
				}
				this.playingIntensityIntro = false;
				this.music.Stop();
				this.music.clip = null;
			}
			else
			{
				this.music.clip = this.musicClip;
			}
			this.music.time = this.musicStartTime;
			this.music.Play();
		}
	}

	private void Start()
	{
		if (this.playDelay <= 0f && this.playAutomatically && Sound.musicOn && (!this.music.isPlaying || this.music.clip == null))
		{
			UnityEngine.Debug.Log("Play Musi8cd");
			this.PlayMusic();
		}
		if (this.audioLowPass != null)
		{
			this.audioLowPass.enabled = false;
		}
	}

	private void LateUpdate()
	{
		if (this.playDelay > 0f && this.playAutomatically && Sound.musicOn)
		{
			float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
			this.playDelay -= num;
			if (this.playDelay <= 0f)
			{
				this.PlayMusic();
			}
		}
		if (Input.GetKeyDown(KeyCode.F2))
		{
			if (this.music.isPlaying)
			{
				this.music.Stop();
			}
			else
			{
				this.music.time = this.musicStartTime;
				this.PlayMusic();
			}
		}
		if (this.useMusicIntensity && this.playDelay <= 0f)
		{
			if (Input.GetKeyDown(KeyCode.PageDown))
			{
				this.intensityMusicVolume -= 0.05f;
				if (this.intensityMusicVolume <= 0f)
				{
					this.intensityMusicVolume = 0f;
				}
			}
			if (Input.GetKeyDown(KeyCode.PageUp))
			{
				this.intensityMusicVolume += 0.05f;
				if (this.intensityMusicVolume >= 0.7f)
				{
					this.intensityMusicVolume = 0.7f;
				}
			}
			if (StatisticsController.GetMusicIntensity() > 0f || this.highIntensityTime > 0f)
			{
				this.highIntensityTime -= Time.deltaTime;
				if (this.highIntensityMusic.volume < 0.05f)
				{
					this.highIntensityMusic.volume = 0.05f;
				}
				if (!this.highIntensityMusic.isPlaying)
				{
					if (this.playingIntensityIntro)
					{
						this.highIntensityMusic.clip = this.highIntensityMusicClip;
						this.lowIntensityMusic.clip = this.lowIntensityMusicClip;
						this.highIntensityMusic.Play();
						this.playingIntensityIntro = false;
					}
					else
					{
						this.highIntensityMusic.Play();
						this.highIntensityMusic.time = this.lowIntensityMusic.time;
					}
				}
				this.highIntensityMusic.volume = Mathf.Lerp(this.highIntensityMusic.volume, this.intensityMusicVolume, Time.deltaTime * 2f);
				this.lowIntensityMusic.volume = Mathf.Lerp(this.lowIntensityMusic.volume, 0f, Time.deltaTime * 2f);
				if (this.lowIntensityMusic.volume < 0.01f)
				{
					this.lowIntensityMusic.Stop();
				}
			}
			else
			{
				if (this.lowIntensityMusic.volume < 0.05f)
				{
					this.lowIntensityMusic.volume = 0.05f;
				}
				if (!this.lowIntensityMusic.isPlaying)
				{
					if (this.playingIntensityIntro)
					{
						this.highIntensityMusic.clip = this.highIntensityMusicClip;
						this.lowIntensityMusic.clip = this.lowIntensityMusicClip;
						this.lowIntensityMusic.Play();
						this.playingIntensityIntro = false;
					}
					else
					{
						this.lowIntensityMusic.Play();
						this.lowIntensityMusic.time = this.highIntensityMusic.time;
					}
				}
				this.highIntensityMusic.volume = Mathf.Lerp(this.highIntensityMusic.volume, 0f, Time.deltaTime);
				this.lowIntensityMusic.volume = Mathf.Lerp(this.lowIntensityMusic.volume, this.intensityMusicVolume * 0.666f, Time.deltaTime);
				if (this.highIntensityMusic.volume < 0.01f)
				{
					this.highIntensityMusic.Stop();
				}
			}
		}
		if (GameModeController.IsLevelFinished() && GameModeController.GetLevelResult() == LevelResult.Success && this.music.clip != this.victorySting)
		{
			this.PlayVictorySting();
		}
		if (Application.isEditor && Input.GetKeyDown(KeyCode.F12))
		{
			if (Sound.musicOn)
			{
				this.music.Stop();
			}
			else if (!Map.isEditing)
			{
				this.music.Play();
				Sound.musicOn = true;
			}
		}
		if (Sound.charging)
		{
			if (!this.charge.isPlaying)
			{
				this.charge.clip = Sound.nextChargeClip;
				this.charge.Play();
				Sound.nextChargeClip = null;
			}
			this.chargingVolume = Mathf.Clamp(this.chargingVolume + Time.deltaTime * 0.7f, 0.01f, 1f);
			this.charge.volume = this.chargingVolume * Sound.volumeEffects;
		}
		else
		{
			this.chargingVolume = Mathf.Clamp(this.chargingVolume - Time.deltaTime * 5f, 0f, 1f);
			if (this.chargingVolume <= 0f)
			{
				if (this.charge.isPlaying)
				{
					this.charge.Stop();
				}
			}
			else
			{
				this.charge.volume = this.chargingVolume * Sound.volumeEffects;
			}
		}
		if (this.musicDimTime > 0f)
		{
			this.musicDimTime -= Time.deltaTime;
			if (this.musicDimTime > 0f)
			{
				float num2 = Mathf.Clamp(this.musicDimTime * 4f, 0f, 0.9f);
				float num3 = 1f - num2;
				this.music.volume = Mathf.Lerp(this.music.volume, PlayerOptions.Instance.musicVolume * num3, Time.deltaTime * 20f);
			}
			else
			{
				this.music.volume = PlayerOptions.Instance.musicVolume;
			}
		}
		if (this.speech.isPlaying && this.speech.time > this.wordTime + 0.05f)
		{
			this.speech.volume -= Mathf.Clamp(Time.deltaTime, 0f, 0.045f);
			if (this.speech.volume <= 0f)
			{
				this.speech.Stop();
			}
		}
		if (PlayerOptions.Instance.musicVolume > 0.01f)
		{
			if (Sound.fading)
			{
				this.music.volume = (this.music.volume -= Mathf.Clamp(Time.deltaTime * 0.5f, 0.0333f, 0.045f));
				if (this.music.volume <= 0.01f)
				{
					this.music.Stop();
				}
			}
			else if (this.overrideVolumeLevel > 0f)
			{
				this.music.volume = Mathf.Clamp(this.music.volume + Mathf.Clamp(Time.deltaTime, 0.0333f, 0.045f), 0f, this.overrideVolumeLevel * PlayerOptions.Instance.musicVolume);
			}
			else if (this.music.volume < PlayerOptions.Instance.musicVolume)
			{
				this.music.volume = Mathf.Clamp(this.music.volume + Mathf.Clamp(Time.deltaTime, 0.0333f, 0.045f), 0f, PlayerOptions.Instance.musicVolume);
			}
		}
		this.RunPitch();
		if (this.lowPassFading && Time.timeScale > 0f && this.audioLowPass != null)
		{
			this.lowPassFreqLoss *= 1f - Time.deltaTime * this.lowPassFrequencyJitterDampening / Time.timeScale;
			if (this.lowPassFreqTarget > this.lowPassFreqCurrent)
			{
				this.lowPassFreqCurrent = Mathf.Lerp(this.lowPassFreqCurrent, this.lowPassFreqTarget, Time.deltaTime * 5f / Time.timeScale);
				this.audioLowPass.cutoffFrequency = this.lowPassFreqCurrent - this.lowPassFreqLoss;
			}
			else
			{
				this.lowPassFreqCurrent = Mathf.Lerp(this.lowPassFreqCurrent, this.lowPassFreqTarget, Time.deltaTime * 12f / Time.timeScale);
				this.audioLowPass.cutoffFrequency = this.lowPassFreqCurrent - this.lowPassFreqLoss;
			}
			if (Mathf.Abs(this.lowPassFreqCurrent - this.lowPassFreqTarget) < 20f && this.lowPassFreqLoss < 25f)
			{
				this.audioLowPass.cutoffFrequency = this.lowPassFreqTarget;
				this.lowPassFading = false;
				if (this.lowPassFreqTarget > 3000f)
				{
					this.audioLowPass.enabled = false;
				}
			}
		}
		this.hasPlayedCreatureSound = false;
		this.hasPlayedEffectSound = false;
		this.hasPlayedExplosionSound = false;
		this.hasPlayedHeroSound = false;
		this.hasPlayedHeroSound = false;
		this.hasPlayedChargeSound = false;
	}

	public void PlayDeathMusic()
	{
		UnityEngine.Debug.Log("Play death ");
		if (PlayerOptions.Instance.musicVolume > 0.01f)
		{
			this.music.clip = this.deathClip;
			this.music.loop = false;
			this.music.volume = PlayerOptions.Instance.musicVolume;
			this.music.Play();
		}
	}

	public void PlayMainMusic()
	{
		UnityEngine.Debug.Log("Play main ");
		if (PlayerOptions.Instance.musicVolume > 0.01f)
		{
			this.music.clip = this.musicClip;
			this.music.loop = true;
			this.music.volume = PlayerOptions.Instance.musicVolume;
			this.music.Play();
		}
	}

	public void PlayBossFightMusic()
	{
		if (PlayerOptions.Instance.musicVolume > 0.01f && this.music.clip != this.bossFightClip)
		{
			this.music.clip = this.bossFightClip;
			this.music.loop = true;
			this.music.volume = 0.3f;
			this.music.Play();
			this.useMusicIntensity = false;
			if (this.highIntensityMusic != null)
			{
				this.highIntensityMusic.Stop();
			}
			if (this.lowIntensityMusic != null)
			{
				this.lowIntensityMusic.Stop();
			}
		}
	}

	public void PlayVictorySting()
	{
		this.highIntensityMusic.Stop();
		this.lowIntensityMusic.Stop();
		this.useMusicIntensity = false;
		this.music.Stop();
		this.music.clip = this.victorySting;
		this.music.volume = 0.45f;
		this.music.loop = false;
		this.music.Play();
	}

	public void FadeInMusic()
	{
		if (PlayerOptions.Instance.musicVolume > 0.01f)
		{
			UnityEngine.Debug.Log("Fade music 1");
			this.music.clip = this.musicClip;
			this.music.volume = 0.01f;
			this.music.Play();
			PlayerOptions.Instance.musicVolume = Mathf.Clamp(PlayerOptions.Instance.musicVolume, 0f, 0.25f);
			Sound.fading = false;
		}
	}

	public void FadeInMusic(float musicVolumeM)
	{
		if (PlayerOptions.Instance.musicVolume > 0.01f)
		{
			UnityEngine.Debug.Log("Fade music 2");
			this.music.clip = this.musicClip;
			this.music.volume = 0.01f;
			this.music.Play();
			PlayerOptions.Instance.musicVolume = Mathf.Clamp(PlayerOptions.Instance.musicVolume * musicVolumeM, 0f, 0.25f * musicVolumeM);
			Sound.fading = false;
		}
	}

	public void FadeMusic()
	{
		Sound.fading = true;
	}

	public void PlaySoundEffectSpecial(AudioClip[] clips, float v)
	{
		if (clips.Length == 0)
		{
			return;
		}
		this.speech.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)], v);
	}

	public void PlaySoundEffectAt(AudioClip[] clips, float v, Vector3 pos)
	{
		if (clips.Length == 0)
		{
			return;
		}
		this.PlaySoundEffectAt(clips[UnityEngine.Random.Range(0, clips.Length)], v, pos);
	}

	public void PlaySoundEffectAt(AudioClip[] clips, float v, Vector3 pos, float pitch)
	{
		if (clips.Length == 0)
		{
			return;
		}
		this.PlaySoundEffectAt(clips[UnityEngine.Random.Range(0, clips.Length)], v, pos, pitch);
	}

	public void PlaySoundEffectAt(AudioClip[] clips, float v, Vector3 pos, float pitch, bool bypassReverb)
	{
		if (clips.Length == 0)
		{
			return;
		}
		this.PlaySoundEffectAt(clips[UnityEngine.Random.Range(0, clips.Length)], v, pos, pitch, bypassReverb);
	}

	public void PlaySoundEffectAt(AudioClip[] clips, float v, Vector3 pos, float pitch, bool bypassReverb, float delay)
	{
		if (clips.Length == 0)
		{
			return;
		}
		this.PlaySoundEffectAt(clips[UnityEngine.Random.Range(0, clips.Length)], v, pos, pitch, bypassReverb, delay);
	}

	public void PlaySoundEffectAt(AudioClip clip, float v, Vector3 pos)
	{
		this.PlaySoundEffectAt(clip, v, pos, 1f);
	}

	public void PlaySoundEffectAt(AudioClip clip, float v, Vector3 pos, float pitch)
	{
		this.PlaySoundEffectAt(clip, v, pos, pitch, true);
	}

	public void PlaySoundEffectAt(AudioClip clip, float v, Vector3 pos, float pitch, bool bypassReverb, float delay)
	{
		if (clip == null || this.hasPlayedEffectSound)
		{
			return;
		}
		this.PlayAudioClip(clip, pos, v, pitch, bypassReverb, false, delay, true);
	}

	public void PlaySoundEffectAt(AudioClip[] clips, float v, Vector3 pos, float pitch, bool bypassReverb, bool bypassEffects)
	{
		if (clips.Length == 0)
		{
			return;
		}
		this.PlaySoundEffectAt(clips[UnityEngine.Random.Range(0, clips.Length)], v, pos, pitch, bypassReverb, bypassEffects);
	}

	public void PlaySoundEffectAt(AudioClip clip, float v, Vector3 pos, float pitch, bool bypassReverb, bool bypassEffects)
	{
		if (clip == null || this.hasPlayedEffectSound)
		{
			return;
		}
		this.PlayAudioClip(clip, pos, v, pitch, bypassReverb, bypassEffects, 0f, true);
	}

	public void PlaySoundEffectAt(AudioClip clip, float v, Vector3 pos, float pitch, bool bypassReverb)
	{
		this.PlaySoundEffectAt(clip, v, pos, pitch, bypassReverb, false);
	}

	public AudioSource PlayAudioClip(AudioClip clip, Vector3 position, float volume)
	{
		return this.PlayAudioClip(clip, position, volume, 1f, true, false, 0f, true);
	}

	public AudioSource PlayAudioClip(AudioClip clip, Vector3 position, float volume, float pitch, bool bypassReverb, bool bypassEffects, float delay = 0f, bool destroyOnLoad = true)
	{
		GameObject gameObject = new GameObject("One shot audio");
		gameObject.transform.position = position;
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.volume = volume;
		audioSource.pitch = pitch;
		if (delay > 0f)
		{
			audioSource.Stop();
			audioSource.playOnAwake = false;
			audioSource.PlayDelayed(delay);
		}
		else
		{
			audioSource.Play();
		}
		audioSource.rolloffMode = AudioRolloffMode.Linear;
		audioSource.bypassListenerEffects = bypassEffects;
		audioSource.bypassEffects = bypassEffects;
		audioSource.bypassReverbZones = bypassReverb;
		if (!destroyOnLoad)
		{
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			UnityEngine.Object.DontDestroyOnLoad(audioSource);
			audioSource.maxDistance = 500000f;
			audioSource.minDistance = 400000f;
			audioSource.dopplerLevel = 0f;
			audioSource.spread = 0.001f;
			UnityEngine.Object.Destroy(gameObject, clip.length + 0.1f);
		}
		else
		{
			audioSource.maxDistance = 286f;
			audioSource.minDistance = 64f;
			audioSource.dopplerLevel = 0.1f;
			UnityEngine.Object.Destroy(gameObject, clip.length);
		}
		return audioSource;
	}

	public void PlayAnnouncer(AudioClip[] clips, float v, float pitch, float delay = 0f)
	{
		if (clips.Length == 0)
		{
			return;
		}
		this.PlayAnnouncer(clips[UnityEngine.Random.Range(0, clips.Length)], v, pitch, delay);
	}

	public void PlayAnnouncer(AudioClip clip, float v, float pitch, float delay = 0f)
	{
		Vector3 position = Vector3.zero;
		if (SortOfFollow.GetInstance() != null)
		{
			position = SortOfFollow.GetInstance().transform.position;
		}
		else if (Camera.main != null)
		{
			position = Camera.main.transform.position;
		}
		AudioSource audioSource = this.PlayAudioClip(clip, position, v, pitch, true, true, delay, false);
		if (audioSource != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(audioSource.gameObject);
			audioSource.transform.parent = null;
		}
	}

	public void PlaySoundEffect(AudioClip[] clips, float v)
	{
		if (clips.Length == 0)
		{
			return;
		}
		this.PlaySoundEffect(clips[UnityEngine.Random.Range(0, clips.Length)], v);
	}

	public void PlaySoundEffect(AudioClip clip, float v)
	{
		if (clip == null || this.hasPlayedEffectSound)
		{
			return;
		}
		this.effects.PlayOneShot(clip, v * Sound.volumeEffects);
		this.hasPlayedEffectSound = true;
	}

	public void SetEffectsVolume(float val)
	{
		Sound.volumeEffects = val;
	}

	public void SetMusicVolume(float val)
	{
		if (this.isInMenuScreen)
		{
			this.music.volume = Mathf.Clamp(PlayerOptions.Instance.musicVolume * 0.43f, 0f, 0.43f);
		}
		else
		{
			this.music.volume = Mathf.Clamp(PlayerOptions.Instance.musicVolume * 0.36f, 0f, 0.36f);
		}
	}

	public void Dimmusic(float p)
	{
		this.musicDimTime = p;
	}

	public static void SetPitch(float p)
	{
		Sound.instance.desiredPitch = p;
		Sound.instance.shiftingPitch = true;
	}

	public static void SetPitchNearInstant(float p)
	{
		Sound.instance.currentPitch = (Sound.instance.currentPitch + p * 3f) / 4f;
		Sound.instance.desiredPitch = p;
		Sound.instance.shiftingPitch = true;
	}

	protected void RunPitch()
	{
		if (this.shiftingPitch)
		{
			if (this.currentPitch < this.desiredPitch)
			{
				this.currentPitch += Time.deltaTime * 0.5f;
				if (this.currentPitch >= this.desiredPitch)
				{
					this.shiftingPitch = true;
					this.currentPitch = this.desiredPitch;
				}
			}
			if (this.currentPitch > this.desiredPitch)
			{
				this.currentPitch -= Time.deltaTime * 0.5f;
				if (this.currentPitch <= this.desiredPitch)
				{
					this.shiftingPitch = true;
					this.currentPitch = this.desiredPitch;
				}
			}
			this.RunLowPassFilter(this.currentPitch * this.currentPitch * this.currentPitch * this.currentPitch * this.currentPitch);
			Sound.instance.music.pitch = (1f + this.currentPitch) / 2f;
			Sound.instance.effects.pitch = this.currentPitch;
			Sound.instance.explosions.pitch = this.currentPitch;
			Sound.instance.charge.pitch = this.currentPitch;
		}
	}

	public static void SuddenLowPass(float m)
	{
		Sound.SuddenLowPass(m, Sound.instance.transform.position);
	}

	public static void SuddenLowPass(float m, Vector3 pos)
	{
		if (Sound.instance.useSuddenLowPass)
		{
			Vector3 vector = Sound.instance.transform.position - pos;
			float num = Mathf.Clamp(Mathf.Max(Mathf.Abs(vector.x) - Sound.instance.lowPassMinRange, Mathf.Abs(vector.y) - Sound.instance.lowPassMinRange), 0f, 10000f) / (Sound.instance.lowPassMaxRange - Sound.instance.lowPassMinRange);
			if (num > 0f)
			{
				m *= 1f - num;
			}
			if (m > 0f)
			{
				Sound.instance.lowPassFreqLoss = Sound.instance.lowPassFreqTarget * 0.99f * m;
				Sound.instance.lowPassFading = true;
				Sound.instance.lowPassFreqCurrent = Sound.instance.lowPassFreqTarget - Sound.instance.lowPassFreqLoss;
				Sound.instance.audioLowPass.cutoffFrequency = Sound.instance.lowPassFreqCurrent;
				Sound.instance.audioLowPass.enabled = true;
			}
		}
	}

	protected void RunLowPassFilter(float m)
	{
		if (m >= 1f)
		{
			Sound.instance.audioLowPass.enabled = true;
			Sound.instance.lowPassFreqTarget = 10000f;
			Sound.instance.lowPassFading = true;
		}
		else if (m < 1f)
		{
			Sound.instance.audioLowPass.enabled = true;
			Sound.instance.lowPassFreqTarget = 10000f * m;
			Sound.instance.lowPassFading = true;
		}
	}

	private void OnEnable()
	{
		if (this.musicClip != null && PlayerOptions.Instance.musicVolume > 0.1f && !this.music.isPlaying && this.playAutomatically && this.playDelay <= 0f)
		{
			this.music.Play();
		}
	}

	public bool playAutomatically;

	public bool isInMenuScreen;

	public float playDelay;

	public AudioClip[] ambientEffects;

	public AudioClip musicClip;

	public AudioClip bossFightClip;

	public AudioClip brodownMusic;

	public AudioClip deathClip;

	public AudioClip victorySting;

	public AudioClip lowJungleBlueSkyClip;

	public AudioClip highJungleBlueSkyClip;

	public AudioClip lowJungleRedSkyClip;

	public AudioClip highJungleRedSkyClip;

	public AudioClip lowFactoryClip;

	public AudioClip highFactoryClip;

	protected AudioClip lowIntensityMusicClip;

	protected AudioClip highIntensityMusicClip;

	protected float alertnessCounter;

	public AudioClip achievementSoundClip;

	protected static Sound instance;

	public AudioSource explosions;

	public AudioSource effects;

	public AudioSource heroes;

	public AudioSource speech;

	public AudioSource charge;

	public AudioSource music;

	public AudioSource highIntensityMusic;

	public AudioSource lowIntensityMusic;

	protected bool useMusicIntensity;

	public float intensityMusicVolume = 0.18f;

	public float highIntensityTime = 1.2f;

	protected bool playingIntensityIntro;

	public static float volumeEffects = 1f;

	protected static bool fading;

	protected bool hasPlayedExplosionSound;

	protected bool hasPlayedEffectSound;

	protected bool hasPlayedCreatureSound;

	protected bool hasPlayedHeroSound;

	protected bool hasPlayedSpeechSound;

	protected bool hasPlayedChargeSound;

	protected float musicDimTime;

	protected float wordTime = 0.4f;

	private static bool charging;

	private static AudioClip nextChargeClip;

	private float chargingVolume;

	public float musicStartTime;

	protected static bool musicOn = true;

	public float overrideVolumeLevel = -1f;

	public AudioLowPassFilter audioLowPass;

	protected float lowPassFreqTarget = 10000f;

	protected float lowPassFreqCurrent = 10000f;

	protected float lowPassFreqLoss;

	protected bool lowPassFading;

	public float lowPassFrequencyJitterDampening = 12f;

	protected float desiredPitch = 1f;

	protected float currentPitch = 1f;

	protected bool shiftingPitch;

	public bool useSuddenLowPass = true;

	public float lowPassMinRange = 160f;

	public float lowPassMaxRange = 300f;
}
