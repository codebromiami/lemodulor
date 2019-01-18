using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGraph : MonoBehaviour {

	public Node parentNode;

	private void Update()
	{
		if(parentNode != null)
			parentNode.Update();	
	}
}
