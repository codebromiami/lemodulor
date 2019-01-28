using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class RadiatingLinesScene : MonoBehaviour {

	public List<Vector3> points = new List<Vector3>();

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

	public void onPointerDown(Vector3 pos){
		points = new List<Vector3>();
	}

	public void onPointer(Vector3 pos){

		points.Add(pos);
		Debug.DrawRay(pos, Vector3.up, Color.blue);
		Debug.Log("Did Hit");
	}

	public void onPointerUp(Vector3 pos){
		
		var go = Resources.Load<GameObject>("Prefabs/RadiatingLines");
		go = Instantiate(go);
		var rl = go.GetComponent<RadiatingLines>();
		rl.transform.transform.position = points[0];
		rl.start = points[0];
		rl.end = points[points.Count-1];
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
