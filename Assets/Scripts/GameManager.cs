using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const int MAX_SCORE_EASY = 40;
    const int MAX_SCORE_MEDIUM = 80;
    const int MAX_SCORE_HARD = 120;
    public TextMeshProUGUI scoreText;
    private int scoreValue;
    private Blade _blade;
    private Spawner _spawner;
    public Image _fadeImg;
    public TextMeshProUGUI levelText;
    private int currLevel = 1;
    public TextMeshProUGUI _winMessage;
    public TextMeshProUGUI _loseMessage;
    public RawImage _levelUpImg;
    private bool flagEasy = true;
    private bool flagMedium = true;
    [HideInInspector]
    public AudioSource winAudio;
    
    private void Awake()
    {
        _blade = FindObjectOfType<Blade>();
        _spawner = FindObjectOfType<Spawner>();
        winAudio = transform.GetComponent<AudioSource>();
    }

    private void Start()
    {
        InitGame();
    }

    private void Update()
    {
        //handling the difficulty levels with the bombs rate
        if (scoreValue <= MAX_SCORE_EASY)
        {
            FindObjectOfType<Spawner>().bombRate = 0.05f;
            
            if (scoreValue == MAX_SCORE_EASY && flagEasy == true) //showing the level-up image
            {
                flagEasy = false;
                StartCoroutine(LevelUpImgAppear());
            }
        }
        else if (scoreValue <= MAX_SCORE_MEDIUM)
        {
            FindObjectOfType<Spawner>().bombRate = 0.15f;
            currLevel = 2;
            levelText.text = currLevel.ToString();
            
            if (scoreValue == MAX_SCORE_MEDIUM && flagMedium == true) //showing the level-up image
            {
                flagMedium = false;
                StartCoroutine(LevelUpImgAppear());
            }
        }
        else if (scoreValue < MAX_SCORE_HARD)
        {
            FindObjectOfType<Spawner>().bombRate = 0.4f;
            currLevel = 3;
            levelText.text = currLevel.ToString();
        }
        else //the user won the game
        {
            GameOver();
        }
    }

    private IEnumerator LevelUpImgAppear()
    {
        _levelUpImg.enabled = true;
        yield return new WaitForSeconds(1.5f);
        _levelUpImg.enabled = false;
    }
    private void InitGame()
    {
        Time.timeScale = 1f;
        _blade.enabled = true;
        _spawner.enabled = true;
        _winMessage.enabled = false;
        _loseMessage.enabled = false;
        _levelUpImg.enabled = false;
        
        //init the score to 0
        scoreValue = 0;
        scoreText.text = scoreValue.ToString();
        
        //clear screen from fruits
        Fruit[] fruitsArr = FindObjectsOfType<Fruit>();

        foreach (Fruit fruit in fruitsArr)//destroying the fruits objects that were created
        {
            Destroy(fruit.gameObject);
        }
        
        //clear screen from bombs
        Bomb[] bombsArr = FindObjectsOfType<Bomb>();

        foreach (Bomb bomb in bombsArr)//destroying the bombs objects that were created (if there are any)
        {
            Destroy(bomb.gameObject);
        }
    }
    
    public void AddPoint()
    {
        scoreValue++;
        scoreText.text = scoreValue.ToString();
    }

    public void GameOver()
    {
        _blade.enabled = false;
        _spawner.enabled = false;
        StartCoroutine(Explode());
    }

    private IEnumerator Explode() //when we cut a bomb, this function handles the screen's fading away
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            _fadeImg.color = Color.Lerp(Color.clear, Color.white, Mathf.Clamp01(elapsed / duration));
            Time.timeScale = 1f - Mathf.Clamp01(elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);
        
        if (scoreValue == MAX_SCORE_HARD)
        {
            _winMessage.enabled = true;
        }
        else
        {
            _loseMessage.enabled = true;
        }
        
        yield return new WaitForSecondsRealtime(5f);
        elapsed = 0f;
        
        while (elapsed < duration)
        {
            _fadeImg.color = Color.Lerp(Color.white, Color.clear, Mathf.Clamp01(elapsed / duration));
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        
        SceneManager.LoadScene("GameMenu");
    }
}
