using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class GroundPlanScene : MonoBehaviour {

	public List<Vector3> points = new List<Vector3>();
	public float distance = 0;
	public int density = 0;
	public Vector3 a = Vector3.zero;
	public Vector3 b = Vector3.zero;
	public Vector3 c = Vector3.zero;
	public Vector3 d = Vector3.zero;
	public Vector3 x = Vector3.zero;
	public Vector3 y = Vector3.zero;
	public Vector3 z = Vector3.zero;
	public bool ziggy = false;
	public GroundPlan gp;
	public int state = 0;
	public GameObject visGo = null;
	public GameObject visGo2 = null;
	public GameObject t;

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

	private void Start()
	{
		
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.R)){
			gp.transform.rotation = visGo.transform.rotation;
		}
	}

	public void onPointerDown(RaycastHit hit){

		if(state == 0){

			a = Vector3.zero;
			b = Vector3.zero;
			c = Vector3.zero;	
			d = Vector3.zero;

			visGo.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
			visGo.transform.position = hit.point;
			a = hit.point;
			
			points = new List<Vector3>();

			if(gp){
				Destroy(gp.gameObject);
			}

			visGo.GetComponent<Renderer>().enabled = true;
			visGo2.GetComponent<Renderer>().enabled = true;

		} else if(state == 1){
			
		}else if(state == 2){

		}
	}

	public void onPointer(RaycastHit hit){

		if(state == 0){

			points.Add(hit.point);
			b = hit.point;
			visGo.transform.position = Vector3.Lerp(a,b,0.5f);
			visGo2.transform.position = Vector3.Lerp(a,b,0.5f);
			Vector3 scale = visGo.transform.localScale;
			scale.z = Vector3.Distance(a,b);
			visGo2.transform.localScale = scale;
			float red = Modulor.GetClosestFromList(Modulor.redSeries, scale.z);
			float blue = Modulor.GetClosestFromList(Modulor.blueSeries, scale.z);
			float closest = Modulor.GetClosest(red,blue, scale.z);
			scale.z = closest;
			visGo.transform.localScale = scale;
			t.transform.position = b;
			visGo.transform.LookAt(t.transform);
			visGo2.transform.LookAt(t.transform);
			
		} else if(state == 1){

			points.Add(hit.point);
			c = hit.point;
			Vector3 scale = visGo.transform.localScale;
			scale.y = Vector3.Distance(Vector3.Lerp(a,b,0.5f),c);
			visGo2.transform.localScale = scale;
			Vector3 pos = visGo2.transform.position;
			pos.y = scale.y /2;
			visGo2.transform.position = pos;
			float red = Modulor.GetClosestFromList(Modulor.redSeries, scale.y);
			float blue = Modulor.GetClosestFromList(Modulor.blueSeries, scale.y);
			float closest = Modulor.GetClosest(red,blue, scale.y);
			scale.y = closest;
			pos = visGo.transform.position;
			pos.y = scale.y /2;
			visGo.transform.position = pos;
			visGo.transform.localScale = scale;

		} else if(state == 2){
			
			points.Add(hit.point);
			d = hit.point;
			Vector3 scale = visGo.transform.localScale;
			scale.x = Vector3.Distance(Vector3.Lerp(Vector3.Lerp(a,b,0.5f),c,0.5f),d);
			visGo2.transform.localScale = scale;
			float red = Modulor.GetClosestFromList(Modulor.redSeries, scale.x);
			float blue = Modulor.GetClosestFromList(Modulor.blueSeries, scale.x);
			float closest = Modulor.GetClosest(red,blue, scale.x);
			scale.x = closest;
			visGo.transform.localScale = scale;
		}
	}

	public void onPointerUp(RaycastHit hit){

		if(state == 0){
			state++;

		} else if(state == 1){
			state++;

		}else if(state == 2){
			
			state++;
			density = points.Count;
			var go = Resources.Load<GameObject>("Prefabs/GroundPlan");
			go = Instantiate(go,visGo.transform.position, Quaternion.identity);
			gp = go.GetComponent<GroundPlan>();
			gp.points = points;
			gp.size = visGo.transform.localScale;
			gp.limit = Random.Range(2,density);
			gp.rays = true;
			visGo.GetComponent<Renderer>().enabled = false;
			visGo2.GetComponent<Renderer>().enabled = false;
			
		}else{
			state = 0;
		}
	}

	public Vector3 GetHalfway(Vector3 a, Vector3 b){
		return (a+b) / 2;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		var scale = Vector3.one * 0.1f;
		foreach(var p in points){
			Gizmos.DrawCube(p,scale);
		}

		Gizmos.color = Color.black;
		Gizmos.DrawLine(a,b);
		Gizmos.DrawLine(b,c);
		Gizmos.DrawLine(c,d);
		Gizmos.DrawLine(d,a);

		Gizmos.color = Color.red;
		Gizmos.DrawCube(a, scale);
		Gizmos.color = Color.green;
		Gizmos.DrawCube(b, scale);
		Gizmos.color = Color.blue;
		Gizmos.DrawCube(c, scale);
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(d, scale);
	}
}
