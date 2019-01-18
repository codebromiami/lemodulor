using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Node  {

	public int divs = 0;
	public List<Node> childNodes = new List<Node>();
	
	public Node(){

	}

	public void Update()
	{
		if(divs > 0){
			if(divs > childNodes.Count){
				while(divs > childNodes.Count){
					childNodes.Add(new Node());
				}
			}else if(divs < childNodes.Count){
				while(divs < childNodes.Count){
					childNodes.RemoveAt(childNodes.Count -1);
				}
			}			
		}else{
			childNodes = null;
		}
	}
}

public class SizeNode : Node {

}
