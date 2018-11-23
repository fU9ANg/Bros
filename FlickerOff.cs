// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class FlickerOff : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.3f));
		yield return new WaitForSeconds(1f);
		FlexController.instance.SwitchOnLights();
		base.GetComponent<Renderer>().enabled = false;
		yield return new WaitForSeconds(0.05f);
		base.GetComponent<Renderer>().enabled = true;
		yield return new WaitForSeconds(0.03f);
		base.GetComponent<Renderer>().enabled = false;
		yield return new WaitForSeconds(0.06f);
		base.GetComponent<Renderer>().enabled = true;
		yield return new WaitForSeconds(0.03f);
		base.GetComponent<Renderer>().enabled = false;
		yield return new WaitForSeconds(0.05f);
		base.GetComponent<Renderer>().enabled = true;
		yield return new WaitForSeconds(0.03f);
		base.GetComponent<Renderer>().enabled = false;
		yield return new WaitForSeconds(0.06f);
		base.GetComponent<Renderer>().enabled = true;
		yield return new WaitForSeconds(0.03f);
		base.GetComponent<Renderer>().enabled = false;
		yield return new WaitForSeconds(0.01f);
		base.GetComponent<Renderer>().enabled = true;
		yield return new WaitForSeconds(0.2f);
		base.GetComponent<Renderer>().enabled = false;
		yield break;
	}
}
