using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private Sprite buttonSprite;
    [SerializeField] private AudioClip pressSFX;
    [SerializeField] private Dragableblock connectedBlock;
    [SerializeField] private bool isPressed = false;
    [SerializeField] private Color32 particleColor;
    [SerializeField] private AudioMixerGroup SFXMixer; 
    public bool IsPressed { get => isPressed; private set => isPressed = value; }
    private SpriteRenderer myRenderer;
    private AudioSource myAudioSource;
    private ParticleSystem myParticleSystem;
    private Dragableblock hit;
    
    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();
        myParticleSystem = GetComponent<ParticleSystem>();

        myRenderer.sprite = buttonSprite;
        myParticleSystem.startColor = particleColor;
        myAudioSource.outputAudioMixerGroup = SFXMixer;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        hit = other.transform.GetComponent<Dragableblock>();
        if (hit == connectedBlock)
        {
            IsPressed = true;
            connectedBlock.BlockIsOnButton = true;
            myAudioSource.clip = pressSFX;
            myAudioSource.Play();
            myParticleSystem.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        hit = other.transform.GetComponent<Dragableblock>();
        if (hit == connectedBlock)
        {
            IsPressed = false;
            connectedBlock.BlockIsOnButton = false;
        }
    }
}
