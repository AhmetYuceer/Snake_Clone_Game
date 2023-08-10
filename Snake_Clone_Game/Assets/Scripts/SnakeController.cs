using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] [Range(0.1f,1f)] private float moveDelay;
    public static bool isDead;
    public static bool ateFoot;
    Vector2 direction = new Vector2(0,-1);

    [SerializeField] private List<GameObject> pieces = new List<GameObject>();

    public bool isPressRight => Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
    public bool isPressLeft => Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
    public bool isPressUp => Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
    public bool isPressDown => Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);

    private bool isRotated = false;

    [SerializeField] private AudioClip eatClip;
    [SerializeField] private AudioClip deathClip;
    private AudioSource audioSource;

    private void Start()
    {
        isDead = false;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(moveE());
    }

    private void Update()
    {
        DeathSoundEffect();
        Inputs();
        EatFood();
    }
    
    private void DeathSoundEffect()
    {
        if (isDead)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                if (!audioSource.isPlaying)
                {     
                    audioSource.PlayOneShot(deathClip);
                    Destroy(pieces[i].gameObject);
                    pieces.RemoveAt(i);
                }
            }
        }   
    }

    private void Inputs()
    {
        if(isPressRight && direction.x != -1 && !isRotated)
        {
            direction.y = 0 ;
            direction.x = 1; 
            isRotated = true;
        }
        else if(isPressLeft && direction.x != 1 && !isRotated)
        {
            direction.y = 0 ;
            direction.x = -1;
            isRotated = true;
        }
        else if(isPressUp && direction.y != -1 && !isRotated)
        {
            direction.y = 1;
            direction.x = 0;
            isRotated = true;
        }
        else if(isPressDown && direction.y != 1 && !isRotated)
        {
            direction.y = -1 ;
            direction.x = 0;
            isRotated = true;
        }    
    }

    private void Move()
    {
        Vector2 snakeDirection = transform.position;
        snakeDirection += direction;
        var headPosPreview = transform.position;
        transform.position = snakeDirection;

        for (int i = 0; i < pieces.Count; i++)
        {
            var piecePosPreview = pieces[i].transform.position;
            pieces[i].transform.position = headPosPreview;
            if (i -1 != -1)
            {   
                pieces[i-1].transform.position = piecePosPreview;
            }
        }
        GridManager.snakePos.Clear();
        GridManager.snakePos.Add(transform.position);
        foreach (var item in pieces)
        {
            GridManager.snakePos.Add(item.transform.position);
        }
        isRotated = false;
    }

    IEnumerator moveE()
    {
        while (!isDead)
        {
            Move();
            DeadControl();
            yield return new WaitForSeconds(moveDelay);
        }
    }


    private void DeadControl()
    {
        var headPos = transform.position;
        int x = Mathf.RoundToInt(headPos.x);
        int y = Mathf.RoundToInt(headPos.y);

        for (int i = 0; i < GridManager.gridX; i++)
        {
            for (int a = 0; a < GridManager.gridY; a++)
            {
                if (GridManager.grid[i,a])
                {
                    if (x == i && y == a)
                    {
                        isDead = true;
                    }
                }
                else if( x > GridManager.gridX || y > GridManager.gridY)
                {
                    isDead = true;
                }
            }
        }
    }
    public void EatFood()
    {
        Vector2 headPos = transform.position;
        if (GameManager.foodPos == headPos && !ateFoot)
        {
            audioSource.PlayOneShot(eatClip);
            ateFoot = true;
            Destroy(GameManager.food);
            pieces.Add(Instantiate(pieces[1].gameObject,transform.position,Quaternion.identity));
            GameManager.spawnedFoodCount = 0;
            GameManager.addScore();
            moveDelay -= 0.01f;
        }   
    }
}