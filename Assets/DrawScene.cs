using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawScene : StateBehaviour
{
    public Vector3 startPoint;
    public Vector2 screenDelta;
    public Vector2 currentPress;
    public Vector2 previousPress;
    public GameObject squareGameObject;
    public bool xDone;
    public bool yDone;
    public float thresholdMax;
    public bool moveCamera;
    
    void Start()
    {
        Vector3 localScale = squareGameObject.transform.localScale;
        localScale.x = 0;
        localScale.y = 0;
        squareGameObject.transform.localScale = localScale;
    }
    void Update()
    {
        //click and drag to draw a square on the screen, square will be done when it reaches 2.26m
        if(Input.GetMouseButtonDown(0)){

            startPoint = Input.mousePosition;
            Ray rayCurrent = Camera.main.ScreenPointToRay(startPoint);
            float distance = Vector3.Distance(Camera.main.transform.position, squareGameObject.transform.position);
            Vector3 worldCurrent = rayCurrent.GetPoint(distance);
            squareGameObject.transform.position = worldCurrent;
            xDone = false;
            yDone = false;
            moveCamera = false;
            Vector3 localScale = squareGameObject.transform.localScale;
            localScale.x = 0;
            localScale.y = 0;
            squareGameObject.transform.localScale = localScale;
        }

        if(Input.GetMouseButton(0)){
            currentPress = Input.mousePosition;
            if(currentPress != previousPress & previousPress != Vector2.zero){
                screenDelta = currentPress - previousPress;
                Ray rayCurrent = Camera.main.ScreenPointToRay(currentPress);
                Ray rayPrevious = Camera.main.ScreenPointToRay(previousPress);
                float distance = Vector3.Distance(Camera.main.transform.position, squareGameObject.transform.position);
                Vector3 worldCurrent = rayCurrent.GetPoint(distance);
                Vector3 worldPrevious = rayPrevious.GetPoint(distance);
                Vector3 worldDelta = worldCurrent - worldPrevious;
                Vector3 localScale = squareGameObject.transform.localScale;
                if(Mathf.Abs(localScale.x) > thresholdMax)
                    xDone = true;
                if(Mathf.Abs(localScale.y) > thresholdMax)
                    yDone = true;
                if(!xDone)
                    localScale.x += worldDelta.x;
                else
                    localScale.x = 2.26f;
                if(!yDone)
                    localScale.y += worldDelta.y;
                else
                    localScale.y = 2.26f;
                squareGameObject.transform.localScale = localScale;
                Vector3 position = squareGameObject.transform.localPosition;
                position.x += localScale.x;
                position.y += localScale.y;
            }
            previousPress = currentPress;  
        }

        if(Input.GetMouseButtonUp(0)){
            currentPress = Vector3.zero;
            previousPress = Vector3.zero;
            moveCamera = true;
        }

        //camera will move to fit the square in the center of the screen
        bool atTarget = false;
        if(moveCamera){
            Vector3 target = squareGameObject.transform.localPosition;
            target.z = Camera.main.transform.position.z;
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, target, 0.1f);
            if(Camera.main.transform.position == target)
                atTarget = true;
        }
        
        bool complete = false;
        if(atTarget & xDone & yDone)
            complete = true;

        if(complete)
            FindObjectOfType<StateManager>().currentState = StateBehaviour.state.two;
            
    }
}
