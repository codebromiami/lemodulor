using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class ModuleSwitchScene : MonoBehaviour
{
    public bool subdivide = false;
    public LeModule leModule;
    public PilotiController pilotiController;
    public static ModuleSwitchScene instance;
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
        instance = this;
        LeModular.Init();
    }
    bool move = false;
    // Update is called once per frame
    public List<GameObject> pilotiGos = new List<GameObject>();
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Alpha1)){
            pilotiController.Build();
       }

       if(Input.GetKeyDown(KeyCode.Alpha2)){

            var gos = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach(var go in gos){
                Transform[] ts = go.GetComponentsInChildren<Transform>();
                foreach(Transform t in ts){
                    if(t.gameObject.name.Contains("Ground") & !t.gameObject.name.Contains("Roof")){
                        if(t.gameObject.GetComponent<LeModule>().meshGo.activeInHierarchy)
                            pilotiGos.Add(t.gameObject);
                    }
                }
            }
            foreach(var go in pilotiGos){
                pilotiController.pilotiModules.Add(go.GetComponent<LeModule>());
            }	
       }
       if(Input.GetKeyDown(KeyCode.Alpha3)){
           foreach(LePilotiAgent pilotiAgent in pilotiController.pilotiAgents){
                var colliderA = pilotiAgent.GetComponent<Collider>();
                foreach(var module in pilotiController.pilotiModules){
                    var colliderB = module.meshGo.GetComponent<Collider>();
                    if(colliderA.bounds.Intersects(colliderB.bounds)){
                        pilotiAgent.hit = true;
                        pilotiAgent.tags.Add(colliderB.gameObject.name);
                    }
                }
            }
       }
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
