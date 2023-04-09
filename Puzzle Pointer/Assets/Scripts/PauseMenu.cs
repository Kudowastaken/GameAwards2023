using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : SingletonPersistent<PauseMenu>
{
    public static bool isPaused;
    GameObject pauseMenu;
    private void Start()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused == false && Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            isPaused = true;
        }
        else if (isPaused == true && Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(false);
            isPaused = false;
        }
    }
}
