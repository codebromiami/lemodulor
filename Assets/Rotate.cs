using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    Vector3 rotation = Vector3.zero;
    // Update is called once per frame
    void Update()
    {       
        rotation.x = Random.Range(0f,1f);
        rotation.y = Random.Range(0f,1f);
        rotation.z = Random.Range(0f,1f);
        this.transform.Rotate(rotation, Space.Self);
    }
}
