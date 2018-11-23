// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapCutsceneController : MonoBehaviour
{
	public static void ReactivateWorld()
	{
		WorldMapController.Activate();
		WorldMapCutsceneController.instance.americaCutscene.SetActive(false);
		WorldMapCutsceneController.instance.gameObject.SetActive(false);
	}

	public static void DeactivateWorld()
	{
		WorldMapController.Deactivate();
		WorldMapCutsceneController.instance.gameObject.SetActive(true);
	}

	public static void DeactivateCutscenes()
	{
		if (WorldMapCutsceneController.instance != null)
		{
			WorldMapCutsceneController.instance.gameObject.SetActive(false);
		}
	}

	public static void AmericanPresidentCutscene()
	{
		WorldMapCutsceneController.DeactivateWorld();
		WorldMapCutsceneController.instance.americaCutscene.SetActive(true);
	}

	private void Awake()
	{
		WorldMapCutsceneController.instance = this;
	}

	private void Update()
	{
	}

	public GameObject americaCutscene;

	public AmplifyColorEffect amplifyColor;

	protected static WorldMapCutsceneController instance;
}
