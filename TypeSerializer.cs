// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class TypeSerializer
{
	private static TypeSerializer Instance
	{
		get
		{
			if (TypeSerializer.instance == null)
			{
				TypeSerializer.instance = new TypeSerializer();
				TypeSerializer.RegisterTypes();
			}
			return TypeSerializer.instance;
		}
	}

	private static List<Type> GetTypeList()
	{
		Assembly assembly = Assembly.GetExecutingAssembly();
		List<Type> list = new List<Type>(assembly.GetTypes());
		assembly = Assembly.GetAssembly(typeof(Transform));
		list.AddRange(assembly.GetTypes());
		assembly = Assembly.GetAssembly(typeof(bool));
		list.AddRange(assembly.GetTypes());
		return list;
	}

	private static void Register<T>(SerializationMethod<T> serializer, Func<BinaryReader, object> deserializer)
	{
		if (TypeSerializer.Instance.typesHaveBeenRegistered)
		{
			UnityEngine.Debug.LogError("Types can only be registered at the start from RegisterTypes()");
			return;
		}
		TypeSerializer.RegisteredType<T> registeredType = new TypeSerializer.RegisteredType<T>();
		registeredType.type = typeof(T);
		registeredType.serializer = serializer;
		registeredType.deserializer = deserializer;
		TypeSerializer.RegisteredType registeredType2 = registeredType;
		TypeSerializer typeSerializer = TypeSerializer.Instance;
		byte key;
		key = typeSerializer.nextKey;
        //typeSerializer.nextKey = (key ) + 1;
        typeSerializer.nextKey = ++key;
        --key;
		registeredType2.key = key;
		TypeSerializer.Instance.keys_Regtype.Add(registeredType.key, registeredType);
		TypeSerializer.Instance.type_Regtype.Add(registeredType.type, registeredType);
	}

	public static void RegisterTypes()
	{
		TypeSerializer.typeList = TypeSerializer.GetTypeList();
		TypeSerializer.Register<Color>(delegate(Color obj, BinaryWriter stream)
		{
			stream.Write(obj.r);
			stream.Write(obj.g);
			stream.Write(obj.b);
			stream.Write(obj.a);
		}, (BinaryReader stream) => new Color(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle()));
		TypeSerializer.Register<Vector2>(delegate(Vector2 obj, BinaryWriter stream)
		{
			stream.Write(obj.x);
			stream.Write(obj.y);
		}, (BinaryReader stream) => new Vector2(stream.ReadSingle(), stream.ReadSingle()));
		TypeSerializer.Register<Vector3>(delegate(Vector3 obj, BinaryWriter stream)
		{
			stream.Write(obj.x);
			stream.Write(obj.y);
			stream.Write(obj.z);
		}, (BinaryReader stream) => new Vector3(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle()));
		TypeSerializer.Register<Quaternion>(delegate(Quaternion obj, BinaryWriter stream)
		{
			stream.Write(obj.x);
			stream.Write(obj.y);
			stream.Write(obj.z);
			stream.Write(obj.w);
		}, (BinaryReader stream) => new Quaternion(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle()));
		TypeSerializer.Register<string>(delegate(string obj, BinaryWriter stream)
		{
			stream.Write(obj.Length);
			for (int i = 0; i < obj.Length; i++)
			{
				stream.Write(obj[i]);
			}
		}, delegate(BinaryReader stream)
		{
			int count = stream.ReadInt32();
			return new string(stream.ReadChars(count));
		});
		TypeSerializer.Register<NullWrapper>(delegate(NullWrapper obj, BinaryWriter stream)
		{
			TypeSerializer.Serialize<Type>(obj.type, stream);
		}, delegate(BinaryReader stream)
		{
			Type type = (Type)TypeSerializer.Deserialize(stream);
			return new NullWrapper(type);
		});
		TypeSerializer.Register<byte>(delegate(byte obj, BinaryWriter stream)
		{
			stream.Write(obj);
		}, (BinaryReader stream) => stream.ReadByte());
		TypeSerializer.Register<float>(delegate(float obj, BinaryWriter stream)
		{
			stream.Write(obj);
		}, (BinaryReader stream) => stream.ReadSingle());
		TypeSerializer.Register<short>(delegate(short obj, BinaryWriter stream)
		{
			stream.Write(obj);
		}, (BinaryReader stream) => stream.ReadInt16());
		TypeSerializer.Register<int>(delegate(int obj, BinaryWriter stream)
		{
			stream.Write(obj);
		}, (BinaryReader stream) => stream.ReadInt32());
		TypeSerializer.Register<bool>(delegate(bool obj, BinaryWriter stream)
		{
			stream.Write(obj);
		}, (BinaryReader stream) => stream.ReadBoolean());
		TypeSerializer.Register<ulong>(delegate(ulong obj, BinaryWriter stream)
		{
			stream.Write(obj);
		}, (BinaryReader stream) => stream.ReadUInt64());
		TypeSerializer.Register<long>(delegate(long obj, BinaryWriter stream)
		{
			stream.Write(obj);
		}, (BinaryReader stream) => stream.ReadInt64());
		TypeSerializer.Register<uint>(delegate(uint obj, BinaryWriter stream)
		{
			stream.Write(obj);
		}, (BinaryReader stream) => stream.ReadUInt32());
		TypeSerializer.Register<char>(delegate(char obj, BinaryWriter stream)
		{
			stream.Write(obj);
		}, (BinaryReader stream) => stream.ReadChar());
		TypeSerializer.Register<double>(delegate(double obj, BinaryWriter stream)
		{
			stream.Write(obj);
		}, (BinaryReader stream) => stream.ReadDouble());
		TypeSerializer.Register<ParentedPosition>(delegate(ParentedPosition obj, BinaryWriter stream)
		{
			stream.Write(obj.localX);
			stream.Write(obj.localY);
			TypeSerializer.Serialize<Transform>(obj.parent, stream);
		}, delegate(BinaryReader stream)
		{
			float localX = stream.ReadSingle();
			float localY = stream.ReadSingle();
			Transform parent = (Transform)TypeSerializer.Deserialize(stream);
			return new ParentedPosition
			{
				localX = localX,
				localY = localY,
				parent = parent
			};
		});
		TypeSerializer.Register<int[]>(delegate(int[] obj, BinaryWriter stream)
		{
			stream.Write(obj.Length);
			for (int i = 0; i < obj.Length; i++)
			{
				stream.Write(obj[i]);
			}
		}, delegate(BinaryReader stream)
		{
			int num = stream.ReadInt32();
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = stream.ReadInt32();
			}
			return array;
		});
		TypeSerializer.Register<byte[]>(delegate(byte[] obj, BinaryWriter stream)
		{
			stream.Write(obj.Length);
			for (int i = 0; i < obj.Length; i++)
			{
				stream.Write(obj[i]);
			}
		}, delegate(BinaryReader stream)
		{
			int num = stream.ReadInt32();
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = stream.ReadByte();
			}
			return array;
		});
		TypeSerializer.Register<float[]>(delegate(float[] obj, BinaryWriter stream)
		{
			stream.Write(obj.Length);
			for (int i = 0; i < obj.Length; i++)
			{
				stream.Write(obj[i]);
			}
		}, delegate(BinaryReader stream)
		{
			int num = stream.ReadInt32();
			float[] array = new float[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = stream.ReadSingle();
			}
			return array;
		});
		TypeSerializer.Register<bool[]>(delegate(bool[] obj, BinaryWriter stream)
		{
			stream.Write(obj.Length);
			for (int i = 0; i < obj.Length; i++)
			{
				stream.Write(obj[i]);
			}
		}, delegate(BinaryReader stream)
		{
			int num = stream.ReadInt32();
			bool[] array = new bool[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = stream.ReadBoolean();
			}
			return array;
		});
		TypeSerializer.Register<double[]>(delegate(double[] obj, BinaryWriter stream)
		{
			stream.Write(obj.Length);
			for (int i = 0; i < obj.Length; i++)
			{
				stream.Write(obj[i]);
			}
		}, delegate(BinaryReader stream)
		{
			int num = stream.ReadInt32();
			double[] array = new double[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = stream.ReadDouble();
			}
			return array;
		});
		TypeSerializer.Register<MessageInfo>(delegate(MessageInfo obj, BinaryWriter stream)
		{
			stream.Write(obj.TimeStamp);
			stream.Write((byte)obj.Sender);
			stream.Write((byte)obj.Destination);
		}, delegate(BinaryReader stream)
		{
			double timeStamp = stream.ReadDouble();
			PID sender = (PID)stream.ReadByte();
			PID destination = (PID)stream.ReadByte();
			return new MessageInfo(sender, destination, timeStamp);
		});
		TypeSerializer.Register<NonStaticRPCObject>(delegate(NonStaticRPCObject obj, BinaryWriter stream)
		{
			TypeSerializer.Serialize<NID>(obj.targetID, stream);
			TypeSerializer.Serialize<SID>(obj.sessionID, stream);
			TypeSerializer.Serialize<MethodInfo>(obj.methodInfo, stream);
			stream.Write(obj.executeImmediately);
			TypeSerializer.Serialize<byte[]>(obj.parametersasBytes, stream);
			TypeSerializer.Serialize<MessageInfo>(obj.messageInfo, stream);
		}, delegate(BinaryReader stream)
		{
			NonStaticRPCObject nonStaticRPCObject = new NonStaticRPCObject();
			nonStaticRPCObject.targetID = (NID)TypeSerializer.Deserialize(stream);
			nonStaticRPCObject.sessionID = (SID)TypeSerializer.Deserialize(stream);
			nonStaticRPCObject.methodInfo = (MethodInfo)TypeSerializer.Deserialize(stream);
			nonStaticRPCObject.executeImmediately = stream.ReadBoolean();
			RPCController.CurrentRPCName = nonStaticRPCObject.methodInfo.Name;
			nonStaticRPCObject.parametersasBytes = (byte[])TypeSerializer.Deserialize(stream);
			nonStaticRPCObject.messageInfo = (MessageInfo)TypeSerializer.Deserialize(stream);
			return nonStaticRPCObject;
		});
		TypeSerializer.Register<StaticRPCObject>(delegate(StaticRPCObject obj, BinaryWriter stream)
		{
			TypeSerializer.Serialize<MethodInfo>(obj.methodInfo, stream);
			stream.Write(obj.executeImmediately);
			TypeSerializer.Serialize<SID>(obj.sessionID, stream);
			TypeSerializer.Serialize<byte[]>(obj.parametersasBytes, stream);
			TypeSerializer.Serialize<MessageInfo>(obj.messageInfo, stream);
		}, (BinaryReader stream) => new StaticRPCObject
		{
			methodInfo = (MethodInfo)TypeSerializer.Deserialize(stream),
			executeImmediately = stream.ReadBoolean(),
			sessionID = (SID)TypeSerializer.Deserialize(stream),
			parametersasBytes = (byte[])TypeSerializer.Deserialize(stream),
			messageInfo = (MessageInfo)TypeSerializer.Deserialize(stream)
		});
		TypeSerializer.Register<object>(delegate(object obj, BinaryWriter stream)
		{
			NID nid = Registry.GetNID(obj);
			if (nid == NID.NoID && !(obj is HiddenExplosives) && obj != null && !obj.Equals(null))
			{
				UnityEngine.Debug.LogWarning("NoID for " + obj);
			}
			TypeSerializer.Serialize<NID>(nid, stream);
		}, delegate(BinaryReader stream)
		{
			NID nid = (NID)TypeSerializer.Deserialize(stream);
			object result = null;
			if (Registry.Components.ContainsKey(nid))
			{
				result = Registry.Components[nid];
			}
			else if (Registry.DestroyedMapObjects.Contains(nid))
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"Object has already been destroyed ",
					nid,
					" ",
					RPCController.CurrentRPC
				}));
				return null;
			}
			return result;
		});
		TypeSerializer.Register<UnityStream>(delegate(UnityStream obj, BinaryWriter stream)
		{
			byte[] byteArray = obj.ByteArray;
			TypeSerializer.Serialize<byte[]>(byteArray, stream);
		}, delegate(BinaryReader stream)
		{
			byte[] byteArray = (byte[])TypeSerializer.Deserialize(stream);
			return new UnityStream(byteArray);
		});
		TypeSerializer.Register<Type>(delegate(Type obj, BinaryWriter stream)
		{
			int num = TypeSerializer.typeList.IndexOf(obj);
			if (num < 0)
			{
				UnityEngine.Debug.LogError("Type not found " + obj);
			}
			stream.Write((short)num);
		}, delegate(BinaryReader stream)
		{
			short num = stream.ReadInt16();
			if (num > 0 && (int)num <= TypeSerializer.typeList.Count)
			{
				return TypeSerializer.typeList[(int)num];
			}
			UnityEngine.Debug.LogError("Type cold not be indexed " + num);
			return null;
		});
		TypeSerializer.Register<SID>(delegate(SID obj, BinaryWriter stream)
		{
			stream.Write(obj.AsByte);
		}, (BinaryReader stream) => new SID(stream.ReadByte()));
		TypeSerializer.Register<PID>(delegate(PID obj, BinaryWriter stream)
		{
			stream.Write(obj.AsByte);
		}, delegate(BinaryReader stream)
		{
			PID pid = new PID(stream.ReadByte());
			if (pid == PID.MyID)
			{
				return PID.MyID;
			}
			return pid;
		});
		TypeSerializer.Register<NID>(delegate(NID obj, BinaryWriter stream)
		{
			stream.Write((byte)obj.Sid);
			stream.Write((byte)obj.OwnerID);
			stream.Write(obj.GameObjectID);
			stream.Write(obj.NestedID);
			stream.Write(obj.ComponentID);
			if (Application.isEditor)
			{
				stream.Write(obj.objName);
			}
		}, delegate(BinaryReader stream)
		{
			SID sid = (SID)stream.ReadByte();
			PID ownerID = (PID)stream.ReadByte();
			uint goID = stream.ReadUInt32();
			byte nestedID = stream.ReadByte();
			byte componentID = stream.ReadByte();
			NID nid = new NID(sid, ownerID, goID, nestedID, componentID);
			if (Application.isEditor)
			{
				nid.objName = stream.ReadString();
			}
			return nid;
		});
		TypeSerializer.Register<Enum>(delegate(Enum obj, BinaryWriter stream)
		{
			Type type = obj.GetType();
			string fullName = type.FullName;
			string fullName2 = type.Assembly.FullName;
			int value = Convert.ToInt32(obj);
			stream.Write(fullName);
			stream.Write(fullName2);
			stream.Write(value);
		}, delegate(BinaryReader stream)
		{
			string typeName = stream.ReadString();
			string assemblyName = stream.ReadString();
			int value = stream.ReadInt32();
			//Type type = Types.GetType(typeName, assemblyName);
            Type type = System.Reflection.Assembly.Load(assemblyName).GetType(typeName);
			return (Enum)Enum.ToObject(type, value);
		});
		TypeSerializer.Register<DamageObject>(delegate(DamageObject obj, BinaryWriter stream)
		{
			stream.Write(obj.damage);
			stream.Write((int)obj.damageType);
			stream.Write(obj.xForce);
			stream.Write(obj.yForce);
			TypeSerializer.Serialize<MonoBehaviour>(obj.damageSender, stream);
		}, delegate(BinaryReader stream)
		{
			int damage = stream.ReadInt32();
			DamageType damageType = (DamageType)stream.ReadInt32();
			float xI = stream.ReadSingle();
			float yI = stream.ReadSingle();
			MonoBehaviour damageSender = (MonoBehaviour)TypeSerializer.Deserialize(stream);
			return new DamageObject(damage, damageType, xI, yI, damageSender);
		});
		TypeSerializer.Register<MethodInfo>(delegate(MethodInfo obj, BinaryWriter stream)
		{
			short methodIndex = TypeSerializer.GetMethodIndex(obj);
			stream.Write(methodIndex);
			TypeSerializer.Serialize<Type>(obj.DeclaringType, stream);
		}, delegate(BinaryReader stream)
		{
			short index = stream.ReadInt16();
			Type type = (Type)TypeSerializer.Deserialize(stream);
			return TypeSerializer.GetMethodInfo(type, index);
		});
		TypeSerializer.Register<GridPos>(delegate(GridPos obj, BinaryWriter stream)
		{
			stream.Write(obj.c);
			stream.Write(obj.r);
		}, (BinaryReader stream) => new GridPos(stream.ReadInt32(), stream.ReadInt32()));
		TypeSerializer.Register<Ack>(delegate(Ack obj, BinaryWriter stream)
		{
			stream.Write(obj.AckID);
			stream.Write(obj.IsResponse);
		}, delegate(BinaryReader stream)
		{
			int id = stream.ReadInt32();
			bool isResponse = stream.ReadBoolean();
			return new Ack(id, isResponse);
		});
		TypeSerializer.Register<ActionObject>(delegate(ActionObject obj, BinaryWriter stream)
		{
			stream.Write((byte)obj.type);
			if (!object.ReferenceEquals(obj.gridPoint, null))
			{
				stream.Write(true);
				stream.Write((byte)obj.gridPoint.collumn);
				stream.Write((byte)obj.gridPoint.row);
			}
			else
			{
				stream.Write(false);
			}
			stream.Write(obj.duration);
		}, (BinaryReader stream) => new ActionObject((EnemyActionType)stream.ReadByte(), (!stream.ReadBoolean()) ? null : new GridPoint((int)stream.ReadByte(), (int)stream.ReadByte()), stream.ReadSingle()));
		TypeSerializer.Register<State>(delegate(State obj, BinaryWriter stream)
		{
			TypeSerializer.Serialize<NID>(obj.Key, stream);
			stream.Write(obj.TimeStamp);
			byte[] obj2 = TypeSerializer.SerializeParameterList(obj.Types, obj.Values);
			TypeSerializer.Serialize<byte[]>(obj2, stream);
		}, delegate(BinaryReader stream)
		{
			NID key = (NID)TypeSerializer.Deserialize(stream);
			float timeStamp = stream.ReadSingle();
			byte[] byteArray = (byte[])TypeSerializer.Deserialize(stream);
			object[] values = TypeSerializer.DeserializeParameterList(byteArray, null);
			return new State(key, timeStamp, values);
		});
		TypeSerializer.Instance.typesHaveBeenRegistered = true;
	}

	public static BinaryWriter SerializeGeneral<T>(T obj, TypeSerializer.RegisteredType regTypeAbstract, BinaryWriter writer)
	{
		if (TypeSerializer.DebugMode)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				regTypeAbstract,
				"   obj ",
				obj,
				" ",
				typeof(T),
				" ",
				obj.GetType()
			}));
		}
		TypeSerializer.RegisteredType<Enum> registeredType = regTypeAbstract as TypeSerializer.RegisteredType<Enum>;
		if (registeredType != null)
		{
			if (TypeSerializer.DebugMode)
			{
				UnityEngine.Debug.Log("Serialize as Enum");
			}
			Enum obj2 = obj as Enum;
			registeredType.serializer(obj2, writer);
			return writer;
		}
		TypeSerializer.RegisteredType<T> registeredType2 = regTypeAbstract as TypeSerializer.RegisteredType<T>;
		if (registeredType2 != null)
		{
			try
			{
				if (TypeSerializer.DebugMode)
				{
					UnityEngine.Debug.Log("Serialize as T");
				}
				registeredType2.serializer(obj, writer);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(obj + "  " + ex.Message);
				UnityEngine.Debug.LogError(registeredType2 + "  " + ex.Message);
				UnityEngine.Debug.LogError(regTypeAbstract + "  " + ex.Message);
				UnityEngine.Debug.LogException(ex);
			}
		}
		TypeSerializer.RegisteredType<object> registeredType3 = regTypeAbstract as TypeSerializer.RegisteredType<object>;
		if (registeredType3 != null)
		{
			if (TypeSerializer.DebugMode)
			{
				UnityEngine.Debug.Log("Serialize as object");
			}
			object obj3 = obj;
			registeredType3.serializer(obj3, writer);
			return writer;
		}
		return writer;
	}

	private static TypeSerializer.RegisteredType GetRegType(Type type)
	{
		if (TypeSerializer.Instance.type_Regtype.ContainsKey(type))
		{
			return TypeSerializer.Instance.type_Regtype[type];
		}
		if (type.IsEnum)
		{
			return TypeSerializer.Instance.type_Regtype[typeof(Enum)];
		}
		return TypeSerializer.Instance.type_Regtype[typeof(object)];
	}

	public static BinaryWriter Serialize<T>(T obj, BinaryWriter writer = null)
	{
		if (obj == null)
		{
			NullWrapper obj2 = new NullWrapper(typeof(T));
			if (obj2.type != null)
			{
				TypeSerializer.Serialize<NullWrapper>(obj2, writer);
				return writer;
			}
		}
		if (typeof(T).IsArray && !(obj is byte[]))
		{
			return TypeSerializer.SerializeArray(obj as Array, writer);
		}
		if (writer == null)
		{
			writer = new BinaryWriter(new MemoryStream());
		}
		TypeSerializer.RegisteredType regType = TypeSerializer.GetRegType(typeof(T));
		if (regType != null)
		{
			writer.Write(regType.key);
			return TypeSerializer.SerializeGeneral<T>(obj, regType, writer);
		}
		return writer;
	}

	public static BinaryWriter SerializeArray(Array array, BinaryWriter writer = null)
	{
		if (writer == null)
		{
			writer = new BinaryWriter(new MemoryStream());
		}
		writer.Write(1);
		Type elementType = array.GetType().GetElementType();
		TypeSerializer.RegisteredType regType = TypeSerializer.GetRegType(elementType);
		writer.Write(regType.key);
		writer.Write(array.Length);
		TypeSerializer.Serialize<Type>(elementType, writer);
		MethodInfo methodInfo = typeof(TypeSerializer).GetMethod("SerializeGeneral").MakeGenericMethod(new Type[]
		{
			elementType
		});
		if (array.Length > 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				methodInfo.Invoke(null, new object[]
				{
					array.GetValue(i),
					regType,
					writer
				});
			}
		}
		return writer;
	}

	public static byte[] ObjectToByteArray<T>(T Obj)
	{
		UnityStream unityStream = new UnityStream();
		unityStream.Serialize<T>(Obj);
		return unityStream.ByteArray;
	}

	public static string SerializeToString<T>(T Obj)
	{
		UnityStream unityStream = new UnityStream();
		unityStream.Serialize<T>(Obj);
		return Convert.ToBase64String(unityStream.ByteArray);
	}

	public static object DeserializefromString(string str)
	{
		byte[] byteArray = Convert.FromBase64String(str);
		UnityStream unityStream = new UnityStream(byteArray);
		return unityStream.DeserializeNext();
	}

	public static object BytesToObject(byte[] byteArray)
	{
		UnityStream unityStream = new UnityStream(byteArray);
		return unityStream.DeserializeNext();
	}

	public static object Deserialize(BinaryReader reader)
	{
		byte b = reader.ReadByte();
		if (b == 1)
		{
			return TypeSerializer.DeserializeArray(reader);
		}
		if (!TypeSerializer.Instance.keys_Regtype.ContainsKey(b))
		{
			UnityEngine.Debug.LogError("Type not registered for serialization: " + b);
			return null;
		}
		TypeSerializer.RegisteredType registeredType = TypeSerializer.Instance.keys_Regtype[b];
		if (TypeSerializer.DebugMode)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Deserialize ",
				b,
				" ",
				registeredType
			}));
		}
		object obj = registeredType.deserializer(reader);
		if (obj is NullWrapper)
		{
			Type type = ((NullWrapper)obj).type;
			return Convert.ChangeType(null, type);
		}
		return obj;
	}

	public static object DeserializeArray(BinaryReader reader)
	{
		byte key = reader.ReadByte();
		int num = reader.ReadInt32();
		Type elementType = (Type)TypeSerializer.Deserialize(reader);
		TypeSerializer.RegisteredType registeredType = TypeSerializer.Instance.keys_Regtype[key];
		Array array = Array.CreateInstance(elementType, num);
		for (int i = 0; i < num; i++)
		{
			array.SetValue(registeredType.deserializer(reader), i);
		}
		return array;
	}

	public static byte[] SerializeParameterList(Type[] types, object[] parameters)
	{
		MemoryStream memoryStream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(memoryStream);
		for (int i = 0; i < parameters.Length; i++)
		{
			try
			{
				Type type = types[i];
				if (parameters[i] != null)
				{
					type = parameters[i].GetType();
				}
				TypeSerializer.SerializeObjectWithType(parameters[i], type, writer);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
		return memoryStream.ToArray();
	}

	private static void SerializeObjectWithType(object obj, Type type, BinaryWriter writer = null)
	{
		MethodInfo methodInfo = typeof(TypeSerializer).GetMethod("Serialize").MakeGenericMethod(new Type[]
		{
			type
		});
		methodInfo.Invoke(null, new object[]
		{
			obj,
			writer
		});
	}

	public static object[] DeserializeParameterList(byte[] byteArray, NetworkMessageInfo? info = null)
	{
		List<object> list = new List<object>();
		MemoryStream input = new MemoryStream(byteArray);
		BinaryReader binaryReader = new BinaryReader(input);
		while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
		{
			object item = TypeSerializer.Deserialize(binaryReader);
			list.Add(item);
		}
		if (info != null)
		{
			list.Add(info.Value);
		}
		return list.ToArray();
	}

	public static Type GetType(string TypeName)
	{
		Type type = Type.GetType(TypeName);
		if (type != null)
		{
			return type;
		}
		string assemblyString = TypeName.Substring(0, TypeName.IndexOf('.'));
		Assembly assembly = Assembly.Load(assemblyString);
		if (assembly == null)
		{
			return null;
		}
		return assembly.GetType(TypeName);
	}

	private static short GetMethodIndex(MethodInfo methodInfo)
	{
		Type declaringType = methodInfo.DeclaringType;
		MethodInfo[] methods = declaringType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		if (methods.Length > 32767)
		{
			UnityEngine.Debug.Log("Short is not large enought to store all possibilites");
		}
		for (int i = 0; i < methods.Length; i++)
		{
			if (methodInfo == methods[i])
			{
				return (short)i;
			}
		}
		UnityEngine.Debug.Log("Method was not found " + methodInfo);
		return -1;
	}

	private static MethodInfo GetMethodInfo(Type type, short index)
	{
		MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		if (index >= 0 && (int)index < methods.Length)
		{
			return methods[(int)index];
		}
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"index ",
			index,
			" is out of range for type ",
			type
		}));
		return null;
	}

	public static byte GetByteFromBoolArray(bool[] boolArray)
	{
		BitArray bitArray = new BitArray(boolArray);
		return (byte)TypeSerializer.GetIntFromBitArray(bitArray);
	}

	public static short GetShortFromBoolArray(bool[] boolArray)
	{
		BitArray bitArray = new BitArray(boolArray);
		return (short)TypeSerializer.GetIntFromBitArray(bitArray);
	}

	public static int GetIntFromBitArray(BitArray bitArray)
	{
		if (bitArray.Length > 16)
		{
			throw new ArgumentException("Argument length shall be at most 32 bits.");
		}
		int[] array = new int[1];
		bitArray.CopyTo(array, 0);
		return array[0];
	}

	public static bool[] BoolArrayFromInt(int asInt)
	{
		int[] values = new int[]
		{
			asInt
		};
		BitArray bitArray = new BitArray(values);
		bool[] array = new bool[bitArray.Length];
		bitArray.CopyTo(array, 0);
		return array;
	}

	public static bool[] GetBoolArrayFromShort(short s)
	{
		return TypeSerializer.BoolArrayFromInt((int)s);
	}

	public static bool[] GetBoolArrayFromByte(byte b)
	{
		return TypeSerializer.BoolArrayFromInt((int)b);
	}

	public const byte NoKey = 0;

	public const byte ArrayKey = 1;

	private Dictionary<byte, TypeSerializer.RegisteredType> keys_Regtype = new Dictionary<byte, TypeSerializer.RegisteredType>();

	private Dictionary<Type, TypeSerializer.RegisteredType> type_Regtype = new Dictionary<Type, TypeSerializer.RegisteredType>();

	private bool typesHaveBeenRegistered;

	public static bool DebugMode = false;

	private static List<Type> typeList = new List<Type>();

	private byte nextKey = 2;

	private static TypeSerializer instance;

	public abstract class RegisteredType
	{
		public Func<BinaryReader, object> deserializer;

		public byte key;

		public Type type;
	}

	private class RegisteredType<T> : TypeSerializer.RegisteredType
	{
		public SerializationMethod<T> serializer;
	}
}
