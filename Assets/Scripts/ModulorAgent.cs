using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulorAgent : MonoBehaviour {

	public string id;
	public BonedCube bc;
	public Vector3 center = Vector3.zero;
	public List<Vector3> pilotis = new List<Vector3>();

	// a and b do not have the same magnitude. 
	public static Vector3 VectorBetween2(Vector3 a, Vector3 b){
		
		// Position half-way between a and b: 
		// (This is the same as above)
		Vector3 cL = (a + b) / 2.0f;
		
		// Rotated half-way between a and b: 
		return Vector3.Slerp(a, b, 0.5f);
	}

	// Use this for initialization
	void Start () {

		this.gameObject.name = id;

		// Set the size of the volume to the closest measurement contained in the red series
		float length = LeModular.GetClosest(bc.length);
		float width = LeModular.GetClosest(bc.width);
		float height = LeModular.GetClosest(bc.height);
		bc.Set(length,width,height);
		
		// Get the center of object
		center = VectorBetween2(bc.a.position, bc.b.position);	
		center.y += bc.height/2;	

		GameObject go = new GameObject();
		go.transform.SetParent(this.transform);
		go.transform.position = center;
		go.name = "center";
				
		// // Check to see if we're off the ground
		// if(bc.a.position.y > (3 * scale)){
		// 	bool hasPilotis = false;
		// 	// Check to see if we're big enough to have pilotis
		// 	if(bc.width > 1 * scale){
		// 		// lay pilotis along the length and then the width
		// 		hasPilotis = true;
		// 		float distance = 0;
		// 		while(distance < bc.length){
		// 			var cylinder = Resources.Load<GameObject>("BonedCube/Cylinder");
		// 			var pos = center;
		// 			pos.y -= (bc.height /2) + (cylinder.transform.localScale.y /2) * scale;
		// 			pos.x -= bc.width /2 - cylinder.transform.localScale.x * scale;
		// 			pos.z -= bc.length/2 - cylinder.transform.localScale.x * scale;
		// 			pos.z += distance; 
		// 			cylinder = Instantiate(cylinder, pos, Quaternion.identity, go.transform);
		// 			cylinder.transform.localScale = (Vector3.one * 0.5f) * scale;
		// 			distance += 1f * scale;
		// 		}
		// 		distance = 0;
		// 		while(distance < bc.length){
		// 			var cylinder = Resources.Load<GameObject>("BonedCube/Cylinder");
		// 			var pos = center;
		// 			pos.y -= (bc.height /2) + (cylinder.transform.localScale.y /2) * scale;
		// 			pos.x += bc.width /2 + (cylinder.transform.localScale.x /2) * scale;
		// 			pos.z -= bc.length/2 - (cylinder.transform.localScale.x /2) * scale;
		// 			pos.z += distance; 
		// 			cylinder = Instantiate(cylinder, pos, Quaternion.identity, go.transform);
		// 			cylinder.transform.localScale = (Vector3.one * 0.5f) * scale;
		// 			distance += 1f * scale;
		// 		}
		// 	}
		// 	if(bc.length > 1 * scale){
		// 		hasPilotis = true;
		// 		float distance = 0;
		// 		while(distance < bc.width){
		// 			var cylinder = Resources.Load<GameObject>("BonedCube/Cylinder");
		// 			var pos = center;
		// 			pos.y -= (bc.height /2) + (cylinder.transform.localScale.y /2) * scale;
		// 			pos.x -= bc.width /2 - (cylinder.transform.localScale.x /2) * scale;
		// 			pos.z -= bc.length/2 - (cylinder.transform.localScale.x /2) * scale;
		// 			pos.x += distance; 
		// 			cylinder = Instantiate(cylinder, pos, Quaternion.identity, go.transform);
		// 			cylinder.transform.localScale = (Vector3.one * 0.5f) * scale;
		// 			distance += 1f * scale;
		// 		}
		// 		distance = 0;
		// 		while(distance < bc.width){
		// 			var cylinder = Resources.Load<GameObject>("BonedCube/Cylinder");
		// 			var pos = center;
		// 			pos.y -= (bc.height /2) + (cylinder.transform.localScale.y /2) * scale;
		// 			pos.x += (bc.width /2) + (cylinder.transform.localScale.x /2) * scale;
		// 			pos.z += bc.length/2 + (cylinder.transform.localScale.x /2) * scale;
		// 			pos.x -= distance; 
		// 			cylinder = Instantiate(cylinder, pos, Quaternion.identity, go.transform);
		// 			cylinder.transform.localScale = (Vector3.one * 0.5f) * scale;
		// 			distance += 1f * scale;
		// 		}
		// 	}
		// 	Debug.LogFormat("{0} is off the ground, has pilotis = {1}", id, hasPilotis);
		// }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDrawGizmos() {
		
		foreach (var item in pilotis)
		{
			Gizmos.DrawCube(item, Vector3.one * 0.01f);	
		}
	}
}
