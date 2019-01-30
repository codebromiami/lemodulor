using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class GroundPlanScene : MonoBehaviour {

	public List<Vector3> points = new List<Vector3>();
	public float distance = 0;
	public int density = 0;
	private void OnEnable()
	{
		Signals.Get<Pointer.OnPointerDown>().AddListener(onPointerDown);
		Signals.Get<Pointer.OnPointer>().AddListener(onPointer);
		Signals.Get<Pointer.OnPointerUp>().AddListener(onPointerUp);
	}

	private void OnDisable()
	{
		Signals.Get<Pointer.OnPointerDown>().RemoveListener(onPointerDown);
		Signals.Get<Pointer.OnPointer>().RemoveListener(onPointer);	
		Signals.Get<Pointer.OnPointerUp>().RemoveListener(onPointerUp);
	}

	public void onPointerDown(RaycastHit hit){

		points = new List<Vector3>();
		a = Vector3.zero;
		b = Vector3.zero;
		c = Vector3.zero;	
		ziggy = false;
	}

	public void onPointer(RaycastHit hit){

		points.Add(hit.point);
		// Debug.DrawRay(hit.point, Vector3.up, Color.blue);
		// Debug.Log("Did Hit");
		// if(hit.collider.tag == "Ground"){
		// 	var go = Resources.Load<GameObject>("Prefabs/Module");
		// 	go = Instantiate(go);
		// 	go.transform.position = hit.point;
		// 	go.transform.localScale = Vector3.one * 0.1f;
		// }
	}

	public Vector3 a = Vector3.zero;
	public Vector3 b = Vector3.zero;
	public Vector3 c = Vector3.zero;
	public Vector3 d = Vector3.zero;
	public bool ziggy = false;

	public void onPointerUp(RaycastHit hit){

		a = points[0];
		b = points[1];
		foreach(var p in points){
			if(Vector3.Distance(a, p) > Vector3.Distance(a,b)){
				b = p;
			}else{
			}
		}
		foreach(var p in points){
			if(Vector3.Distance(b, p) > Vector3.Distance(a,b)){
				ziggy = true;
				c = p;
			}
		}
		if(ziggy){
			distance = Vector3.Distance(b,c);
			d = GetHalfway(b,c);
		}else{
			distance = Vector3.Distance(a,b);
			d = GetHalfway(a,b);
		}
		density = points.Count;
		var go = Resources.Load<GameObject>("Prefabs/Module");
		go = Instantiate(go,d,Quaternion.identity);
	}

	public Vector3 GetHalfway(Vector3 a, Vector3 b){
		return (a+b) / 2;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		var scale = Vector3.one * 0.1f;
		foreach(var p in points){
			Gizmos.DrawCube(p,scale);
		}
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(a,b);
		Gizmos.DrawCube(a, scale);
		Gizmos.DrawCube(b, scale);
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(c, scale);
		Gizmos.DrawLine(b,c);
		Gizmos.color = Color.magenta;
		Gizmos.DrawCube(d, scale);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
