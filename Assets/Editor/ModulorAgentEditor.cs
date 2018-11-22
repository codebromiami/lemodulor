using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ModulorAgent))]
public class ModulorAgentEditor : Editor {

	private void OnSceneGUI(){
		
		ModulorAgent modulorAgent = (ModulorAgent)target;
		if(!modulorAgent)
			return;
		Handles.color = Color.blue;
		Handles.Label(modulorAgent.bc.a.position, modulorAgent.id);            
	}
}
