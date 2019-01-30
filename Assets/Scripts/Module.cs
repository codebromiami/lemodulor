using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;
using UnityEngine.Events;

public class Module : MonoBehaviour {

	public class ModuleStart : ASignal <Module> {};
	public enum axis {x,y,z}
	public axis divAxis = axis.x;
	public string id = "Module";
	public int divs = 0;
	public Module parentNode;
	public List<Module> childNodes = new List<Module>();
	public Vector3 size = Vector3.one;
	public GameObject meshGo;
	public Modulor leModulor;
	public bool visible = true;
	public bool hit = false;
	private void Start()
	{
		id += parentNode ? " " + parentNode.childNodes.IndexOf(this).ToString(): "";
		gameObject.name += id;
		List<Vector3> dirs = new List<Vector3>();
		foreach(var dir in dirs){

		}
		if(!meshGo){
			var prefab = Resources.Load<GameObject>("Prefabs/Cube");
			meshGo = GameObject.Instantiate(prefab, this.transform);
			meshGo.transform.localPosition = Vector3.zero;
			meshGo.transform.localScale = size;
		}
		Signals.Get<ModuleStart>().Dispatch(this);
	}

	public void OnDestroy()
	{
		GameObject.Destroy(this.gameObject);
	}

	void Update()
	{	
		// todo: switch statement to set the current axis count
		if(divs < 0)
			divs = 0;
			
		if(divs > childNodes.Count){
			while(divs > childNodes.Count){
				var go = new GameObject();
				go.transform.SetParent(this.transform);
				go.transform.localPosition = Vector3.zero;
				var node = go.AddComponent<Module>();
				childNodes.Add(node);
				node.parentNode = this;
				var prefab = Resources.Load<GameObject>("Prefabs/Cube");
				node.meshGo = GameObject.Instantiate(prefab);
				node.meshGo.transform.SetParent(node.transform);
				node.meshGo.transform.localPosition = Vector3.zero;
				// Debug.Log("Added child");
			}
		}else if(divs < childNodes.Count){
			while(divs < childNodes.Count){
				var node = childNodes[childNodes.Count-1];
				GameObject.Destroy(node.gameObject);
				childNodes.RemoveAt(childNodes.Count-1);
				// Debug.Log("Removed child");
			}
		}
		// Set the size of the un effected axises to the values in the parent
		if(parentNode){
			switch(parentNode.divAxis){
				case axis.x:
					size.y = parentNode.size.y;
					size.z = parentNode.size.z;
				break;
				case axis.y:
					size.x = parentNode.size.x;
					size.z = parentNode.size.z;
				break;
				case axis.z:
					size.x = parentNode.size.x;
					size.y = parentNode.size.y;
				break;					
			}
		}
		// Apply scale based child count
		if(childNodes != null){
			if(childNodes.Count > 0){
				List<float> ms = new List<float>();
				foreach (Module item in childNodes)
				{
					switch(divAxis){
						case axis.x:
						ms = Modulor.GetList(size.x,divs);
						break;
						case axis.y:
						ms = Modulor.GetList(size.y,divs);
						break;
						case axis.z:
						ms = Modulor.GetList(size.z,divs);
						break;
					}
				}
				for(int i = 0; i < childNodes.Count; i++){
					
					switch(divAxis){
						case axis.x:
						childNodes[i].size.x = ms[i];
						break;
						case axis.y:
						childNodes[i].size.y = ms[i];
						break;
						case axis.z:
						childNodes[i].size.z = ms[i];
						break;
					}
				}
				foreach (Module item in childNodes) {
					var scale = item.meshGo.transform.localScale;
					switch(divAxis){
						case axis.x:
							scale.x = item.size.x;
							scale.y = size.y;
							scale.z = size.z;
							// scale.x -= margin;
						break;
						case axis.y:
							scale.x = size.x;
							scale.y = item.size.y;
							scale.z = size.z;
							// scale.y -= margin;
						break;
						case axis.z:
							scale.x = size.x;
							scale.y = size.y;
							scale.z = item.size.z;
							// scale.z -= margin;
						break;					
					}
					item.meshGo.transform.localScale = scale;
				}
				
					// foreach (Module item in childNodes)
					// {
					// 	switch(parentNode.divAxis){
					// 		case axis.x:
					// 			size.y = parentNode.size.y;
					// 			size.z = parentNode.size.z;
					// 		break;
					// 		case axis.y:
					// 			size.x = parentNode.size.x;
					// 			size.z = parentNode.size.z;
					// 		break;
					// 		case axis.z:
					// 			size.x = parentNode.size.x;
					// 			size.y = parentNode.size.y;
					// 		break;					
					// 	}
					// }
				// Effect position
				// index 0 pos = index 1 scale / 2
				// index 1 should be moved the scale of index 0 / 2
				foreach (Module item in childNodes)
				{
					var pos = item.transform.localPosition;
					int index = childNodes.IndexOf(item);
					if(index == 0){
						switch(divAxis){
							case axis.x:
								pos.x = childNodes[1].size.x /2 * -1;
							break;
							case axis.y:
								pos.y = childNodes[1].size.y /2 * -1;
							break;
							case axis.z:
								pos.z = childNodes[1].size.z /2 * -1;
							break;					
						}
					}else if(index == 1){
						switch(divAxis){
							case axis.x:
								pos.x = childNodes[0].size.x /2;
							break;
							case axis.y:
								pos.y = childNodes[0].size.y /2;
							break;
							case axis.z:
								pos.z = childNodes[0].size.z /2;
							break;					
						}
					}else{
						Debug.LogError("Index should not be greater than 1");
					}
					item.transform.localPosition = pos;
				}
				// // Effect position
				// foreach (Module item in childNodes)
				// {
				// 	var pos = item.transform.localPosition;
				// 	int index = childNodes.IndexOf(item);
				// 	float a = 0;
				// 	float b = 0;
				// 	float offset = 0;
				// 	switch(divAxis){
				// 		case axis.x:
				// 			a = size.x /2;
				// 			b = a / divs;
				// 			offset = a - b;
				// 			pos.x = (item.size.x * index) - offset;	
				// 		break;
				// 		case axis.y:
				// 			a = size.y /2;
				// 			b = a / divs;
				// 			offset = a - b;
				// 			pos.y = (item.size.y * index) - offset;	
				// 		break;
				// 		case axis.z:
				// 			a = size.z /2;
				// 			b = a / divs;
				// 			offset = a - b;
				// 			pos.z = (item.size.z * index) - offset;	
				// 		break;					
				// 	}
				// 	item.transform.localPosition = pos;
				// }
			}	
		}
		if(meshGo){
			if(divs > 0){
				visible = false;
			}
		}
		if(visible){
			meshGo.SetActive(true);
		}else{
			meshGo.SetActive(false);
		}
	}

