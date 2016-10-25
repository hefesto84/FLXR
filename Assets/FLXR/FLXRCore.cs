using UnityEngine;
using System.Collections;
using UniRx;

public class FLXRCore : IFLXR_Network{

	private FLXRNetworkManager _flxrNetworkManager;
	private FLXRAccountManager _flxrAccountManager;

	public static Subject<FLXRWebserviceRequest> RequestStream = new Subject<FLXRWebserviceRequest> ();
	public static Subject<FLXRWebserviceResponse> ResponseStream = new Subject<FLXRWebserviceResponse>();

	public static Subject<FLXRSession> SessionStream = new Subject<FLXRSession>();

	private bool _isInitialized = false;

	public FLXRCore()
	{

	}

	public void Initialize(int userId, string password)
	{
		Debug.Log("<color=yellow>FLXR</color>: Initializing core.");

		if (!_isInitialized)
			_isInitialized = true;

		_flxrNetworkManager = new FLXRNetworkManager ();
		_flxrAccountManager = new FLXRAccountManager ();

		_flxrNetworkManager.Initialize (userId,password,this);
		_flxrAccountManager.Initialize ();
	}

	public void OnConnectionStablished(string id)
	{
		Debug.Log("<color=yellow>FLXR</color>: Connection established with id: "+id);
	}

	public void OnSessionEstablished(FLXRSession session)
	{
		Debug.Log("<color=yellow>FLXR</color>: Session Established.");
		SessionStream.OnNext (session);
	}

	public void OnConnectionError(string error)
	{
		Debug.Log("<color=yellow>FLXR</color>: Connection error: "+error);
	}

}
