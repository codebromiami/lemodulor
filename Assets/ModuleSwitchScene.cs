using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class ModuleSwitchScene : MonoBehaviour
{
    
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

        ModuleSwitcher module = hit.collider.GetComponent<ModuleSwitcher>();
        if(module){
            module.RandomDivision();
        }
		
	}

	public void onPointerUp(RaycastHit hit){

	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
