using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit;

public class TestProceduralToolkit : MonoBehaviour {

	public Material material;
	// Use this for initialization
	void Start () {
		var cube = MeshDraft.Cube(1, true);
		var mesh = cube.ToMesh(true, true);
		GameObject go = new GameObject();
		var mf = go.AddComponent<MeshFilter>();
		var mr = go.AddComponent<MeshRenderer>();
		mf.mesh = mesh;
		mr.material = material;

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
