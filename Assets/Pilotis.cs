using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public float pilotiHeight = 0.5f;

	private void Start() {

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
			
			go.transform.position = a + new Vector3(x, 0, 0);
			var localPosition = go.transform.localPosition;
			localPosition.y = pilotiHeight * -1;
			go.transform.localPosition = localPosition;
			go.transform.localScale = new Vector3(pilotiWidth, pilotiHeight, pilotiWidth);
			x += xDivision;
		}

		foreach (GameObject go in bGos)
		{
			go.transform.position = b + new Vector3(0, 0,z);
			var localPosition = go.transform.localPosition;
			localPosition.y = pilotiHeight * -1;
			go.transform.localPosition = localPosition;
			go.transform.localScale = new Vector3(pilotiWidth, pilotiHeight, pilotiWidth);
			z += zDivision;
		}

		z = (zDivision /2);

		foreach (GameObject go in cGos)
		{
			go.transform.position = a + new Vector3(0,0,z);
			var localPosition = go.transform.localPosition;
			localPosition.y = pilotiHeight * -1;
			go.transform.localPosition = localPosition;
			go.transform.localScale = new Vector3(pilotiWidth, pilotiHeight, pilotiWidth);
			z += zDivision;
		}

		x = (xDivision /2);

		foreach (GameObject go in dGos)
		{
			go.transform.position = d + new Vector3(x, 0,0);
			var localPosition = go.transform.localPosition;
			localPosition.y = pilotiHeight * -1;
			go.transform.localPosition = localPosition;
			go.transform.localScale = new Vector3(pilotiWidth, pilotiHeight, pilotiWidth);
			x += xDivision;
		}
	}

	private void OnDrawGizmos() {
		
		if(!Application.isPlaying)
			return;

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(a,b);
		Gizmos.DrawCube(a, Vector3.one * 0.1f);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(b,c);
		Gizmos.DrawCube(b, Vector3.one * 0.1f);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(c,d);
		Gizmos.DrawCube(c, Vector3.one * 0.1f);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(d,a);
		Gizmos.DrawCube(d, Vector3.one * 0.1f);
	}
}
