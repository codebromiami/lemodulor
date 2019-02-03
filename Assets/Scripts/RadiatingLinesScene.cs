using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class RadiatingLinesScene : MonoBehaviour {

	public Module.axis axis;

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

		var m = hit.collider.gameObject.GetComponentInParent<Module>();
		var go = Resources.Load<GameObject>("Prefabs/RadiatingLines");
		var rl = go.GetComponent<RadiatingLines>();
		rl.visibleEnds = true;
		rl.axis = axis;
		rl.size = m.size;
		rl.parentModule = m;
		go = Instantiate(go,m.transform);
	}

	public void onPointer(RaycastHit hit){

		
	}

	public void onPointerUp(RaycastHit hit){
		
	}

	// Use this for initialization
	void Start () {
		
		var go = Resources.Load<GameObject>("Prefabs/Module");
		go = Instantiate(go);
		var m = go.GetComponent<Module>();
		m.divs = 2;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
