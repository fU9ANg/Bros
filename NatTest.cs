// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class NatTest : MonoBehaviour
{
	private void OnGUI()
	{
		GUILayout.BeginArea(new Rect(0f, (float)(Screen.height - 100), 800f, 100f));
		GUILayout.Label("Current Status: " + this.testStatus, new GUILayoutOption[0]);
		GUILayout.Label(string.Concat(new object[]
		{
			"Test result : ",
			this.connectionTestResult,
			" - ",
			this.testMessage
		}), new GUILayoutOption[0]);
		GUILayout.Label(this.shouldEnableNatMessage, new GUILayoutOption[0]);
		if (!this.doneTesting)
		{
			this.TestConnection();
		}
		GUILayout.EndArea();
	}

	private void TestConnection()
	{
		this.connectionTestResult = Network.TestConnection();
		ConnectionTesterStatus connectionTesterStatus = this.connectionTestResult;
		switch (connectionTesterStatus + 2)
		{
		case ConnectionTesterStatus.PrivateIPNoNATPunchthrough:
			this.testMessage = "Problem determining NAT capabilities";
			this.doneTesting = true;
			goto IL_19A;
		case ConnectionTesterStatus.PrivateIPHasNATPunchThrough:
			this.testMessage = "Undetermined NAT capabilities";
			this.doneTesting = false;
			goto IL_19A;
		case ConnectionTesterStatus.PublicIPNoServerStarted:
			this.testMessage = "Directly connectable public IP address.";
			this.doneTesting = true;
			this.useNAT = false;
			goto IL_19A;
		case ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted:
			this.testMessage = "Non-connectble public IP address (port  blocked), running a server is impossible.";
			if (!this.probingPublicIP)
			{
				UnityEngine.Debug.Log("Testing if firewall can be circumvented");
				this.connectionTestResult = Network.TestConnectionNAT();
				this.probingPublicIP = true;
				this.timer = Time.time + 10f;
				this.useNAT = false;
			}
			else if (Time.time > this.timer)
			{
				this.probingPublicIP = false;
				this.doneTesting = true;
				this.useNAT = true;
			}
			goto IL_19A;
		case ConnectionTesterStatus.LimitedNATPunchthroughSymmetric:
			this.testMessage = "Server not started, it is needed to check ability to connect with server. Restart the test when ready.";
			goto IL_19A;
		case ConnectionTesterStatus.NATpunchthroughFullCone:
			this.testMessage = "Everyone except Symmetric NATs can connect.";
			this.doneTesting = true;
			this.useNAT = true;
			goto IL_19A;
		case ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone:
			this.testMessage = "NAT Punchthrough is limited to asymmetric NAT systems.";
			this.doneTesting = true;
			this.useNAT = true;
			goto IL_19A;
		case (ConnectionTesterStatus)9:
		case (ConnectionTesterStatus)10:
			this.testMessage = "NAT can punchthrough as necessary.";
			this.doneTesting = true;
			this.useNAT = true;
			goto IL_19A;
		}
		this.testMessage = "Error in test routine, got " + this.connectionTestResult;
		IL_19A:
		if (this.doneTesting)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Test Result: ",
				this.testMessage,
				"\nNAT Capability: ",
				this.connectionTestResult,
				"\nProbing Public IP: ",
				this.probingPublicIP,
				"\nDone Testing NAT: ",
				this.doneTesting
			}));
		}
	}

	public string testStatus = "Testing network connection capabilities.";

	private string testMessage = "Test in progress";

	public string shouldEnableNatMessage = string.Empty;

	public bool doneTesting;

	public bool useNAT;

	public bool probingPublicIP;

	public int serverPort = 9999;

	public float timer;

	private ConnectionTesterStatus connectionTestResult = ConnectionTesterStatus.Undetermined;
}
