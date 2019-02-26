using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelExerciseScene : MonoBehaviour
{
    public GameObject prefab;
    public GameObject go;
    public int max = 50;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            if(go){
                GameObject.Destroy(go);
            }
            go = Instantiate(prefab);
            PanelExercise panelExercise = go.GetComponent<PanelExercise>();
            panelExercise.limit = Random.Range(2,max);
        }    
    }
}
