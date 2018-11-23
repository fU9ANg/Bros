// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ConfirmKick : MonoBehaviour
{
	public static ConfirmKick Instance
	{
		get
		{
			if (ConfirmKick.instance == null)
			{
				ConfirmKick.instance = (UnityEngine.Object.FindObjectsOfTypeAll(typeof(ConfirmKick))[0] as ConfirmKick);
			}
			return ConfirmKick.instance;
		}
	}

	public static void Open(PID playerToKick, int controllerID)
	{
		ConfirmKick.Instance.PlayerToKick = playerToKick;
		MonoBehaviour.print("Open " + playerToKick);
		ConfirmKick.Instance.gameObject.SetActive(true);
		ConfirmKick.Instance.controlledByControllerID = controllerID;
		ConfirmKick.Instance.highlight.gameObject.SetActive(true);
	}

	public static void Close()
	{
		ConfirmKick.Instance.PlayerToKick = null;
		ConfirmKick.Instance.gameObject.SetActive(false);
		KickPlayerMenu.OpenMenu(ConfirmKick.Instance.controlledByControllerID);
		ConfirmKick.Instance.controlledByControllerID = -1;
	}

	private void KickPlayer()
	{
		MonoBehaviour.print("Kick Player " + this.PlayerToKick);
		if (this.PlayerToKick != null)
		{
			Connect.SendKick(this.PlayerToKick);
		}
	}

	private void Update()
	{
		if (this.state == ConfirmKick.State.No)
		{
			this.highlight.SetTargetPos(this.NoButton.GetComponent<Renderer>().bounds.center, true);
			this.highlight.SetTargetSize(this.NoButton.GetComponent<Renderer>().bounds.size);
		}
		else if (this.state == ConfirmKick.State.Yes)
		{
			this.highlight.SetTargetPos(this.YesButton.GetComponent<Renderer>().bounds.center, true);
			this.highlight.SetTargetSize(this.YesButton.GetComponent<Renderer>().bounds.size);
		}
		bool flag6;
		bool flag5;
		bool flag4;
		bool flag3;
		bool flag2;
		flag6 = false; bool flag = flag2 = (flag3 = (flag4 = (flag5 = (flag6 ))));
		if (this.controlledByControllerID == -1)
		{
			InputReader.GetMenuInputCombined(ref flag2, ref flag, ref flag3, ref flag4, ref flag5, ref flag6);
		}
		else
		{
			InputReader.GetMenuInput(this.controlledByControllerID, ref flag2, ref flag, ref flag3, ref flag4, ref flag5, ref flag6);
		}
		flag5 = (flag5 || Input.GetKeyDown(KeyCode.Return));
		if (flag3)
		{
			this.state = ConfirmKick.State.No;
		}
		if (flag4)
		{
			this.state = ConfirmKick.State.Yes;
		}
		if (flag6)
		{
			ConfirmKick.Close();
		}
		if (flag5)
		{
			if (this.state == ConfirmKick.State.Yes)
			{
				this.KickPlayer();
			}
			ConfirmKick.Close();
		}
	}

	public TextMesh YesButton;

	public TextMesh NoButton;

	public MenuHighlightTween highlight;

	private PID PlayerToKick = PID.NoID;

	private int controlledByControllerID = -1;

	private static ConfirmKick instance;

	private ConfirmKick.State state = ConfirmKick.State.No;

	private enum State
	{
		Yes,
		No
	}
}
