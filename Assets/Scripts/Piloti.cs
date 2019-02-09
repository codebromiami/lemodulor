using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piloti : MonoBehaviour {

	public List<Vector3> points = new List<Vector3>();
	public Module module;	
	public float divs = 1;
	public List<Line> lines = new List<Line>();

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
		lines = new List<Line>();
		for(int i = 0; i < points.Count; i++){
			if(i == 0){
				lines.Add(new Line(points[i], points[i +1]));
			}else if(i == points.Count - 1){
				lines.Add(new Line(points[i], points[0]));
			}else{
				lines.Add(new Line(points[i], points[i +1]));
			}
			
		}
		foreach(Line line in lines){
			line.divs = divs;
			line.Update();
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
			Gizmos.DrawLine(line.p1,line.p2);
			Gizmos.color = Color.red;
			foreach(var point in line.points){
				Gizmos.DrawCube(this.transform.TransformPoint(point), Vector3.one);
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
