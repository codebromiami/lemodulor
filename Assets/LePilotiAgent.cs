using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class LePilotiAgent : MonoBehaviour {
	
	public bool hit = false;
	public List<string> tags = new List<string>();
	public GameObject triggerGo;
	public Trigger[] triggers;
	public Vector3 hitPoint = Vector3.zero;
	public bool unTriggered = false;
	public bool set = false;
	public void Update()
	{
		if(!hit)
			return;
		
		if(!set){
			int layermask = 9;
			layermask = ~layermask;
			RaycastHit[] hitInfos = Physics.RaycastAll(transform.position, Vector3.up,100, layermask, QueryTriggerInteraction.Ignore);
			if(hitInfos != null){
				if(hitInfos.Length > 1){
					foreach(var hitInfo in hitInfos){
						Debug.Log(hitInfo.collider.gameObject.name);
						if(hitInfo.collider.gameObject.name.Contains("Roof")){
							hitPoint = hitInfo.point;
							triggerGo.transform.position = hitPoint;
							set = true;
							break;
						}
					}
				}
			}
		}
		
		
		// if(hitPoint != Vector3.zero){
		// 	Vector3 floorHeight = transform.position;
		// 	float height = Vector3.Distance(floorHeight, hitPoint) / 2;
		// 	float width = (ModuleSwitchScene.instance.pilotiController.size / ModuleSwitchScene.instance.pilotiController.divs);
		// 	transform.localScale = new Vector3(width, height, width);
		// 	var pos = transform.position;
		// 	pos.y = height;
		// 	transform.position = pos;
		// }
		
		// unTriggered = false;
		// if(hitPoint != Vector3.zero){
		// 	foreach(var trigger in triggers){
		// 		if(!trigger.triggered){
		// 			unTriggered = true;
		// 			break;
		// 		}
		// 	}
		// 	if(!unTriggered){
		// 		gameObject.GetComponent<MeshRenderer>().enabled = true;	
		// 	}else{
		// 		gameObject.GetComponent<MeshRenderer>().enabled = false;
		// 	}
		// }
	}

	private void OnDrawGizmos()
	{
		// Vector3 floorHeight = transform.position;
		// if(hitPoint != Vector3.zero){
		// 	Gizmos.color = Color.yellow;
		// 	Gizmos.DrawLine(floorHeight, hitPoint);
		// 	Gizmos.color = Color.magenta;
		// 	Gizmos.DrawCube(floorHeight, Vector3.one * 0.05f);
		// }else{
			Gizmos.color = Color.cyan;
			Gizmos.DrawCube(transform.position, Vector3.one);
			Gizmos.DrawLine(transform.position, triggerGo.transform.position);
			Gizmos.color = Color.cyan;
			Gizmos.DrawCube(triggerGo.transform.position, Vector3.one);
		// }
	}
}
