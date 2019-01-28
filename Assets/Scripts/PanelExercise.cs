using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class PanelExercise : MonoBehaviour {

	public Module module;
	public Modulor leModulor;
	public List<Module> modules = new List<Module>();
	public float limit = 50;
	
	private void OnEnable()
	{
		Signals.Get<Module.ModuleStart>().AddListener(onModuleStart);
	}

	private void OnDisable()
	{
		Signals.Get<Module.ModuleStart>().RemoveListener(onModuleStart);
	}

	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.Space)){
			StartCoroutine(Refresh());
		}
	}

	public void onModuleStart(Module _module)
	{	
		modules.Add(_module);
		if(modules.Count >= Random.Range(1, limit)){
			return;
		}

		var rand = Random.value;
		if(Random.value < 0.5f){
			_module.divAxis = Module.axis.x;	
		}else{
			_module.divAxis = Module.axis.y;	
		}
		_module.divs = 2;
	}

	public IEnumerator Refresh(){
		module.divs = 0;
		modules = new List<Module>();
		yield return new WaitForSeconds(0.1f);
		var rand = Random.value;
		if(Random.value < 0.5f){
			module.divAxis = Module.axis.x;	
		}else{
			module.divAxis = Module.axis.y;	
		}
		module.divs = 2;
	}
	

}
