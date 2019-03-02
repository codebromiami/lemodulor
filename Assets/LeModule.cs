﻿using System.Collections;
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
	public int divs = 0;
	public LeModule parentNode;
	public List<LeModule> childNodes;
	public Vector3 size = Vector3.one;
	public GameObject meshGo;
	public Modulor leModulor;
	public bool visible = true;
	public bool hit = false;
		
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
			var node = childNodes[childNodes.Count-1];
			GameObject.Destroy(node.gameObject);
			childNodes.RemoveAt(childNodes.Count-1);
			Debug.Log(uid + " Removed child");
		}
	}

	public void Divide(LeModule.axis axis)
	{	
        divs = 2;
        divAxis = axis;
		// Create modules if this is the first time we've been divided
		if(divs > childNodes.Count){
			while(divs > childNodes.Count){
				var go = new GameObject();
				go.transform.SetParent(this.transform);
				go.transform.localPosition = Vector3.zero;
				var node = go.AddComponent<LeModule>();
				if(childNodes == null)
					childNodes = new List<LeModule>();
				childNodes.Add(node);
				node.parentNode = this;
				var prefab = Resources.Load<GameObject>("Prefabs/Cube");
				node.meshGo = GameObject.Instantiate(prefab);
				node.meshGo.transform.SetParent(node.transform);
				node.meshGo.transform.localPosition = Vector3.zero;
				Debug.Log( uid + "Added child");
			}
		}
		if(childNodes != null){
			if(childNodes.Count < 2){
				Debug.LogError(uid + "childNodes.Count < 2");
				return;
			}
		}else{
			Debug.LogError(uid + "childNodes list == null");
			return;
		}
		// Apply scale based child count
		
		List<float> ms = new List<float>();
		foreach (LeModule item in childNodes)
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
		foreach (LeModule item in childNodes) {
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
		// Effect position
		// index 0 pos = index 1 scale / 2
		// index 1 should be moved the scale of index 0 / 2
		foreach (LeModule item in childNodes)
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
				Debug.LogError(uid + "Index should not be greater than 1");
			}
			item.transform.localPosition = pos;
		}		
	}

    private void Update()
    {
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

	private void OnDrawGizmos()
	{
		
	}	
}