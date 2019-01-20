using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class TestScript : MonoBehaviour {

	public Module node;
	
	private void OnEnable()
	{
		
		Signals.Get<Module.ModuleStart>().AddListener(onNodeStart);
	}

	private void OnDisable()
	{
		Signals.Get<Module.ModuleStart>().RemoveListener(onNodeStart);
	}

	public void onNodeStart(Module node){
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
