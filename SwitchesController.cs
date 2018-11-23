// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SwitchesController : MonoBehaviour
{
	private void Awake()
	{
		SwitchesController.instance = this;
	}

	public static MobileSwitch CreatePilotMookSwitch(Mook mook)
	{
		MobileSwitch mobileSwitch = UnityEngine.Object.Instantiate(SwitchesController.instance.pilotMookSwitchPrefab, mook.transform.position + Vector3.up * 6f, Quaternion.identity) as MobileSwitch;
		mobileSwitch.affectedGameObject = mook.gameObject;
		mobileSwitch.transform.parent = mook.transform;
		return mobileSwitch;
	}

	public static MobileSwitch CreateGoUpSwitch(GameObject gameObject, Vector3 offset)
	{
		MobileSwitch mobileSwitch = UnityEngine.Object.Instantiate(SwitchesController.instance.goUpSwitchPrefab, gameObject.transform.position + offset, Quaternion.identity) as MobileSwitch;
		mobileSwitch.affectedGameObject = gameObject;
		mobileSwitch.transform.parent = gameObject.transform;
		return mobileSwitch;
	}

	public static MobileSwitch CreateGoDownSwitch(GameObject gameObject, Vector3 offset)
	{
		MobileSwitch mobileSwitch = UnityEngine.Object.Instantiate(SwitchesController.instance.goDownSwitchPrefab, gameObject.transform.position + offset, Quaternion.identity) as MobileSwitch;
		mobileSwitch.affectedGameObject = gameObject;
		mobileSwitch.transform.parent = gameObject.transform;
		return mobileSwitch;
	}

	public MobileSwitch pilotMookSwitchPrefab;

	public MobileSwitch goUpSwitchPrefab;

	public MobileSwitch goDownSwitchPrefab;

	private static SwitchesController instance;
}
