// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class MessageInfo
{
	public MessageInfo(PID _destination)
	{
		this.timeStamp = Connect.Time;
		this.sender = PID.MyID;
		this.destination = _destination;
	}

	public MessageInfo(PID _destination, double _timeStamp)
	{
		this.timeStamp = _timeStamp;
		this.sender = PID.MyID;
		this.destination = _destination;
	}

	public MessageInfo(PID _sender, PID _destination, double _timeStamp)
	{
		this.timeStamp = _timeStamp;
		this.sender = _sender;
		this.destination = _destination;
	}

	public void OverwriteDestination(PID _destination)
	{
		this.destination = _destination;
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"Sender: ",
			this.sender,
			"   Destination: ",
			this.destination,
			"  Timestamp: ",
			this.timeStamp
		});
	}

	public double TimeStamp
	{
		get
		{
			return this.timeStamp;
		}
		set
		{
			this.timeStamp = value;
		}
	}

	public PID Sender
	{
		get
		{
			return this.sender;
		}
	}

	public PID Destination
	{
		get
		{
			return this.destination;
		}
	}

	private double timeStamp;

	private PID sender;

	private PID destination;
}
