// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public static class Networking
{
	public static bool StreamIsPaused
	{
		get
		{
			return Networking.streamIsPaused;
		}
		set
		{
			RPCBatcher.FlushQueue();
			Networking.streamIsPaused = value;
		}
	}

	public static int RandomSeed
	{
		get
		{
			return Networking.randomSeed;
		}
	}

	public static void SetSeed(int newSeed)
	{
		Networking.randomSeed = newSeed;
	}

	public static T Instantiate<T>(T Prefab, object[] instantiationData, bool executeImmediately = false) where T : UnityEngine.Object
	{
		return InstantiationController.NetworkedInstantiate<T>(PID.MyID, Prefab, Vector3.zero, Quaternion.identity, instantiationData, false, executeImmediately);
	}

	public static T Instantiate<T>(T Prefab, Vector3 position, Quaternion rotation, object[] instantiationData, bool executeImmediately = false) where T : UnityEngine.Object
	{
		return InstantiationController.NetworkedInstantiate<T>(PID.MyID, Prefab, position, rotation, instantiationData, false, executeImmediately);
	}

	public static T Instantiate<T>(T Prefab, Vector3 position, Quaternion rotation, bool executeImmediately = false) where T : UnityEngine.Object
	{
		return InstantiationController.NetworkedInstantiate<T>(PID.MyID, Prefab, position, rotation, null, false, executeImmediately);
	}

	public static T InstantiateBuffered<T>(T Prefab, Vector3 position, Quaternion rotation, object[] instantiationData, bool executeImmediately = false) where T : UnityEngine.Object
	{
		return InstantiationController.NetworkedInstantiate<T>(PID.MyID, Prefab, position, rotation, instantiationData, true, executeImmediately);
	}

	public static T InstantiateBuffered<T>(T Prefab, Vector3 position, Quaternion rotation, bool executeImmediately = false) where T : UnityEngine.Object
	{
		return InstantiationController.NetworkedInstantiate<T>(PID.MyID, Prefab, position, rotation, null, true, executeImmediately);
	}

	public static T InstantiateBuffered<T>(T Prefab, object[] instantiationData, bool executeImmediately = false) where T : UnityEngine.Object
	{
		return InstantiationController.NetworkedInstantiate<T>(PID.MyID, Prefab, Vector3.zero, Quaternion.identity, instantiationData, true, executeImmediately);
	}

	public static T InstantiateSceneOwned<T>(T Prefab, Vector3 position, Quaternion rotation, object[] instantiationData, bool executeImmediately = false) where T : UnityEngine.Object
	{
		return InstantiationController.NetworkedInstantiate<T>(PID.TargetServer, Prefab, position, rotation, instantiationData, true, executeImmediately);
	}

	public static T InstantiateSceneOwned<T>(T Prefab, Vector3 position, Quaternion rotation, bool executeImmediately = false) where T : UnityEngine.Object
	{
		return InstantiationController.NetworkedInstantiate<T>(PID.TargetServer, Prefab, position, rotation, null, true, executeImmediately);
	}

	public static T InstantiateSceneOwned<T>(T Prefab, object[] instantiationData, bool executeImmediately = false) where T : UnityEngine.Object
	{
		return InstantiationController.NetworkedInstantiate<T>(PID.TargetServer, Prefab, Vector3.zero, Quaternion.identity, instantiationData, true, executeImmediately);
	}

	public static void UnreliableRPC(PID target, bool immediate, bool ignoreSessionID, RpcSignature method)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method();
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, false, ignoreSessionID, new object[0]);
		}
	}

	public static void UnreliableRPC<T1>(PID target, bool immediate, bool ignoreSessionID, RpcSignature<T1> method, T1 arg1)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, false, ignoreSessionID, new object[]
			{
				arg1
			});
		}
	}

	public static void UnreliableRPC<T1, T2>(PID target, bool immediate, bool ignoreSessionID, RpcSignature<T1, T2> method, T1 arg1, T2 arg2)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, false, ignoreSessionID, new object[]
			{
				arg1,
				arg2
			});
		}
	}

	public static void RPC(PID target, bool immediate, bool ignoreSessionID, RpcSignature method)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method();
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, ignoreSessionID, new object[0]);
		}
	}

	public static void RPC<T1>(PID target, bool immediate, bool ignoreSessionID, RpcSignature<T1> method, T1 arg1)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, ignoreSessionID, new object[]
			{
				arg1
			});
		}
	}

	public static void RPC<T1, T2>(PID target, bool immediate, bool ignoreSessionID, RpcSignature<T1, T2> method, T1 arg1, T2 arg2)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, ignoreSessionID, new object[]
			{
				arg1,
				arg2
			});
		}
	}

	public static void RPC<T1, T2, T3>(PID target, bool immediate, bool ignoreSessionID, bool addExecutionDelay, RpcSignature<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, addExecutionDelay, immediate, true, ignoreSessionID, new object[]
			{
				arg1,
				arg2,
				arg3
			});
		}
	}

	public static void RPC<T1, T2, T3, T4>(PID target, bool immediate, bool ignoreSessionID, RpcSignature<T1, T2, T3, T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, ignoreSessionID, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5>(PID target, bool immediate, bool ignoreSessionID, RpcSignature<T1, T2, T3, T4, T5> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, ignoreSessionID, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5, T6>(PID target, bool immediate, bool ignoreSessionID, RpcSignature<T1, T2, T3, T4, T5, T6> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, ignoreSessionID, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5, T6, T7>(PID target, bool immediate, bool ignoreSessionID, RpcSignature<T1, T2, T3, T4, T5, T6, T7> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, ignoreSessionID, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6,
				arg7
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5, T6, T7, T8>(PID target, bool immediate, bool ignoreSessionID, RpcSignature<T1, T2, T3, T4, T5, T6, T7, T8> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, ignoreSessionID, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6,
				arg7,
				arg8
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5, T6, T7, T8, T9>(PID target, bool immediate, bool ignoreSessionID, RpcSignature<T1, T2, T3, T4, T5, T6, T7, T8, T9> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, ignoreSessionID, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6,
				arg7,
				arg8,
				arg9
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(PID target, bool immediate, bool ignoreSessionID, RpcSignature<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, ignoreSessionID, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6,
				arg7,
				arg8,
				arg9,
				arg10
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(PID target, bool immediate, bool ignoreSessionID, RpcSignature<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, ignoreSessionID, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6,
				arg7,
				arg8,
				arg9,
				arg10,
				arg11
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(PID target, bool immediate, bool ignoreSessionID, RpcSignature<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, ignoreSessionID, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6,
				arg7,
				arg8,
				arg9,
				arg10,
				arg11,
				arg12
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(PID target, bool immediate, bool ignoreSessionID, RpcSignature<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, ignoreSessionID, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6,
				arg7,
				arg8,
				arg9,
				arg10,
				arg11,
				arg12,
				arg13
			});
		}
	}

	public static void RPC(PID target, RpcSignature method, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method();
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[0]);
		}
	}

	public static void RPC<T1>(PID target, RpcSignature<T1> method, T1 arg1, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1
			});
		}
	}

	public static void RPC<T1, T2>(PID target, RpcSignature<T1, T2> method, T1 arg1, T2 arg2, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2
			});
		}
	}

	public static void RPC<T1, T2, T3>(PID target, RpcSignature<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2,
				arg3
			});
		}
	}

	public static void RPC<T1, T2, T3, T4>(PID target, RpcSignature<T1, T2, T3, T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5>(PID target, RpcSignature<T1, T2, T3, T4, T5> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5, T6>(PID target, RpcSignature<T1, T2, T3, T4, T5, T6> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5, T6, T7>(PID target, RpcSignature<T1, T2, T3, T4, T5, T6, T7> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6,
				arg7
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5, T6, T7, T8>(PID target, RpcSignature<T1, T2, T3, T4, T5, T6, T7, T8> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6,
				arg7,
				arg8
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5, T6, T7, T8, T9>(PID target, RpcSignature<T1, T2, T3, T4, T5, T6, T7, T8, T9> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6,
				arg7,
				arg8,
				arg9
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(PID target, RpcSignature<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6,
				arg7,
				arg8,
				arg9,
				arg10,
				arg11,
				arg12
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(PID target, RpcSignature<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6,
				arg7,
				arg8,
				arg9,
				arg10,
				arg11,
				arg12,
				arg13
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(PID target, RpcSignature<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6,
				arg7,
				arg8,
				arg9,
				arg10,
				arg11,
				arg12,
				arg13,
				arg14
			});
		}
	}

	public static void RPC<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(PID target, RpcSignature<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6,
				arg7,
				arg8,
				arg9,
				arg10,
				arg11,
				arg12,
				arg13,
				arg14,
				arg15
			});
		}
	}

	public static void RPC2<T0>(PID target, Func<T0> method, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method();
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[0]);
		}
	}

	public static void RPC2<T1, T0>(PID target, Func<T1, T0> method, T1 arg1, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1
			});
		}
	}

	public static void RPC2<T1, T2, T0>(PID target, Func<T1, T2, T0> method, T1 arg1, T2 arg2, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2
			});
		}
	}

	public static void RPC2<T1, T2, T3, T0>(PID target, Func<T1, T2, T3, T0> method, T1 arg1, T2 arg2, T3 arg3, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2,
				arg3
			});
		}
	}

	public static void RPC2<T1, T2, T3, T4, T0>(PID target, Func<T1, T2, T3, T4, T0> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4
			});
		}
	}

	public static void RPC2<T1, T2, T3, T4, T5, T0>(PID target, Networking.Func<T1, T2, T3, T4, T5, T0> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5
			});
		}
	}

	public static void RPC2<T1, T2, T3, T4, T5, T6, T0>(PID target, Networking.Func<T1, T2, T3, T4, T5, T6, T0> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6
			});
		}
	}

	public static void RPC2<T1, T2, T3, T4, T5, T6, T7, T0>(PID target, Networking.Func<T1, T2, T3, T4, T5, T6, T7, T0> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, bool immediate = false)
	{
		if (Connect.BypassNetworkLayer)
		{
			if (RPCController.MustExecute(target))
			{
				method(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			}
		}
		else
		{
			RPCController.SendRPC(target, method.Target, method.Method, false, immediate, true, false, new object[]
			{
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6,
				arg7
			});
		}
	}

	private static bool streamIsPaused;

	private static int randomSeed;

	public delegate TResult Func<T1, T2, T3, T4, T5, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

	public delegate TResult Func<T1, T2, T3, T4, T5, T6, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

	public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

	public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
}
