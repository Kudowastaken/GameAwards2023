using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// ReSharper disable InconsistentNaming
public class GameManager : MonoBehaviour
{
    private Button nextLevelButton;
    private Button restartLevelButton;
    private Image nextLevelImage;
    private Button mainMenuGameButton;
    private AudioSource myAudioSource;
    [SerializeField] private TMP_Text moveDisplay;
    [SerializeField] private TMP_Text challengeDisplay;
    [SerializeField] private ButtonScript[] buttonScripts;
    [SerializeField] private float textUpdateInterval;
    [SerializeField] private AudioClip winSFX;
    [SerializeField] private AudioMixerGroup SFXMixer;

    private static bool currentLevelHasBeenFinished = false;
    private bool isLoadingNextScene = false;
    private bool hasPlayedWinSound;

    private void Start()
    {
        Dragableblock.BlockMoves = 0f;
        nextLevelButton = GameObject.FindGameObjectWithTag("NextLevelButton").GetComponent<Button>();
        nextLevelButton.enabled = false;
        nextLevelImage = GameObject.FindGameObjectWithTag("NextLevelButton").GetComponent<Image>();
        nextLevelImage.enabled = false;
        //nextLevelButton.onClick.AddListener(LoadNextScene);
        restartLevelButton = GameObject.FindGameObjectWithTag("RestartLevelButton").GetComponent<Button>();
        //restartLevelButton.onClick.AddListener(ReloadScene);
        isLoadingNextScene = false;
        myAudioSource = GetComponent<AudioSource>();
        hasPlayedWinSound = false;
    }

    private void Update()
    {
        if (!currentLevelHasBeenFinished)
        {
            challengeDisplay.enabled = false;
        }
        else
        {
            challengeDisplay.enabled = true;
        }
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
            moveDisplay.text = Dragableblock.BlockMoves >= 999f ? $"999" : $"{Dragableblock.BlockMoves}";
        }
    }

    private void CheckForButtons()
    {
        if (isLoadingNextScene) { return; }
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
        currentLevelHasBeenFinished = true;
        challengeDisplay.enabled = true;
        if (!hasPlayedWinSound)
        {
            myAudioSource.clip = winSFX;
            myAudioSource.outputAudioMixerGroup = SFXMixer;
            myAudioSource.Play();
            hasPlayedWinSound = true;
        }

        foreach (var dragableBlock in FindObjectsOfType<Dragableblock>())
        {
            dragableBlock.LevelHasBeenFinished = true;
        }
    }

    public void LoadNextScene()
    {
        if (PauseMenu.isPaused)
        {
            return;
        }
        isLoadingNextScene = true;
        SceneManagerExtended.LoadNextScene();
        currentLevelHasBeenFinished = false;
    }

    public void ReloadScene()
    {
        SceneManagerExtended.ReloadScene();
    }
}
