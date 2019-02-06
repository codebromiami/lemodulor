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
	public static int piltoiGroups = 0;
	public static List<List<string>> groups = new List<List<string>>();
	public static List<Color> colors = new List<Color>(){
		Color.red,
		Color.blue,
		Color.yellow,
		Color.magenta,
		Color.cyan,
		Color.green,
		Color.black,
		Color.gray,
	};
	static int colorMainIndex = 0;
	public int colorIndex;
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
					m.gameObject.AddComponent<NeighborCheck>();
				}else{
					m.visible = false;
				}
			}
		}

		if(Input.GetKeyDown(KeyCode.Alpha2)){
			var neighbors = FindObjectsOfType<NeighborCheck>();
			foreach (var item in neighbors)
			{
				if(item.module.visible){
					if(item.tags.Contains("Ground") && !item.tags.Contains("Roof")){
						var p = item.gameObject.AddComponent<Piloti>();
					}
				}
			}
		}

		if(Input.GetKeyDown(KeyCode.Alpha3)){
			var piltois = FindObjectsOfType<Piloti>();
			foreach(var piloti in piltois){
				var lst = IsInList(piloti.gameObject.name, groups);
				if(lst != null){
					foreach(var str in piloti.neighborCheck.groudIds)
					if(!lst.Contains(str)){
						lst.Add(str);
					}
				}else{
					bool b = false;
					foreach(var str in piloti.neighborCheck.groudIds){
						var subLst = IsInList(str, groups);
						if(subLst != null){
							subLst.Add(piloti.gameObject.name);
							b = true;
						}
					}
					if(!b){
						var g = new List<string>();
						g.Add(piloti.gameObject.name);
						g.AddRange(piloti.neighborCheck.groudIds);
						groups.Add(g);
					}
				}
			}
			// Create lists of vertexes from all the gameobjects that have been tagged with piloti
			int count = 0;
			List<NeighborCheck> inUse = new List<NeighborCheck>();
			foreach(var list in groups){
				var bigStr = count.ToString();
				var vl = new List<Vertex>();
				NeighborCheck tmpP = null; 
				foreach(var str in list){
					var go = GameObject.Find(str);
					var p = go.GetComponent<Piloti>();
					tmpP = go.GetComponent<NeighborCheck>();
					foreach(var v in p.neighborCheck.boundingPoints.yNegative){
						vl.Add(new Vertex(v));
					}
					bigStr += " " + str;
				}
				inUse.Add(tmpP);
				tmpP.vertexGroups = vl;
				Debug.Log(bigStr);
				count++;
			}
			foreach(var tmP in inUse){
				var vg = tmP.vertexGroups;
				var vl = JarvisMarchAlgorithm.GetConvexHull(vg);
				var go = new GameObject();
				go.transform.SetParent(tmP.gameObject.transform);
				var p = go.AddComponent<Piloti>();
				colorIndex = colorMainIndex;
				colorMainIndex++;
				foreach(var v in vl){
					p.points.Add(v.position);
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
	public List<string> IsInList(string str, List<List<string>> listOfLists){
		int count = 0;
		foreach(var lst in listOfLists){
			if(lst.Contains(str)){
				return listOfLists[count];
				break;
			}
			count++;
		}
		return null;
	} 
}
