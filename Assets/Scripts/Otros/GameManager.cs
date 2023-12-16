using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField]
    private bool gameOnPause = false;
    public GameObject menu;
    public GameObject controls;
    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI playerPoints;
    public TextMeshProUGUI timerPointsMinutes;
    public TextMeshProUGUI timerPointsSeconds;
    public LoadScene sceneLoader;
    [Header("Player information")]
    public PlayerController player;
    private int timerMinutes = 5;
    private float timerSeconds = 0;

    void Awake()
    {
        ResumeGame();
    }
    void Update()
    {
        playerHealth.text = player.GetLife().ToString();
        playerPoints.text = player.GetPoints().ToString();
        timerPointsMinutes.text = GetTimerMinutes().ToString();
        timerPointsSeconds.text = GetTimerSeconds().ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameOnPause && menu.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        if (!gameOnPause)
        {
            Timer();
        }
    }

    //<--------------------Game Pause & Resume--------------------------->

    public void PauseGame()
    {
        gameOnPause = true;
        Time.timeScale = 0f;
        player.SetCanShoot(false);
        if (!controls.activeSelf)
        {
            menu.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        gameOnPause = false;
        Time.timeScale = 1f;
        player.SetCanShoot(true);
        menu.SetActive(false);
    }

    //<----------------------Timer-------------------------------->

    public int GetTimerMinutes()
    {
        return timerMinutes;
    }
    public int GetTimerSeconds()
    {
        return (int)timerSeconds;
    }

    public void SetTimerMinutes(int time)   
    {
        timerMinutes += time;
    }

    public void Timer()
    {
        timerSeconds -= Time.deltaTime;
        
        if (timerSeconds <= 0)
        {
            if (timerMinutes == 0)
            {
                sceneLoader.LoadGivenScene("Derrota");
            }
            else
            {
                timerMinutes --;
            }

            timerSeconds = 60f;
        }
    }
}
