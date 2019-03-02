using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;
using UnityEngine.Events;

public class LeModule : MonoBehaviour {

	public class ModuleStart : ASignal <Module> {};
	public enum axis {x,y,z}
	public axis divAxis = axis.x;
	public string uid = "Add Monobehaviour instance ID";
	public string id = "Module";
	public LeModule parent;
	public List<LeModule> children;
	public Vector3 size = Vector3.one;
	public GameObject meshGo;
		
	private void Start()
	{
		gameObject.name = id;
		if(!meshGo){
			var prefab = Resources.Load<GameObject>("Prefabs/Cube");
			meshGo = GameObject.Instantiate(prefab, this.transform);
			meshGo.transform.localPosition = Vector3.zero;
			meshGo.transform.localScale = size;
		}
		uid = GetInstanceID().ToString();
		gameObject.name += uid;
	}

	public void OnDestroy()
	{
		GameObject.Destroy(this.gameObject);
	}
	
	public void UnDivide(){
		for(int i = 0; i < 2; i++){
			var node = children[children.Count-1];
			GameObject.Destroy(node.gameObject);
			children.RemoveAt(children.Count-1);
			Debug.Log(uid + " Removed child");
		}
	}

	public void Subdivide(LeModule.axis axis)
	{	
        divAxis = axis;
		// Create modules if this is the first time we've been divided
		while(children.Count < 2){
			var go = new GameObject();
			go.transform.SetParent(this.transform);
			go.transform.localPosition = Vector3.zero;
			var node = go.AddComponent<LeModule>();
			if(children == null)
				children = new List<LeModule>();
			children.Add(node);
			node.parent = this;
			var prefab = Resources.Load<GameObject>("Prefabs/Cube");
			node.meshGo = GameObject.Instantiate(prefab);
			node.meshGo.transform.SetParent(node.transform);
			node.meshGo.transform.localPosition = Vector3.zero;
			Debug.Log( uid + " Added child");
		}
		if(children != null){
			if(children.Count < 2){
				Debug.LogError(uid + " childNodes.Count < 2");
				return;
			}
		}else{
			Debug.LogError(uid + " childNodes list == null");
			return;
		}
		// Get a list containing two modulor values
		List<float> ms = new List<float>();
		foreach (LeModule item in children)
		{
			switch(divAxis){
				case axis.x:
				ms = Modulor.GetList(size.x,2);
				break;
				case axis.y:
				ms = Modulor.GetList(size.y,2);
				break;
				case axis.z:
				ms = Modulor.GetList(size.z,2);
				break;
			}
		}
		// Revese the list randomly
		if(ExtRandom<bool>.Chance(1,2)){
			ms.Reverse();
		}
		
		for(int i = 0; i < children.Count; i++){
			switch(divAxis){
				case axis.x:
				children[i].size.x = ms[i];
				break;
				case axis.y:
				children[i].size.y = ms[i];
				break;
				case axis.z:
				children[i].size.z = ms[i];
				break;
			}
		}
		foreach (LeModule item in children) {
			var scale = item.meshGo.transform.localScale;
			switch(divAxis){
				case axis.x:
					scale.x = item.size.x;
					scale.y = size.y;
					scale.z = size.z;
				break;
				case axis.y:
					scale.x = size.x;
					scale.y = item.size.y;
					scale.z = size.z;
				break;
				case axis.z:
					scale.x = size.x;
					scale.y = size.y;
					scale.z = item.size.z;
				break;					
			}
			item.meshGo.transform.localScale = scale;
		}

		// Effect position
		// index 0 pos = index 1 scale / 2
		// index 1 should be moved the scale of index 0 / 2
		foreach (LeModule item in children)
		{
			var pos = item.transform.localPosition;
			int index = children.IndexOf(item);
			if(index == 0){
				switch(divAxis){
					case axis.x:
						pos.x = children[1].size.x /2 * -1;
						pos.y = 0;
						pos.z = 0;
					break;
					case axis.y:
						pos.x = 0;
						pos.y = children[1].size.y /2 * -1;
						pos.z = 0;
					break;
					case axis.z:
						pos.x = 0;
						pos.y = 0;
						pos.z = children[1].size.z /2 * -1;
					break;					
				}
			}else if(index == 1){
				switch(divAxis){
					case axis.x:
						pos.x = children[0].size.x /2;
						pos.y = 0;
						pos.z = 0;
					break;
					case axis.y:
						pos.x = 0;
						pos.y = children[0].size.y /2;
						pos.z = 0;
					break;
					case axis.z:
						pos.x = 0;
						pos.y = 0;
						pos.z = children[0].size.z /2;
					break;					
				}
			}else{
				Debug.LogError(uid + "Index should not be greater than 1");
			}
			item.transform.localPosition = pos;
		}	

		// Set the size of the un effected axises to the values in the parent
		if(parent){
			switch(parent.divAxis){
				case axis.x:
					size.y = parent.size.y;
					size.z = parent.size.z;
				break;
				case axis.y:
					size.x = parent.size.x;
					size.z = parent.size.z;
				break;
				case axis.z:
					size.x = parent.size.x;
					size.y = parent.size.y;
				break;					
			}
		}

		// Hide self
		foreach (LeModule item in children)
		{
			item.meshGo.SetActive(true);
		}
		meshGo.SetActive(false);
	}

	private void OnDrawGizmos()
	{
		
	}	
}