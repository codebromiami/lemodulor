using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class LePilotiAgent : MonoBehaviour {
	
	public List<string> tags = new List<string>();
	public GameObject triggerGo;
	public Trigger[] triggers;
	public Vector3 hitPoint = Vector3.zero;
	public bool unTriggered = false;
	public bool hitRoof = false;
	
	public void Update()
	{
		
		bool moveTrigger = false;
		int layermask = 9;
		layermask = ~layermask;
		RaycastHit[] hitInfos = Physics.RaycastAll(transform.position, Vector3.up, 117773.5f, layermask, QueryTriggerInteraction.Ignore);
		if(hitInfos != null){
			if(hitInfos.Length > 1){
				foreach(var hitInfo in hitInfos){
					if(hitInfo.collider.gameObject.name.Contains("Roof")){
						Debug.Log(string.Format("{0} hit roof", transform.position));
						hitRoof = true;
						hitPoint = hitInfo.point;
						triggerGo.transform.position = hitPoint;
						moveTrigger = true;
					}
				}
			}
		}
		
		if(!moveTrigger){
			triggerGo.transform.localPosition = Vector3.zero;
		}

		if(moveTrigger){

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
		if(hitRoof)
			Gizmos.color = Color.cyan;
		else
			Gizmos.color = Color.magenta;
		Gizmos.DrawCube(transform.position, Vector3.one);
		Gizmos.DrawLine(transform.position, triggerGo.transform.position);
		Gizmos.DrawCube(triggerGo.transform.position, Vector3.one);
	}
}
