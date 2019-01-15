using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class Ground : MonoBehaviour {

	public GameObject go;
	private void OnEnable()
	{
		Signals.Get<Pointer.OnPointer>().AddListener(onPointer);
	}

	private void OnDisable()
	{
		Signals.Get<Pointer.OnPointer>().RemoveListener(onPointer);	
	}

	public void onPointer(Vector3 pos){

		Debug.DrawRay(pos, Vector3.up, Color.blue);
		Debug.Log("Did Hit");
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
