using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, IReset
{
    public void Reset()
    {
        doorStatusLastFrame = buttonTrigger.IsPressed;
        myBoxCollider.enabled = true;
        mySpriteRenderer.sprite = closedSprite;
    }

    [SerializeField] ButtonScript buttonTrigger;
    [SerializeField] Sprite closedSprite;
    [SerializeField] Sprite openSprite;
    [SerializeField] AudioClip openCloseSound;

    private bool doorStatusLastFrame;

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

        doorStatusLastFrame = buttonTrigger.IsPressed;
    }

    // Update is called once per frame
    void Update()
    {
        DoorStatus();
    }

    private void DoorStatus()
    {
        if (buttonTrigger.IsPressed == doorStatusLastFrame) { return; }

        if (buttonTrigger.IsPressed)
        {
            myAudioSource.Play();
            myBoxCollider.enabled = false;
            mySpriteRenderer.sprite = openSprite;
        }
        else if(!buttonTrigger.IsPressed) 
        {
            myAudioSource.Play();
            myBoxCollider.enabled = true;
            mySpriteRenderer.sprite = closedSprite;
        }

        doorStatusLastFrame = buttonTrigger.IsPressed;
    }
}
