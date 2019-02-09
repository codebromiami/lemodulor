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
	public List<NeighborCheck> neighborChecks = new List<NeighborCheck>();
	public List<Piloti> pilotis = new List<Piloti>();
	public int exceed = 0;
	public int piltoiGroups = 0;
	public List<List<string>> groups = new List<List<string>>();
	// public List<GameObject> neighbors = new List<GameObject>();
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
			neighborChecks = new List<NeighborCheck>();
			foreach(var module in modules){
				if(module.hit){
					module.visible = true;
					var neighborCheck = module.gameObject.AddComponent<NeighborCheck>();
					neighborCheck.module = module;
					neighborChecks.Add(neighborCheck);
				}else{
					module.visible = false;
				}
			}
		}
		// We iterate through all the visible modules that are tagged to be piloti
		if(Input.GetKeyDown(KeyCode.Alpha3)){

			foreach (var neighborCheck in neighborChecks)
			{
				if(neighborCheck.module.visible){
					if(neighborCheck.tags.Contains("Ground") && !neighborCheck.tags.Contains("Roof")){	
						neighborCheck.tags.Add("Piloti: " + GroundPlan.instance.piltoiGroups);
						GroundPlan.instance.piltoiGroups++;
					}
				}
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)){
			foreach (var neighborCheck in neighborChecks)
			{
				if(neighborCheck.module.visible){
					if(neighborCheck.tags.Contains("Ground") && !neighborCheck.tags.Contains("Roof")){	
						Debug.Log(string.Format("Checking: {0}", neighborCheck.gameObject.name));
						// groupList is the list that contains this gameObject
						var groupList = IsInLists(neighborCheck.gameObject.name, groups);
						if(groupList != null){
							//Add the gameObjects this gameObjects has been tagged with
							foreach(var str in neighborCheck.groudIds){
								if(!groupList.Contains(str)){
									groupList.Add(str);
								}
							}
						}else{
							// This gameObject isn't in any list, check if any of the gameObjects it's been tagged with are in any of the groups
							bool b = false;
							foreach(var groupID in neighborCheck.groudIds){
								var subList = IsInLists(groupID, groups);
								if(subList != null){
									// One of the gameObjects is in a group, so lets add this gameObject to that list
									subList.Add(neighborCheck.gameObject.name);
									b = true;
								}
							}
							// This gameObjects groupID's were not in any group
							if(!b){
								// create a new list around a particular module
								var newList = new List<string>();
								newList.Add(neighborCheck.gameObject.name);
								newList.AddRange(neighborCheck.groudIds);
								groups.Add(newList);
								// neighbors.Add(neighborCheck.gameObject);
							}
						}
					}
				}
			}
			// Log the group info
			int index = 0;
			foreach(var group in groups){
				Debug.Log(string.Format("Group {0} count: {1}",index, group.Count));
				string groupIDs = "Group: " + index.ToString();
				foreach(string name in group){
					groupIDs += " " + name + ",";
				}
				Debug.Log(groupIDs);
				index++;
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha5)){
			// For each group add the vertex positions of their yNegative bounding box
			// Use JarvisMarch alogrith to wrap those points and create piloti areas
			var vertexGroupList = new List<List<Vertex>>();
			var gos = new List<GameObject>();
			foreach(var group in groups){
				var vertexList = new List<Vertex>();
				GameObject go = null;
				
				foreach(string name in group){
					go = GameObject.Find(name);
					var neighborCheck = go.GetComponent<NeighborCheck>();
					foreach(Vector3 point in neighborCheck.boundingPoints.yNegative){
						vertexList.Add(new Vertex(go.transform.TransformPoint(point)));
					}
					if(group.IndexOf(name) == -0)
						gos.Add(go);
				}
				vertexGroupList.Add(vertexList);
			}
			int index = 0;
			pilotis = new List<Piloti>();
			foreach(var vertexGroup in vertexGroupList){
				var vl = JarvisMarchAlgorithm.GetConvexHull(vertexGroup);
				GameObject go = new GameObject();
				go.transform.SetParent(gos[index].gameObject.transform);
				go.transform.localPosition = Vector3.zero;
				NeighborCheck neighborCheck = gos[index].gameObject.GetComponent<NeighborCheck>();
				Piloti piloti = go.AddComponent<Piloti>();
				piloti.module = neighborCheck.module;
				piloti.divs = 4;
				pilotis.Add(piloti);
				go.name = "Piloti";
				colorIndex++;
				foreach(var v in vl){
					piloti.points.Add(go.transform.InverseTransformPoint(v.position));
				}
				index++;
			}
			// Log that there was an error with one of the piloti
			foreach(Piloti piloti in pilotis){

				if(piloti.transform.position.y > piloti.module.size.y /2)
					Debug.LogWarning(string.Format("{0} is too damn high!", piloti.module.gameObject.name));
					piloti.module.gameObject.GetComponent<NeighborCheck>().tooDamnHigh = true;
			}
		}		
	}

	private void OnDrawGizmos()
	{
		foreach (var neighborCheck in neighborChecks){
			if(neighborCheck.tags.Contains("Roof") & neighborCheck.tags.Contains("Ground")){
				Gizmos.color = Color.green;
				Gizmos.DrawCube(neighborCheck.transform.position, neighborCheck.module.size);
			}else if(neighborCheck.tags.Contains("Roof")){
				Gizmos.color = Color.red;
				Gizmos.DrawCube(neighborCheck.transform.position, neighborCheck.module.size);
			}else if(neighborCheck.tags.Contains("Ground")){
				Gizmos.color = Color.blue;
				Gizmos.DrawCube(neighborCheck.transform.position, neighborCheck.module.size);
			}
		}	
	}

	public List<string> IsInLists(string str, List<List<string>> listOfLists){
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
