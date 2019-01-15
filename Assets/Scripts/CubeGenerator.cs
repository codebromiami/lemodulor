using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour {

	public static CubeGenerator Instance;
	public float minXZ = 1f; 	// Their min width or height
	public float maxXZ = 10f; // Their max width or height
	public float maxHeight = 5;
	public float Radius = 10f; // How far from zero their a transform can be placed
	public int num = 10;// The maximum number of cubes to be placed
	public List<BonedCube> cubes = new List<BonedCube>();	// A list to retrieve the cubes from
	public List<string> alphabet = new List<string>(){"a","b","c","d","e","f","h","i","j","k","l","m","n","o","p","q","r","s","t","v","w","x","y","z"};
	public int ready = 0;
	public Dictionary<ModulorAgent, List<Vector3>> agentDict = new Dictionary<ModulorAgent, List<Vector3>>();
	public List<Vector3> points = new List<Vector3>();
	public Dictionary<float, Level> levels = new Dictionary<float,Level>();
	public List<Level> indexLevels = new List<Level>();
	public MeshFilter meshfilter;
	public int counter = 0;
	public float scale = 0.01f;
	public List<GameObject> lrGos = new List<GameObject>();
	public static Vector3 RandomVector(float min, float max){
		return new Vector3(Random.Range(min,max),Random.Range(min,max), Random.Range(min,max));
	}


	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.UpArrow)){
			counter++;
			if(counter > indexLevels.Count -1){
				counter = 0;
			}
		}
	}

	public IEnumerator Start(){
		
		Instance = this;

		num = Random.Range(1, num);	
		while(cubes.Count < num){
			var bc = BonedCube.Make();	
			bc.transform.SetParent(this.transform);	// The cubes are children of this transform so they can be placed on a ground plane in AR
			bc.a.transform.position = this.transform.position + new Vector3(LeModular.GetClosest(Random.Range(minXZ, maxXZ)),LeModular.GetClosest(Random.Range((maxHeight * -1), maxHeight)),LeModular.GetClosest(Random.Range(minXZ, maxXZ)));
			bc.Set(Random.Range(minXZ, maxXZ),Random.Range(minXZ, maxXZ),Random.Range(minXZ, maxXZ));
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
			agentDict.Add(modulorAgent, new List<Vector3>());
			// transform the world position of this transform to the local space of bc
			if(bc.a.position.y < this.transform.position.y){
				// if i'm 10 meters tall and located at -1 then the visible section of me is going to be 4
				// if 10 meters tall and locate at +1 then the visible section of me is going to be 6
				float val = Random.Range(0f,1f);
				Color col = val > 0.5f ? Color.white : Color.black;
				col.a = 0.5f;
				bc.GetComponentInChildren<Renderer>().material.color = col;
				bc.a.position = new Vector3(bc.a.transform.position.x, this.transform.position.y, bc.a.transform.position.z);
				bc.b.position = new Vector3(bc.b.transform.position.x, this.transform.position.y, bc.b.transform.position.z);
				modulorAgent.offGround = false;
			} else {
				float val = Random.Range(0f,1f);
				Color col = val > 0.5f ? Color.blue : Color.red;
				col.a = 0.5f;
				bc.GetComponentInChildren<Renderer>().material.color = col;
				modulorAgent.offGround = true;
			}
			count++;
		}
		yield return new WaitUntil(()=> ready == cubes.Count);
		yield return new WaitForSeconds(0.5f);
		Dictionary<float, Level> tmpLevels = new Dictionary<float,Level>();
		List<float> heights = new List<float>();
		foreach (Vector3 point in CubeGenerator.Instance.points)
		{
			if(!tmpLevels.ContainsKey(point.y)){
				tmpLevels.Add(point.y,new Level(point));
				tmpLevels[point.y].vertexPoints.Add(new Vertex(point));
			}else{
				tmpLevels[point.y].points.Add(point);
				tmpLevels[point.y].vertexPoints.Add(new Vertex(point));
			}	
		}
		heights.AddRange(tmpLevels.Keys);
		heights.Sort();
		foreach(var f in heights){
			Debug.Log(string.Format("Level {0}", f));
		}
		var tmpHeights = new List<float>();
		for(int i = 0; i < heights.Count; i++){
			if(i < heights.Count -1 && heights[i] != -1.1f){
				var dif = heights[i +1] - heights[i];
				if(dif > 0.6){
					if(tmpLevels.ContainsKey(heights[i])){
						levels.Add(heights[i],tmpLevels[heights[i]]);
					}
				}else{
					if(tmpLevels.ContainsKey(heights[i])){
						tmpLevels[heights[i]].vertexPoints.AddRange(tmpLevels[heights[i +1]].vertexPoints);
						levels.Add(heights[i],tmpLevels[heights[i]]);
						heights[i+1] = 1.1f;
					}
				}
			}
		}
		indexLevels.AddRange(levels.Values);
		foreach(var entry in levels){
			entry.Value.convexVertexes = JarvisMarchAlgorithm.GetConvexHull(entry.Value.vertexPoints);
			Debug.Log(string.Format("Merged Levels {0}", entry.Key));
		}
		foreach (var item in indexLevels)
		{
			if(item.convexVertexes == null)
				continue;
			
			var go = new GameObject();
			lrGos.Add(go);
			go.transform.SetParent(this.transform);
			LineRenderer lr = go.AddComponent<LineRenderer>();
			lr.material = new Material(Shader.Find("Sprites/Default"));
			lr.positionCount = item.GetPointsFromVertexes().Count;
			lr.SetPositions(item.GetPointsFromVertexes().ToArray());
			lr.loop = true;
			lr.startWidth = 0.1f;
			lr.endWidth = 0.1f;
		}
		// this.transform.localScale = Vector3.one * scale;
		// Debug.Log(string.Format("Ready {0}", cubes.Count));
	}

	public void Reset(){
		foreach(var bc in cubes){
			if(bc == null)
				continue;
			bc.Stop();
		}
		cubes.Clear();
		var lrs = FindObjectsOfType<LineRenderer>();
		for(int i = 0; i < lrGos.Count; i++){
			var go = lrGos[i];
			lrGos = null;
			GameObject.Destroy(go);
		}
		lrGos.Clear();	
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		foreach (var item in indexLevels)
		{
			if(item.convexVertexes == null)
				continue;
			Gizmos.color = Color.red;
			for (int i = 0; i < item.convexVertexes.Count; i++)
			{	
				if(i < item.convexVertexes.Count -1){
					var a = item.convexVertexes[i].position * scale;
					var b = item.convexVertexes[i +1].position * scale;
					Gizmos.DrawLine(a,b);
				}else{
					var a = item.convexVertexes[0].position * scale;
					var b = item.convexVertexes[i].position * scale;
					Gizmos.DrawLine(a,b);
				}
			}	
		}
		Gizmos.color = Color.green;
		if(indexLevels.Count > 0){

			if(indexLevels[counter].convexVertexes != null){
				for (int i = 0; i < indexLevels[counter].convexVertexes.Count; i++)
				{	
					if(i < indexLevels[counter].convexVertexes.Count -1){
						var a = indexLevels[counter].convexVertexes[i].position * scale;
						var b = indexLevels[counter].convexVertexes[i +1].position * scale;
						Gizmos.DrawLine(a,b);
					}else{
						var a = indexLevels[counter].convexVertexes[0].position * scale;
						var b = indexLevels[counter].convexVertexes[i].position * scale;
						Gizmos.DrawLine(a,b);
					}
				}
			}
		}
	}
}
