using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborCheck : MonoBehaviour {
	
	public Module module;
	public BoundingPoints boundingPoints;	
	Vector3[] dirs = new Vector3[]{
			new Vector3(1,0,0),
			new Vector3(-1,0,0),
			new Vector3(0,1,0),
			new Vector3(0,-1,0),
			new Vector3(0,0,1),
			new Vector3(0,0,-1),
			};
	public bool[] hitBoys = new bool[]{
		false,
		false,
		false,
		false,
		false,
		false
	};
	public List<string> tags = new List<string>();
	public List<string> groudIds = new List<string>();
	public List<Vertex> vertexGroups = new List<Vertex>();
	// Use this for initialization
	void Start () {
		
		module = GetComponent<Module>();
		if(module.meshGo){
			var filter = module.meshGo.GetComponent<MeshFilter>();
			if(filter){
				var bounds = filter.sharedMesh.bounds;
				var center = bounds.center;
				var size = module.size;
				boundingPoints = new BoundingPoints(center, new Vector3(size.x, size.y, size.z));
				// Debug.LogFormat("x{0} y{1} z{2}",size.x, size.y, size.z);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		var visible = module.visible;
		var size = module.size;

		var id = "Module";
		if(visible){
			foreach(var str in tags){
				id += " " + str;
			}
		}
		gameObject.name = id;

		if(visible){
			for(int i = 0; i < dirs.Length; i++){
				RaycastHit hit;
				if(i == 0){
					Physics.Raycast(transform.position, dirs[0] * size.x /2, out hit);
					if(hit.collider){
						hitBoys[0] = true;
						if(this.gameObject.name.Contains("Piloti")){
							if(hit.collider.transform.parent.gameObject.name.Contains("Piloti")){
								if(!groudIds.Contains(hit.collider.transform.parent.gameObject.name)){
									groudIds.Add(hit.collider.transform.parent.gameObject.name);
								}
							}else{
								groudIds.Clear();
							}
						}
					}else{
						hitBoys[0] = false;
					}
				}else if(i == 1){
					Physics.Raycast(transform.position, dirs[1] * size.x /2, out hit);
					if(hit.collider){
						hitBoys[1] = true;
						if(this.gameObject.name.Contains("Piloti")){
							if(hit.collider.transform.parent.gameObject.name.Contains("Piloti")){
								if(!groudIds.Contains(hit.collider.transform.parent.gameObject.name)){
									groudIds.Add(hit.collider.transform.parent.gameObject.name);
								}
							}else{
								groudIds.Clear();
							}
						}
					}else{
						hitBoys[1] = false;
					}
				}else if(i == 2){
					Physics.Raycast(transform.position, dirs[2] * size.y /2, out hit);
					if(hit.collider){
						hitBoys[2] = true;
						if(tags.Contains("Roof")){
							tags.Remove("Roof");
						}
					}else{
						hitBoys[2] = false;
						if(!tags.Contains("Roof")){
							tags.Add("Roof");
						}
					}
				}else if(i == 3){
					Physics.Raycast(transform.position, dirs[3] * size.y /2, out hit);
					if(hit.collider){
						hitBoys[3] = true;
						if(hit.collider.gameObject.name == "Ground"){
							if(!tags.Contains("Ground")){
								tags.Add("Ground");
							}
						}else{
							if(tags.Contains("Ground")){
								tags.Remove("Ground");
							}	
						}
					}else{
						hitBoys[3] = false;
						if(tags.Contains("Ground")){
							tags.Remove("Ground");
						}
					}	
				}else if(i == 4){
					Physics.Raycast(transform.position, dirs[4] * size.z /2, out hit);
					if(hit.collider){
						hitBoys[4] = true;
						if(this.gameObject.name.Contains("Piloti")){
							if(hit.collider.transform.parent.gameObject.name.Contains("Piloti")){
								if(!groudIds.Contains(hit.collider.transform.parent.gameObject.name)){
									groudIds.Add(hit.collider.transform.parent.gameObject.name);
								}
							}else{
								groudIds.Clear();
							}
						}
					}else{
						hitBoys[4] = false;
						
					}
				}else if(i == 5){
					Physics.Raycast(transform.position, dirs[5] * size.z /2, out hit);
					if(hit.collider){
						hitBoys[5] = true;
						if(this.gameObject.name.Contains("Piloti")){
							if(hit.collider.transform.parent.gameObject.name.Contains("Piloti")){
								if(!groudIds.Contains(hit.collider.transform.parent.gameObject.name)){
									groudIds.Add(hit.collider.transform.parent.gameObject.name);
								}
							}else{
								groudIds.Clear();
							}
						}
					}else{
						hitBoys[5] = false;
						
					}
				}	
			}
		}
	}

	private void OnDrawGizmos()
	{
		var visible = module.visible;
		var size = module.size;

		// draw neighbor checking ray casts
		if(visible){
			foreach(var dir in dirs){
				RaycastHit hit;
				if(dir == dirs[0] | dir == dirs[1]){
					Gizmos.color = Color.red;
					Gizmos.DrawRay(transform.position,dir * size.x/2);
				}else if(dir == dirs[2] | dir == dirs[3]){
					Gizmos.color = Color.green;
					Gizmos.DrawRay(transform.position,dir * size.y/2);
				}else{
					Gizmos.color = Color.blue;
					Gizmos.DrawRay(transform.position,dir * size.z/2);
				}
			}
		}
		// draw bounding points
		foreach(var p in boundingPoints.all){
			Gizmos.DrawCube(transform.TransformPoint(p), Vector3.one * 0.1f);
		}
	}

}
