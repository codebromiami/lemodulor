using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class PlaceModule : MonoBehaviour {

	public Vector3 a = Vector3.zero;
	public Vector3 b = Vector3.zero;
	public Vector3 c = Vector3.zero;
	public Vector3 d = Vector3.zero;
	public GroundPlan gp;
	public int state = 0;
	public LeModule leModule;
	public GameObject leModuleGO = null;
	public GameObject go = null;
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
		leModuleGO = leModule.go;
		leModuleGO.SetActive(true);
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.R)){
			gp.transform.rotation = leModuleGO.transform.rotation;
		}
	}

	public void onPointerDown(RaycastHit hit){

		if(state == 0){

			a = Vector3.zero;
			b = Vector3.zero;
			c = Vector3.zero;	
			d = Vector3.zero;

			leModuleGO.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
			leModuleGO.transform.position = hit.point;
			a = hit.point;
			

			if(gp){
				Destroy(gp.gameObject);
			}

			leModuleGO.GetComponent<Renderer>().enabled = true;
			go.GetComponent<Renderer>().enabled = true;

		} else if(state == 1){
			
		}else if(state == 2){

		}
	}

	public void onPointer(RaycastHit hit){

		if(state == 0){

			b = hit.point;
			leModuleGO.transform.position = Vector3.Lerp(a,b,0.5f);
			go.transform.position = Vector3.Lerp(a,b,0.5f);
			Vector3 scale = leModuleGO.transform.localScale;
			scale.z = Vector3.Distance(a,b);
			go.transform.localScale = scale;
			float red = Modulor.GetClosestFromList(Modulor.redSeries, scale.z);
			float blue = Modulor.GetClosestFromList(Modulor.blueSeries, scale.z);
			float closest = Modulor.GetClosest(red,blue, scale.z);
			scale.z = closest;
			leModuleGO.transform.localScale = scale;
			t.transform.position = b;
			leModuleGO.transform.LookAt(t.transform);
			go.transform.LookAt(t.transform);
			
		} else if(state == 1){

			c = hit.point;
			Vector3 scale = leModuleGO.transform.localScale;
			scale.y = Vector3.Distance(Vector3.Lerp(a,b,0.5f),c);
			go.transform.localScale = scale;
			Vector3 pos = go.transform.position;
			pos.y = scale.y /2;
			go.transform.position = pos;
			float red = Modulor.GetClosestFromList(Modulor.redSeries, scale.y);
			float blue = Modulor.GetClosestFromList(Modulor.blueSeries, scale.y);
			float closest = Modulor.GetClosest(red,blue, scale.y);
			scale.y = closest;
			pos = leModuleGO.transform.position;
			pos.y = scale.y /2;
			leModuleGO.transform.position = pos;
			leModuleGO.transform.localScale = scale;

		} else if(state == 2){
			
			d = hit.point;
			Vector3 scale = leModuleGO.transform.localScale;
			scale.x = Vector3.Distance(Vector3.Lerp(Vector3.Lerp(a,b,0.5f),c,0.5f),d);
			go.transform.localScale = scale;
			float red = Modulor.GetClosestFromList(Modulor.redSeries, scale.x);
			float blue = Modulor.GetClosestFromList(Modulor.blueSeries, scale.x);
			float closest = Modulor.GetClosest(red,blue, scale.x);
			scale.x = closest;
			leModuleGO.transform.localScale = scale;
		}
	}

	public void onPointerUp(RaycastHit hit){

		if(state == 0){
			state++;

		} else if(state == 1){
			state++;

		}else if(state == 2){
			
			state++;
			
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
