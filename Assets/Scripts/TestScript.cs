using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class TestScript : MonoBehaviour {

	public Module module;
	public int count = 0;
	public int limit = 50;
	public Modulor leModulor;
	public List<Module> modules = new List<Module>();

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
		modules.Add(module);
		if(count > limit)
			return;

		module.divAxis = Random.value < 0.3f ? module.divAxis = Module.axis.x : module.divAxis = Module.axis.y;
		module.divAxis = Random.value > 0.6f ? module.divAxis = Module.axis.y : module.divAxis = Module.axis.z;
		module.divs = Random.value < 0.5f ? 0 : 2;
		count++;
	}

	public void Refresh(){
		module.divs = 0;
		count = 0;
		module.divAxis = Random.value < 0.3f ? module.divAxis = Module.axis.x : module.divAxis = Module.axis.y;
		module.divAxis = Random.value > 0.6f ? module.divAxis = Module.axis.y : module.divAxis = Module.axis.z;
		module.divs = Random.value < 0.5f ? 0 : 2;	
	}

	// Use this for initialization
	void Start () {

		module.divAxis = Random.value < 0.3f ? module.divAxis = Module.axis.x : module.divAxis = Module.axis.y;
		module.divAxis = Random.value > 0.6f ? module.divAxis = Module.axis.y : module.divAxis = Module.axis.z;
		module.divs = 2;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.Space)){
			Refresh();
		}
		// int i = Random.Range(0,modules.Count -1);
		// if(i > 0 | i < modules.Count){
		// 	modules[i].divs = Random.Range(0,3);
		// }
	}
}
