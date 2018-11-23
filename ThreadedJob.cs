// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Threading;

public class ThreadedJob
{
	public bool IsDone
	{
		get
		{
			object handle = this.m_Handle;
			bool isDone;
			lock (handle)
			{
				isDone = this.m_IsDone;
			}
			return isDone;
		}
		set
		{
			object handle = this.m_Handle;
			lock (handle)
			{
				this.m_IsDone = value;
			}
		}
	}

	public virtual void Start()
	{
		this.m_Thread = new Thread(new ThreadStart(this.Run));
		this.m_Thread.Start();
	}

	public virtual void Abort()
	{
		this.m_Thread.Abort();
	}

	protected virtual void ThreadFunction()
	{
	}

	protected virtual void OnFinished()
	{
	}

	private void Run()
	{
		this.ThreadFunction();
		this.IsDone = true;
	}

	private bool m_IsDone;

	private object m_Handle = new object();

	protected Thread m_Thread;
}
