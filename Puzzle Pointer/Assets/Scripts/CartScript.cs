using System;
using UnityEngine;

public class CartScript : MonoBehaviour
{
    private PushTheBlockPLS carryObject;
    private DropBlock dropZone;
    private AudioSource myAudioSource;
    [SerializeField] private AudioClip pickUpSFX;

    private void Start()
    {
        dropZone = GetComponentInChildren<DropBlock>();
        myAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (carryObject != null && !dropZone.ShouldDropBlock)
        {
            carryObject.transform.position = transform.position;
            carryObject.transform.rotation = transform.rotation;
        }
        else if (dropZone.ShouldDropBlock && carryObject != null)
        {
            myAudioSource.clip = pickUpSFX;
            myAudioSource.Play();
            carryObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            carryObject = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PushableBlock"))
        {
            carryObject = other.GetComponent<PushTheBlockPLS>();
            myAudioSource.clip = pickUpSFX;
            myAudioSource.Play();
        }
    }
}
