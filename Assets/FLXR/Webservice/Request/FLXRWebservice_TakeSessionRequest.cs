using System;

public class FLXRWebservice_TakeSessionRequest : FLXRWebserviceRequest
{
	public FLXRTakeSessionRequestData data;

	public FLXRWebservice_TakeSessionRequest()
	{
		this.to = "";
		this.type = "com.akamon.jreactive.sessions.TakeSessionRequest";
		this.data = new FLXRTakeSessionRequestData ();
		this.data.clientUUID = System.Guid.NewGuid().ToString();
		this.data.id = null;
	}
}


