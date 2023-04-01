using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private Sprite buttonSprite;
    [SerializeField] private AudioClip pressSFX;
    [SerializeField] private Dragableblock connectedBlock;
    [SerializeField] private bool isPressed = false;
    public bool IsPressed { get => isPressed; private set => isPressed = value; }
    private SpriteRenderer myRenderer;
    private AudioSource myAudioSource;
    
    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();

        myRenderer.sprite = buttonSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.transform.TryGetComponent(out Dragableblock otherBlock) == connectedBlock)
        {
            IsPressed = true;
            myAudioSource.clip = pressSFX;
            myAudioSource.Play();
        }
    }
}
