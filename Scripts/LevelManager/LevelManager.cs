using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "LevelManager")]

public class LevelManager : ScriptableObject
{
    [SerializeField] private int mainMenuBuildIndex = 0;
    [SerializeField] private int firstLevelBuildIndex = 1;

    public void GoToMainMenu()
    {
        LoadSceneByIndex(mainMenuBuildIndex);
    }

    public void LoadFirstLevel()
    {
        LoadSceneByIndex(firstLevelBuildIndex);
    }

    public void RestartCurrentLevel()
    {
        LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
        GamePlay.SetGamePaused(false);
    }
}
