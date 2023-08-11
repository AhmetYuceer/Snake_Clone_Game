using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int spawnedFoodCount;
    [SerializeField] private GameObject foodPrefabs;
    public static GameObject food;
    public static Vector2 foodPos;


    public static int score;
    public static int scoreValue = 5;
    [SerializeField] private Text scoreText;

    //RestartPanel
    [SerializeField] private Text restartCountText;
    [SerializeField] private GameObject restartPanel;

    private void Start() 
    {
        spawnedFoodCount = 0;
        score = 0;
    }

    private void Update() 
    {
       SpawnFood();
       RestartGameUI();
    }

    public static void addScore()
    {
        score += scoreValue;
    }

    private void SpawnFood()
    {
        if (spawnedFoodCount == 0)
        {
            SnakeController.ateFoot = false;
            var foodPosition = new Vector2(0,0);
            foodPosition.x = Random.Range(0,(int)GridManager.gridX);
            foodPosition.y = Random.Range(0,(int)GridManager.gridY);
            food = Instantiate(foodPrefabs,foodPosition,Quaternion.identity);
            spawnedFoodCount = 1;
            foodPos = foodPosition;
            scoreText.text = score.ToString();
        }
    }

    private void RestartGameUI()
    {
        if (SnakeController.isDead)
        {
            restartPanel.SetActive(true);
            restartCountText.text = score.ToString();
        }
        else
        {
            restartPanel.SetActive(false);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}