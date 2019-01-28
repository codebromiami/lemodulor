using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class Pointer : MonoBehaviour {

	public class OnPointerDown : ASignal<RaycastHit>{}
	public class OnPointer : ASignal<RaycastHit>{}
	public class OnPointerUp : ASignal<RaycastHit>{}

	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButtonDown(0)){
			// Bit shift the index of the layer (8) to get a bit mask
			int layerMask = 1 << 8;

			// This would cast rays only against colliders in layer 8.
			// But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
			layerMask = ~layerMask;

			RaycastHit hit;
			// Does the ray intersect any objects excluding the player layer
			var mousePos = Input.mousePosition;
			var cam = Camera.main;
			Ray ray = cam.ScreenPointToRay(mousePos);
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
			{
				Signals.Get<OnPointerDown>().Dispatch(hit);
				Debug.DrawLine(ray.origin, hit.point, Color.blue);
				// Debug.Log("Did Hit");
			}
			else
			{
				// Debug.DrawRay(ray.origin,ray.direction * 100 , Color.red);
				// Debug.Log("Did not Hit");
			}
		} else if(Input.GetMouseButton(0)){
			// Bit shift the index of the layer (8) to get a bit mask
			int layerMask = 1 << 8;

			// This would cast rays only against colliders in layer 8.
			// But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
			layerMask = ~layerMask;

			RaycastHit hit;
			// Does the ray intersect any objects excluding the player layer
			var mousePos = Input.mousePosition;
			var cam = Camera.main;
			Ray ray = cam.ScreenPointToRay(mousePos);
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
			{
				Signals.Get<OnPointer>().Dispatch(hit);
				
				Debug.DrawLine(ray.origin, hit.point, Color.blue);
				// Debug.Log("Did Hit");
			}
			else
			{
				// Debug.DrawRay(ray.origin,ray.direction * 100 , Color.red);
				// Debug.Log("Did not Hit");
			}
		}else if(Input.GetMouseButtonUp(0)){
			// Bit shift the index of the layer (8) to get a bit mask
			int layerMask = 1 << 8;

			// This would cast rays only against colliders in layer 8.
			// But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
			layerMask = ~layerMask;

			RaycastHit hit;
			// Does the ray intersect any objects excluding the player layer
			var mousePos = Input.mousePosition;
			var cam = Camera.main;
			Ray ray = cam.ScreenPointToRay(mousePos);
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
			{
				Signals.Get<OnPointerUp>().Dispatch(hit);
				Debug.DrawLine(ray.origin, hit.point, Color.blue);
				// Debug.Log("Did Hit");
			}
			else
			{
				// Debug.DrawRay(ray.origin,ray.direction * 100 , Color.red);
				// Debug.Log("Did not Hit");
			}
		}
	}
}
