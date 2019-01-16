using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modulor : MonoBehaviour {

	public List<float> redSeries = new List<float>(){
		0.6f,
		0.9f,
		1.5f,
		2.4f,
		3.9f,
		6.3f,
		10.2f,
		16.5f,
		26.7f,
		43.2f,
		69.8f,
		113.0f,
		182.9f,
		295.9f,
		478.8f,
		774.7f,
		1253.5f,
		2028.2f,
		3981.6f,
		5309.8f,
		8591.4f,
		13901.3f,
		22492.7f,
		36394.0f,
		58886.7f,
		95280.7f
	};
	public List<float> blueSeries = new List<float>(){
		1.1f,
		1.8f,
		3.0f,
		4.8f,
		7.8f,
		12.6f,
		20.4f,
		33.0f,
		53.4f,
		86.3f,
		139.7f,
		226.0f,
		365.8f,
		591.8f,
		957.6f,
		1549.4f,
		2506.9f,
		4056.3f,
		6563.3f,
		10619.6f,
		17182.9f,
		27802.5f,
		44985.5f,
		72788.0f,
		117773.5f
	};
	public enum series {blue, red};
	public series seriesChoice = series.blue;
	public float distance;
	public int index;
	public float indexValue;
	public float closest;
	public List<float> divs;

	private List<float> seriesList;
	private void Update()
	{
		
		seriesList = seriesChoice == series.red ? redSeries : blueSeries; 
		divs = new List<float>();
		indexValue = 0;
		for(int i = 0; i < seriesList.Count; i++){
			indexValue = seriesList[i];
			if(indexValue >= distance){
				index = seriesList.IndexOf(indexValue);
				break;
			}
		}
		if(index > 0){
			float a = seriesList[index -1];
			float b = seriesList[index];
			float c = a + indexValue / 2;
			closest = Mathf.Abs(distance - a) < Mathf.Abs(distance - b) ? a : b;
		}
	}

	private void OnGUI()
	{
		if(GUILayout.Button("Calculate")){
			
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
