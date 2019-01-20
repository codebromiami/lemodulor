using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BoundingPoints {

	public BoundingPoints(Vector3 center, Vector3 size){

		// Add these clockwise so we can reason about them easier
		// Below a and b are the changing components { (-a,-b),(-a,+b),(+a,+b),(+a,-b)} }
		yPositive.Add(center + new Vector3(-size.x/2, size.y/2, -size.z/2));
		yPositive.Add(center + new Vector3(-size.x/2, size.y/2, size.z/2));
		yPositive.Add(center + new Vector3(size.x/2, size.y/2, size.z/2));
		yPositive.Add(center + new Vector3(size.x/2, size.y/2, -size.z/2));

		yNegative.Add(center + new Vector3(-size.x/2, -size.y/2, -size.z/2));		
		yNegative.Add(center + new Vector3(-size.x/2, -size.y/2, size.z/2));
		yNegative.Add(center + new Vector3(size.x/2, -size.y/2, size.z/2));
		yNegative.Add(center + new Vector3(size.x/2, -size.y/2, -size.z/2));

		xPositive.Add(center + new Vector3(size.x/2, -size.y/2, -size.z/2));
		xPositive.Add(center + new Vector3(size.x/2, size.y/2, -size.z/2));
		xPositive.Add(center + new Vector3(size.x/2, size.y/2, size.z/2));
		xPositive.Add(center + new Vector3(size.x/2, -size.y/2, size.z/2));

		xNegative.Add(center + new Vector3(-size.x/2, -size.y/2, -size.z/2));
		xNegative.Add(center + new Vector3(-size.x/2, size.y/2, -size.z/2));
		xNegative.Add(center + new Vector3(-size.x/2, size.y/2, size.z/2));
		xNegative.Add(center + new Vector3(-size.x/2, -size.y/2, size.z/2));

		zPositive.Add(center + new Vector3(-size.x/2, -size.y/2, size.z/2));
		zPositive.Add(center + new Vector3(-size.x/2, size.y/2, size.z/2));
		zPositive.Add(center + new Vector3(size.x/2, size.y/2, size.z/2));
		zPositive.Add(center + new Vector3(size.x/2, -size.y/2, size.z/2));

		zNegative.Add(center + new Vector3(-size.x/2, -size.y/2, -size.z/2));
		zNegative.Add(center + new Vector3(-size.x/2, size.y/2, -size.z/2));
		zNegative.Add(center + new Vector3(size.x/2, size.y/2, -size.z/2));
		zNegative.Add(center + new Vector3(size.x/2, -size.y/2, -size.z/2));
		
		List<Vector3> tmpVectors = new List<Vector3>();
		
		tmpVectors.AddRange(xPositive);
		tmpVectors.AddRange(xNegative);
		tmpVectors.AddRange(zPositive);
		tmpVectors.AddRange(zNegative);
		
		foreach (Vector3 vector in tmpVectors)
		{
			if(!all.Contains(vector)){
				all.Add(vector);
			}	
		}
		// Create edges between the top and bottom plane
		Edge edge = new Edge(yPositive[0], yNegative[0]);
		edges.Add(edge);
		edge = new Edge(yPositive[1], yNegative[1]);
		edges.Add(edge);
		edge = new Edge(yPositive[2], yNegative[2]);
		edges.Add(edge);
		edge = new Edge(yPositive[3], yNegative[3]);
		edges.Add(edge);
		//Create edges for the top and bottom planes
		for (int i = 0; i < yPositive.Count; i++)
		{
			if(i < yPositive.Count - 1){
				edge = new Edge(yPositive[i], yPositive[i+1]);
				edges.Add(edge);
			}else{
				edge = new Edge(yPositive[i], yPositive[0]);
				edges.Add(edge);
			}
		}
		for (int i = 0; i < yNegative.Count; i++)
		{
			if(i < yNegative.Count - 1){
				edge = new Edge(yNegative[i], yNegative[i+1]);
				edges.Add(edge);
			}else{
				edge = new Edge(yNegative[i], yNegative[0]);
				edges.Add(edge);
			}
		}
	}

	public List<Vector3> all = new List<Vector3>();
	
	public List<Vector3> yPositive = new List<Vector3>();
	public List<Vector3> yNegative = new List<Vector3>();
	public List<Vector3> xPositive = new List<Vector3>();
	public List<Vector3> xNegative = new List<Vector3>();
	public List<Vector3> zPositive = new List<Vector3>();
	public List<Vector3> zNegative = new List<Vector3>();
	public Dictionary<Vector3, List<Edge>> edgeDict = new Dictionary<Vector3, List<Edge>>();
	public List<Edge> edges = new List<Edge>();	
}

