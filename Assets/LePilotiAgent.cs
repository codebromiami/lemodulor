using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class LePilotiAgent : MonoBehaviour {
	
	public List<string> tags = new List<string>();
	public GameObject triggerGo;
	public Trigger[] triggers;
	public Vector3 hitPoint = Vector3.zero;
	public MeshRenderer meshRenderer;
	public bool hitRoof = false;
	bool visible = false;

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
			meshRenderer.enabled = true;
			float width = 2.26f;
			float height = Vector3.Distance(transform.position, triggerGo.transform.position) / 2;
			meshRenderer.transform.localScale = new Vector3(width, height, width);
			var newPosition = meshRenderer.transform.localPosition;
			newPosition.y = height;
			meshRenderer.transform.localPosition = newPosition;
		}else{
			meshRenderer.enabled = false;
		}
		
		visible = false;
		if(moveTrigger){
			foreach(var trigger in triggers){
				if(!trigger.triggered){
					visible = true;
					break;
				}
			}
			if(visible){
				meshRenderer.enabled = false;
			}
		}
	}

	private void OnDrawGizmos()
	{
		if(visible)
			Gizmos.color = Color.cyan;
		else if (hitRoof)
			Gizmos.color = Color.magenta;
		else
			Gizmos.color = Color.gray;
		Gizmos.DrawCube(transform.position, Vector3.one);
		Gizmos.DrawLine(transform.position, triggerGo.transform.position);
		Gizmos.DrawCube(triggerGo.transform.position, Vector3.one);
	}
}
