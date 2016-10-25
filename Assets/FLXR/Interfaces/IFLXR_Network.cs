using System;

public interface IFLXR_Network
{
	void OnConnectionStablished(string id);
	void OnSessionEstablished(FLXRSession session);
	void OnConnectionError(string error);
}

