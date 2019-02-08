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
	public List<NeighborCheck> neighborChecks = new List<NeighborCheck>();
	public int exceed = 0;
	public int piltoiGroups = 0;
	public List<List<string>> groups = new List<List<string>>();
	public List<GameObject> neighbors = new List<GameObject>();
	public List<Color> colors = new List<Color>(){
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

	// When each module is created we subdivide it along a random axis
	// The swipe generated a number of points, we subdivide if the number of modules 
	// subdivded is lower than the points created.
	public void onModuleStart(Module _module)
	{	
		modules.Add(_module);
		if(exceed > limit){
			// Debug.Log("Limit hit");
		}
		
		if(modules.Count > limit){
			// Debug.Log("Limit exceded");
			exceed++;
			return;
		}
		// _module.id += modules.Count.ToString();
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
		_module.gameObject.name = "Module Divided";
	}

	// Update is called once per frame
	void Update () {
		
		// foreach (var item in neighborChecks)
		// {
		// 	if(item.tags.Contains("Ground") && !item.tags.Contains("Roof")){
		// 		if(!item.tags.Contains("Piloti")){
		// 			item.tags.Add("Piloti");
		// 			item.tags.Add("Piloti" + GroundPlan.instance.piltoiGroups);
		// 			GroundPlan.instance.piltoiGroups++;
		// 		}
		// 	}
		// }

		// The first step is to use the points made by the swipe to define modules that we wish to include going forward.
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			
			foreach(var point in points){
				Vector3 position = point;
				position.y -= 1;
				Ray ray = new Ray(position, Vector3.up);
				RaycastHit[] hits;
				hits = Physics.RaycastAll(ray, 100);
				foreach(var hit in hits){
					Module module = hit.collider.gameObject.GetComponentInParent<Module>();
					if(module != null){
						module.hit = true;
					}
				}
			}
			foreach(var module in modules){
				if(module.hit){
					module.visible = true;
					module.gameObject.name = "Module Visible";
				}else{
					module.visible = false;
					module.gameObject.name = "Module Invisible";
				}
			}
		}

		if(Input.GetKeyDown(KeyCode.Alpha2)){
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

		if(Input.GetKeyDown(KeyCode.Alpha3)){

			foreach (var item in neighborChecks)
			{
				if(item.module.visible){
					if(item.tags.Contains("Ground") && !item.tags.Contains("Roof")){
						
						var lst = IsInList(item.gameObject.name, groups);
						if(lst != null){
							foreach(var str in item.groudIds)
							if(!lst.Contains(str)){
								lst.Add(str);
							}
						}else{
							bool b = false;
							foreach(var str in item.groudIds){
								var subLst = IsInList(str, groups);
								if(subLst != null){
									subLst.Add(item.gameObject.name);
									b = true;
								}
							}
							if(!b){
								// create a new list around a particular module
								var g = new List<string>();
								g.Add(item.gameObject.name);
								g.AddRange(item.groudIds);
								groups.Add(g);
								neighbors.Add(item.gameObject);
							}
						}
						// Create lists of vertexes from all the gameobjects that have been tagged with piloti
						int count = 0;
						foreach(var list in groups){
							var bigStr = count.ToString();
							var vl = new List<Vertex>();
							NeighborCheck tmpP = neighbors[count].gameObject.GetComponent<NeighborCheck>();
							NeighborCheck tmpPee = null;
							foreach(var str in list){
								var go = GameObject.Find(str);
								tmpPee = go.GetComponent<NeighborCheck>();
								foreach(var v in tmpPee.boundingPoints.yNegative){
									vl.Add(new Vertex(v));
								}
								bigStr += " " + str;
							}
							tmpP.vertexGroups = vl;
							Debug.Log(bigStr);
							count++;
						}
						// foreach(var tmP in neighbors){
						// 	var vg = tmP.GetComponent<NeighborCheck>().vertexGroups;
						// 	var vl = JarvisMarchAlgorithm.GetConvexHull(vg);
						// 	var go = new GameObject();
						// 	go.transform.SetParent(tmP.gameObject.transform);
						// 	go.transform.localPosition = Vector3.zero;
						// 	var p = go.AddComponent<Piloti>();
						// 	colorIndex = colorMainIndex;
						// 	colorMainIndex++;
						// 	foreach(var v in vl){
						// 		p.points.Add(v.position);
						// 	}
						// }
					}
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

	private void OnDrawGizmos()
	{
		foreach (var neighborCheck in neighborChecks){
			if(neighborCheck.tags.Contains("Roof") & neighborCheck.tags.Contains("Ground")){

			}else if(neighborCheck.tags.Contains("Roof")){
				Gizmos.DrawWireCube(neighborCheck.transform.position, neighborCheck.module.size);
			}else if(neighborCheck.tags.Contains("Ground")){

			}
		}	
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
