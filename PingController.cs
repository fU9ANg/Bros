// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class PingController : SingletonMono<PingController>
{
	public void Reset()
	{
		this.pingSamplers.Clear();
	}

	public static void Ping(PID requestee, double timeStamp)
	{
		if (SingletonMono<NetworkTimeSync>.Instance.TimeHasBeenSet || PID.IamServer())
		{
			Networking.RPC<PID, double>(requestee, true, false, new RpcSignature<PID, double>(SingletonMono<PingController>.Instance.Pong), PID.MyID, timeStamp);
		}
	}

	private void Pong(PID respondee, double timeStamp)
	{
		if (!this.pingSamplers.ContainsKey(respondee))
		{
			this.pingSamplers.Add(respondee, new PingController.PingSampler());
		}
		double num = (Connect.Time - timeStamp) / 2.0;
		this.pingSamplers[respondee].AddSample((float)num);
	}

	public static float GetPing(PID playerID)
	{
		if (!SingletonMono<PingController>.Instance.pingSamplers.ContainsKey(playerID))
		{
			return -1f;
		}
		return SingletonMono<PingController>.Instance.pingSamplers[playerID].Ping;
	}

	public static float GetRawPing(PID playerID)
	{
		if (!SingletonMono<PingController>.Instance.pingSamplers.ContainsKey(playerID))
		{
			return 0f;
		}
		return SingletonMono<PingController>.Instance.pingSamplers[playerID].RawPing;
	}

	public static float GetInterpolationOffset(PID playerID)
	{
		if (playerID == PID.MyID)
		{
			return 0f;
		}
		if (!SingletonMono<PingController>.Instance.pingSamplers.ContainsKey(playerID))
		{
			return 0f;
		}
		float num = SingletonMono<PingController>.Instance.pingSamplers[playerID].InterpolationOffset + SyncController.SendInterval * 2f;
		if ((double)num > 3.5)
		{
			return 3.5f;
		}
		return num;
	}

	public static PingController.PingSampler GetSampler(PID playerID)
	{
		if (SingletonMono<PingController>.Instance.pingSamplers.ContainsKey(playerID))
		{
			return SingletonMono<PingController>.Instance.pingSamplers[playerID];
		}
		return new PingController.PingSampler();
	}

	private void Update()
	{
		this.pingSendTimer -= NetworkTimeSync.RealDeltaTime;
		this.pingSendTimer = Mathf.Max(0f, this.pingSendTimer);
		if (this.pingSendTimer <= 0f)
		{
			if (SingletonMono<NetworkTimeSync>.Instance.TimeHasBeenSet || PID.IamServer())
			{
				Networking.RPC<PID, double>(PID.TargetAll, true, false, new RpcSignature<PID, double>(PingController.Ping), PID.MyID, Connect.Time);
			}
			this.pingSendTimer += 1f;
		}
		foreach (PingController.PingSampler pingSampler in this.pingSamplers.Values)
		{
			pingSampler.LerpToTarget();
		}
		this.DrawSamples();
	}

	private void DrawSamples()
	{
		Vector3 vector = Vector3.up * 5f;
		foreach (PingController.PingSampler pingSampler in this.pingSamplers.Values)
		{
			vector = pingSampler.DebugDraw(vector);
			vector += Vector3.right * 1f;
		}
	}

	private Dictionary<PID, PingController.PingSampler> pingSamplers = new Dictionary<PID, PingController.PingSampler>();

	private float pingSendTimer;

	public class PingSampler
	{
		public float Ping
		{
			get
			{
				return this.ping;
			}
		}

		public float RawPing
		{
			get
			{
				return this.rawPing;
			}
		}

		public float InterpolationOffset
		{
			get
			{
				return this.interpolationOffset;
			}
		}

		private bool ShouldPrintWarning()
		{
			if (Time.realtimeSinceStartup - this.lastWarningTime > 2f)
			{
				this.lastWarningTime = Time.realtimeSinceStartup;
				return true;
			}
			return false;
		}

		public void AddSample(float newSample)
		{
			this.rawPing = newSample;
			if (newSample < 0f)
			{
				if (this.ShouldPrintWarning())
				{
					UnityEngine.Debug.LogWarning("Discarding negative sample " + newSample);
				}
				return;
			}
			if (newSample > 5f)
			{
				if (this.ShouldPrintWarning())
				{
					UnityEngine.Debug.LogWarning("Large Sample " + newSample);
				}
				return;
			}
			this.samples.Add(newSample);
			int num = 20;
			if (this.samples.Count > num)
			{
				this.samples.RemoveRange(0, this.samples.Count - num);
			}
			List<float> values = this.WithoutOutliers(this.samples);
			this.ping = PingController.PingSampler.CalculateMean(values);
			this.deviation = this.CalculateStandardDeviation(values);
			if (this.ping + this.deviation * 1.5f > this.lerpTo)
			{
				this.SetTarget(this.ping + this.deviation * 2f);
			}
			if (this.ping + this.deviation * 3f < this.lerpTo)
			{
				this.SetTarget(this.ping + this.deviation * 1.5f);
			}
		}

		private void SetTarget(float newTarget)
		{
			this.lerpTo = newTarget;
		}

		private List<float> WithoutOutliers(List<float> values)
		{
			float mean = PingController.PingSampler.CalculateMean(this.samples);
			float standardDev = this.CalculateStandardDeviation(this.samples);
			List<float> list = new List<float>();
			foreach (float num in values)
			{
				float num2 = num;
				if (!this.IsOutlier(num2, mean, standardDev))
				{
					list.Add(num2);
				}
			}
			return list;
		}

		private bool IsOutlier(float sample, float mean, float standardDev)
		{
			return sample > mean + standardDev * 2f || sample < mean - standardDev * 2f;
		}

		public void LerpToTarget()
		{
			this.interpolationOffset = this.lerpTo;
		}

		private float CalculateStandardDeviation(List<float> values)
		{
			float num = PingController.PingSampler.CalculateMean(values);
			List<float> list = new List<float>();
			foreach (float num2 in values)
			{
				float num3 = num2;
				float num4 = num3 - num;
				list.Add(num4 * num4);
			}
			float f = PingController.PingSampler.CalculateMean(list);
			return Mathf.Sqrt(f);
		}

		private static float CalculateMean(List<float> values)
		{
			float num = 0f;
			if (values.Count > 0)
			{
				foreach (float num2 in values)
				{
					float num3 = num2;
					num += num3;
				}
				num /= (float)values.Count;
			}
			return num;
		}

		public Vector3 DebugDraw(Vector3 offset)
		{
			float num = 1f;
			float d = 1f;
			for (int i = 0; i < this.samples.Count - 1; i++)
			{
				Vector3 vector = Vector3.right * (float)i * num + offset;
				Vector3 vector2 = Vector3.right * (float)(i + 1) * num + offset;
				Vector3 vector3 = Vector3.up * this.samples[i] * d + vector;
				Vector3 vector4 = Vector3.up * this.samples[i + 1] * d + vector2;
				UnityEngine.Debug.DrawLine(vector, vector2, Color.black, 0f);
				UnityEngine.Debug.DrawLine(vector3, vector4, Color.white, 0f);
				this.DrawPoint(vector3, this.samples[i]);
				this.DrawPoint(vector3, this.samples[i]);
				this.DrawPoint(vector4, this.samples[i + 1]);
				this.DrawPoint(vector4, this.samples[i + 1]);
			}
			this.DrawLine(offset, num * (float)this.samples.Count, this.ping, Color.blue);
			this.DrawLine(offset, num * (float)this.samples.Count, this.ping + this.deviation, Color.cyan);
			this.DrawLine(offset, num * (float)this.samples.Count, this.ping - this.deviation, Color.cyan);
			this.DrawLine(offset, num * (float)this.samples.Count, this.lerpTo, Color.yellow);
			this.DrawLine(offset, num * (float)this.samples.Count, this.InterpolationOffset, Color.red);
			return offset + Vector3.right * (float)this.samples.Count * num;
		}

		private void DrawPoint(Vector3 point, float sample)
		{
			float d = 0.2f;
			if (this.IsOutlier(sample, this.ping, this.deviation))
			{
				UnityEngine.Debug.DrawRay(point, NonDeterministicRandom.onUnitSphere * d, Color.grey);
			}
			else
			{
				UnityEngine.Debug.DrawRay(point, NonDeterministicRandom.onUnitSphere * d, Color.white);
			}
		}

		private void DrawLine(Vector3 offset, float length, float height, Color color)
		{
			Vector3 start = new Vector3(0f, height, 0f) + offset;
			Vector3 end = new Vector3(length, height, 0f) + offset;
			UnityEngine.Debug.DrawLine(start, end, color, 0f);
		}

		private List<float> samples = new List<float>();

		private float ping;

		private float rawPing;

		private float lerpTo;

		private float interpolationOffset;

		public float deviation;

		private float lastWarningTime;
	}
}
