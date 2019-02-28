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
	public Piloti piloti;
	public int exceed = 0;
	public int initCount = 0;
	public int piltoiGroups = 0;
	public List<List<string>> groups = new List<List<string>>();
	public float scaleFactor = 0.1f;
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
		piloti = go.AddComponent<Piloti>();
		piloti.module = childModule;
		var a = size.y * 10;
		var b = a / size.x;
		piloti.divs = Mathf.RoundToInt(b);
		go.name = "Piloti";
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.C)){
			
			MaterialPropertyBlock props = new MaterialPropertyBlock();
			Renderer renderer;
			foreach(var module in modules){
				if(module.meshGo){
					renderer = module.meshGo.GetComponent<Renderer>();
					float r = Random.Range(0.0f, 1.0f);
					float g = Random.Range(0.0f, 1.0f);
					float b = Random.Range(0.0f, 1.0f);
					props.SetColor("_Color", new Color(r, g, b));
					if(module.size.x > module.size.z){
						props.SetVector("_ST", new Vector4(module.size.x * scaleFactor, module.size.y * scaleFactor,1,1));
					}else{
						props.SetVector("_ST", new Vector4(module.size.z * scaleFactor, module.size.y * scaleFactor,1,1));
					}
					renderer.SetPropertyBlock(props);
				}
			}
		}	

		if(Input.GetKeyDown(KeyCode.B)){
			childModule.divs = 0;
			modules.Clear();
		}
		if(Input.GetKeyDown(KeyCode.N)){
		}
	}

	// When each module is created we subdivide it along a random axis
	// The swipe generated a number of points, we subdivide if the number of modules 
	// subdivded is lower than the points created.
	public void onModuleStart(Module _module)
	{	
		modules.Add(_module);

		MaterialPropertyBlock props = new MaterialPropertyBlock();
		Renderer renderer;
		if(_module.meshGo){
		renderer = _module.meshGo.GetComponent<Renderer>();
		float r = Random.Range(0.0f, 1.0f);
		float g = Random.Range(0.0f, 1.0f);
		float b = Random.Range(0.0f, 1.0f);
		props.SetColor("_Color", new Color(r, g, b));
		if(_module.size.x > _module.size.z){
			props.SetVector("_ST", new Vector4(_module.size.x * scaleFactor, _module.size.y * scaleFactor,1,1));
		}else{
			props.SetVector("_ST", new Vector4(_module.size.z * scaleFactor, _module.size.y * scaleFactor,1,1));
		}
		renderer.SetPropertyBlock(props);
	}
		if(modules.Count >= limit){
			if(modules.Count == limit){
				// StartCoroutine(Build());
				
			}else{
				exceed++;
			}
			return;
		}
		foreach(var mod in modules){
			Divide(mod);
		}
	}

	public void Divide(Module _module){
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

	IEnumerator Build(){
		
		yield return new WaitForSeconds(1f);
		childModule.divs = 0;
		modules.Clear();
		yield return new WaitForSeconds(0.1f);
		childModule.divs = 2;

		// foreach(var point in points){
		// 	Vector3 position = point;
		// 	position.y -= 1;
		// 	Ray ray = new Ray(position, Vector3.up);
		// 	RaycastHit[] hits;
		// 	hits = Physics.RaycastAll(ray, 100);
		// 	foreach(var hit in hits){
		// 		Module module = hit.collider.gameObject.GetComponentInParent<Module>();
		// 		if(module != null){
		// 			module.hit = true;
		// 		}
		// 	}
		// }

		// foreach(var module in modules){
		// 	if(module.hit){
		// 		module.visible = true;
		// 		module.gameObject.name = "Module Visible";
		// 	}else{
		// 		module.visible = false;
		// 		module.gameObject.name = "Module Invisible";
		// 	}
		// }
		// // Take all the modules that were hit by the touch and add a NeighborCheck
		// neighborChecks = new List<NeighborCheck>();
		// foreach(var module in modules){
		// 	if(module.hit){
		// 		var neighborCheck = module.gameObject.AddComponent<NeighborCheck>();
		// 		neighborCheck.module = module;
		// 		neighborChecks.Add(neighborCheck);
		// 	}else{
		// 		module.visible = false;
		// 	}
		// }
		// foreach (var neighborCheck in neighborChecks){
		// 	neighborCheck.Check();
		// }
		// // We iterate through all the visible modules that are tagged to be piloti
		// foreach (var neighborCheck in neighborChecks)
		// {
		// 	if(neighborCheck.module.visible){
		// 		if(neighborCheck.tags.Contains("Ground") && !neighborCheck.tags.Contains("Roof")){	
		// 			neighborCheck.tags.Add("Piloti");
		// 			neighborCheck.tags.Add(piltoiGroups.ToString());
		// 			GroundPlan.instance.piltoiGroups++;
		// 		}
		// 	}
		// }
		// foreach (var neighborCheck in neighborChecks){
		// 		neighborCheck.Check();
		// }
		// // Loop through all the modules tagged piloti and double check they on the ground
		// foreach (var neighborCheck in neighborChecks)
		// {
		// 	if(neighborCheck.gameObject.name.Contains("Piloti")){
		// 		var value = neighborCheck.transform.position.y;
		// 		if(!Mathf.Approximately(value,size.y /2)){
		// 			if(value > size.y /2){
		// 				neighborCheck.tags.Remove("Piloti");
		// 				Debug.Log(string.Format("Piloti '{0}' is too high. Position: {1} Module Size / 2: {2}",neighborCheck.gameObject.name, value, size.y / 2));
		// 			}
		// 		}
		// 	}
		// }
		// foreach (var neighborCheck in neighborChecks){
		// 		neighborCheck.Check();
		// }

		// foreach (var neighborCheck in neighborChecks)
		// {
		// 	// For all the piloti modules we look for neihboring piloti modules
		// 	if(neighborCheck.module.visible){
		// 		if(neighborCheck.tags.Contains("Piloti")){	
		// 			Debug.Log(string.Format("Checking: {0}", neighborCheck.gameObject.name));
		// 			// groupList is the list that contains this gameObject
		// 			var groupList = IsInLists(neighborCheck.gameObject.name, groups);
		// 			if(groupList != null){
		// 				//Add the gameObjects this gameObjects has been tagged with
		// 				foreach(var str in neighborCheck.groudIds){
		// 					if(!groupList.Contains(str)){
		// 						groupList.Add(str);
		// 					}
		// 				}
		// 			}else{
		// 				// This gameObject isn't in any list, check if any of the gameObjects it's been tagged with are in any of the groups
		// 				bool b = false;
		// 				foreach(var groupID in neighborCheck.groudIds){
		// 					var subList = IsInLists(groupID, groups);
		// 					if(subList != null){
		// 						// One of the gameObjects is in a group, so lets add this gameObject to that list
		// 						subList.Add(neighborCheck.gameObject.name);
		// 						b = true;
		// 					}
		// 				}
		// 				// This gameObjects groupID's were not in any group
		// 				if(!b){
		// 					// create a new list around a particular module
		// 					var newList = new List<string>();
		// 					newList.Add(neighborCheck.gameObject.name);
		// 					newList.AddRange(neighborCheck.groudIds);
		// 					groups.Add(newList);
		// 					// neighbors.Add(neighborCheck.gameObject);
		// 				}
		// 			}
		// 		}
		// 	}
		// }
		// foreach (var neighborCheck in neighborChecks){
		// 	neighborCheck.Check();
		// }
		// // Log the group info and give the modules in groups an index to the group
		// int index = 0;
		// foreach(var group in groups){
		// 	Debug.Log(string.Format("Group {0} count: {1}",index, group.Count));
		// 	string groupIDs = "Group: " + index.ToString();
		// 	foreach(string name in group){
		// 		groupIDs += " " + name + ",";
		// 	}
		// 	Debug.Log(groupIDs);
		// 	index++;
		// }
		// foreach (var neighborCheck in neighborChecks)
		// {
		// 	if(neighborCheck.module.visible){
		// 		if(neighborCheck.gameObject.name.Contains("Piloti")){	
		// 			neighborCheck.module.meshGo.GetComponent<Collider>().isTrigger = true;
		// 		}
		// 	}
		// }
		// var piloti = FindObjectOfType<Piloti>();
		// List<Vector3> tmp = new List<Vector3>();
		
		// foreach(var line in piloti.lines){
			
		// 	if(piloti.lines.IndexOf(line) == 0)
		// 		continue;

		// 	foreach(var point in line.points){
		// 		if(!Vector3Contains(tmp, point)){
		// 			tmp.Add(point);
		// 		}else{
		// 			Debug.Log("Duplicate Point");
		// 		}
		// 	}
		// }
		// var go = Resources.Load<GameObject>("Prefabs/PilotiAgent");
		// foreach(var point in tmp){
		// 	go = Instantiate(go);
		// 	go.transform.SetParent(piloti.transform);
		// 	go.transform.localPosition = point;
		// 	go.name = "Piloti Agent: " + point.ToString();
		// 	var scale = go.transform.localScale;
		// 	scale.x = piloti.size;
		// 	scale.z = piloti.size;
		// 	go.transform.localScale = scale;
		// 	var pilotiAgent = go.GetComponent<PilotiAgent>();
		// 	piloti.pilotiAgents.Add(pilotiAgent);
		// }
		// // Hide all the modules tagged 'piloti'
		// foreach (var neighborCheck in neighborChecks)
		// {
		// 	if(neighborCheck.module.visible){
		// 		if(neighborCheck.gameObject.name.Contains("Piloti")){	
		// 			neighborCheck.module.meshGo.GetComponent<MeshRenderer>().enabled = false;
		// 			yield return null;
		// 		}
		// 	}
		// }

		// // Add all the modules tagged 'piloti' to list and scale them
		// foreach (var neighborCheck in neighborChecks)
		// {
		// 	if(neighborCheck.module.visible){
		// 		if(neighborCheck.gameObject.name.Contains("Piloti")){	
		// 			// neighborCheck.module.size *= 0.6f;
		// 			piloti.pilotiModules.Add(neighborCheck.module);
		// 		}
		// 	}
		// }
		// // Check through each of the pilotiAgents to see if it collides with a piloti module
		// foreach(var pilotiAgent in piloti.pilotiAgents){
			
		// 	var colliderA = pilotiAgent.GetComponent<Collider>();
		// 	foreach(var module in piloti.pilotiModules){
		// 		var colliderB = module.meshGo.GetComponent<Collider>();
		// 		if(colliderA.bounds.Intersects(colliderB.bounds)){
		// 			pilotiAgent.hit = true;
		// 			pilotiAgent.tags.Add(colliderB.gameObject.name);
		// 		}
		// 	}
		// }
	}
	
	private void OnDrawGizmos()
	{
		if(!Application.isPlaying)
			return;

		Vector3 cubeScale = Vector3.one * 0.05f;
		// if(piloti){
		// 	Gizmos.color = Color.white;
		// 	Gizmos.DrawCube(piloti.transform.position, piloti.module.size);
		// }
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
				
			}else{
				if(neighborCheck.module.visible){
				Gizmos.color = Color.black;
				Gizmos.DrawWireCube(neighborCheck.module.transform.position, neighborCheck.module.size);
			}else{
				Gizmos.color = Color.white;
				Gizmos.DrawWireCube(neighborCheck.module.transform.position, neighborCheck.module.size);
			}
			}
		}	

		foreach(var module in piloti.pilotiModules){
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(module.transform.position, module.size);
			Gizmos.color = Color.white;
			foreach(var p in module.GetComponent<NeighborCheck>().boundingPoints.all){
				Gizmos.DrawCube(module.transform.TransformPoint(p), cubeScale);
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
