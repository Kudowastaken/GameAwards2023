using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Button nextLevelButton;
    private Button restartLevelButton;
    [SerializeField] private TMP_Text moveDisplay;
    [SerializeField] private ButtonScript[] buttonScripts;
    [SerializeField] private float textUpdateInterval;

    private void Start()
    {
        Dragableblock.BlockMoves = 0f;
        nextLevelButton = GameObject.FindGameObjectWithTag("NextLevelButton").GetComponent<Button>();
        nextLevelButton.enabled = false;
        nextLevelButton.onClick.AddListener(LoadNextScene);
        restartLevelButton = GameObject.FindGameObjectWithTag("RestartLevelButton").GetComponent<Button>();
        restartLevelButton.onClick.AddListener(ReloadScene);
    }

    private void Update()
    {
        UpdateMovesText();
        CheckForButtons();
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
}
