using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour {

	public float Min = 1f; 	// Their min width or height
	public float Max = 10f; // Their max width or height
	public float Radius = 10f; // How far from zero their a transform can be placed
	public int num = 10;// The maximum number of cubes to be placed
	public List<BonedCube> cubes = new List<BonedCube>();	// A list to retrieve the cubes from
	public List<string> alphabet = new List<string>(){"a","b","c","d","e","f","h","i","j","k","l","m","n","o","p","q","r","s","t","v","w","x","y","z"};
	
	public static Vector3 RandomVector(float min, float max){
		return new Vector3(Random.Range(min,max),Random.Range(min,max), Random.Range(min,max));
	}

	public IEnumerator Start(){
				
		num = Random.Range(1, num);	
		while(cubes.Count < num){
			var bc = BonedCube.Make();	
			bc.transform.SetParent(this.transform);	// The cubes are children of this transform so they can be placed on a ground plane in AR
			bc.a.transform.position = this.transform.position + RandomVector(Radius * -1, Radius);
			bc.Set(Random.Range(Min, Max),Random.Range(Min, Max),Random.Range(Min, Max));
			cubes.Add(bc);
		}

		int count = 0;
		foreach(var bc in cubes){
			if(bc == null)
				continue;
			
			// Add ModulorAgent script that will handle it's building behaviour
			var modulorAgent = bc.gameObject.AddComponent<ModulorAgent>();
			modulorAgent.bc = bc;
			modulorAgent.id = alphabet[count];

			// transform the world position of this transform to the local space of bc
			if(bc.a.position.y < this.transform.position.y){
				// if i'm 10 meters tall and located at -1 then the visible section of me is going to be 4
				// if 10 meters tall and locate at +1 then the visible section of me is going to be 6
				float val = Random.Range(0f,1f);
				Color col = val > 0.5f ? Color.white : Color.black;
				bc.GetComponentInChildren<Renderer>().material.color = col;
				bc.a.position = new Vector3(bc.a.transform.position.x, this.transform.position.y, bc.a.transform.position.z);
				bc.b.position = new Vector3(bc.b.transform.position.x, this.transform.position.y, bc.b.transform.position.z);
				modulorAgent.offGround = false;
			}else{
				float val = Random.Range(0f,1f);
				Color col = val > 0.5f ? Color.blue : Color.red;
				bc.GetComponentInChildren<Renderer>().material.color = col;
				modulorAgent.offGround = true;
			}

			count++;
		}
		yield return new WaitForSecondsRealtime(0.1f);
		this.transform.localScale = new Vector3(0.01f,0.01f,0.01f);
	}

	public void Reset(){
		foreach(var bc in cubes){
			if(bc == null)
				continue;
			bc.Stop();
		}
		cubes.Clear();
	}
}
