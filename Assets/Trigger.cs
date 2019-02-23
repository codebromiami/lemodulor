using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{   
    public bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PilotiAgent>())
            return;
        triggered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<PilotiAgent>())
            return;
        triggered = false;
    }
}
