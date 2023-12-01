using System.Collections.Generic;
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
    public LoadScene sceneLoader;
    [Header("Player information")]
    public PlayerController player;

    void Awake()
    {
        ResumeGame();
    }
    void Update()
    {
        playerHealth.text = player.GetLife().ToString();
        playerPoints.text = player.GetPoints().ToString();

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
    }

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
}
