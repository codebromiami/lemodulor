using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class ModuleMove : MonoBehaviour
{
    LeModule module;
    Rigidbody rb;
    public bool active = false;
    public float force = 1000;
    public Vector3 cachePosition;
    public Quaternion cacheRotation;

    List<Vector3> dirs = new List<Vector3>{
        new Vector3(1,0,0),
        new Vector3(-1,0,0),
        new Vector3(0,1,0),
        new Vector3(0,-1,0),
        new Vector3(0,0,1),
        new Vector3(0,0,-1),
    };

    public List<bool> hitBoys = new List<bool>{
		false,
		false,
		false,
		false,
		false,
		false
	};
    
    float[] lengths;

    // Start is called before the first frame update
    void Start()
    {
        module = gameObject.GetComponent<LeModule>();
        lengths = new float[]{
            module.size.x/2,
            module.size.x/2,
            module.size.y/2,
            module.size.y/2,
            module.size.z/2,
            module.size.z/2
        };
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < dirs.Count; i++){

            RaycastHit hit;
            Physics.Raycast(transform.position, dirs[i] * lengths[i], out hit);
            if(hit.collider){
                hitBoys[i] = true;
            }else{
                hitBoys[i] = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.R)){
            
            if(active && !hitBoys.Contains(true))
                return;

            if(active && hitBoys.Contains(false)){
                active = false;
                if(rb){
                    Destroy(rb);
                }else{
                    Debug.LogError(module.uid + " Was expecting a rigidbody");
                }
                if(Vector3.Distance(transform.position, cachePosition) < 100){
                    transform.position = cachePosition;
                    transform.rotation = cacheRotation;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if(active && hitBoys.Contains(false)){
            
            if(rb == null){
                rb = gameObject.AddComponent<Rigidbody>();
                rb.mass = module.size.magnitude;
                cachePosition = transform.position;
                cacheRotation = transform.rotation;
            }
            foreach(var hit in hitBoys){
                if(hit == false){
                    int i = hitBoys.IndexOf(hit);
                    Vector3 direction = dirs[i];
                    rb.AddForce(direction * force * Time.time);
                }
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        for(int i = 0; i < dirs.Count; i++){
           
            RaycastHit hit;
            Physics.Raycast(transform.position, dirs[i] * lengths[i], out hit);
            if(hit.collider){
                Gizmos.color = Color.red;
            }else{
                Gizmos.color = Color.white;
            }
            if(hitBoys.Contains(true)){
                Gizmos.DrawRay(transform.position, dirs[i] * lengths[i]);
            }
        }
    }
}
