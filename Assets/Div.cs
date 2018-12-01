using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Div  {

	public Container container;
    [Range(0f,1f)]
	public float scalar;
	public float previous;
	public float next;
	public Vector3 position;
    public int index = 0;

    public bool reflow = false;

	// update must be called outside the class
	public void Update () {

		if(!container)
			return;
        
        if(reflow){
        // if we're not the first or last
        if(index > 0 & index < container.divs.Count -1){
            position = Vector3.Lerp(container.divs[index-1].position, container.divs[index+1].position, scalar);
            previous = Vector3.Distance(container.divs[index-1].position, position);
            next = Vector3.Distance(position, container.divs[index+1].position);
        }else{
            if(index == 0){
                if(container.divs.Count > 1){
                    position = Vector3.Lerp(container.startPoint, container.divs[index+1].position, scalar);
                    previous = Vector3.Distance(container.startPoint, position);
                    next = Vector3.Distance(position, container.divs[index+1].position);
                }else{
                    position = Vector3.Lerp(container.startPoint, container.endPoint, scalar);
                    previous = Vector3.Distance(container.startPoint, position);
                    next = Vector3.Distance(position, container.endPoint);
                }
            }else{
                position = Vector3.Lerp(container.divs[index-1].position, container.endPoint, scalar);
                previous = Vector3.Distance(container.divs[index-1].position, position);
                next = Vector3.Distance(position, container.endPoint);
            }
        }
        }
        
	}
}
