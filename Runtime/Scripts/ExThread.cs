using UnityEngine;
using System.Collections;

public class ExThread
{
	bool m_IsDone = false;
	object m_Handle = new object ();
	System.Threading.Thread m_Thread = null;

	public bool IsDone {
		get {
			bool tmp;
			lock (m_Handle) {
				tmp = m_IsDone;
			}
			return tmp;
		}
		set {
			lock (m_Handle) {
				m_IsDone = value;
			}
		}
	}


	public virtual void Start ()
	{
		m_Thread = new System.Threading.Thread (Run);
		m_Thread.Start ();
	}

	public virtual void Abort ()
	{
		m_Thread.Abort ();
		onAbort.TryInvoke ();
		OnAbort ();
	}

	protected virtual void ThreadFunction ()
	{
	}

	protected virtual void OnStarted ()
	{
	}

	protected virtual void OnFinished ()
	{
	}
	protected virtual void OnAbort ()
	{
	}

	void Run ()
	{
		onStart.TryInvoke ();
		OnStarted ();
		ThreadFunction ();
		OnFinished ();
		IsDone = true;
		onFinish.TryInvoke ();
	}

	public void Sleep ()
	{
		System.Threading.Thread.Sleep (System.Threading.Timeout.Infinite);
		onSleep.TryInvoke ();
	}

	public void Awake ()
	{
		m_Thread.Interrupt ();
		onAwake.TryInvoke ();
	}

	#region Events

	public System.Action onStart = null;
	public System.Action onFinish = null;
	public System.Action onSleep = null;
	public System.Action onAwake = null;
	public System.Action onAbort = null;

	#endregion
}