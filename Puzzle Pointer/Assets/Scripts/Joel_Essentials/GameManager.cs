using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Button nextLevelButton;
    private Button restartLevelButton;
    [SerializeField] private TMP_Text moveDisplay;
    [SerializeField] private ButtonScript[] buttonScripts;

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
        moveDisplay.text = $"Moves: {Dragableblock.BlockMoves}";
        CheckForButtons();
    }

    private void CheckForButtons()
    {
        foreach (ButtonScript button in buttonScripts)
        {
            if (!button.IsPressed)
            {
                return;
            }
        }

        nextLevelButton.enabled = true;
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
