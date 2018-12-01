using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class FloatHelper
{
	public static float Truncate(this float value, int digits)
	{
		string thing = value.ToString();
		if(digits > thing.Length)
			digits = thing.Length;
		thing = thing.Substring(0,digits);
		float result = 0;
		if(float.TryParse(thing, out result)){
			// Debug.Log(result);
		}else{
			Debug.Log("Fail");
		}
		return result;
	}
}
	
public class Pilotis : MonoBehaviour {

	public MeshFilter meshFilter;
	
	public Vector3 a;
	public Vector3 b;
	public Vector3 c;
	public Vector3 d;

	public float xDist;
	public float zDist;

	public int divisor = 1;
	
	public float xDivision;
	public float xRemainder;
	public float zDivision;
	public float zRemainder;

	public List<GameObject> aGos = new List<GameObject>();
	public List<GameObject> bGos = new List<GameObject>();
	public List<GameObject> cGos = new List<GameObject>();
	public List<GameObject> dGos = new List<GameObject>();

	public float length = 1;
	public float width = 1;

	public float pilotiWidth = 0.5f;
	// format a string a to centain number of digits, then parse the string to find a float value, hilarious
	public static float Round(float value, int digits) {
		string a = "{0:";
		for(int i = 0; i < digits; i++){
			a += "0";
		}
		a += "}";
		string b = string.Format(a, value);
		float c = float.Parse(b);
		Debug.Log(value + " " + b + " " + c);
		return c;
	}

	public void Create(float length, float width){

	}

	private void Update() {


		meshFilter.transform.localScale = new Vector3(length, width,1);
		
		if(aGos.Count < divisor){
			var go = (GameObject)Resources.Load<GameObject>("Cylinder");
			go = Instantiate(go,this.transform);
			aGos.Add(go);
		}

		if(bGos.Count < divisor){
			var go = (GameObject)Resources.Load<GameObject>("Cylinder");
			go = Instantiate(go,this.transform);
			bGos.Add(go);
		}

		if(cGos.Count < divisor){
			var go = (GameObject)Resources.Load<GameObject>("Cylinder");
			go = Instantiate(go,this.transform);
			cGos.Add(go);
		}

		if(dGos.Count < divisor){
			var go = (GameObject)Resources.Load<GameObject>("Cylinder");
			go = Instantiate(go,this.transform);
			dGos.Add(go);
		}
		// if the length is less than the piloti width
		// if the width is less than the piloti width

		// Get the four corners of the plane
		a = meshFilter.transform.TransformPoint(meshFilter.mesh.vertices[0]);
		b = meshFilter.transform.TransformPoint(meshFilter.mesh.vertices[2]);
		c = meshFilter.transform.TransformPoint(meshFilter.mesh.vertices[1]);
		d = meshFilter.transform.TransformPoint(meshFilter.mesh.vertices[3]);

		// x is width and z is length
		xDist = Vector3.Distance(a, b);
		zDist = Vector3.Distance(a, d);

        xDivision = xDist / divisor;
        xRemainder = xDist % divisor;

		zDivision = zDist / divisor;
        zRemainder = zDist % divisor;
		
		float x = (xDivision /2);
		float z = (zDivision /2);

		foreach (GameObject go in aGos)
		{	
			var pilotiScript = go.GetComponent<PilotiAgent>();
			if(!pilotiScript.set){
				pilotiScript.Build(this, a + new Vector3(x, 0, 0));
			}
			x += xDivision;
		}

		foreach (GameObject go in bGos)
		{
			var pilotiScript = go.GetComponent<PilotiAgent>();
			if(!pilotiScript.set){
				pilotiScript.Build(this, b + new Vector3(0, 0,z));
			}
			z += zDivision;
		}

		z = (zDivision /2);

		foreach (GameObject go in cGos)
		{
			var pilotiScript = go.GetComponent<PilotiAgent>();
			if(!pilotiScript.set){
				pilotiScript.Build(this, a + new Vector3(0,0,z));
			}
			z += zDivision;
		}

		x = (xDivision /2);

		foreach (GameObject go in dGos)
		{
			var pilotiScript = go.GetComponent<PilotiAgent>();
			if(!pilotiScript.set){
				pilotiScript.Build(this, d + new Vector3(x, 0,0));
			}
			x += xDivision;
		}
	}

	private void OnDrawGizmos() {
		
		if(!Application.isPlaying)
			return;
		float scale = 0.01f;
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(a,b);
		Gizmos.DrawCube(a, Vector3.one * scale);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(b,c);
		Gizmos.DrawCube(b, Vector3.one * scale);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(c,d);
		Gizmos.DrawCube(c, Vector3.one * scale);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(d,a);
		Gizmos.DrawCube(d, Vector3.one * scale);
	}
}
