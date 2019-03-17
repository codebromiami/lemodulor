using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class PilotiAgent : MonoBehaviour {
	
	public bool hit = false;
	public List<string> tags = new List<string>();
	public GameObject triggerGo;
	public Trigger[] triggers;
	public Vector3 hitPoint = Vector3.zero;
	public bool unTriggered = false;

	public void Update()
	{
		// int layermask = 0 << 8;
		// layermask = ~layermask;
		// RaycastHit[] hitInfos = Physics.RaycastAll(transform.position, Vector3.up,100, layermask, QueryTriggerInteraction.Ignore);
		// if(hitInfos != null){
		// 	if(hitInfos.Length > 1){
		// 		foreach(var hitInfo in hitInfos){
		// 			if(hitInfo.collider.gameObject.transform.parent.gameObject.name.Contains("Module")){
		// 				hitPoint = hitInfo.point;
		// 				triggerGo.transform.position = hitPoint;
		// 			}
		// 		}
		// 	}
		// }
		
		// if(hitPoint != Vector3.zero){
		// 	Vector3 floorHeight = transform.position;
		// 	floorHeight.y = 0;
		// 	float height = Vector3.Distance(floorHeight, hitPoint) / 2;
		// 	float width = (GroundPlan.instance.piloti.size / GroundPlan.instance.piloti.divs);
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
		Vector3 floorHeight = transform.position;
		floorHeight.y = 0;
		if(hitPoint != Vector3.zero){
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(floorHeight, hitPoint);
			Gizmos.color = Color.magenta;
			Gizmos.DrawCube(floorHeight, Vector3.one * 0.05f);
		}else{
			Gizmos.color = Color.cyan;
			Gizmos.DrawCube(floorHeight, Vector3.one * 0.05f);
		}
	}
}
