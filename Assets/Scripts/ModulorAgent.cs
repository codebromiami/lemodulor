using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulorAgent : MonoBehaviour {

	public string id;
	public BonedCube bc;
	public Vector3 center = Vector3.zero;
	public bool offGround;
	
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
		if(offGround){
			var piloti = Resources.Load<GameObject>("piloti");
			piloti = Instantiate(piloti, center, Quaternion.identity, this.transform);
			var pilotiScript = piloti.GetComponent<Pilotis>();
			pilotiScript.length = bc.width;
			pilotiScript.width = bc.length;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDrawGizmos() {
		

	}
}
