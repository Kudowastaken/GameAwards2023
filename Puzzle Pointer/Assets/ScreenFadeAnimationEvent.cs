using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFadeAnimationEvent : MonoBehaviour
{
    private Animator myAnimator;

    bool canReload;
    private static bool goNextLevel;
    private static int currentSceneIndex = 0;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        Debug.Log(canReload);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // This tells the script to call the function "OnSceneLoaded" when the scene manager detects the scene has finished loading with the parameters of the scene object and the mode of how the scene was loaded
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex >= 2)
        {
            if(goNextLevel)
            {
                FindObjectOfType<AnimationEvents>().playOpeningNoTransAnim();
            }else if(!goNextLevel) 
            {
                ScreenFadeIn();
            }
        }
    }

    // called third
    void Start()
    {

    }

    // called when the game or scene closes
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // This tells the script to stop calling OnSceneLoaded when the scene manager detects a new scene has been loaded
    }

    public void EnableNextLevelBool()
    {
        goNextLevel = true;
    }

    public void DisableNextLevelBool()
    {
        goNextLevel = false;
    }

    private void FadeInWhenInScene()
    {
        ScreenFadeIn();
    }

    public void ScreenFadeOut()
    {
        myAnimator.SetBool("Fading", true);
        myAnimator.SetBool("FadingIn", false);
    }

    public void ScreenFadeIn()
    {
        myAnimator.SetBool("FadingIn", true);
        myAnimator.SetBool("Fading", false);
    }

    public void ReloadSceneFade()
    {
        canReload = true;
        myAnimator.SetBool("NoTransFade", true);
        myAnimator.SetBool("FadingIn", false);
    }

    public void NextScene()
    {
        canReload = false;
        SceneManagerExtended.LoadNextScene();
    }

    public void ReloadScene()
    {
        SceneManagerExtended.ReloadScene();
    }

}
