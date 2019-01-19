using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class TestScript : MonoBehaviour {

	public Node node;
	
	private void OnEnable()
	{
		
		Signals.Get<Node.NodeStart>().AddListener(onNodeStart);
	}

	private void OnDisable()
	{
		Signals.Get<Node.NodeStart>().RemoveListener(onNodeStart);
	}

	public void onNodeStart(Node node){
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
