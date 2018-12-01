using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class VolumeAgent : MonoBehaviour {

	public float startToDiv;
	public float divToEnd;
	Vector3 xPoint;
	Vector3 startPoint = Vector3.zero;
	Vector3 endPoint = new Vector3(1,0,0);
	[Range(0,1)]
	public float x;
	public int panels = 1;
	public List<float> widths = new List<float>();

	// Update is called once per frame
	void Update () {
		
		xPoint = new Vector3(x,0,0);
		startToDiv = Vector3.Distance(startPoint, xPoint);
		divToEnd = Vector3.Distance(xPoint, endPoint);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawLine(startPoint, endPoint);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(startPoint, xPoint);
	}
}
