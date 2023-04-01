using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour, IReset
{
    public void Reset()
    {
        isPressed = buttonStatusAtStart;
        myRenderer.sprite = notPressed;
    }
    
    [SerializeField] private Sprite notPressed;
    [SerializeField] private Sprite pressed;
    [SerializeField] private AudioClip pressSFX;
    [SerializeField] private Dragableblock connectedBlock;

    [SerializeField] private bool isPressed = false;
    public bool IsPressed { get => isPressed; private set => isPressed = value; }
    private SpriteRenderer myRenderer;
    private AudioSource myAudioSource;
    private Dragableblock blockHit;
    [SerializeField] private float buttonTimer;
    private bool buttonTimerOver = false;

    [SerializeField] private bool isHoldMode;
    private List<GameObject> gameObjects = new List<GameObject>();
    private bool buttonStatusAtStart;
    private float buttonTimerAtStart;
    private bool shouldCount;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();

        myRenderer.sprite = notPressed;
        
        ButtonCache();
    }

    private void Update()
    {
        if (shouldCount && buttonTimer <= 0f)
        {
            buttonTimer -= Time.deltaTime;
            if (buttonTimer <= 0f)
            {
                buttonTimerOver = true;
            }
        }

        if (buttonTimerOver)
        {
            Debug.Log("Button has been pressed");
        }
    }

    private void ButtonCache()
    {
        buttonStatusAtStart = isPressed;
        buttonTimerAtStart = buttonTimer;
    }

    public void ButtonModeSingle()
    {
        isHoldMode = false;
        Debug.Log("Button is in single press mode");
    }

    public void ButtonModeHold()
    {
        isHoldMode = true;
        Debug.Log("Button is in hold mode");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DragableBlock"))
        {
            blockHit = other.gameObject.GetComponent<Dragableblock>();
            
            if (blockHit == connectedBlock)
            {
                buttonTimer = buttonTimerAtStart;
                buttonTimerOver = false;
                shouldCount = true;
            }

            /*
            myRenderer.sprite = pressed;
            isPressed = true;
            myAudioSource.clip = pressSFX;
            myAudioSource.Play();
            */
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PushableBlock") || other.CompareTag("DragableBlock"))
        {
            gameObjects.Remove(other.gameObject);
            if (isHoldMode && buttonTimerOver)
            {
                Invoke("ButtonReset", 0.5f);
            }
        }
    }

    private void ButtonReset()
    {
        myRenderer.sprite = notPressed;
        isPressed = false;
    }
}
