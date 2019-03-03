using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class ModuleSwitchScene : MonoBehaviour
{
    public bool subdivide = false;
	private void OnEnable()
	{
		Signals.Get<Pointer.OnPointerDown>().AddListener(onPointerDown);
		Signals.Get<Pointer.OnPointer>().AddListener(onPointer);
		Signals.Get<Pointer.OnPointerUp>().AddListener(onPointerUp);
	}

	private void OnDisable()
	{
		Signals.Get<Pointer.OnPointerDown>().RemoveListener(onPointerDown);
		Signals.Get<Pointer.OnPointer>().RemoveListener(onPointer);	
		Signals.Get<Pointer.OnPointerUp>().RemoveListener(onPointerUp);
	}

    public void onPointerDown(RaycastHit hit){
        ModuleSwitcher switcher = hit.collider.GetComponent<ModuleSwitcher>();
        if(switcher){

            if(subdivide){
                LeModule.axis newAxis = switcher.RandomAxis();
                
                switcher.childModule.Subdivide(newAxis);
                
                foreach(var child in switcher.childModule.children){
                    Renderer renderer = child.meshGo.GetComponent<Renderer>();
                    
                    var childSwitch = child.gameObject.AddComponent<ModuleSwitcher>();
                    childSwitch.RandomColorFromList(renderer, LeModular.colours);
                    child.gameObject.AddComponent<ModuleCollider>();
                }
            }else{
                switcher.childModule.UnDivide();
            }
        }

	}

	public void onPointer(RaycastHit hit){

        
		
	}

	public void onPointerUp(RaycastHit hit){

	}

    // Start is called before the first frame update
    void Start()
    {
        LeModular.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)){
            subdivide = false;
        }else{
            subdivide = true;
        }
    }
}
