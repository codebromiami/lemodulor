using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using deVoid.Utils;

public class EdgeDetection : MonoBehaviour
{
    public LeModule leModule;
    public GameObject[] edgeGos;
    public Vector3[] directions = new Vector3[]{
        new Vector3(1,1,0),
        new Vector3(-1,1,0),
        new Vector3(1,-1,0),
        new Vector3(-1,-1,0),
        new Vector3(1,0,1),
        new Vector3(-1,0,1),
        new Vector3(1,0,-1),
        new Vector3(-1,0,-1),
        new Vector3(0,-1,-1),
        new Vector3(0,1,-1),
        new Vector3(0,1,-1),
        new Vector3(0,-1,1),
        new Vector3(0,1,1)
    };

    private void OnEnable()
    {
        Signals.Get<Pointer.OnPointer>().AddListener(onPointer);
    }

    private void OnDisable()
    {
        Signals.Get<Pointer.OnPointer>().RemoveListener(onPointer);
    }
    RaycastHit previousHit;
    public void onPointer(RaycastHit hitInfo){
        
        for(int i = 0; i < edgeGos.Length; i++){
            if(hitInfo.collider.gameObject.GetInstanceID() == edgeGos[i].GetInstanceID()){
                Vector3 delta = hitInfo.point - previousHit.point;
                leModule.transform.position += delta;
                previousHit = hitInfo;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        edgeGos = new GameObject[transform.childCount];
        for(int i = 0; i < edgeGos.Length; i++){
            edgeGos[i] = transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
    }
}
