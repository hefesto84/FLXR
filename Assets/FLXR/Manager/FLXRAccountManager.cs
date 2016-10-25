using System;
using UnityEngine;
using UniRx;

public class FLXRAccountManager
{
	private FLXRSession _session;
	private bool _isInitialized = false;

	public FLXRAccountManager ()
	{
		
	}

	public void Initialize()
	{
		_isInitialized = true;
		PrepareStreams ();
	}

	private void PrepareStreams()
	{
		FLXRCore.SessionStream.Subscribe (session => {
			UpdateSession(session);
		});
	}

	private void UpdateSession(FLXRSession session)
	{
		Debug.Log ("<color=yellow>FLXR</color>: Session updated."); 
		_session = session;
	}
}

