// dnSpy decompiler from Assembly-CSharp.dll
using System;
using Badumna;

public static class NetworkConfiguration
{
	private static string SeedPeer1
	{
		get
		{
			return NetworkConfiguration.ServerIP + ":" + NetworkConfiguration.ServerPort;
		}
	}

	private static string MatchmakingServerAddress
	{
		get
		{
			return NetworkConfiguration.ServerIP + ":" + NetworkConfiguration.ServerPort;
		}
	}

	public static Options GenerateNetworkConfiguration()
	{
		Options options = new Options();
		ConnectivityModule connectivity = options.Connectivity;
		options.Logger.LogLevel = LogLevel.Information;
		options.Logger.LoggerType = LoggerType.File;
		if (NetworkConfiguration.IsConfigureForLan)
		{
			connectivity.ConfigureForLan();
		}
		connectivity.SeedPeers.Add(NetworkConfiguration.SeedPeer1);
		connectivity.StartPortRange = NetworkConfiguration.StartPortRange;
		connectivity.EndPortRange = NetworkConfiguration.EndPortRange;
		connectivity.ApplicationName = NetworkConfiguration.ApplicationName;
		options.Matchmaking.ServerAddress = NetworkConfiguration.MatchmakingServerAddress;
		options.Save("MyBadumnaOptions.xml");
		return options;
	}

	private static bool IsConfigureForLan;

	private static bool IsBroadcastEnabled;

	private static int StartPortRange = 10001;

	private static int EndPortRange = 10051;

	private static string ApplicationName = "Broforce";

	private static string ServerIP = "ec2-50-112-235-213.us-west-2.compute.amazonaws.com";

	private static int ServerPort = 10000;
}
