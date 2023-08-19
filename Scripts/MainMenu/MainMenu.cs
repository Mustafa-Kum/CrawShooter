using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Components")]
    [Space]
    [SerializeField] private Button startButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button backButton;
    [SerializeField] private CanvasGroup menuUI;
    [SerializeField] private CanvasGroup controlsUI;
    [SerializeField] private LevelManager levelManager;

    private void Start()
    {
        InitializeUIButtons();
    }

    private void InitializeUIButtons()
    {
        startButton.onClick.AddListener(StartGame);
        controlsButton.onClick.AddListener(SwitchToControlsUI);
        backButton.onClick.AddListener(SwitchToMenuUI);
    }

    private void SwitchUI(CanvasGroup showUI, CanvasGroup hideUI)
    {
        showUI.blocksRaycasts = true;
        showUI.alpha = 1;

        hideUI.blocksRaycasts = false;
        hideUI.alpha = 0;
    }

    private void SwitchToMenuUI()
    {
        SwitchUI(menuUI, controlsUI);
    }

    private void SwitchToControlsUI()
    {
        SwitchUI(controlsUI, menuUI);
    }

    private void StartGame()
    {
        levelManager.LoadFirstLevel();
    }
}
