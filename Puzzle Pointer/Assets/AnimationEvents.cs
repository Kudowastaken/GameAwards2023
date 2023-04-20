using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AnimationEvents : MonoBehaviour
{
    public static bool hasWatchedTutorial = false;
    private Animator myAnimator;
    private static readonly int Opening = Animator.StringToHash("Opening");

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void NextScene()
    {
        if (hasWatchedTutorial)
        {
            SceneManagerExtended.LoadScene(2);
        }
        else
        {
            SceneManagerExtended.LoadNextScene();
        }
    }

    public void PlayOpenAnim()
    {
        myAnimator.SetBool(Opening, true);
    }
}