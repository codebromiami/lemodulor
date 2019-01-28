using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOutline : MonoBehaviour {

	public LineRenderer lineRenderer;
	public Mesh mesh;
	public List<Vector3> points = new List<Vector3>();

	// Use this for initialization
	void Start () {
		mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
		points.AddRange(mesh.vertices);
		lineRenderer.positionCount = mesh.vertices.Length -1;
		lineRenderer.SetPositions(mesh.vertices);
		// GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDrawGizmos()
	{
		// for(int i = 0; i < points.Count; i++){
		// 	if(i > 0 | i == points.Count -1)
		// 		Gizmos.DrawLine(transform.TransformPoint(points[i]), transform.TransformPoint(points[i -1]));
		// }
	}
}
