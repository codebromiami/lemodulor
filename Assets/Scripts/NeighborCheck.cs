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
			if(Input.GetKeyDown(KeyCode.Alpha2)){
				if(tags.Contains("Ground") && !tags.Contains("Roof")){
					var p = gameObject.AddComponent<Piloti>();
					p.module = module;
				}
			}
		}

		if(visible){
			for(int i = 0; i < dirs.Length; i++){
				RaycastHit hit;
				if(i == 0){
					Physics.Raycast(transform.position, dirs[0] * size.x /2, out hit);
					if(hit.collider)
						hitBoys[0] = true;
				}else if(i == 1){
					Physics.Raycast(transform.position, dirs[1] * size.x /2, out hit);
					if(hit.collider){
						hitBoys[1] = true;
						
					}else{
						
					}
				}else if(i == 2){
					Physics.Raycast(transform.position, dirs[2] * size.y /2, out hit);
					if(hit.collider){
						hitBoys[2] = true;
						if(tags.Contains("Roof")){
							tags.Remove("Roof");
						}
					}else{
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
						if(tags.Contains("Ground")){
							tags.Remove("Ground");
						}
					}	
				}else if(i == 4){
					Physics.Raycast(transform.position, dirs[4] * size.z /2, out hit);
					if(hit.collider)
						hitBoys[4] = true;
				}else if(i == 5){
					Physics.Raycast(transform.position, dirs[5] * size.z /2, out hit);
					if(hit.collider)
						hitBoys[5] = true;
				}	
			}
		}
	}

	private void OnDrawGizmos()
	{
		var visible = module.visible;
		var size = module.size;

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
	}

}
