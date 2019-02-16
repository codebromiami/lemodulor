using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piloti : MonoBehaviour {

	public List<Vector3> points = new List<Vector3>();
	public Module module;	
	public float divs = 4;
	public List<Line> lines = new List<Line>();
	public float size = 1;
	public List<PilotiAgent> pilotiAgents = new List<PilotiAgent>();
	public List<Module> pilotiModules = new List<Module>();
	public bool init = false;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
		if(init)
			return;
			
		lines = new List<Line>();
		var size = module.size;
		var a = new Vector3(-(size.x/2), 0, size.z /2);
		var b = new Vector3(-(size.x/2), 0, -(size.z /2));
		var newLine = new Line(a,b);
		lines.Add(newLine);
		foreach(Line line in lines){
			line.divs = divs;
			line.Update();
		}
		foreach(var p in newLine.points){
			var crossLine = new Line(p, p + new Vector3(module.size.x,0,0));
			lines.Add(crossLine);
			crossLine.divs = divs;
			crossLine.Update();
		}
	}	

	public void Build(){

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
[System.Serializable]
public class Line {
	
	public Vector3 p1;
	public Vector3 p2;
	public float divs;
	public Vector3[] points;
	public float[] floats;
	public Line(Vector3 p1, Vector3 p2){
		
		this.p1 = p1;
		this.p2 = p2;

	}

	public void Update(){

		points = new Vector3[Mathf.RoundToInt(divs)];
		floats = new float[Mathf.RoundToInt(divs)];
		float divisor = divs -1f;
		floats = new float[Mathf.RoundToInt(divs)];
		for(int i = 0; i < Mathf.RoundToInt(divs); i++){
			if(i == 0){
				floats[i] = 0;
			}else if(i == Mathf.RoundToInt(divs) - 1){
				floats[i] = 1;
			}else{
				float t = i;
				float v = t / divisor;
				floats[i] = v;
				
			}
		}

		for(int i = 0; i < floats.Length; i++){
			points[i] = Vector3.Lerp(p1,p2,floats[i]);
		}
	}
}
