using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Components")]
    [Space]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private LevelManager levelManager;

    private void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(MainMenu);
    }

    private void MainMenu()
    {
        levelManager.GoToMainMenu();
    }

    private void RestartGame()
    {
        levelManager.RestartCurrentLevel();
    }

    private void ResumeGame()
    {
        uiManager.SwitchToGamePlayUI();
    }
}
