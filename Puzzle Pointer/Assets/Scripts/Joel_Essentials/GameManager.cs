using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Button nextLevelButton;
    private Button restartLevelButton;
    private Image nextLevelImage;
    private Button mainMenuGameButton;
    [SerializeField] private TMP_Text moveDisplay;
    [SerializeField] private ButtonScript[] buttonScripts;
    [SerializeField] private float textUpdateInterval;

    private void Start()
    {
        Dragableblock.BlockMoves = 0f;
        nextLevelButton = GameObject.FindGameObjectWithTag("NextLevelButton").GetComponent<Button>();
        nextLevelButton.enabled = false;
        nextLevelImage = GameObject.FindGameObjectWithTag("NextLevelButton").GetComponent<Image>();
        nextLevelImage.enabled = false;
        nextLevelButton.onClick.AddListener(LoadNextScene);
        restartLevelButton = GameObject.FindGameObjectWithTag("RestartLevelButton").GetComponent<Button>();
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
                foreach (var dragableBlock in FindObjectsOfType<Dragableblock>())
                {
                    dragableBlock.LevelHasBeenFinished = false;
                }
                return;
            }
        }

        nextLevelButton.enabled = true;
        nextLevelImage.enabled = true;
        foreach (var dragableBlock in FindObjectsOfType<Dragableblock>())
        {
            dragableBlock.LevelHasBeenFinished = true;
        }
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
        PauseMenu.isPaused = false;
        PauseMenu.Instance.myAnimator.Play("Close");
    }
}
