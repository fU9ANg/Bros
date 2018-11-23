// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ConnectionTester : SingletonMono<ConnectionTester>
{
	public static string Status
	{
		get
		{
			if (SingletonMono<ConnectionTester>.Instance == null)
			{
				return "No connection Tester in scene";
			}
			return SingletonMono<ConnectionTester>.Instance.testMessage;
		}
	}

	private void Update()
	{
		this.TestConnection();
	}

	private void TestConnection()
	{
		this.connectionTestResult = Network.TestConnection();
		ConnectionTesterStatus connectionTesterStatus = this.connectionTestResult;
		switch (connectionTesterStatus + 2)
		{
		case ConnectionTesterStatus.PrivateIPNoNATPunchthrough:
			this.testMessage = "Problem determining NAT capabilities";
			return;
		case ConnectionTesterStatus.PrivateIPHasNATPunchThrough:
			this.testMessage = "Testing network connection capabilities";
			return;
		case ConnectionTesterStatus.PublicIPNoServerStarted:
			this.testMessage = "Directly connectable public IP address.";
			return;
		case ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted:
			this.testMessage = "Non-connectable public IP address (port  blocked), running a server is impossible.";
			if (!this.probingPublicIP)
			{
				this.connectionTestResult = Network.TestConnectionNAT();
				this.probingPublicIP = true;
				this.testMessage = "Testing if blocked public IP can be circumvented";
				this.timer = Time.time + 10f;
			}
			else if (Time.time > this.timer)
			{
				this.probingPublicIP = false;
			}
			return;
		case ConnectionTesterStatus.LimitedNATPunchthroughSymmetric:
			this.testMessage = "Public IP address but server not initialized, it must be started to check server accessibility. Restart connection test when ready.";
			return;
		case ConnectionTesterStatus.NATpunchthroughFullCone:
			this.testMessage = "Limited NAT punchthrough capabilities. Cannot connect to all types of NAT servers. Running a server is ill advised as not everyone can connect.";
			return;
		case ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone:
			this.testMessage = "Limited NAT punchthrough capabilities. Cannot connect to all types of NAT servers. Running a server is ill advised as not everyone can connect.";
			return;
		case (ConnectionTesterStatus)9:
		case (ConnectionTesterStatus)10:
			this.testMessage = "NAT punchthrough capable. Can connect to all servers and receive connections from all clients. Enabling NAT punchthrough functionality.";
			return;
		}
		this.testMessage = "Error in test routine, got " + this.connectionTestResult;
	}

	private string testMessage = "Testing network connection capabilities";

	private bool probingPublicIP;

	private ConnectionTesterStatus connectionTestResult = ConnectionTesterStatus.Undetermined;

	private float timer;
}
