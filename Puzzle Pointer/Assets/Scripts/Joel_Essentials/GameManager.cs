using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    private UnityEngine.UI.Button nextLevelButton;
    private UnityEngine.UI.Button restartLevelButton;
    private Image nextLevelImage;
    private UnityEngine.UI.Button mainMenuGameButton;
    [SerializeField] private TMP_Text moveDisplay;
    [SerializeField] private ButtonScript[] buttonScripts;
    [SerializeField] private float textUpdateInterval;

    SoundSettings soundSettings;

    private void Start()
    {
        soundSettings = FindObjectOfType<SoundSettings>();
        Dragableblock.BlockMoves = 0f;
        nextLevelButton = GameObject.FindGameObjectWithTag("NextLevelButton").GetComponent<UnityEngine.UI.Button>();
        nextLevelButton.enabled = false;
        nextLevelImage = GameObject.FindGameObjectWithTag("NextLevelButton").GetComponent<Image>();
        nextLevelImage.enabled = false;
        nextLevelButton.onClick.AddListener(LoadNextScene);
        restartLevelButton = GameObject.FindGameObjectWithTag("RestartLevelButton").GetComponent<UnityEngine.UI.Button>();
        restartLevelButton.onClick.AddListener(ReloadScene);
    }

    private void Update()
    {
        GameIsPaused();
        UpdateMovesText();
        CheckForButtons();
    }

    private void GameIsPaused()
    {
        if(PauseMenu.isPaused) 
        {
            nextLevelButton.enabled = false;
            restartLevelButton.enabled = false;
        }
        else
        {
            nextLevelButton.enabled = true;
            restartLevelButton.enabled = true;
        }
    }

    private void UpdateMovesText()
    {
        if (Time.frameCount % textUpdateInterval == 0f)
        {
            moveDisplay.text = Dragableblock.BlockMoves >= 999f ? $"Moves: 999" : $"Moves: {Dragableblock.BlockMoves}";
        }
    }

    private void CheckForButtons()
    {
        foreach (ButtonScript button in buttonScripts)
        {
            if (!button.IsPressed)
            {
                Dragableblock.LevelHasBeenFinished = false;
                return;
            }
        }

        nextLevelButton.enabled = true;
        nextLevelImage.enabled = true;
        Dragableblock.LevelHasBeenFinished = true;
    }

    public void LoadNextScene()
    {
        SceneManagerExtended.LoadNextScene();
    }

    public void ReloadScene()
    {
        Debug.Log("Reloading scene");
        SceneManagerExtended.ReloadScene();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManagerExtended.LoadScene(0);
        PauseMenu.Instance.myAnimator.Play("Close");
        PauseMenu.isPaused = false;
    }
}
