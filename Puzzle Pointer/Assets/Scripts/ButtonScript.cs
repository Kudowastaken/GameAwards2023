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

    private bool isPressed = false;
    public bool IsPressed { get => isPressed; private set => isPressed = value; }
    private SpriteRenderer myRenderer;
    private AudioSource myAudioSource;

    [SerializeField] private bool isHoldMode;
    private List<GameObject> gameObjects = new List<GameObject>();
    private bool buttonStatusAtStart;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();

        myRenderer.sprite = notPressed;
        
        ButtonCache();
    }

    private void ButtonCache()
    {
        buttonStatusAtStart = isPressed;
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
        if (other.CompareTag("Player") || other.CompareTag("PushableBlock"))
        {
            gameObjects.Add(other.gameObject);

            if (isPressed) { return; }

            myRenderer.sprite = pressed;
            isPressed = true;
            myAudioSource.clip = pressSFX;
            myAudioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PushableBlock"))
        {
            gameObjects.Remove(other.gameObject);
            if (isHoldMode && gameObjects.Count == 0)
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
