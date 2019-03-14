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
        

	}

	public void onPointer(RaycastHit hit){

        if(move){
            ModuleMove moveModule = hit.collider.GetComponent<ModuleMove>();
            if(moveModule != null){
                moveModule.active = true;
            }
        }else{
            ModuleSwitcher switcher = hit.collider.GetComponent<ModuleSwitcher>();
            if(switcher){

                if(subdivide){
                    LeModule.axis newAxis = switcher.RandomAxis();
                    
                    switcher.childModule.Subdivide(newAxis);
                    bool active = switcher.GetComponent<ModuleMove>().active;
                    foreach(var child in switcher.childModule.children){
                        Renderer renderer = child.meshGo.GetComponent<Renderer>();
                        
                        var childSwitch = child.gameObject.AddComponent<ModuleSwitcher>();
                        childSwitch.RandomColorFromList(renderer, LeModular.colours);
                        child.gameObject.AddComponent<ModuleCollider>();
                        child.gameObject.AddComponent<ModuleCheck>();
                        ModuleMove moduleMove = child.gameObject.AddComponent<ModuleMove>();
                        moduleMove.active = active;

                    }
                }else{
                    switcher.childModule.UnDivide();
                }
            }

        }
        
        
	}

	public void onPointerUp(RaycastHit hit){

	}

    // Start is called before the first frame update
    void Start()
    {
        LeModular.Init();
    }
    bool move = false;
    // Update is called once per frame
    void Update()
    {
       
    }

    public void SetSubdivide(){
        subdivide = true;
        move = false;

    }

    public void SetUndevide(){
        subdivide = false;
        move = false;
    }

    public void SetMove(){
        move = true;
    }
}
