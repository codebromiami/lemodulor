﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class Node : MonoBehaviour {

	public class NodeStart : ASignal <Node> {};

	public string id = "Node";
	public int divs = 0;
	public Node parentNode;
	public List<Node> childNodes;
	public float size = 1;
	public GameObject meshGo;

	private void Start()
	{
		id += parentNode ? " " + parentNode.childNodes.IndexOf(this).ToString(): "";
		gameObject.name = id;
		Signals.Get<NodeStart>().Dispatch(this);
	}

	public void OnDestroy()
	{
		GameObject.Destroy(this.gameObject);
	}

	void Update()
	{	
		if(divs > 0){
			
			if(divs > childNodes.Count){
				while(divs > childNodes.Count){
					var go = new GameObject();
					go.transform.SetParent(this.transform);
					go.transform.localPosition = Vector3.zero;
					var node = go.AddComponent<Node>();
					childNodes.Add(node);
					node.parentNode = this;
					node.meshGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
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
		}
		// Apply scale based child count
		if(childNodes != null){
			if(childNodes.Count > 0){
				foreach (Node item in childNodes)
				{
					item.size = size / divs;
				}
				// foreach (Node item in childNodes)
				// {
				// 	item.scale -= margin / (divs + 1);
				// }
				foreach (Node item in childNodes)
				{
					var scale = item.meshGo.transform.localScale;
					scale.x = item.size;
					item.meshGo.transform.localScale = scale;
				}
				foreach (Node item in childNodes)
				{
					var pos = item.transform.localPosition;
					int index = childNodes.IndexOf(item);
					float a = size /2;
					float b = a / divs;
					float offset = a - b;
					pos.x = (item.size * index) - offset;
					item.transform.localPosition = pos;
				}
			}	
		}

		if(meshGo)
			if(divs > 0) meshGo.SetActive(false); else meshGo.SetActive(true);
	}

	private void OnGUI()
	{
		
	}
}