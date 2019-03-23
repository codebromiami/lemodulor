using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public List<StateBehaviour> states = new List<StateBehaviour>();
    public StateBehaviour.state currentState = StateBehaviour.state.none;

    // Start is called before the first frame update
    void Start()
    {
        states.AddRange(FindObjectsOfType<StateBehaviour>());   
    }

    // Update is called once per frame
    void Update()
    {
        foreach(StateBehaviour state in states){
            if(state.thisState == currentState){
                state.enabled = true;
            }else{
                state.enabled = false;
            }
        }
    }
}

public class StateBehaviour : MonoBehaviour {
    
    public enum state {none, one, two};
    public state thisState = state.none;
}

public interface IStatefullable
{
    void ChangeState(string stateName);
}