public class Level{
	
	public static List<float> levels = new List<float>();
	float yComponent;
	public List<Vector3> points = new List<Vector3>();
	public List<Vertex> vertexPoints = new List<Vertex>();
	public List<Vertex> convexVertexes = new List<Vertex>();
	public List<Vector3> GetPointsFromVertexes(){
		
		List<Vector3> tmpList = new List<Vector3>();
		foreach (Vertex vertex in convexVertexes)
		{
			tmpList.Add(vertex.position);
		}
		return tmpList;
	}
	public Level(Vector3 point){
		yComponent = point.y;
		levels.Add(yComponent);
		points.Add(point);
	}

}

public class ModulorAgent : MonoBehaviour {

	public string id;
	public BonedCube bc;
	public Vector3 center = Vector3.zero;
	public bool offGround;
	public float height;
	public List<float> storeys = new List<float>();
	public BoxCollider boxCollider;
	public Rigidbody rb;
	public Bounds bounds;
	public SkinnedMeshRenderer skinnedMesh;
	[SerializeField] public BoundingPoints boundingPoints;
	public List<ModulorAgent> modulorAgents = new List<ModulorAgent>();
	public bool showPoints = false;
	public Dictionary<ModulorAgent, List<Vector3>> encapsulatedPoints = new Dictionary<ModulorAgent, List<Vector3>>();
	public bool showEncapsulatedPoints = false;
	public bool showEdges = false;
	public List<Vector3> intersectionPoints = new List<Vector3>();
	public List<Edge> intersectionEdges =new List<Edge>();
	public static List<Vector3> all = new List<Vector3>();

	public bool init = false;
	public bool triggerStay = false;
	// Use this for initialization
	IEnumerator Start () {

		this.gameObject.name = id;
		
		showPoints = true;
		showEncapsulatedPoints = true;
		showEdges = true;

		// Set the size of the volume to the closest measurement contained in the red series
		float length = LeModular.GetClosest(bc.length * 100) / 100;
		float width = LeModular.GetClosest(bc.width * 100) / 100;
		float height = LeModular.GetClosest(bc.height * 100) / 100;
		bc.Set(length,width,height);
		
		// Find all the bounding points
		skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
		skinnedMesh.updateWhenOffscreen = true;
		bounds = skinnedMesh.bounds;
		center = bounds.center;
		boundingPoints = new BoundingPoints(center, new Vector3(width, height, length));
		// Add box collider and rigidbody to generate collisions
		boxCollider = bc.gameObject.AddComponent<BoxCollider>();
		boxCollider.enabled = true;
		boxCollider.isTrigger = true;
		boxCollider.center = center;
		boxCollider.size = new Vector3(bc.width, bc.height, bc.length);
		rb = bc.gameObject.AddComponent<Rigidbody>();
		rb.isKinematic = true;
		yield return new WaitUntil(()=> triggerStay);
		CubeGenerator.Instance.ready++;
		// Debug.Log(string.Format("Modulor Agent {0} Agent List Count{1}",id, modulorAgents.Count));
		// Iterate through our bounding points to see if any of them are within the bounds of another agent
		CubeGenerator.Instance.agentDict[this].AddRange(boundingPoints.all);
		
 		foreach(Vector3 point in boundingPoints.all){
			foreach (ModulorAgent agent in modulorAgents)
			{
				if(agent.bounds.Contains(point)){
					if(encapsulatedPoints.ContainsKey(agent)){
						encapsulatedPoints[agent].Add(point);
					}else{
						List<Vector3> points = new List<Vector3>();
						points.Add(point);
						encapsulatedPoints.Add(agent, points);
					}
				}else{
					//
				}
			}
		}
		

		foreach(var entry in this.encapsulatedPoints){	
			foreach(var edge in boundingPoints.edges){
				if(!entry.Value.Contains(edge.v1.position) & !entry.Value.Contains(edge.v2.position)){
					continue;
				}
				var dir = edge.v2.position - edge.v1.position;
				dir.Normalize();
				Ray ray = new Ray(edge.v1.position,dir);
				float distance = 0;
				
				if(entry.Key.bounds.IntersectRay(ray, out distance)){
					Vector3 point = ray.origin + ray.direction * distance;
					intersectionPoints.Add(point);
					intersectionEdges.Add(new Edge(ray.origin, point));
				}
			}
		}
		
		foreach (Vector3 vector in boundingPoints.all)
		{
			Vector3 worldPoint = this.transform.TransformPoint(vector);
			if(!CubeGenerator.Instance.points.Contains(worldPoint)){
				CubeGenerator.Instance.points.Add(worldPoint);
			}
		}

		foreach (Vector3 vector in intersectionPoints)
		{
			Vector3 worldPoint = this.transform.TransformPoint(vector);
			if(!CubeGenerator.Instance.points.Contains(worldPoint)){
				CubeGenerator.Instance.points.Add(worldPoint);
			}
		}

		// bc.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;

		// TODO: refactor what's below
		var refactor = false;
		if(refactor){
		// Instantiate piloti if the volume is off the ground
		if(offGround){
			var piloti = Resources.Load<GameObject>("piloti");
			var pos = center;
			pos.y -= bc.height/2;
			piloti = Instantiate(piloti, pos, Quaternion.identity, this.transform);
			var pilotiScript = piloti.GetComponent<Pilotis>();
			// get the shortest side
			pilotiScript.length = bc.width;
			pilotiScript.width = bc.length;
			var a = pilotiScript.length > pilotiScript.width ? pilotiScript.width : pilotiScript.length;	// get the shortet side
			a *= 0.1f;
			a = Mathf.Clamp(a, 0.1f, 0.75f);
			pilotiScript.pilotiWidth = a;
			pilotiScript.width -= a;
			pilotiScript.length -= a;
			var ground = bc.a.position;
			ground.y = 0;
			height = Vector3.Distance(bc.a.position, ground);
		}

		List<float> floors = LeModular.Divisions(bc.height);
		var floorBC = BonedCube.Make();
		floorBC.transform.SetParent(this.transform);
		floorBC.a.transform.position = this.bc.a.transform.position;
		floorBC.Set(bc.length, bc.width, 0.1f);
		var _floors = BuildFloors(floors);
		if(_floors != null){
			foreach(var floor in _floors){
				floorBC = BonedCube.Make();
				floorBC.transform.SetParent(this.transform);
				floorBC.a.transform.position = this.bc.a.transform.position + new Vector3(0, floor, 0);
				floorBC.Set(bc.length, bc.width, 0.1f);
			}
		}
		
		}
		
	}

