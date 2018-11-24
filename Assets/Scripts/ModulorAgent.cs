using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulorAgent : MonoBehaviour {

	public string id;
	public BonedCube bc;
	public Vector3 center = Vector3.zero;
	public bool offGround;
	public float height;
	// Use this for initialization
	void Start () {

		this.gameObject.name = id;

		// Set the size of the volume to the closest measurement contained in the red series
		float length = LeModular.GetClosest(bc.length);
		float width = LeModular.GetClosest(bc.width);
		float height = LeModular.GetClosest(bc.height);
		bc.Set(length,width,height);
		
		// get the center of the volume
		GetComponentInChildren<SkinnedMeshRenderer>().updateWhenOffscreen = true;
		center = GetComponentInChildren<SkinnedMeshRenderer>().bounds.center;
		
		// Instantiate piloti if the volume is off the ground
		if(offGround){
			var piloti = Resources.Load<GameObject>("piloti");
			var pos = center;
			pos.y -= bc.height/2;
			piloti = Instantiate(piloti, pos, Quaternion.identity, this.transform);
			var pilotiScript = piloti.GetComponent<Pilotis>();
			pilotiScript.length = bc.width;
			pilotiScript.width = bc.length;
			var ground = bc.a.position;
			ground.y = 0;
			height = Vector3.Distance(bc.a.position, ground);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDrawGizmos() {
		

	}
}
