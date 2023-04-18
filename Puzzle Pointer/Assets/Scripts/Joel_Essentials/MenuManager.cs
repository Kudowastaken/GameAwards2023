using UnityEngine;
public class MenuManager : MonoBehaviour
{ 
    public void NextScene()
    {
        SceneManagerExtended.LoadNextScene();
    }
    
    public void ReloadScene()
    {
        SceneManagerExtended.ReloadScene();
    }
    
    public void LoadPreviousScene()
    {
        SceneManagerExtended.LoadPreviousScene();
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManagerExtended.LoadScene(sceneIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManagerExtended.LoadScene(0);
        PauseMenu.isPaused = false;
        PauseMenu.Instance.myAnimator.Play("Close");
    }
}