	List<float> BuildFloors(List<float> floors){
		// get random number that is the =< the size of the list
		int a = Random.Range(0, floors.Count);
		int b = Random.Range(floors.Count - a, floors.Count);
		Debug.Log(string.Format("{0} a {1} b {2}", id, a, b));
		if(a + b > floors.Count){
			Debug.Log(string.Format("! a {0} b {1}", a, b));
			return BuildFloors(floors);
		}
		return floors.GetRange(a,b);;
	}
	private void OnTriggerEnter(Collider other)
	{
		var go = other.gameObject;
		var modulorAgent = go.GetComponent<ModulorAgent>();
		if(modulorAgent){
			this.modulorAgents.Add(modulorAgent);
		}else{
			Debug.LogWarning(string.Format("No ModulorAgent script attached to gameObject {0} that collided with gameObject {1}.", go.name, this.id));
		}
		// Debug.Log(string.Format("id {0} on trigger enter {0}", id, other.gameObject.name));

	}

	private void OnTriggerStay(Collider other)
	{
		// Debug.Log(string.Format("id {0} on trigger stay {0}", id, other.gameObject.name));
		triggerStay = true;
	}

	

	
	private void OnDrawGizmos() {
		
		Vector3 size = Vector3.one * 0.0025f;

		Gizmos.color = Color.red;
		if(showPoints){
			foreach(var point in boundingPoints.all){
				Gizmos.DrawCube(this.transform.TransformPoint(point), size);
			}
		}
		Gizmos.color = Color.yellow;
		if(showEncapsulatedPoints){
			foreach(List<Vector3> value in encapsulatedPoints.Values){
				foreach(Vector3 point in value){
					Gizmos.DrawCube(this.transform.TransformPoint(point), size);
				}	
			}
		}
		Gizmos.color = Color.blue;
		if(showEdges){
			foreach(Edge edge in boundingPoints.edges){
				Gizmos.DrawLine(this.transform.TransformPoint(edge.v1.position), this.transform.TransformPoint(edge.v2.position));
			}
		}
		Gizmos.color = Color.magenta;
		foreach(Vector3 point in intersectionPoints){
			Gizmos.DrawCube(this.transform.TransformPoint(point), size);
		}	
		Gizmos.color = Color.blue;
		foreach(Edge edge in intersectionEdges){
			Gizmos.DrawLine(this.transform.TransformPoint(edge.v1.position), this.transform.TransformPoint(edge.v2.position));
		}
		Gizmos.color = Color.white;
		foreach(Vector3 point in all){
			Gizmos.DrawCube(point, size);
		}	

		

		
	}
}
