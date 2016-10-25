using UnityEngine;
using System.Collections;

public class FLXRCoreModule : MonoBehaviour {

	private FLXRCore _core;

	void Start () {
		_core = new FLXRCore ();
		_core.Initialize (35673961,"la5rtzqdimmmhkl44rcfh6p7k7wad1zu");
	}

}
