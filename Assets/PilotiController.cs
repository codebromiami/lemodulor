using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotiController : MonoBehaviour {

	public List<Vector3> points = new List<Vector3>();
	public LeModule leModule;	
	public float divs = 4;
	public List<Line> lines = new List<Line>();
	public float size = 1;
	public List<LePilotiAgent> pilotiAgents = new List<LePilotiAgent>();
	public List<LeModule> pilotiModules = new List<LeModule>();
	public bool init = false;
	
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
		if(init)
			return;
			
		lines = new List<Line>();
		var size = leModule.size;
		var a = new Vector3(-(size.x/2), -leModule.size.y /2, size.z /2);
		var b = new Vector3(-(size.x/2), -leModule.size.y /2, -(size.z /2));
		var newLine = new Line(a,b);
		lines.Add(newLine);
		foreach(Line line in lines){
			line.divs = divs;
			line.Update();
		}
		foreach(var p in newLine.points){
			var crossLine = new Line(p, p + new Vector3(leModule.size.x,0,0));
			lines.Add(crossLine);
			crossLine.divs = divs;
			crossLine.Update();
		}
	}	

	public void Build(){
			//required number of divs needs to be set by this point
            GameObject prefab = Resources.Load<GameObject>("Prefabs/LePilotiAgent");
            foreach(var line in lines){
                points.AddRange(line.points);
            }
            foreach(var p in points){
                var go = Instantiate(prefab,p, Quaternion.identity, transform);
                pilotiAgents.Add(go.GetComponent<LePilotiAgent>());
            }
	}

	private void OnDrawGizmos()
	{
		int count = 0;
		foreach(Line line in lines){
			if(count == 0){
				foreach(var point in line.points){
					Gizmos.DrawRay(this.transform.TransformPoint(point), Vector3.right);
				}	
			}
			count++;
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(this.transform.TransformPoint(line.p1),this.transform.TransformPoint(line.p2));
			Gizmos.color = Color.yellow;
			foreach(var point in line.points){
				Gizmos.DrawCube(this.transform.TransformPoint(point), Vector3.one * 0.01f);
			}
		
		}
	}	
}
