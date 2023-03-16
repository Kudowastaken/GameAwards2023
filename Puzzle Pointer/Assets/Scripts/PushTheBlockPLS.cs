using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushTheBlockPLS : MonoBehaviour
{
    private bool audioSourceLock;

    private Rigidbody2D blockRigidBody;
    private AudioSource movementSFX;

    void Start()
    {
        blockRigidBody = GetComponent<Rigidbody2D>();
        movementSFX = GetComponent<AudioSource>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if ((collision.transform.position.y - 0.1f) <= transform.position.y) //What??
            {
                if (blockRigidBody.velocity.x != 0)
                {
                    if (!audioSourceLock && !movementSFX.isPlaying)
                    {
                        movementSFX.Play();
                    }
                }

                audioSourceLock = true;
            }

            if (collision.gameObject.GetComponent<PlayerMovement>().moveSpeed == 0)
            {
                movementSFX.Stop();
                audioSourceLock = false;
            }
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            movementSFX.Stop();
            audioSourceLock = false;
        }
    }
}
