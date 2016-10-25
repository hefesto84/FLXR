using System;

[Serializable]
public class FLXRWebservice_LoginRequest : FLXRWebserviceRequest
{
	public FLXRLoginRequestData data = new FLXRLoginRequestData();


	public FLXRWebservice_LoginRequest(int userId, string token)
	{
		to = "MundijuegosLoginService";
		type = "com.akamon.suite.platform.protocol.client.MundijuegosLoginRequest";
		data.clientTypeId = "viva_client_android";
		data.gameId = 71;
		data.userId = userId;
		data.userOAuthToken = token;
	}

}


