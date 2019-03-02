using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSwitcher : MonoBehaviour
{   
    public LeModule module;
    public float scaleFactor = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!switchEnumerating){
            StartCoroutine(Switch());
        }
    }

    bool switchEnumerating = false;
    IEnumerator Switch(){
        
        // Change colour randomly
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        Renderer renderer;
        foreach(var module in module.gameObject.GetComponentsInChildren<LeModule>()){
            if(module.meshGo){
                renderer = module.meshGo.GetComponent<Renderer>();
                float r = Random.Range(0.0f, 1.0f);
                float g = Random.Range(0.0f, 1.0f);
                float b = Random.Range(0.0f, 1.0f);
                props.SetColor("_Color", new Color(r, g, b));
                if(module.size.x > module.size.z){
                    props.SetVector("_ST", new Vector4(module.size.x * scaleFactor, module.size.y * scaleFactor,1,1));
                }else{
                    props.SetVector("_ST", new Vector4(module.size.z * scaleFactor, module.size.y * scaleFactor,1,1));
                }
                renderer.SetPropertyBlock(props);
            }
        }
        // Get a random axis
        List<string> axis = new List<string>();
        axis.Add(Module.axis.x.ToString());
        axis.Add(Module.axis.y.ToString());
        axis.Add(Module.axis.z.ToString());
        var choice = ExtRandom<string>.WeightedChoice(axis, new int[]{33,33,33});
        LeModule.axis newAxis = LeModule.axis.x;
        switch(choice){
            case "x":
                newAxis = LeModule.axis.x;
            break;
            case "y":
                newAxis = LeModule.axis.y;
            break;
            case "z":
                newAxis = LeModule.axis.z;
            break;					
        }
        module.Divide(newAxis);
        Debug.Log(string.Format("Axis: {0}", newAxis));

        switchEnumerating = true;
        yield return new WaitForSeconds(1f);
        switchEnumerating = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 2.26f);
    }
}
