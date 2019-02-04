using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class GroundPlan : MonoBehaviour {

	public static GroundPlan instance;

	public Module childModule;
	public List<Module> modules = new List<Module>();
	public Module.axis axis = Module.axis.x;
	public float limit = 50;
	public Vector3 size;
	public bool rays = false;
	public List<Vector3> points;
	public List<Piloti> pilotis = new List<Piloti>();
	public int exceed = 0;
	
	private void OnEnable()
	{
		Signals.Get<Module.ModuleStart>().AddListener(onModuleStart);
	}

	private void OnDisable()
	{
		Signals.Get<Module.ModuleStart>().RemoveListener(onModuleStart);
	}

	void Start(){

		instance = this;	
		childModule.size = size;
		childModule.divs = 2;
	}
	
	public void onModuleStart(Module _module)
	{	
		_module.gameObject.AddComponent<NeighborCheck>();
		modules.Add(_module);
		if(exceed > limit){
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
					}
				}
			}
			foreach(var m in modules){
				if(m.hit){
					m.visible = true;
				}else{
					m.visible = false;
				}
			}
			// Debug.Log("Limit hit");
		}
		if(modules.Count > limit){
			// Debug.Log("Limit exceded");
			exceed++;
			return;
		}
		_module.id += modules.Count;
		List<string> axis = new List<string>();
		axis.Add(Module.axis.x.ToString());
		axis.Add(Module.axis.y.ToString());
		axis.Add(Module.axis.z.ToString());
		var choice = ExtRandom<string>.WeightedChoice(axis, new int[]{33,33,33});
		switch(choice){
			case "x":
				_module.divAxis = Module.axis.x;
			break;
			case "y":
				_module.divAxis = Module.axis.y;
			break;
			case "z":
				_module.divAxis = Module.axis.z;
			break;					
		}
		_module.divs = 2;

	}
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Alpha1)){
			foreach(var p in points){
				var pos = p;
				pos.y -= 1;
				Ray ray = new Ray(pos, Vector3.up);
				RaycastHit[] hits;
				hits = Physics.RaycastAll(ray, 100);
				// RaycastHit hit;
				// if(Physics.Raycast(ray.origin,ray.direction * 100, out hit)){
				// 	var mod = hit.collider.gameObject.GetComponentInParent<Module>();
				// 	if(mod != null){
				// 		mod.hit = true;
				// 	}
				// }
				foreach(var hit in hits){
					var mod = hit.collider.gameObject.GetComponentInParent<Module>();
					if(mod != null){
						mod.hit = true;
					}
				}
			}
			foreach(var m in modules){
				if(m.hit){
					m.visible = true;
				}else{
					m.visible = false;
				}
			}
		}

		// if(rays){
		// 	foreach(var p in points){
		// 		var pos = p;
		// 		pos.y -= 1;
		// 		Ray ray = new Ray(pos, Vector3.up);
		// 		RaycastHit[] hits;
		// 		hits = Physics.RaycastAll(ray, 100);
		// 		foreach(var hit in hits){
		// 			var mod = hit.collider.gameObject.GetComponentInParent<Module>();
		// 			if(mod != null){
		// 				mod.hit = true;
		// 			}
		// 		}
		// 	}
		// }
	}
}
