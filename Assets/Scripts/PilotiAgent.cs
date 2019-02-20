using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class PilotiAgent : MonoBehaviour {
	
	public bool hit = false;
	public List<string> tags = new List<string>();
	
	private void OnTriggerEnter(Collider other)
	{
		// var groundPlanModule = GroundPlan.instance.childModule;
		// var parentModule = other.gameObject.GetComponentInParent<Module>();
		// var a = groundPlanModule.size.x;
		// var b = groundPlanModule.size.z;
		// var c = a > b ? b : a;
		// c *= 0.05f;
		// transform.localScale = new Vector3(c,parentModule.size.y /2, c);
		// var pos = transform.position;
		// pos.y = parentModule.size.y /2;
		// transform.position = pos;
	}
	public Vector3 hitPoint;
	private void Update()
	{
		
		RaycastHit[] hitInfos = Physics.RaycastAll(transform.position, Vector3.up * 100);
		if(hitInfos != null){
			if(hitInfos.Length > 1){
				foreach(var hitInfo in hitInfos){
					if(hitInfo.collider.gameObject.transform.parent.gameObject.name.Contains("Module")){
						hitPoint = hitInfo.point;
					}
				}
			}
		}
		
	}
	private void OnDrawGizmos()
	{
		if(hitPoint != Vector3.zero){
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(transform.position, hitPoint);
			Gizmos.color = Color.magenta;
			Gizmos.DrawCube(transform.position, Vector3.one * 0.05f);
		}else{
			Gizmos.color = Color.cyan;
			Gizmos.DrawCube(transform.position, Vector3.one * 0.05f);
		}
	}
}
