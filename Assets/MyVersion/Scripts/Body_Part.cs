using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Body_Part : MonoBehaviour
{
    Vector2 _dPosition;
    public Body_Part following = null;
 

    private SpriteRenderer spriteRenderer=null;
    const int PARTSREMEMBERED=10;
    public Vector3[] previousPositions = new Vector3[PARTSREMEMBERED];
    public int setIndex = 0;
    public int getIndex = -(PARTSREMEMBERED - 1);
    public int eating = Animator.StringToHash("eating");
    Animator animator;
    // Start is called before the first frame update
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }
    void Start()
    {
        
    }
    public void ResetMemory()
    {
        setIndex = 0;
        getIndex = -(PARTSREMEMBERED - 1);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(!Game_Controller.instance.alive) { return; }

        Vector3 followPosition;
        if (following != null)
        {
            if(following.getIndex>-1)
                followPosition = following.previousPositions[following.getIndex];
            else followPosition= following.transform.position;
        }
        else { followPosition = gameObject.transform.position; }

       
        previousPositions[setIndex].x = gameObject.transform.position.x;
        previousPositions[setIndex].y = gameObject.transform.position.y;
        previousPositions[setIndex].z = gameObject.transform.position.z;

        setIndex++;
        if (setIndex >= PARTSREMEMBERED) setIndex = 0;
        getIndex++;
        if (getIndex >= PARTSREMEMBERED) getIndex = 0;

        if (following != null)
        {
            Vector3 newPosition;
            if (following.getIndex > -1)
            {
                newPosition = followPosition;
            }
            else
            {
                newPosition = following.transform.position;
            }

            newPosition.z=newPosition.z + 0.01f;
            

            SetMovement(newPosition - gameObject.transform.position);
            UpdateDirection();
            UpdatePosition();

        }

        


    }
 
    public void SetMovement(Vector2 movement)
    {
        _dPosition = movement;
    }
    public void UpdatePosition()
    {
       
        gameObject.transform.position += (Vector3)_dPosition;

    }
    public void UpdateDirection()
    {
        if (_dPosition.y > 0)
        {
            //up
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        }else if(_dPosition.y < 0)
        {
            //down
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);

        }
        else if (_dPosition.x < 0)
        {
            //left
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);

        }
        else if(_dPosition.x > 0)
        {
            //right
            gameObject.transform.localEulerAngles = new Vector3(0, 0, -90);

        }

    }

    public void TurnInToTail()
    { if (spriteRenderer != null)
        {
            spriteRenderer.sprite = Game_Controller.instance.tailSprite;
        }
        else
        {
           // Debug.LogError("spriteRenderer yok");
        }
    }
    public void TurnInToBodyPart()
    {
   
        spriteRenderer.sprite = Game_Controller.instance.bodySprite;
    }

    public void PlayAnim()
    {
        animator.Play(eating);
    }
}
