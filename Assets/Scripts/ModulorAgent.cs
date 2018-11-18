using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulorAgent : MonoBehaviour {

	float scale = 100;
	public BonedCube bc;
	// Use this for initialization
	void Start () {

		float a = LeModular.GetClosest(bc.length * scale);
		float b = LeModular.GetClosest(bc.width * scale);
		float c = LeModular.GetClosest(bc.height * scale);
		bc.Set(a * 0.1f,b * 0.1f,c * 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
