using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class TestScript : MonoBehaviour {

	public Module module;
	public int count = 0;
	public int limit = 50;

	private void OnEnable()
	{
		Signals.Get<Module.ModuleStart>().AddListener(onModuleStart);
	}

	private void OnDisable()
	{
		Signals.Get<Module.ModuleStart>().RemoveListener(onModuleStart);
	}

	public void onModuleStart(Module module)
	{	
		if(count > limit)
			return;

		module.divAxis = (Module.axis)Random.Range(0,3);
		module.divs = Random.Range(1,10);	
		count++;
	}

	public void Refresh(){
		module.divs = 0;
		count = 0;
		float x = 226.0f;
		float y = 226.0f;
		float z = 20.4f;
		module.size = new Vector3(x,y,z);
		module.divs = Random.Range(1,10);
	}

	// Use this for initialization
	void Start () {

		module.divs = Random.Range(1,10);
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.Space)){
			Refresh();
		}
	}
}
