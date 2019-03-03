using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleCollider : MonoBehaviour
{
    LeModule childModule;
    public BoxCollider collider;
    // Start is called before the first frame update
    void Start()
    {
        childModule = gameObject.GetComponent<LeModule>();
        collider = gameObject.AddComponent<BoxCollider>();
        collider.size = childModule.size;
    }

    // Update is called once per frame
    void Update()
    {
        collider.size = childModule.size;
        if(childModule.children != null && childModule.children.Count > 0)
            collider.enabled = childModule.meshGo.activeSelf;
    }
}
