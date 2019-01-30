using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modulor : MonoBehaviour {

	[System.Serializable]
	public class KeyPair{
		public KeyPair(float size, float fit){
			this.size = size;
			this.count = fit / size;
			this.remainder = fit % size;
		}
		public float size;
		public float count;
		public float remainder;
	}
	public List<float> redSeries = new List<float>(){
		0.006f,
		0.009f,
		0.015f,
		0.024f,
		0.039f,
		0.063f,
		0.102f,
		0.165f,
		0.267f,
		0.432f,
		0.698f,
		1.130f,
		1.829f,
		2.959f,
		4.788f,
		7.747f,
		12.535f,
		20.282f,
		39.816f,
		53.098f,
		85.914f,
		139.013f,
		224.927f,
		363.940f,
		588.867f,
		952.807f
	};
	public List<float> blueSeries = new List<float>(){
		0.011f,
		0.018f,
		0.030f,
		0.048f,
		0.078f,
		0.126f,
		0.204f,
		0.330f,
		0.534f,
		0.863f,
		1.397f,
		2.260f,
		3.658f,
		5.918f,
		9.576f,
		15.494f,
		25.069f,
		40.563f,
		65.633f,
		106.196f,
		171.829f,
		278.025f,
		449.855f,
		727.880f,
		1177.735f
	};

	public enum series {blue, red};
	public series seriesChoice = series.blue;
	public float distance;
	public float closest;
	public float remainder;
	public float ab;
	public float a;
	public float b;
	public int count;
	public List<KeyPair> divs = new List<KeyPair>();

	private List<float> seriesList;
	
	private void Update() {	
		
		seriesList = seriesChoice == series.red ? redSeries : blueSeries; 
		closest = GetClosestFromList(seriesList, distance);
		remainder = distance - closest;
		// within closest how many combinations can we find
		divs = GetPossibilities(seriesList, closest);
		a = ab / 1.6180339887f;
		b = a /  1.6180339887f;

	}

	public static Vector2 GetTwo(float length){
		Vector2 goldenDivs = new Vector2();
		goldenDivs.x = length / 1.6180339887f;
		goldenDivs.y = goldenDivs.x /  1.6180339887f;
		return goldenDivs;
	}

	public static List<float> GetList(float length, int count){
		List<float> goldenDivs = new List<float>();
		goldenDivs.Add(length / 1.6180339887f);
		for(int i = 0; i < count; i++){
			goldenDivs.Add(goldenDivs[goldenDivs.Count-1] / 1.6180339887f);
		}
		return goldenDivs;
	}

	public List<KeyPair> GetPossibilities(List<float> list, float value){

		List<KeyPair> values = new List<KeyPair>();
		for(int i = 0; i < list.Count; i++){
			if(list[i] <= value){
				values.Add(new KeyPair(list[i], closest));
			}else{
				break;
			}
		}
		return values;
	}

	public float GetClosestFromList(List<float> list, float distance){

		int index = 0;
		float value = 0;
		for(int i = 0; i < list.Count; i++){
			value = list[i];
			if(value >= distance){
				index = list.IndexOf(value);
				break;
			}
		}
		float closest = 0;
		if(index > 0){
			float a = list[index -1];
			float b = list[index];
			closest = GetClosest(a,b, distance);
			if(closest > distance){
				closest = a;
			}
		}
		return closest;
	}

	public float GetHalfway(float a, float b){
		return a + b / 2;
	}

	public float GetClosest(float a, float b, float c){
		 return Mathf.Abs(c - a) < Mathf.Abs(c - b) ? a : b;
	}



	private void OnGUI()
	{
		if(GUILayout.Button("Calculate")){
			foreach(var i in GetList(distance, count)){
				Debug.Log(i);
			}
		}
	}
	
	// private static void SearchAndInsert(List<string> list, 
    //     string insert, DinoComparer dc)
    // {
    //     Console.WriteLine("\nBinarySearch and Insert \"{0}\":", insert);
    //     int index = list.BinarySearch(insert, dc);
    //     if (index < 0)
    //     {
    //         list.Insert(~index, insert);
    //     }
    // }
}
