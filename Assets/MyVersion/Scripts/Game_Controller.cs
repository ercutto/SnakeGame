using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Controller : MonoBehaviour
{
    public static Game_Controller instance=null;
    const float width = 3.7f;
    const float height = 7f;
    public GameObject _rockPrefab = null;
    public GameObject _eggPrefab = null;
    public GameObject _goldenEggPrefab = null;
    public float _snakeSpeed;
    public Body_Part bodyPrefab=null;
    public Sprite tailSprite=null;
    public Sprite bodySprite=null;
    public Snake_Head snake_Head=null;
    public bool waitingToPlay = true;

    public bool alive = true;
    void Start()
    {
        instance = this;
        CreateWalls();
        StartGame();
        CreateEgg();
        alive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingToPlay)
        {
            foreach(Touch touch in Input.touches)
            {
                if(touch.phase == TouchPhase.Ended)
                {
                    StartGamePlay();
                }
            }

            if(Input.GetMouseButtonUp(0))
            {
                StartGamePlay();
            }
        }
    }
    void StartGame()
    {
        snake_Head.ResetSnake();
    }

    void CreateWalls()
    {
        float z = -1f;
        Vector3 start = new Vector3(-width, -height, z);
        Vector3 finish = new Vector3(-width, +height, z);
        CreateWall(start, finish);

        start = new Vector3(+width, -height, z);
        finish = new Vector3(+width, +height, z);
        CreateWall(start, finish);
        
        start = new Vector3(-width, -height, z);
        finish = new Vector3(+width, -height, z);
        CreateWall(start, finish);

        start = new Vector3(-width, +height, z);
        finish = new Vector3(+width, +height, z);
        CreateWall(start, finish);
    }
    void CreateWall(Vector3 start,Vector3 finish)
    {
        float distance=Vector3.Distance(start, finish);
        int noOfRocks = (int)(distance * 3);
        Vector3 delta=(finish - start)/noOfRocks;

        Vector3 position = start;
        for(int rock = 0; rock <= noOfRocks; rock++)
        {
            float rotation = Random.Range(0, 360f);
            float scale = Random.Range(1.5f, 2f);
            CreateRock(position, scale, rotation);
            position = position + delta;
        }
    }
    void CreateRock(Vector3 position,float scale,float rotation)
    {
        GameObject _rock= Instantiate(_rockPrefab,position,Quaternion.Euler(0,0,rotation));
        _rock.transform.localScale=new Vector3(scale,scale,1);
    }

    void CreateEgg(bool golden=false)
    {
        Vector3 position;
        position.x=-width+Random.Range(1f,(width*2)-2f);
        position.y = -height + Random.Range(1f, (height * 2) - 2f);
        position.z = -1;
        if(golden)
            Instantiate(_goldenEggPrefab, position, Quaternion.identity);
        else
            Instantiate(_eggPrefab, position, Quaternion.identity);
    }
    void StartGamePlay()
    {
        waitingToPlay = false;
        alive = true;
    }
    public void GameOver()
    {
        alive = false;
        waitingToPlay = true;
    }
    public void EggEaten(Egg_Mine egg)
    {
        Destroy(egg.gameObject);
    }

 
}
