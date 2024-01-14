using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_Head : Body_Part
{
    Vector2 _movement;
    private Body_Part tail = null;
    const float timeToAdBodyPart = 0.01f;
    float addTimer = timeToAdBodyPart;
    float _snakeSpeed=0f; 
   
    private int partToAdd = 0;
    // Start is called before the first frame update
    void Start()
    {
        Swipe_Controller.OnSwipe += SwipeDetection;
        _snakeSpeed = Game_Controller.instance._snakeSpeed;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!Game_Controller.instance.alive) { return; }

        base.Update();

        SetMovement( _movement );
        UpdateDirection();
        UpdatePosition();

        if (partToAdd > 0)
        {
            addTimer -= Time.deltaTime;
            if(addTimer <=0)
            {
                addTimer = timeToAdBodyPart;
                AddboddyPart();
                partToAdd--;
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Egg_Mine egg=collision.GetComponent<Egg_Mine>();
        if(egg != null) { EatEgg(egg); }
        else { Debug.Log("Hit obstacle");
            Game_Controller.instance.GameOver();
        }
    }
    void SwipeDetection(Swipe_Controller.SwipeDirection direction)
    {
        switch (direction)
        {
            case Swipe_Controller.SwipeDirection.up:
                Move(Vector2.up);
                break;
            case Swipe_Controller.SwipeDirection.down:
                Move(Vector2.down);
                break;
            case Swipe_Controller.SwipeDirection.left:
                Move(Vector2.left);
                break;
            case Swipe_Controller.SwipeDirection.right:
                Move(Vector2.right);
                break;
        }
    }

    private void Move(Vector2 dir)
    {
        _movement = _snakeSpeed * Time.deltaTime * dir;
    }
   
    void AddboddyPart()
    {
        if (tail == null)
        {
            Vector3 newPosition = transform.position;
            newPosition.z += 0.01f;

            Body_Part newPart=Instantiate(Game_Controller.instance.bodyPrefab,newPosition,Quaternion.identity);
            newPart.following = this;
            tail = newPart;
            newPart.TurnInToTail();
        }
        else
        {
            Vector3 newPosition=tail.transform.position;
            newPosition.z +=0.01f;

            Body_Part newPart = Instantiate(Game_Controller.instance.bodyPrefab, newPosition, Quaternion.identity);
            newPart.following = tail;
            newPart.TurnInToTail();
            tail.TurnInToBodyPart();
            tail = newPart;


        }
    }

     public void ResetSnake()
    {
        StartCoroutine(ResetAction()
        );
    }
    IEnumerator ResetAction()
    {
        yield return new WaitForSeconds(2);
        tail = null;
        Move(Vector2.up);
        partToAdd = 5;
        addTimer = timeToAdBodyPart;
    }
    private void EatEgg(Egg_Mine egg)
    {
        partToAdd = 5;
        addTimer = 0;
        Game_Controller.instance.EggEaten(egg);
    }
}
