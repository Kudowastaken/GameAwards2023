using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : SingletonPersistent<PauseMenu>
{
    public static bool isPaused;
    GameObject pauseMenu;
    public Animator myAnimator;
    public static bool cantPause;
     
    [SerializeField] Button mainMenuButton;

    [SerializeField] float animationDuration = 0.5f;

    float animationSpeed = 1f;

    private void Start()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        isPaused = false;
        myAnimator = GetComponent<Animator>();

        animationSpeed = 1 / animationDuration;
        myAnimator.speed = animationSpeed;
        myAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;

        OnLevelWasLoaded(0);
    }

    private void OnLevelWasLoaded(int level)
    {
        mainMenuButton.onClick.AddListener(FindObjectOfType<GameManager>().MainMenu);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            cantPause = false;
        }
        else
        {
            cantPause = true;
        }
        if (cantPause)
        {
            return;
        }
            if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            myAnimator.Play(isPaused ? "Open" : "Close", -1);
            Time.timeScale = isPaused ? 0f : 1f;
        }
    }
}
