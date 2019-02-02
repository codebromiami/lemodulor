using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piloti : MonoBehaviour {

	public List<Vector3> points = new List<Vector3>();
	public List<GameObject> gos = new List<GameObject>();
	public float distance = 0;
	public bool loop = false;
	public Module module;
	public NeighborCheck neighborCheck;

	// Use this for initialization
	void Start () {

		module = gameObject.GetComponent<Module>();
		neighborCheck = gameObject.GetComponent<NeighborCheck>();
	}

	private void OnDrawGizmos()
	{
		// foreach(var itme in boundingPoints.yPositive){
		// 	Gizmos.DrawCube(transform.TransformPoint(itme), Vector3.one);
		// }
		// Gizmos.DrawCube(transform.position, Vector3.one);
	}
	
	// Update is called once per frame
	void Update () {

		module.visible = false;

		if(Input.GetKeyDown(KeyCode.Alpha3)){
			foreach(var point in neighborCheck.boundingPoints.yPositive){
				points.Add(transform.TransformPoint(point));
			}
		}
		distance = 0;
		
		if(points != null & points.Count > 0){
			if(gos != null){
				if(gos.Count < points.Count){
					while(gos.Count < points.Count){
						gos.Add(GameObject.CreatePrimitive(PrimitiveType.Cylinder));
					}
				}else if(gos.Count > points.Count){
					while(gos.Count > points.Count){
						var go = gos[gos.Count-1];
						GameObject.Destroy(go);
						gos.RemoveAt(gos.Count-1);
					}
				}
				int index = 0;
				foreach (var p in points)
				{	
					if(index < points.Count -1)
						distance += Vector3.Distance(points[index], points[index +1]);
					gos[index].transform.position = p;
					index++;
				}
				if(loop)
					distance += Vector3.Distance(points[0], points[index +1]);
				index = 0;
				float divisor = distance / points.Count;
				// foreach(var go in gos){
				// 	float t = Mathf.InverseLerp(0,distance, divisor * index);
				// 	if(index == 0){

				// 	}else if(index == points.Count -1){

				// 	}else{
				// 		Vector3 newPoint = Vector3.Lerp(points[index],points[index+1],t);
				// 		go.transform.position = newPoint;
				// 	}
				// 	index++;
				// }
			}
		}
	}

	private void OnDestroy()
	{
		foreach(var go in gos){
			GameObject.Destroy(go);
		}	
	}
		
}
