using UnityEngine;
using System.Collections;
using BestHTTP.WebSocket;
using System;
using UniRx;

public class FLXRNetworkManager {

	private bool _isInitialized = false;
	private WebSocket _websocket;
	private IFLXR_Network _networkHandler;
	private FLXRSession _session;
	private int _userId;
	private string _password;
	private string _sessionId;

	private const string URL = "wss://gameserver-mj02.prod.akamon.com:443/";

	public FLXRNetworkManager()
	{
		
	}

	public void Initialize(int userId, string password, IFLXR_Network handler)
	{
		_isInitialized = true;

		_userId = userId;
		_password = password;

		_networkHandler = handler;

		PrepareStreams ();
		PrepareWebsocket ();
	}
		
	private void PrepareWebsocket()
	{
		Debug.Log("<color=yellow>FLXR</color>: Establishing connection with: "+URL);

		_websocket = new WebSocket(new Uri(URL));
		_websocket.OnOpen += OnWebSocketOpen;
		_websocket.OnError += OnError;
		_websocket.OnMessage += OnMessageReceived;

		Debug.Log("<color=yellow>FLXR</color>: Opening socket."); 

		_websocket.Open ();

	}

	private void PrepareStreams()
	{
		Debug.Log("<color=yellow>FLXR</color>: Preparing streams."); 

		FLXRCore.RequestStream.Subscribe (s => {
			Debug.Log("<color=yellow>FLXR</color>: Sending request throught the subscriber."); 
			ProcessServerRequest(s);
		});

		FLXRCore.ResponseStream.Subscribe (s => {
			Debug.Log("<color=yellow>FLXR</color>: Receiving request throught the subscriber."); 
			ProcessServerResponse(s);
		});
	}

	private void OnWebSocketOpen(WebSocket webSocket) 
	{
		Debug.Log("<color=yellow>FLXR</color>: Socked opened. Sending TakeSessionRequest."); 
		FLXRCore.RequestStream.OnNext (new FLXRWebservice_TakeSessionRequest ());
	}

	private void OnError(WebSocket ws, Exception exception) 
	{
		string errorMsg = string .Empty;
		if (ws.InternalRequest.Response != null)
			errorMsg = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
		_networkHandler.OnConnectionError ("Error establishing connection.");
	}

	private void OnMessageReceived(WebSocket webSocket, string message)
	{
		Debug.Log("<color=yellow>FLXR</color>: RAW Message: "+message);

		FLXRWebserviceResponse response = JsonUtility.FromJson<FLXRWebserviceResponse> (message);

		if (response.type.Equals ("com.akamon.jreactive.sessions.TakeSessionResponse")) 
		{
			FLXRCore.ResponseStream.OnNext (JsonUtility.FromJson<FLXRWebservice_TakeSessionResponse> (message));
		}
		else if(response.type.Equals("com.akamon.gameapi.login.LoginResponse"))
		{
			FLXRCore.ResponseStream.OnNext (JsonUtility.FromJson<FLXRWebservice_LoginResponse> (message));
		}
	}

	private void ProcessServerRequest(FLXRWebserviceRequest request)
	{
		_websocket.Send (JsonUtility.ToJson (request));
	}

	private void ProcessServerResponse(FLXRWebserviceResponse response)
	{
		if (response is FLXRWebservice_TakeSessionResponse) {
			_sessionId = ((FLXRWebservice_TakeSessionResponse)response).data.id;
			FLXRCore.RequestStream.OnNext (new FLXRWebservice_LoginRequest (_userId,_password));
		} else if (response is FLXRWebservice_LoginResponse) {
			_session = new FLXRSession();
			_session.sessionId = _sessionId;
			_session.user = ((FLXRWebservice_LoginResponse)response).data.user;
			FLXRCore.SessionStream.OnNext (_session);
		}
	}

}
