using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IReset
{
    public void Reset()
    {
        transform.position = resetPoint;
        isMovingRooms = false;
        movementDirection = resetMoveInput;
        myRigidBody.velocity = new Vector2(0, 0);
        playerTransform.eulerAngles = resetRotation;
        movementAngle = resetAngle;
        StopAllCoroutines();
    }

    //variables
    [Header("Movement Variables")]
    [SerializeField] public float moveSpeed = 1f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float deceleration = 1f;

    [Tooltip("The value should be between 1 and 1.25, otherwise the movement will break")]
    [SerializeField] private float velocityPower = 1f;
    [SerializeField] private float turnBufferTime = 0.2f;

    //Serialized for debugging purposes
    [Header("Private Variables")]
    [SerializeField]
    public Vector2 movementDirection = Vector2.up;
    [SerializeField] private float movementAngle = 0f;

    [SerializeField] float turnBufferCounter;
    [SerializeField] private int moveInput = 0;
    [SerializeField] private float minVelocityBeforeStop = 0.5f;

    [Header("SFX")]
    [SerializeField] private AudioClip walkSFX;
    [SerializeField] private float SFXSpeed;
    [SerializeField] private float SFXFadeDuration;
    [SerializeField, Range(0f, 1f)] float musicVolume = 1f;

    //Cached variables and components
    private Rigidbody2D myRigidBody;
    private Transform playerTransform;
    private AudioSource myAudioSource;

    private float SFXSpeedAtStart;
    private float volumeAtStart;
    private Coroutine currentFadeRoutine;

    [Header("Public variables")]
    public bool insideDialogue = false;
    public bool isMovingRooms = false;
    public Vector3 resetPoint;
    public Vector2 resetMoveInput;
    public Vector3 resetRotation;
    public float resetAngle;

    private void Start()
    {
        myRigidBody = gameObject.GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        myAudioSource = GetComponent<AudioSource>();
        SFXSpeedAtStart = SFXSpeed;
        SFXSpeed = 0f;
        volumeAtStart = myAudioSource.volume;
    }

    void Update()
    {
        UnityEngine.Debug.Log(myRigidBody.velocity);
        ResetPosition();
        CurrentInputDirection();
        TurnBuffer();
        if (Input.GetKey(KeyCode.W))
        {
            SFX(walkSFX);
        }
        else
        {
            SFXStop();
        }
        if (SFXSpeed >= 0)
        {
            SFXSpeed -= Time.deltaTime;
        }
    }

    private void ResetPosition()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }

    #region Inputs
    private void CurrentInputDirection()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            moveInput = 0;
        }

        if (Input.GetKey(KeyCode.W))
        {
            //moveInput = movementDirection is 2 or 3 ? -1 : 1;
            moveInput = 1;
        }

        if (turnBufferCounter > 0f && myRigidBody.velocity == new Vector2(0, 0))
        {
            turnBufferCounter = 0f;
            movementAngle += 90f;
            if (movementAngle > 360f)
            {
                movementAngle = 90f;
            }
            playerTransform.eulerAngles = new Vector3(0, 0, movementAngle);

            movementDirection = Quaternion.AngleAxis(movementAngle, Vector3.forward) * Vector2.up;
            movementDirection = new Vector2(Mathf.RoundToInt(movementDirection.x), Mathf.RoundToInt(movementDirection.y));
            movementDirection = movementDirection.normalized;
        }
    }

    private void TurnBuffer()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !insideDialogue)
        {
            turnBufferCounter = turnBufferTime;
        }
        else
        {
            turnBufferCounter -= Time.deltaTime;
        }
    }

    #endregion
    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (Mathf.Abs(myRigidBody.velocity.magnitude) < minVelocityBeforeStop && !Input.GetKey(KeyCode.W))
        {
            myRigidBody.velocity = new Vector2(0, 0);
        }

        float targetSpeed = moveInput * moveSpeed;
        float speedDif = targetSpeed - Mathf.Abs(myRigidBody.velocity.magnitude);
        float accelRate = (targetSpeed > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velocityPower) * Mathf.Sign(speedDif);
        myRigidBody.AddForce(movementDirection * movement);
    }

    public IEnumerator PlayerRoomTransition()
    {
        while (isMovingRooms)
        {
            myRigidBody.velocity = movementDirection;

            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator SfxFadeRoutine()
    {
        myAudioSource.Play();

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / SFXFadeDuration;

            myAudioSource.volume = Mathf.Lerp(myAudioSource.volume, musicVolume, t);

            yield return new WaitForEndOfFrame();
        }
        myAudioSource.Stop();
        currentFadeRoutine = null;
    }

    private void SFX(AudioClip sfx)
    {
        if (myAudioSource.isPlaying) { return; }
        if(SFXSpeed >= 0) { return; }

        myAudioSource.volume = volumeAtStart;
        SFXSpeed = SFXSpeedAtStart;
        myAudioSource.clip = sfx;
        myAudioSource.Play();
    }

    private void SFXStop()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            currentFadeRoutine = StartCoroutine(SfxFadeRoutine());
        }
    }
}
