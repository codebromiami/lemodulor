using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class BonedCube : MonoBehaviour {

	public float length = 1;
	public float width = 1;
	public float height = 1;
	public Transform a;
	public Transform b;
	public bool update = false;

	public static BonedCube Make(){
		var prefab = Resources.Load("BonedCube/boned-cube");
		GameObject go = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity, null);
		return go.GetComponent<BonedCube>();
	}

	public void Set(float length, float width, float height){
		this.length = length;
		this.height = height;
		this.width = width;
		a.transform.localScale = new Vector3(1, this.width, this.height);
        b.transform.localScale = new Vector3(1, this.width, this.height);
		b.transform.position = a.transform.position + new Vector3(0,0,length);
	}

	private void Update() {
		
		if(!update)
			return;

		Set(this.length,this.width,this.height);
	}

	public void Stop(){
		Destroy(this.gameObject);
	}
}