	public void Check(){

		if(parentNode){
			foreach (Module item in childNodes)
			{
				switch(parentNode.divAxis){
					case axis.x:
						size.x = parentNode.size.x / parentNode.divs;
					break;
					case axis.y:
						size.y = parentNode.size.y / parentNode.divs;
					break;
					case axis.z:
						size.z = parentNode.size.z / parentNode.divs;
					break;					
				}
			}	
			if(parentNode.parentNode){
				foreach (Module item in childNodes)
				{
					switch(parentNode.parentNode.divAxis){
						case axis.x:
							size.x = parentNode.parentNode.size.x / parentNode.parentNode.divs;
						break;
						case axis.y:
							size.y = parentNode.parentNode.size.y / parentNode.parentNode.divs;
						break;
						case axis.z:
							size.z = parentNode.parentNode.size.z / parentNode.parentNode.divs;
						break;					
					}
				}
				if(parentNode.parentNode.parentNode){
					foreach (Module item in childNodes)
					{
						switch(parentNode.parentNode.parentNode.divAxis){
							case axis.x:
								size.x = parentNode.parentNode.parentNode.size.x / parentNode.parentNode.parentNode.divs;
							break;
							case axis.y:
								size.y = parentNode.parentNode.parentNode.size.y / parentNode.parentNode.parentNode.divs;
							break;
							case axis.z:
								size.z = parentNode.parentNode.parentNode.size.z / parentNode.parentNode.parentNode.divs;
							break;					
						}
					}
				}else{
					foreach (Module item in childNodes)
					{
						switch(parentNode.parentNode.divAxis){
							case axis.x:
								size.x = parentNode.parentNode.size.x / parentNode.parentNode.divs;
							break;
							case axis.y:
								size.y = parentNode.parentNode.size.y / parentNode.parentNode.divs;
							break;
							case axis.z:
								size.z = parentNode.parentNode.size.z / parentNode.parentNode.divs;
							break;					
						}
					}
				}
			}else{
				foreach (Module item in childNodes) {
					switch(parentNode.divAxis){
						case axis.x:
							size.x = parentNode.size.x / parentNode.divs;
						break;
						case axis.y:
							size.y = parentNode.size.y / parentNode.divs;
						break;
						case axis.z:
							size.z = parentNode.size.z / parentNode.divs;
						break;					
					}
				}
			}
		}
	}

	private void OnGUI()
	{
		
	}
}