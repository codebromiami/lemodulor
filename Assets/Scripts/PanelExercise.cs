using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class PanelExercise : MonoBehaviour {

	public Module childModel;
	public Modulor leModulor;
	public List<Module> modules = new List<Module>();
	public float limit = 50;
	public List<float> subdivisions = new List<float>(); 

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
		
		if(Input.GetKeyDown(KeyCode.R)){
			
			foreach(var module in modules){
				var list = module.gameObject.GetComponentsInParent<Module>();
				subdivisions.Add(list.Length);
			}
			MaterialPropertyBlock props = new MaterialPropertyBlock();
			Renderer renderer;
			float scaleFactor = 0.01f;
			foreach(var module in modules){
				if(module.meshGo){
					renderer = module.meshGo.GetComponent<Renderer>();
					float r = Random.Range(0.0f, 1.0f);
					float g = Random.Range(0.0f, 1.0f);
					float b = Random.Range(0.0f, 1.0f);
					props.SetColor("_Color", new Color(r, g, b));
					props.SetVector("_ST", new Vector4(module.size.x * scaleFactor, module.size.y * scaleFactor,1,1));
					renderer.SetPropertyBlock(props);
				}
			}
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
		childModel.divs = 0;
		modules = new List<Module>();
		yield return new WaitForSeconds(0.1f);
		var rand = Random.value;
		if(Random.value < 0.5f){
			childModel.divAxis = Module.axis.x;	
		}else{
			childModel.divAxis = Module.axis.y;	
		}
		childModel.divs = 2;
	}
	

}
