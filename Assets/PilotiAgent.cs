using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotiAgent : MonoBehaviour {

	public bool set = false;

	public void Build(Pilotis piloti, Vector3 position)
	{
		transform.position = position;
		float height = Vector3.Distance(position, new Vector3(position.x, 0, position.z));
		height = FloatHelper.Truncate(height, 4);
		var scale = new Vector3(piloti.pilotiWidth, height/2, piloti.pilotiWidth);
		transform.localScale = scale;
		var pos = transform.position;
		pos.y -= height /2;
		transform.position = pos;
		set = true;

		// transform.position = position;
		// int layerMask = 1 << 8;
		// float height = 0;
		// layerMask = ~layerMask;
		// RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, Mathf.Infinity, layerMask);
		// foreach(var hit in hits){
		// 	if(set)
		// 		break;
		// 	if(hit.collider.gameObject.name == "GroundPlane"){
		// 		height = hit.distance;
		// 		height = FloatHelper.Truncate(height, 4);
		// 		var scale = new Vector3(piloti.pilotiWidth, height/2, piloti.pilotiWidth);
		// 		transform.localScale = scale;
		// 		var pos = transform.position;
		// 		pos.y -= height /2;
		// 		transform.position = pos;
		// 		set = true;
		// 	}
		// }
	}
}
