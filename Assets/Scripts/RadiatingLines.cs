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
				Debug.Log("Not visible");
			}else{
				module.visible = true;
				Debug.Log("Visible");
			}
		}
	}

	public void onModuleStart(Module _module)
	{	
		modules.Add(_module);
		
		if(modules.Count >= Random.Range(1, limit)){
			return;
		}
		_module.divAxis = axis;
		_module.divs = 2;
	}
}
