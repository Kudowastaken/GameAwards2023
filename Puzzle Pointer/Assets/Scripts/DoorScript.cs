using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, IReset
{
    public void Reset()
    {
        doorStatusLastFrame = allButtonsPressed;
        myBoxCollider.enabled = true;
        mySpriteRenderer.sprite = closedSprite;
    }

    [SerializeField] private ButtonScript[] buttonTriggers;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private AudioClip openCloseSound;

    [SerializeField] private bool doorStatusLastFrame;
    [SerializeField] private bool allButtonsPressed;

    private BoxCollider2D myBoxCollider;
    private SpriteRenderer mySpriteRenderer;
    private AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        myBoxCollider = GetComponent<BoxCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.clip = openCloseSound;
        mySpriteRenderer.sprite = closedSprite;
    }

    // Update is called once per frame
    void Update()
    {
        DoorStatusSingleButton();
    }

    private void DoorStatusSingleButton()
    {
        if (buttonTriggers == null) { return; }

        allButtonsPressed = true;

        foreach (var button in buttonTriggers)
        {
            if (button.IsPressed == false)
            {
                allButtonsPressed = false;
                break;
            }
        }

        if (allButtonsPressed == doorStatusLastFrame) { return; }

        if (allButtonsPressed)
        {
            myAudioSource.Play();
            myBoxCollider.enabled = false;
            mySpriteRenderer.sprite = openSprite;
        }
        else if (!allButtonsPressed)
        {
            myAudioSource.Play();
            myBoxCollider.enabled = true;
            mySpriteRenderer.sprite = closedSprite;
        }

        doorStatusLastFrame = allButtonsPressed;
    }
}
