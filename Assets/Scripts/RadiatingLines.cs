using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class RadiatingLines : MonoBehaviour {

	public Module parentModule;
	public Module childModule;
	public List<Module> modules = new List<Module>();
	public Module.axis axis = Module.axis.x;
	public float limit = 50;
	public Vector3 size;
	public bool visibleEnds = false;
	public Module startModule;
	public Module endModule;

	private void OnEnable()
	{
		Signals.Get<Module.ModuleStart>().AddListener(onModuleStart);
	}

	private void OnDisable()
	{
		Signals.Get<Module.ModuleStart>().RemoveListener(onModuleStart);
	}


	private float AngleBetweenVector2(Vector3 vec1, Vector3 vec2)
 	{
         Vector3 diference = vec2 - vec1;
         float sign = (vec2.y < vec1.y)? -1.0f : 1.0f;
         return Vector3.Angle(Vector3.right, diference) * sign;
     }

	IEnumerator Start(){
		
		childModule.size = size;
		childModule.parentNode = parentModule;
		childModule.visible = false;
		parentModule.visible = false;
		yield return new WaitForSeconds(0.25f);
		foreach (var module in modules)
		{
			if(Random.value > 0.5){
				module.visible = false;
			}else{
				module.visible = true;
			}
		}
	}

	public void onModuleStart(Module _module)
	{	
		modules.Add(_module);
		if(visibleEnds){

			if(!startModule)
				startModule = _module;

			if(!endModule)
				endModule = _module;

			float a = 0;
			float b = 0;
			float c = 0;
			switch(axis){
				case Module.axis.x:
					a = _module.transform.position.x;
					b = startModule.transform.position.x;
					c = endModule.transform.position.x;
				break;
				case Module.axis.y:
					a = _module.transform.position.y;
					b = startModule.transform.position.y;
					c = endModule.transform.position.y;
				break;
				case Module.axis.z:
					a = _module.transform.position.z;
					b = startModule.transform.position.z;
					c = endModule.transform.position.z;
				break;
			}
			if(a < b){
				startModule = _module;
			}
			if(a > c){
				endModule = _module;
			}
			startModule.visible = true;
			endModule.visible = true;
		}
		if(modules.Count >= Random.Range(1, limit)){
			return;
		}
		_module.divAxis = axis;
		_module.divs = 2;
	}
}
