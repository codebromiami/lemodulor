using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGoldenSection : StateBehaviour
{
    public Vector2 screenDelta;
    public Vector2 currentPress;
    public Vector2 previousPress;
    public GameObject squareGameObject;
    public GameObject goldenSectionGO;
    public enum direction {up,down,left,right,none}
    public direction pointDirection = direction.none;
    
    private void Start()
    {
        squareGameObject.transform.localScale = Vector3.one * 2.26f;
        goldenSectionGO = GameObject.Instantiate(squareGameObject);
    }
    
    void Update()
    {
        if(Input.GetMouseButton(0)){
            currentPress = Input.mousePosition;
            if(currentPress != previousPress & previousPress != Vector2.zero){
                screenDelta = currentPress - previousPress;
                Ray rayCurrent = Camera.main.ScreenPointToRay(currentPress);
                Ray rayPrevious = Camera.main.ScreenPointToRay(previousPress);
                float distance = Vector3.Distance(Camera.main.transform.position, goldenSectionGO.transform.position);
                Vector3 worldCurrent = rayCurrent.GetPoint(distance);
                Vector3 worldPrevious = rayPrevious.GetPoint(distance);
                Vector3 worldDelta = worldCurrent - worldPrevious;

                float xDelta = worldCurrent.x - goldenSectionGO.transform.position.x;
                float yDelta = worldCurrent.y - goldenSectionGO.transform.position.y;
                if(Mathf.Abs(xDelta) > Mathf.Abs(yDelta)){
                    if(xDelta > 0)
                        pointDirection = direction.right;
                    else
                        pointDirection = direction.left;
                }else{
                    if(yDelta > 0)
                        pointDirection = direction.up;
                    else
                        pointDirection = direction.down;
                }

                Vector3 localScale = goldenSectionGO.transform.localScale;
                Vector3 targetPosition = squareGameObject.transform.localPosition;
                Vector3 targetScale = localScale;
                switch(pointDirection){
                    case direction.up:
                    targetScale.x = 2.26f;
                    targetScale.y = 1.13f;
                    targetPosition.y += (squareGameObject.transform.localScale.y /2) * 1.25f;
                    break;
                    case direction.down:
                    targetScale.x = 2.26f;
                    targetScale.y = 1.13f;
                    targetPosition.y -= (squareGameObject.transform.localScale.y / 2) * 1.25f;
                    break;
                    case direction.left:
                    targetScale.x = 1.13f;
                    targetScale.y = 2.26f;
                    targetPosition.x -= (squareGameObject.transform.localScale.x / 2) * 1.25f;
                    break;
                    case direction.right:
                    targetScale.x = 1.13f;
                    targetScale.y = 2.26f;
                    targetPosition.x += (squareGameObject.transform.localScale.x / 2) * 1.25f;
                    break;
                }
                goldenSectionGO.transform.localScale = Vector3.MoveTowards(goldenSectionGO.transform.localScale, targetScale, 0.1f);
                goldenSectionGO.transform.localPosition = Vector3.MoveTowards(goldenSectionGO.transform.localPosition, targetPosition, 0.1f);
                // if(pointDirection != direction.none & pointDirection != direction.up & pointDirection != direction.down){
                //     localScale.x += worldDelta.x;
                //     localScale.y = 2.26f;
                // }else if(pointDirection != direction.none){
                //     localScale.y += worldDelta.y;   
                //     localScale.x = 2.26f;
                // }
                // squareGameObject.transform.localScale = localScale;
            }
            previousPress = currentPress;  
        }

        if(Input.GetMouseButtonUp(0)){
            currentPress = Vector3.zero;
            previousPress = Vector3.zero;
        }    
    }
}
