using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleCheck : MonoBehaviour
{
    public LeModule module;
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
	public bool tooDamnHigh = false;
	public int pilotiGroupId = -1;

	// Use this for initialization
	void Start () {
		
		module = GetComponent<LeModule>();
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
	public void Update () {
		
		var size = module.size;
        if(module.meshGo.activeSelf){

            for(int i = 0; i < dirs.Length; i++){
                RaycastHit hit;
                // Right
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
                // Left
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
                // Up
                }else if(i == 2){
                    Physics.Raycast(transform.position, dirs[2] * size.y /2, out hit);
                    if(hit.collider){
                        hitBoys[2] = true;
                        if(tags.Contains("Roof")){
                            tags.Remove("Roof");
                            gameObject.layer = 0;
                        }
                    }else{
                        hitBoys[2] = false;
                        if(!tags.Contains("Roof")){
                            tags.Add("Roof");
                            gameObject.layer = 9;
                        }
                    }
                // Down
                }else if(i == 3){
                    Physics.Raycast(transform.position, dirs[3] * size.y /2, out hit);
                    if(hit.collider){
                        hitBoys[3] = true;
                        if(tags.Contains("Ground")){
                            tags.Remove("Ground");
                        }
                    }else{
                        hitBoys[3] = false;
                        if(!tags.Contains("Ground")){
                            tags.Add("Ground");
                        }
                    }	
                // Forwards
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
                // Backwards
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
        }else{
            tags.Clear();
        }
        string name = gameObject.name + " ";
        foreach(var tag in tags){
            if(!name.Contains(tag))
                name += tag + " "; 
        }
        gameObject.name = name;
	}

	private void OnDrawGizmos()
	{
        if(tags.Contains("Roof") & tags.Contains("Ground")){
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, module.size);
        }else
		if(tags.Contains("Roof")){
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, module.size);
        }else
        if(tags.Contains("Ground")){
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, module.size);
        }else{
            // Gizmos.color = Color.white;
            // Gizmos.DrawWireCube(transform.position, module.size);
        }
	}
}
