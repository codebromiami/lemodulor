using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class RadiatingLines : MonoBehaviour {

	public Module module;
	public Modulor leModulor;
	public List<Module> modules = new List<Module>();
	public float limit = 50;
	public float distance = 0;
	public float height = 226f;
	public float width = 1.4f;
	public Vector3 start = Vector3.zero;
	public Vector3 end = Vector3.one;

	private void OnEnable()
	{
		Signals.Get<Module.ModuleStart>().AddListener(onModuleStart);
	}

	private void OnDisable()
	{
		Signals.Get<Module.ModuleStart>().RemoveListener(onModuleStart);
	}

	void Start(){
		
		distance = Vector3.Distance(start, end);
		distance = leModulor.GetClosestFromList(leModulor.redSeries, distance);
		module.size.x = distance;
		module.size.y = height;
		module.size.z = width;
		// float angle = Vector3.Angle(start,end);
		// var go = new GameObject();
		// go.transform.SetParent(transform);
		// go.transform.position = end;
		// transform.LookAt(go.transform);
		var vector = transform.position;
		vector.y += height /2;
		transform.position = vector;
		// transform.Rotate(0,angle,0,Space.Self);
	}

	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.Space)){
			StartCoroutine(Refresh());
		}
	}

	public void onModuleStart(Module _module)
	{	
		modules.Add(_module);
		if(modules.Count >= Random.Range(1, limit)){
			return;
		}
		_module.divAxis = Module.axis.x;
		_module.divs = 2;
	}

	public IEnumerator Refresh(){
		module.divs = 0;
		modules = new List<Module>();
		yield return new WaitForSeconds(0.1f);
		var rand = Random.value;
		if(Random.value < 0.5f){
			module.divAxis = Module.axis.x;	
		}else{
			module.divAxis = Module.axis.y;	
		}
		module.divs = 2;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawCube(start, Vector3.one);
		Gizmos.DrawCube(end, Vector3.one);
	}
}
