using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Container : MonoBehaviour {

	public Vector3 startPoint;
	public Vector3 endPoint;
	public Transform a;
	public Transform b;
	[ContextMenuItem("AddDiv", "AddDiv")]
	public string adddiv;
	[ContextMenuItem("ClearDivs", "ClearDivs")]
	public string cleardivs;
	public List<Div> divs = new List<Div>();
	
	// Update is called once per frame
	void Update () {

		startPoint = a.position;
		endPoint = b.position;
		
		foreach(var div in divs){
			div.Update();
		}
	}
    
	public void AddDiv(){
		
		var div = new Div();
		div.index = divs.Count;	
		div.container = this;
		divs.Add(div);
	}

	public void ClearDivs(){
		divs = null;
		divs = new List<Div>();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawLine(startPoint, endPoint);	
		foreach(var div in divs){
			Gizmos.color = Color.yellow;
			Gizmos.DrawCube(div.position, Vector3.one * 0.025f);
		}
	}
}
