// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static float OrthographicSize
	{
		get
		{
			if (CameraController.instance != null)
			{
				return CameraController.instance.mainCamera.orthographicSize;
			}
			return 128f;
		}
		set
		{
			if (CameraController.instance != null)
			{
				CameraController.instance.mainCamera.orthographicSize = value;
				CameraController.instance.distortionCamera.orthographicSize = value;
				CameraController.instance.backgroundCamera.orthographicSize = value;
				CameraController.instance.parallaxCamera.orthographicSize = 128f * CameraController.samRaimiEffectM + CameraController.instance.mainCamera.orthographicSize * (1f - CameraController.samRaimiEffectM);
				if (CameraController.instance.skyCamera != null)
				{
					CameraController.instance.foliageCamera.orthographicSize = value;
					CameraController.instance.lightingCamera.orthographicSize = value;
					CameraController.instance.skyCamera.orthographicSize = value;
					CameraController.instance.effectsamera.orthographicSize = value;
					CameraController.instance.groundCamera.orthographicSize = value;
				}
			}
		}
	}

	public static bool ScreenDistortionEnabled
	{
		get
		{
			return PlayerOptions.Instance.cameraDistortionEnabled;
		}
		set
		{
			PlayerOptions.Instance.cameraDistortionEnabled = value;
		}
	}

	public static float SamRaimiEffectM
	{
		get
		{
			return CameraController.samRaimiEffectM;
		}
		set
		{
			if (CameraController.instance != null)
			{
				CameraController.samRaimiEffectM = value;
				CameraController.instance.parallaxCamera.orthographicSize = 128f * CameraController.samRaimiEffectM + CameraController.instance.mainCamera.orthographicSize * (1f - CameraController.samRaimiEffectM);
			}
		}
	}

	protected void Awake()
	{
		CameraController.instance = this;
		if (!CameraController.ScreenDistortionEnabled)
		{
			UnityEngine.Debug.Log("SCREEN DISTORTION NOT ENABLED ");
			this.displacementScript.enabled = false;
			this.distortionCamera.enabled = false;
			this.imageDistortionScript.enabled = false;
		}
	}

	protected void Update()
	{
		if (this.backgroundCamera.orthographicSize != this.mainCamera.orthographicSize)
		{
			Camera camera = this.backgroundCamera;
			float num = this.mainCamera.orthographicSize;
			this.distortionCamera.orthographicSize = num;
			camera.orthographicSize = num;
			if (this.skyCamera != null)
			{
				Camera camera2 = this.effectsamera;
				num = this.mainCamera.orthographicSize;
				this.foliageCamera.orthographicSize = num;
				num = num;
				this.skyCamera.orthographicSize = num;
				num = num;
				this.lightingCamera.orthographicSize = num;
				num = num;
				this.groundCamera.orthographicSize = num;
				camera2.orthographicSize = num;
			}
			this.parallaxCamera.orthographicSize = 128f * CameraController.samRaimiEffectM + this.mainCamera.orthographicSize * (1f - CameraController.samRaimiEffectM);
		}
		if (Application.loadedLevelName.Equals("FilmScene"))
		{
			SortOfFollow.zoomLevel = Mathf.Clamp(SortOfFollow.zoomLevel - cInput.GetAxis("MWheel") * 0.1f, 0.1f, 20f);
		}
	}

	public Camera mainCamera;

	public Camera backgroundCamera;

	public Camera parallaxCamera;

	public Camera foliageCamera;

	public Camera lightingCamera;

	public Camera skyCamera;

	public Camera effectsamera;

	public Camera groundCamera;

	public Camera distortionCamera;

	public DisplacementCamera displacementScript;

	public ImageRefractionEffect imageDistortionScript;

	private static float samRaimiEffectM;

	public static CameraController instance;
}
