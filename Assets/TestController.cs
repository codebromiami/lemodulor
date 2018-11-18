using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;
using GoogleARCore;

public class TestController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Space)){

			Anchor anchor = new Anchor();
			TrackableHit hit = new TrackableHit();
			Signals.Get<AnchorCreated>().Dispatch(anchor,hit);
		}
		
	}
}
