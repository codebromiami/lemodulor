using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class Module : MonoBehaviour {

	public class ModuleStart : ASignal <Module> {};
	public enum axis {x,y,z}
	public axis divAxis = axis.x;
	public string id = "Node";
	public int divs = 0;
	public Module parentNode;
	public List<Module> childNodes;
	public Vector3 size = Vector3.one;
	public GameObject meshGo;
	public float margin = 0; // how much space on each side to leave between the next
	
	private void Start()
	{
		id += parentNode ? " " + parentNode.childNodes.IndexOf(this).ToString(): "";
		gameObject.name = id;
		Signals.Get<ModuleStart>().Dispatch(this);
	}

	public void OnDestroy()
	{
		GameObject.Destroy(this.gameObject);
	}

	void Update()
	{	
		// todo: switch statement to set the current axis count
		
		if(divs > 0){
			
			if(divs > childNodes.Count){
				while(divs > childNodes.Count){
					var go = new GameObject();
					go.transform.SetParent(this.transform);
					go.transform.localPosition = Vector3.zero;
					var node = go.AddComponent<Module>();
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
				foreach (Module item in childNodes)
				{
					item.size = size / divs;
				}
				margin = Mathf.Clamp(margin,0f, (size.x / divs) - 0.01f);	// 0.01 because if we use the exact value the model doesn't render properly
				foreach (Module item in childNodes)
				{
					var scale = item.meshGo.transform.localScale;
					scale.x = item.size.x;
					scale.x -= margin;
					item.meshGo.transform.localScale = scale;
				}
				foreach (Module item in childNodes)
				{
					var pos = item.transform.localPosition;
					int index = childNodes.IndexOf(item);
					float a = size.x /2;
					float b = a / divs;
					float offset = a - b;
					pos.x = (item.size.x * index) - offset;
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