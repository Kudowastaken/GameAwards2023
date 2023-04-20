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
    private static readonly int Closing = Animator.StringToHash("Closing");
    private static readonly int OpeningNoTrans = Animator.StringToHash("OpeningNoTrans");

    [SerializeField] private AudioClip openRumbling;
    [SerializeField] private AudioClip closingRumbling;
    [SerializeField] private AudioClip closingImpact;
    [SerializeField] private AudioSource openRumblingSource;
    [SerializeField] private AudioSource closingImpactSource;
    [SerializeField] private AudioSource closingRumblingSource;

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

    public void GetNextLevelBool()
    {
        FindObjectOfType<ScreenFadeAnimationEvent>().DisableNextLevelBool();
    }

    public void PlayClosingAnim()
    {
        myAnimator.SetBool(Closing, true);
    }

    public void playOpeningNoTransAnim()
    {
        myAnimator.SetBool(OpeningNoTrans, true);
    }

    public void Stoptheanim()
    {
        myAnimator.SetBool(OpeningNoTrans, false);
    }


    public void OpeningRumbling()
    {
        openRumblingSource.clip = openRumbling;
        openRumblingSource.Play();
    }

    public void ClosingRumbling()
    {
        closingRumblingSource.clip = closingRumbling;
        closingRumblingSource.Play();
    }

    public void ClosingImpact()
    {
        closingRumblingSource.Stop();
        closingImpactSource.clip = closingImpact;
        closingImpactSource.Play();
    }
}