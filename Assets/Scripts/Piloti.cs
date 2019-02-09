using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piloti : MonoBehaviour {

	public List<Vector3> points = new List<Vector3>();
	public List<GameObject> gos = new List<GameObject>();
	public float distance = 0;
	public bool loop = false;
	public Module module;
	public NeighborCheck neighborCheck;
	public float width = 1;
	// public Mesh convexHull;


	// Use this for initialization
	void Start () {

		
		module = gameObject.GetComponent<Module>();
		neighborCheck = gameObject.GetComponent<NeighborCheck>();
		
		if(neighborCheck & module){
			var renderer = module.meshGo.GetComponent<MeshRenderer>();
			renderer.material.color = Color.red;
			var center = this.transform.position;
			var size = module.size;
			// foreach(var p in yNegative){
			// 	var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			// 	go.transform.position = p;
			// 	go.transform.SetParent(transform);
			// }
		}else{
			module = gameObject.GetComponentInParent<Module>();
			neighborCheck = gameObject.GetComponentInParent<NeighborCheck>();
		}
		// module.visible = false;
		// var piloti = Resources.Load<GameObject>("Prefabs/Piloti");
		// var pos = transform.position;
		// var size = module.size;
		// pos.y = size.y;
		// piloti = Instantiate(piloti, pos, Quaternion.identity, transform);
		// var pilotiScript = piloti.GetComponent<Pilotis>();
		// // get the shortest side
		// pilotiScript.length = size.x - size.x * 0.1f;
		// pilotiScript.width = size.z - size.z * 0.1f;
		// var a = pilotiScript.length > pilotiScript.width ? pilotiScript.width : pilotiScript.length;	// get the shortet side
		// a *= 0.1f;
		// a = Mathf.Clamp(a, 0.1f, 0.75f);
		// pilotiScript.pilotiWidth = a;
		// pilotiScript.width -= a;
		// pilotiScript.length -= a;
		// var scale = pilotiScript.transform.localScale;
		// scale.y = size.y;
		// CreateMesh();

		
	}

	// Update is called once per frame
	void Update () {

		// module.visible = false;
		distance = 0;		
		if(points != null & points.Count > 0){
			if(gos != null){
				if(gos.Count < points.Count){
					while(gos.Count < points.Count){
						var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
						go.transform.SetParent(transform);
						gos.Add(go);
						if(GroundPlan.instance)
							go.GetComponent<MeshRenderer>().material.color = GroundPlan.instance.colors[GroundPlan.instance.colorIndex];
					}
				}else if(gos.Count > points.Count){
					while(gos.Count > points.Count){
						var go = gos[gos.Count-1];
						GameObject.Destroy(go);
						gos.RemoveAt(gos.Count-1);
					}
				}
				int index = 0;
				foreach (var p in points)
				{	
					if(index < points.Count -1)
						distance += Vector3.Distance(points[index], points[index +1]);
					gos[index].transform.localPosition = p;
					index++;
				}

				if(loop)
					distance += Vector3.Distance(points[0], points[index +1]);
				index = 0;
				float divisor = distance / points.Count;
				foreach(var go in gos){
					float t = Mathf.InverseLerp(0,distance, divisor * index);
					if(index == 0){
						Vector3 newPoint = points[0];
						go.transform.position = newPoint;
					}else if(index == points.Count -1){
						Vector3 newPoint = points[points.Count -1];
					}else{
						Vector3 newPoint = Vector3.Lerp(points[index],points[index+1],t);
						go.transform.position = newPoint;
					}
					index++;
				}
			}
		}
		if(module){
			var size = module.size;
			if(size.x > size.z){
				width = size.z * 0.1f;
			}else{
				width = size.x * 0.1f;
			}
			foreach(var go in gos){
				// var scale = go.transform.localScale;
				// scale.x = width;
				// scale.y = size.y /2;
				// scale.z = width;
				// go.transform.localScale = scale;
				// var pos = go.transform.localPosition;
				// pos.y = (size.y /2);
			}
		}
	}

	// public void CreateMesh(){

		
	// 	var vertexList = new List<Vertex>();
	// 	foreach(Vector3 point in points){
	// 		vertexList.Add(new Vertex(point));
	// 	}
	// 	var vl = JarvisMarchAlgorithm.GetConvexHull(vertexList);
		
	// 	List<Vector3> newVertices = new List<Vector3>();
	// 	foreach(var vertex in vertexList){
	// 		newVertices.Add(vertex.position);
	// 	}
	// 	Vector2[] newUV = new Vector2[0];
	// 	int[] newTriangles = new int[newVertices.Count * 3];
	// 	for(int i = 0; i < newVertices.Count; i++){
	// 		newTriangles[i] = i;
	// 		newTriangles[i + 1] = i +1;
	// 		newTriangles[i +2] = i + 2;
	// 	}
	// 	Mesh mesh = new Mesh();
	// 	gameObject.AddComponent<MeshRenderer>();
    //     mesh.vertices = newVertices.ToArray();
    //     mesh.uv = newUV;
    //     mesh.triangles = newTriangles;
	// 	mesh.RecalculateBounds();
	// 	Debug.Log(mesh.bounds.size);
	// 	convexHull = mesh;
	// 	gameObject.AddComponent<MeshFilter>().mesh = convexHull;
	// }

	

	private void OnDestroy()
	{
		foreach(var go in gos){
			GameObject.Destroy(go);
		}	
	}
		
}
