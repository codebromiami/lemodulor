using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using deVoid.Utils;
using UnityEngine.EventSystems;

public class OnDoubleClick : ASignal {}
public class OnTouchDown : ASignal {}
public class OnTouch : ASignal {}
public class OnTouchUp : ASignal {}
public class OnPinch : ASignal {}

public class TouchScriptInterface : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool hitUI = false;
    public bool twoFingers = false;
    public bool isDoubleTap = false;    
    public bool isTouching = false;
    public float pinchDistance = 0;
    public float previousPinchDistance = 0;
    public float pinchDelta = 0;
    public float pinchDeltaAbsolute = 0;
    public float pinchThreshold = 0.1f;
    public bool isPinching = false;
    public Vector2 firstTapPosition;
    public bool secondTap = false;
    public enum PINCHSTATE {OUT,IN,BELOWTHRESHOLD,IDLE};
    public PINCHSTATE pinchState = PINCHSTATE.IDLE;
    
    public enum TOUCHDIRECTION { UP, DOWN, LEFT, RIGHT, NONE};
    public TOUCHDIRECTION touchDirection = TOUCHDIRECTION.NONE;
    public enum TOUCHDIMENSION {HORIZONTAL, VERTICAL, NONE};
    public TOUCHDIMENSION touchDimension;
    
    public Vector2 screenPosition;
    public Vector2 previousScreenPosition;
    public Vector2 screenPositionDelta;
    public float doubleClickThreshold = 100;
    public float doubleClickTimeDelta = 1;

    private static TouchScriptInterface instance;

    public static TouchScriptInterface Instance
    {
        get { return instance; }
        
    }

    void Awake()
    {
        
        if (instance != null && instance != this)
        {
            Destroy(this);
            Debug.Log("Another TouchScriptInterface attempted to be created and was destroyed.");
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    
    private void Update()
    {            
        if(secondTap){
            doubleClickTimeDelta -= Time.deltaTime;
        }
        if(doubleClickTimeDelta < 0){
            secondTap = false;
        }

        if(Input.touchCount == 2){
            twoFingers = true;
            Vector2 resolution = new Vector2(Screen.width, Screen.height);
            Vector2 a = InverseLerpVector2(Input.GetTouch(0).position, resolution);
            Vector2 b = InverseLerpVector2(Input.GetTouch(1).position, resolution);
            pinchDistance = Vector2.Distance(a,b);
            pinchDelta = pinchDistance - previousPinchDistance;
            pinchDeltaAbsolute = Mathf.Abs(pinchDelta);
        }else{
            twoFingers = false;
            previousPinchDistance = 0;
            pinchDistance = 0;
            pinchDelta = 0;
            pinchDeltaAbsolute = 0;
        }

        if(pinchDeltaAbsolute < pinchThreshold){
            pinchState = PINCHSTATE.BELOWTHRESHOLD;
        }else if(previousPinchDistance == pinchDistance){
            pinchState = PINCHSTATE.IDLE;
        }else if(previousPinchDistance < pinchDistance){
            pinchState = PINCHSTATE.OUT;
        }else if(previousPinchDistance > pinchDistance){
            pinchState = PINCHSTATE.IN;
        }

        isPinching = previousPinchDistance != 0 ? true : false;
        
        // if(Input.touchCount > 1){
        //     if(!secondTap){
        //     doubleClickTimeDelta = 1;
        //     firstTapPosition = gesture.ScreenPosition;
        //     secondTap = true;
        //     return;
        // }else{
        //     if(Vector2.Distance(firstTapPosition, gesture.ScreenPosition) < doubleClickThreshold){
        //         isDoubleTap = true;
        //         Signals.Get<OnDoubleClick>().Dispatch(sender);
        //         Debug.Log(string.Format("Double Tap {0}", Vector2.Distance(firstTapPosition, gesture.ScreenPosition)));
        //     }else{
        //         Debug.Log(string.Format("No Double Tap {0}", Vector2.Distance(firstTapPosition, gesture.ScreenPosition)));
        //     }
        //     secondTap = false;
            // }
        // }

        if(Input.touchCount > 0){
            screenPositionDelta = Input.GetTouch(0).deltaPosition;
            float xABS = Mathf.Abs(screenPositionDelta.x);
            float yABS = Mathf.Abs(screenPositionDelta.y);
            if(xABS > yABS){
                touchDimension = TOUCHDIMENSION.HORIZONTAL;
            }else if(xABS < yABS){
                touchDimension = TOUCHDIMENSION.VERTICAL;
            }else{
                touchDimension = TOUCHDIMENSION.NONE;
            }
            if(touchDimension == TOUCHDIMENSION.HORIZONTAL){
                if(screenPositionDelta.x > 0){
                    touchDirection = TOUCHDIRECTION.RIGHT;
                }else if(screenPositionDelta.x < 0){
                    touchDirection = TOUCHDIRECTION.LEFT;
                }else{
                    touchDirection = TOUCHDIRECTION.NONE;
                }
            }else if(touchDimension == TOUCHDIMENSION.VERTICAL){
                if(screenPositionDelta.y > 0){
                    touchDirection = TOUCHDIRECTION.UP;
                }else if(screenPositionDelta.y < 0){
                    touchDirection = TOUCHDIRECTION.DOWN;
                }else{
                    touchDirection = TOUCHDIRECTION.NONE;
                }
            }else{
                touchDirection = TOUCHDIRECTION.NONE;
            }
            previousScreenPosition = screenPosition;
            if(!isTouching){
                if(!hitUI){
                    Signals.Get<OnTouchDown>().Dispatch();
                }
                // Debug.Log("On Touch Down");
            }
            isTouching = true;
        }else{
            if(isTouching){
                    Signals.Get<OnTouchUp>().Dispatch();
                // Debug.Log("On Touch Up");
            }
            isTouching = false;
        }

        if(isPinching){
            if(pinchState == PINCHSTATE.IN){
                if(!hitUI){
                    Signals.Get<OnPinch>().Dispatch();
                }
            }else if(pinchState == PINCHSTATE.OUT){
                if(!hitUI){
                    Signals.Get<OnPinch>().Dispatch();
                }
            }
        }
        if(isTouching){
            if(!hitUI){
                Signals.Get<OnTouch>().Dispatch();
            }
        }
        isDoubleTap = false;
        previousPinchDistance = pinchDistance;
    }

    public static Vector2 InverseLerpVector2(Vector2 value, Vector2 range){

        float x = Mathf.InverseLerp(0,range.x, value.x);
        float y = Mathf.InverseLerp(0, range.y, value.y);
        value.x = x;
        value.y = y;
        return value;
    }

    public void OnPointerDown(PointerEventData eventData){
        hitUI = false;
    }

    public void OnPointerUp(PointerEventData eventData){
        hitUI = true;
    }



    public static float GetTouchDeltaMagnitudeDifference(Vector2 positionA, Vector2 positonADelta, Vector2 positionB, Vector2 positionBDelta){

        Vector2 touchOnePreviousPosition = positionA - positonADelta;
        Vector2 touchTwoPreviousPosition = positionA - positionBDelta;

        float previousTouchDeltaMagnitude = (touchOnePreviousPosition - touchTwoPreviousPosition).magnitude;
        float touchDeltaMagnitude = (positionA - positionB).magnitude;

        float deltaMagnitudeDifference = previousTouchDeltaMagnitude - touchDeltaMagnitude;

        return deltaMagnitudeDifference;
    }
        
    public static float GetPinchDistanceNormalisedInScreenSpace(Vector2 positionA, Vector2 positionB){
            
        float distanceNormalised = 0;
        Vector2 touchOneNoralised = positionA;
        Vector2 touchTwoNormalised = positionB;
        touchOneNoralised.x = Mathf.InverseLerp(0, Screen.width, positionA.x);
        touchOneNoralised.y = Mathf.InverseLerp(0,Screen.height, positionA.y);
        touchTwoNormalised.x = Mathf.InverseLerp(0,Screen.width, positionB.x);
        touchTwoNormalised.y = Mathf.InverseLerp(0,Screen.height, positionB.y);
        distanceNormalised = Vector2.Distance(touchOneNoralised, touchTwoNormalised);
        return distanceNormalised;
    }
}
