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
	public int state = 0;
	public bool stateInit = false;
	void Start(){

		instance = this;	
		childModule.size = size;
		childModule.divs = 2;
		GameObject go = new GameObject();
		go.transform.SetParent(transform);
		var pos = Vector3.zero;
		pos.y = -childModule.size.y / 2;
		go.transform.localPosition = pos; 
		Piloti piloti = go.AddComponent<Piloti>();
		piloti.module = childModule;
		var a = size.y * 10;
		var b = a / size.x;
		piloti.divs = Mathf.RoundToInt(b);
		pilotis.Add(piloti);
		go.name = "Piloti";
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
		if(state == 1 & !stateInit){
			stateInit = true;
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

		if(state == 2 && !stateInit){
			stateInit = true;
			// Take all the modules that were hit by the touch and add a NeighborCheck
			neighborChecks = new List<NeighborCheck>();
			foreach(var module in modules){
				if(module.hit){
					var neighborCheck = module.gameObject.AddComponent<NeighborCheck>();
					neighborCheck.module = module;
					neighborChecks.Add(neighborCheck);
				}else{
					module.visible = false;
				}
			}
			foreach (var neighborCheck in neighborChecks){
				neighborCheck.Check();
			}
		}
		// We iterate through all the visible modules that are tagged to be piloti
		if(state == 3 & !stateInit){
			stateInit = true;

			foreach (var neighborCheck in neighborChecks)
			{
				if(neighborCheck.module.visible){
					if(neighborCheck.tags.Contains("Ground") && !neighborCheck.tags.Contains("Roof")){	
						neighborCheck.tags.Add("Piloti");
						neighborCheck.tags.Add(piltoiGroups.ToString());
						GroundPlan.instance.piltoiGroups++;
					}
				}
			}
			foreach (var neighborCheck in neighborChecks){
					neighborCheck.Check();
			}
		}

		if(state == 4 & !stateInit){
			stateInit = true;
			// Loop through all the modules tagged piloti and double check they on the ground
			foreach (var neighborCheck in neighborChecks)
			{
				if(neighborCheck.gameObject.name.Contains("Piloti")){
					var value = neighborCheck.transform.position.y;
					if(!Mathf.Approximately(value,size.y /2)){
						if(value > size.y /2){
							neighborCheck.tags.Remove("Piloti");
							Debug.Log(string.Format("Piloti '{0}' is too high. Position: {1} Module Size / 2: {2}",neighborCheck.gameObject.name, value, size.y / 2));
						}
					}
				}
			}
			foreach (var neighborCheck in neighborChecks){
					neighborCheck.Check();
			}
		}

		if(state == 5 & !stateInit){
			stateInit = true;

			foreach (var neighborCheck in neighborChecks)
			{
				// For all the piloti modules we look for neihboring piloti modules
				if(neighborCheck.module.visible){
					if(neighborCheck.tags.Contains("Piloti")){	
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
			foreach (var neighborCheck in neighborChecks){
				neighborCheck.Check();
			}
			// Log the group info and give the modules in groups an index to the group
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

			// foreach(var group in groups){
			// 	List<GameObject> tmp = new List<GameObject>();
			// 	foreach(string name in group){
			// 		tmp.Add(GameObject.Find(name));
			// 	}
			// 	Vector3 averagePosition = Vector3.zero;
			// 	Vector3 averageScale = Vector3.zero;
			// 	float maxSize = 0;
			// 	int i = 0;
			// 	foreach(GameObject go in tmp){
			// 		averagePosition += go.transform.position;
			// 		var size = go.GetComponent<Module>().size;
			// 		averageScale.x += size.x;
			// 		averageScale.z += size.z;
			// 		if(size.y > maxSize)
			// 			maxSize = size.y;
			// 		i++;
			// 	}
			// 	averagePosition /= i;
			// 	GameObject pilotiGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
			// 	Destroy(pilotiGo.GetComponent<Collider>());
			// 	pilotiGo.transform.SetParent(transform);
			// 	pilotiGo.transform.position = averagePosition;
			// 	averageScale.y = maxSize;
			// 	pilotiGo.transform.localScale = averageScale;
			// }
		}
		if(state == 6 & !stateInit){
			foreach (var neighborCheck in neighborChecks)
			{
				if(neighborCheck.module.visible){
					if(neighborCheck.gameObject.name.Contains("Piloti")){	
						neighborCheck.module.meshGo.GetComponent<Collider>().isTrigger = true;
					}
				}
			}
			var piloti = FindObjectOfType<Piloti>();
			piloti.init = true;
			List<Vector3> tmp = new List<Vector3>();
			
			foreach(var line in piloti.lines){
				foreach(var point in line.points){
					if(!Vector3Contains(tmp, point)){
						tmp.Add(point);
					}else{
						Debug.Log("Duplicate Point");
					}
				}
			}
			var go = Resources.Load<GameObject>("Prefabs/PilotiAgent");
			foreach(var point in tmp){
				go = Instantiate(go);
				go.transform.SetParent(piloti.transform);
				go.transform.localPosition = point;
				go.name = "Piloti Agent: " +point.ToString();
				var scale = go.transform.localScale;
				scale.x = piloti.size;
				scale.z = piloti.size;
				go.transform.localScale = scale;
				var pilotiAgent = go.GetComponent<PilotiAgent>();
				piloti.pilotiAgents.Add(pilotiAgent);
			}
		}
		if(state == 6 & !stateInit){
			stateInit = true;
			// Hide all the modules tagged 'piloti'
			foreach (var neighborCheck in neighborChecks)
			{
				if(neighborCheck.module.visible){
					if(neighborCheck.gameObject.name.Contains("Piloti")){	
						neighborCheck.module.meshGo.GetComponent<MeshRenderer>().enabled = false;
					}
				}
			}
		}

		if(state == 7 & !stateInit){
			stateInit = true;
			var piloti = FindObjectOfType<Piloti>();
			// Add all the modules tagged 'piloti' to list
			foreach (var neighborCheck in neighborChecks)
			{
				if(neighborCheck.module.visible){
					if(neighborCheck.gameObject.name.Contains("Piloti")){	
						neighborCheck.module.size *= 0.6f;
						piloti.pilotiModules.Add(neighborCheck.module);
					}
				}
			}
			foreach(var pilotiAgent in piloti.pilotiAgents){
				
				var colliderA = pilotiAgent.GetComponent<Collider>();
				var hit = false;
				foreach(var module in piloti.pilotiModules){
					var colliderB = module.meshGo.GetComponent<Collider>();
					if(colliderA.bounds.Intersects(colliderB.bounds)){
						hit = true;
						pilotiAgent.hit = true;
						break;
					}
				}
				pilotiAgent.gameObject.GetComponent<MeshRenderer>().enabled = hit;
			}
		}
		

		if(Input.GetKeyDown(KeyCode.RightArrow)){
			stateInit = false;
			state++;
			Debug.Log(string.Format("State: {0}",state));
		}
		// foreach (var neighborCheck in neighborChecks)
		// {
		// 	if(neighborCheck.module.visible){
		// 		if(neighborCheck.gameObject.name.Contains("Piloti")){	
		// 			neighborCheck.module.visible = false;
		// 		}
		// 	}
		// }
		// if(Input.GetKeyDown(KeyCode.Alpha5)){
		// 	// For each group add the vertex positions of their yNegative bounding box
		// 	// Use JarvisMarch alogrith to wrap those points and create piloti areas
		// 	var vertexGroupList = new List<List<Vertex>>();
		// 	var gos = new List<GameObject>();
		// 	foreach(var group in groups){
		// 		var vertexList = new List<Vertex>();
		// 		GameObject go = null;
				
		// 		foreach(string name in group){
		// 			go = GameObject.Find(name);
		// 			var neighborCheck = go.GetComponent<NeighborCheck>();
		// 			foreach(Vector3 point in neighborCheck.boundingPoints.yNegative){
		// 				vertexList.Add(new Vertex(go.transform.TransformPoint(point)));
		// 			}
		// 			if(group.IndexOf(name) == -0)
		// 				gos.Add(go);
		// 		}
		// 		vertexGroupList.Add(vertexList);
		// 	}
		// 	int index = 0;
		// 	pilotis = new List<Piloti>();
		// 	foreach(var vertexGroup in vertexGroupList){
		// 		var vl = JarvisMarchAlgorithm.GetConvexHull(vertexGroup);
		// 		GameObject go = new GameObject();
		// 		go.transform.SetParent(gos[index].gameObject.transform);
		// 		go.transform.localPosition = Vector3.zero;
		// 		NeighborCheck neighborCheck = gos[index].gameObject.GetComponent<NeighborCheck>();
		// 		Piloti piloti = go.AddComponent<Piloti>();
		// 		piloti.module = neighborCheck.module;
		// 		piloti.divs = 4;
		// 		pilotis.Add(piloti);
		// 		go.name = "Piloti";
		// 		colorIndex++;
		// 		foreach(var v in vl){
		// 			piloti.points.Add(go.transform.InverseTransformPoint(v.position));
		// 		}
		// 		index++;
		// 	}
		// 	// Log that there was an error with one of the piloti
		// 	foreach(Piloti piloti in pilotis){

		// 		if(piloti.transform.position.y > piloti.module.size.y /2)
		// 			Debug.LogWarning(string.Format("{0} is too damn high!", piloti.module.gameObject.name));
		// 			piloti.module.gameObject.GetComponent<NeighborCheck>().tooDamnHigh = true;
		// 	}
		// }
		// if(Input.GetKeyDown(KeyCode.Alpha6)){
		// 	// For each group add the vertex positions of their yNegative bounding box
		// 	// Use JarvisMarch alogrith to wrap those points and create piloti areas
		// 	var vertexGroupList = new List<List<Vector3>>();
		// 	var gos = new List<GameObject>();
		// 	foreach(var group in groups){
		// 		var vertexList = new List<Vector3>();
		// 		GameObject go = null;
		// 		foreach(string name in group){
		// 			go = GameObject.Find(name);
		// 			var neighborCheck = go.GetComponent<NeighborCheck>();
		// 			foreach(Vector3 point in neighborCheck.boundingPoints.yNegative){
		// 				var newPoint = go.transform.TransformPoint(point);
		// 				if(!vertexList.Contains(newPoint)){
		// 					vertexList.Add(newPoint);
		// 				}
		// 			}
		// 			if(group.IndexOf(name) == 0)
		// 				gos.Add(go);
		// 		}
		// 		vertexGroupList.Add(vertexList);
		// 	}
		// 	// Log the group info
		// 	int index = 0;
		// 	foreach(var vertexGroup in vertexGroupList){
		// 		Debug.Log(string.Format("Group {0} count: {1}",index, vertexGroup.Count));
		// 		string groupIDs = "Group: " + index.ToString();
		// 		foreach(Vector3 vertex in vertexGroup){
		// 			groupIDs += " " + vertex + ",";
		// 		}
		// 		Debug.Log(groupIDs);
		// 		index++;
		// 	}
		// 	index = 0;
		// 	foreach(var vertexGroup in vertexGroupList){
		// 		GameObject go = new GameObject();
		// 		go.transform.SetParent(gos[index].gameObject.transform);
		// 		go.transform.localPosition = Vector3.zero;
		// 		NeighborCheck neighborCheck = gos[index].gameObject.GetComponent<NeighborCheck>();
		// 		Piloti piloti = go.AddComponent<Piloti>();
		// 		piloti.module = neighborCheck.module;
		// 		piloti.divs = 4;
		// 		pilotis.Add(piloti);
		// 		go.name = "Piloti";
		// 		colorIndex++;
		// 		foreach(var vertex in vertexGroup){
		// 			piloti.points.Add(go.transform.InverseTransformPoint(vertex));
		// 		}
		// 		index++;
		// 	}
		// 	// Log that there was an error with one of the piloti
		// 	foreach(Piloti piloti in pilotis){

		// 		if(piloti.transform.position.y > piloti.module.size.y /2)
		// 			Debug.LogWarning(string.Format("{0} is too damn high!", piloti.module.gameObject.name));
		// 			piloti.module.gameObject.GetComponent<NeighborCheck>().tooDamnHigh = true;
		// 	}
		// }		
	}

	private void OnDrawGizmos()
	{
		if(!Application.isPlaying)
			return;

		Vector3 cubeScale = Vector3.one * 0.05f;
		foreach(var module in modules){
			if(module.visible){
				Gizmos.color = Color.black;
				Gizmos.DrawWireCube(module.transform.position, module.size);
			}else{
				Gizmos.color = Color.white;
				Gizmos.DrawWireCube(module.transform.position, module.size);
			}
		}
		foreach (var neighborCheck in neighborChecks){
			
			if(neighborCheck.tags.Contains("Roof") & neighborCheck.tags.Contains("Ground")){
				Gizmos.color = Color.green;
				Gizmos.DrawWireCube(neighborCheck.transform.position, neighborCheck.module.size);
				foreach(var p in neighborCheck.boundingPoints.all){
					Gizmos.DrawCube(neighborCheck.transform.TransformPoint(p), cubeScale);
				}
			}else if(neighborCheck.tags.Contains("Roof")){
				Gizmos.color = Color.red;
				Gizmos.DrawWireCube(neighborCheck.transform.position, neighborCheck.module.size);
				foreach(var p in neighborCheck.boundingPoints.all){
					Gizmos.DrawCube(neighborCheck.transform.TransformPoint(p), cubeScale);
				}
			}else if(neighborCheck.tags.Contains("Ground")){
				Gizmos.color = Color.blue;
				Gizmos.DrawWireCube(neighborCheck.transform.position, neighborCheck.module.size);
				foreach(var p in neighborCheck.boundingPoints.all){
					Gizmos.DrawCube(neighborCheck.transform.TransformPoint(p), cubeScale);
				}
			}
		}	
	}
	public static bool Vector3Contains(List<Vector3> list, Vector3 value){
		
		bool found = false;
		foreach (Vector3 vector in list)
		{
			if (Mathf.Approximately(Vector3.Distance(vector, value),0f)){
				found = true;  
				break; 
			}
		}
		return found;
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
