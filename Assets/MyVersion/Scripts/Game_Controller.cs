using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Spikes")]
    public GameObject spikePrefab= null;
    public List<GameObject> spikeList= new List<GameObject>();
    [Space]

    List<Egg_Mine> eggs = new List<Egg_Mine>();

    public Text scoreText = null;
    public Text HighscoreText = null; 
    public GameObject gameOverText = null; 
    public GameObject tapToPlayText = null;

    int score = 0;
    public int hiScore = 0;
    [Header("Level variables")]
    public int level = 0;
    int numberOfEggsForNextLevel = 0;

    public bool alive = true;
    void Start()
    {
        instance = this;
        CreateWalls();
       
       
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
        Egg_Mine egg = null;
        if(golden)
           egg= Instantiate(_goldenEggPrefab, position, Quaternion.identity).GetComponent<Egg_Mine>();
        else
           egg= Instantiate(_eggPrefab, position, Quaternion.identity).GetComponent<Egg_Mine>();

        eggs.Add(egg);    
    }
    void KillOldEggs()
    {
        foreach(Egg_Mine egg in eggs)
        {
            Destroy(egg.gameObject);
        }
        eggs.Clear();
    }
    void StartGamePlay()
    {
        score = 0;
        level = 0;
       
        UpdateScoreText(score);
        UpdateHighScoreText(hiScore);
        gameOverText.gameObject.SetActive(false);
        tapToPlayText.gameObject.SetActive(false);
        waitingToPlay = false;
        alive = true;
        KillOldEggs();
        ClearSpikes();
       
        LevelUp();
    }
    public void GameOver()
    {
        alive = false;
        waitingToPlay = true;
        gameOverText.gameObject.SetActive(true);
        tapToPlayText.gameObject.SetActive(true);
     

    }
    public void EggEaten(Egg_Mine egg)
    {
        score++;

        numberOfEggsForNextLevel--;

        if(numberOfEggsForNextLevel == 0)
        {
            score += 10;
            
            LevelUp();
        }
        else if(numberOfEggsForNextLevel == 1)
        {
            CreateEgg(true);
        }
        else { CreateEgg(false); }

        if (score > hiScore)
        {
            hiScore = score;
            UpdateHighScoreText(hiScore);
        }

        
        UpdateScoreText(score);
        eggs.Remove(egg);
        Destroy(egg.gameObject);
    }

    void LevelUp()
    {
        level++;
        numberOfEggsForNextLevel =4 + (level * 2);
        _snakeSpeed = 1f+(level / 4f);
        if(_snakeSpeed>6f) { _snakeSpeed = 6f; }


        snake_Head.ResetSnake();
        CreateEgg();
        AddSpike();
    }
    void AddSpike()
    {
        Vector3 position;
        position.x = -width + Random.Range(1f, (width * 2) - 2f);
        position.y = -height + Random.Range(1f, (height * 2) - 2f);
        position.z = -1;
        GameObject createdSpike = Instantiate(spikePrefab, position, Quaternion.identity);
        spikeList.Add(createdSpike);
    }
    void ClearSpikes()
    {
        foreach (var spike in spikeList)
        {
            Destroy(spike);
        }
        spikeList.Clear();
    }
    void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }
    void UpdateHighScoreText(int _score)
    {
        HighscoreText.text = _score.ToString();
    }
}
