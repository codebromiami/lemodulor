using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class Ground : MonoBehaviour {

	public List<Vector3> points = new List<Vector3>();

	private void OnEnable()
	{
		Signals.Get<Pointer.OnPointer>().AddListener(onPointer);
		Signals.Get<Pointer.OnPointerUp>().AddListener(onPointerUp);
	}

	private void OnDisable()
	{
		Signals.Get<Pointer.OnPointer>().RemoveListener(onPointer);	
		Signals.Get<Pointer.OnPointerUp>().RemoveListener(onPointerUp);
	}

	public void onPointer(Vector3 pos){

		points.Add(pos);
		Debug.DrawRay(pos, Vector3.up, Color.blue);
		Debug.Log("Did Hit");
	}

	public void onPointerUp(Vector3 pos){
		var go = new GameObject();
		go.transform.position = pos;
		var script = go.AddComponent<Piloti>();
		script.points = points;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
