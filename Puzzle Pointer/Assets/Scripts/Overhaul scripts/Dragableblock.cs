using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Audio;
// ReSharper disable InconsistentNaming
public class Dragableblock : MonoBehaviour
{
    [SerializeField] private BoxCollider2D childBoxCollider;
    [SerializeField] private float dragThreshold;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float blockMovesUpdateInterval;
    [SerializeField] private AudioClip moveSFX;
    [SerializeField] AudioMixerGroup SFXMixer;

    private Rigidbody2D myRigidBody;
    private BoxCollider2D myBoxCollider;
    private AudioSource myAudioSource;
    private Animator boxAnimator;
    
    private bool xMoreThan;
    private bool yMoreThan;
    private bool isMoving;
    private bool mouseReleased = true;
    private Vector2 direction;
    private Vector2 childColliderSizeAtStart;
    private Vector2 difference;
    private Vector2 mouseStartPosition = Vector2.zero;

    private static Dragableblock movingBlock;
    public static float BlockMoves { get; set; }
    public static bool LevelHasBeenFinished { get; set; }
    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        myAudioSource = GetComponent<AudioSource>();
        boxAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        myRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        childColliderSizeAtStart = childBoxCollider.size;
        myAudioSource.outputAudioMixerGroup = SFXMixer;
    }

    private void OnMouseDown()
    {
        if (LevelHasBeenFinished) { return;}
        mouseStartPosition = Input.mousePosition;
        mouseReleased = false;
    }

    private void OnMouseUp()
    {
        mouseReleased = true;
    }

    private void OnMouseDrag()
    {
        if (isMoving) { return; }
        if (mouseReleased) { return; }
        if (movingBlock != null) { return; }
        if (LevelHasBeenFinished) { return;}
        if (PauseMenu.isPaused) { return; }
        
        difference = (Vector2)Input.mousePosition - mouseStartPosition;
        xMoreThan = Mathf.Abs(difference.x) > Mathf.Abs(difference.y);
        yMoreThan = Mathf.Abs(difference.y) > Mathf.Abs(difference.x);

        if(xMoreThan && Mathf.Abs(difference.x) < dragThreshold){return;}
        if(yMoreThan && Mathf.Abs(difference.y) < dragThreshold){return;}
        
        if( xMoreThan && difference.x > 0) {direction = new Vector2(1, 0); isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation; myBoxCollider.size = new Vector2(1f, 0.5f); childBoxCollider.size = new Vector2(childColliderSizeAtStart.x, childColliderSizeAtStart.y / 2); myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse); movingBlock = this; myAudioSource.clip = moveSFX; myAudioSource.Play(); Invoke(nameof(AddToBlockMovesCount), blockMovesUpdateInterval); boxAnimator.SetBool("IsMovingHorizontally", true); }
        else if(xMoreThan && difference.x < 0) {direction = new Vector2(-1, 0); isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation; myBoxCollider.size = new Vector2(1f, 0.5f); childBoxCollider.size = new Vector2(childColliderSizeAtStart.x, childColliderSizeAtStart.y / 2); myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse); movingBlock = this; myAudioSource.clip = moveSFX; myAudioSource.Play(); Invoke(nameof(AddToBlockMovesCount), blockMovesUpdateInterval); boxAnimator.SetBool("IsMovingHorizontally", true); }
        else if(yMoreThan && difference.y > 0) {direction = new Vector2(0, 1);isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation; myBoxCollider.size = new Vector2(0.5f, 1f); childBoxCollider.size = new Vector2(childColliderSizeAtStart.x / 2f, childColliderSizeAtStart.y); myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse); movingBlock = this; myAudioSource.clip = moveSFX; myAudioSource.Play(); Invoke(nameof(AddToBlockMovesCount), blockMovesUpdateInterval); boxAnimator.SetBool("IsMovingVertically", true); }
        else if (yMoreThan && difference.y < 0) { direction = new Vector2(0, -1); isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation; myBoxCollider.size = new Vector2(0.5f, 1f); childBoxCollider.size = new Vector2(childColliderSizeAtStart.x / 2f, childColliderSizeAtStart.y); myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse); movingBlock = this; myAudioSource.clip = moveSFX; myAudioSource.Play(); Invoke(nameof(AddToBlockMovesCount), blockMovesUpdateInterval); boxAnimator.SetBool("IsMovingVertically", true); }
        else return;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("DragableBlock") || collision.transform.CompareTag("DragableCollider"))
        {
            isMoving = false;
            mouseReleased = true;
            movingBlock = null;
            mouseStartPosition = Vector2.zero;
            direction = Vector2.zero;
            myRigidBody.velocity = Vector2.zero;
            myBoxCollider.size = new Vector2(1, 1);
            childBoxCollider.size = childColliderSizeAtStart;
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            boxAnimator.SetBool("IsMovingHorizontally", false);
            boxAnimator.SetBool("IsMovingVertically", false);
        }
    }

    private void Update()
    {
        StopMoving();
    }

    private void StopMoving()
    {
        if (!isMoving) { return;}
        
        if (myRigidBody.velocity == new Vector2(0, 0) || Mathf.Abs(myRigidBody.velocity.x) < 0.1f && myRigidBody.velocity.y == 0f || Mathf.Abs(myRigidBody.velocity.y) < 0.1f && myRigidBody.velocity.x == 0f)
        {
            isMoving = false;
            mouseReleased = true;
            movingBlock = null;
            mouseStartPosition = Vector2.zero;
            direction = Vector2.zero;
            myRigidBody.velocity = Vector2.zero;
            myBoxCollider.size = new Vector2(1, 1);
            childBoxCollider.size = childColliderSizeAtStart;
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            boxAnimator.SetBool("IsMovingHorizontally", false);
            boxAnimator.SetBool("IsMovingVertically", false);
        }
    }

    private void AddToBlockMovesCount()
    {
        if (!isMoving)
        {
            return;
        }
        
        BlockMoves++;
        
    }
}
