using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;
using UnityEngine.Events;

public class LeModule : MonoBehaviour {

	public class OnStart : ASignal<LeModule> {}
	public class ModuleStart : ASignal <Module> {};
	public enum axis {x,y,z}
	public string uid = "Add Monobehaviour instance ID";
	public string id = "Module";
	public axis subdivisionAxis = axis.x;
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
		Signals.Get<OnStart>().Dispatch(this);
	}

	public void OnDestroy()
	{
		GameObject.Destroy(this.gameObject);
	}
	
	public void UnDivide(){
		if(!parent){
			Debug.LogError(uid + " has no parent, cannot un divide");
		}else{
			if(children != null){
				Debug.LogError(uid + " has children, Undivide should be called on the children");
				return;
			}else{
				
				for(int i = 0; i < parent.children.Count; i++){
					var child = parent.children[i];
					GameObject.Destroy(child.gameObject);
					Debug.Log(string.Format("{0} removed children: {1}", parent.uid, child.uid));
				}
				parent.children = null;
				parent.meshGo.SetActive(true);
				parent.GetComponent<BoxCollider>().enabled = true;
			}
		}
	}

	public void Subdivide(LeModule.axis axis)
	{	
        subdivisionAxis = axis;
		if(children == null)
			children = new List<LeModule>();
		// Create modules if this is the first time we've been divided
		while(children.Count < 2){
			var go = new GameObject();
			go.transform.SetParent(this.transform);
			go.transform.localPosition = Vector3.zero;
			var node = go.AddComponent<LeModule>();
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
		foreach (LeModule child in children)
		{
			switch(subdivisionAxis){
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
		// Check sizes to see if they are out of bounds
		foreach(var value in ms){

		}
		// Revese the list randomly
		if(ExtRandom<bool>.Chance(1,2)){
			ms.Reverse();
		}
		
		for(int i = 0; i < children.Count; i++){
			switch(subdivisionAxis){
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
		foreach (LeModule child in children) {
			var scale = child.meshGo.transform.localScale;
			switch(subdivisionAxis){
				case axis.x:
					scale.x = child.size.x;
					scale.y = size.y;
					scale.z = size.z;
				break;
				case axis.y:
					scale.x = size.x;
					scale.y = child.size.y;
					scale.z = size.z;
				break;
				case axis.z:
					scale.x = size.x;
					scale.y = size.y;
					scale.z = child.size.z;
				break;					
			}
			child.meshGo.transform.localScale = scale;
		}

		// Effect position
		// index 0 pos = index 1 scale / 2
		// index 1 should be moved the scale of index 0 / 2
		foreach (LeModule child in children)
		{
			var pos = child.transform.localPosition;
			int childIndex = children.IndexOf(child);
			if(childIndex == 0){
				switch(subdivisionAxis){
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
			}else if(childIndex == 1){
				switch(subdivisionAxis){
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
			child.transform.localPosition = pos;
		}	
		// Set the size of the un effected axises to the values in the parent
		foreach (LeModule child in children){
			switch(subdivisionAxis){
				case axis.x:
					child.size.y = size.y;
					child.size.z = size.z;
				break;
				case axis.y:
					child.size.x = size.x;
					child.size.z = size.z;
				break;
				case axis.z:
					child.size.x = size.x;
					child.size.y = size.y;
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
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(transform.position, size);
	}	
}