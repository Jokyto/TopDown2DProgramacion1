using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField]
    private bool gameOnPause = false;
    public GameObject menu;
    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI playerPoints;
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
            if (gameOnPause)
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
        menu.SetActive(true);
        player.SetCanShoot(false);
    }

    public void ResumeGame()
    {
        gameOnPause = false;
        Time.timeScale = 1f;
        menu.SetActive(false);
        player.SetCanShoot(true);
    }
}
