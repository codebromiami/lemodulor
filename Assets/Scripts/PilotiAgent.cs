using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class PilotiAgent : MonoBehaviour {
	public bool hit = false;
	
	private void OnTriggerEnter(Collider other)
	{
		var groundPlanModule = GroundPlan.instance.childModule;
		var parentModule = other.gameObject.GetComponentInParent<Module>();
		var a = groundPlanModule.size.x;
		var b = groundPlanModule.size.z;
		var c = a > b ? b : a;
		c *= 0.05f;
		transform.localScale = new Vector3(c,parentModule.size.y /2, c);
		var pos = transform.position;
		pos.y = parentModule.size.y /2;
		transform.position = pos;
	}
}
