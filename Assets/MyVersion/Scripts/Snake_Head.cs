using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_Head : Body_Part
{
    Vector2 _movement;
    // Start is called before the first frame update
    void Start()
    {
        Swipe_Controller.OnSwipe += SwipeDetection;
    }

    // Update is called once per frame
    void Update()
    {
        SetMovement( _movement );
        UpdateDirection();
        UpdatePosition();

    }
    void SwipeDetection(Swipe_Controller.SwipeDirection direction)
    {
        switch (direction)
        {
            case Swipe_Controller.SwipeDirection.up:
                MoveUp();
                break;
            case Swipe_Controller.SwipeDirection.down:
                MoveDown();
                break;
            case Swipe_Controller.SwipeDirection.left:
                MoveLeft();
                break;
            case Swipe_Controller.SwipeDirection.right:
                MoveRight();
                break;
        }
    }

    void MoveUp()
    {
        _movement = Vector2.up * Game_Controller.instance._snakeSpeed*Time.deltaTime;
    }

    void MoveDown()
    {
        _movement = Vector2.down * Game_Controller.instance._snakeSpeed * Time.deltaTime;

    }
    void MoveLeft()
    {
        _movement = Vector2.left * Game_Controller.instance._snakeSpeed * Time.deltaTime;

    }
    void MoveRight()
    {
        _movement = Vector2.right * Game_Controller.instance._snakeSpeed * Time.deltaTime;

    }
}
