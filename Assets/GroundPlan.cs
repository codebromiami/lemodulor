using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class GroundPlan : MonoBehaviour {

	public Module childModule;
	public List<Module> modules = new List<Module>();
	public Module.axis axis = Module.axis.x;
	public float limit = 50;
	public Vector3 size;
	public bool rays = false;
	public List<Vector3> points;

	private void OnEnable()
	{
		Signals.Get<Module.ModuleStart>().AddListener(onModuleStart);
	}

	private void OnDisable()
	{
		Signals.Get<Module.ModuleStart>().RemoveListener(onModuleStart);
	}

	void Start(){
		
		childModule.size = size;
		childModule.divs = 2;
	}

	public void onModuleStart(Module _module)
	{	
		modules.Add(_module);
		// if(modules.Count >= limit){
		// 	return;
		// }
		// if(!_module.hit){
		// 	return;
		// }
		// _module.divAxis = axis;
		// _module.divs = 2;
	}

	// Update is called once per frame
	void Update () {
		
		if(modules.Count >= limit){
			return;
		}

		if(rays){
			foreach(var p in points){
				var pos = p;
				pos.y -= 1;
				Ray ray = new Ray(pos, Vector3.up);
				RaycastHit[] hits;
				hits = Physics.RaycastAll(ray, 100);
				foreach(var hit in hits){
					var mod = hit.collider.gameObject.GetComponentInParent<Module>();
					if(mod != null){
						mod.hit = true;
						mod.divs = 2;
					}
				}
			}
		}
	}
}
