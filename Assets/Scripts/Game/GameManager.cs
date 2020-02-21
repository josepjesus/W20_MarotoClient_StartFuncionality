using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    private float spawnRate = 1.0f;
    private int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerNameText;
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public bool isGameActive;

    public Player player;

    public Text leftColumn;

    bool seconds = false;
    int sec = 0;
    int min = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        playerNameText.text = player.Name;
        scoreText.text = score.ToString();
        
    }

    public void Update()
    {
        if (!seconds)
        {
            
            StartCoroutine(HandleIt());
            sec += 3;
            min = sec / 60;
            leftColumn.text = player.Name + GetPlayerAge() + "\n" + min + "\"" + sec%60 + "'";
            
        }
    }

    private IEnumerator HandleIt()
    {
        seconds = true;
        // process pre-yield
        yield return new WaitForSeconds(3.0f);
        // process post-yield
        seconds = false;
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true;
        score = 0;
        UpdateScore(0);
        titleScreen.gameObject.SetActive(false);
        spawnRate /= difficulty;
        StartCoroutine(SpawnTarget());
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int randomIndex = UnityEngine.Random.Range(0, 4);
            Instantiate(targets[randomIndex]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
        isGameActive = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private string GetPlayerAge()
    {
        DateTime now = DateTime.Now;

        TimeSpan diference = now - player.BirthDay;

        return "(" + diference.Days / 365 + ")";
    }

    void OnApplicationQuit()
    {
        Destroy(player);
    }
}
