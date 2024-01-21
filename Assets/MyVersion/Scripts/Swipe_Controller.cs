using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe_Controller : MonoBehaviour
{
    Vector2 swipeStart;
    Vector2 swipeEnd;
    float minimumDistance = 10f;
    public static event System.Action<SwipeDirection> OnSwipe = delegate {  };
    public enum SwipeDirection
    {
        up,down,left,right
    }
    
    // burda ekrana dokunus arraylerden baslagic ve bitis noktalari arasindaki yönleri tespit ediyoruz
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //SwipeMove();
        //MouseMove();
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                swipeStart = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                swipeEnd = touch.position;
                //baslangic yapilmis ve bitmis demektir ozaman dokunusun yonunu tespit edebilriiz
                ProcessSwipe();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            swipeStart = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            swipeEnd = Input.mousePosition;
            ProcessSwipe();
        }
    }

    //burayi gecici yaptim
   /* void SwipeMove()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                swipeStart = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                swipeEnd = touch.position;
                //baslangic yapilmis ve bitmis demektir ozaman dokunusun yonunu tespit edebilriiz
                ProcessSwipe();
            }
        }
    }
    void MouseMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swipeStart = Input.mousePosition; 
        }else if(Input.GetMouseButtonUp(0))
        {
            swipeEnd = Input.mousePosition;
            ProcessSwipe();
        }

    }
   */
    void ProcessSwipe()
    {
        float _distance=Vector2.Distance(swipeStart, swipeEnd);

        if(_distance > minimumDistance) {
            if (IsVerticalSwipe()) 
            {
                //eger bitis vector baslagic vectorden buyuk ise yukari 
                if(swipeEnd.y > swipeStart.y) { 
                    OnSwipe(SwipeDirection.up);
                }
                else //degilse asagi
                {
                    OnSwipe(SwipeDirection.down);
                }
            }
            else// so its horizontal
            {
                //eger bitis vector baslangic vectorden buyukse saga dogru 
                if(swipeEnd.x > swipeStart.x)
                {
                    OnSwipe(SwipeDirection.right);
                }
                else //degilse sola dogru
                {
                    OnSwipe(SwipeDirection.left);
                }
            }
        }

    }
    //dikey mi yoksa yataymi sorusuna cevabi iki yonden hangisine daha uzun sure
    //dokunuldugunu iki yondeki bitis vectorlerinden baslangic vectorleri cikartarak buluyoruz
    bool IsVerticalSwipe()
    {
       
        float _vertical=Mathf.Abs(swipeEnd.y - swipeStart.y);
        float _horizontal=Mathf.Abs(swipeEnd.x - swipeStart.x);
        if(_vertical > _horizontal)
        {
            return true;
        }
        return false;
    }
}
