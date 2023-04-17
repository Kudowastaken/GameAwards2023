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
    [SerializeField] private AudioMixerGroup SFXMixer;
    [SerializeField] private ParticleSystem UpParticle;
    [SerializeField] private ParticleSystem DownParticle;
    [SerializeField] private ParticleSystem LeftParticle;
    [SerializeField] private ParticleSystem RightParticle;

    private Rigidbody2D myRigidBody;
    private BoxCollider2D myBoxCollider;
    private AudioSource myAudioSource;
    private Animator boxAnimator;
    private SpriteMask mySpriteMask;
    
    private bool xMoreThan;
    private bool yMoreThan;
    private bool isMoving;
    private bool mouseReleased = true;
    private Vector2 direction;
    private Vector2 childColliderSizeAtStart;
    private Vector2 difference;
    private Vector2 mouseStartPosition = Vector2.zero;
    private ParticleSystem currentParticleSystem;

    private static Dragableblock movingBlock;
    public static float BlockMoves { get; set; }
    public bool LevelHasBeenFinished { get; set; }
    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        myAudioSource = GetComponent<AudioSource>();
        boxAnimator = GetComponent<Animator>();
        mySpriteMask = GetComponentInChildren<SpriteMask>();
    }

    private void Start()
    {
        myRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        childColliderSizeAtStart = childBoxCollider.size;
        mySpriteMask.enabled = false;
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
        
        if( xMoreThan && difference.x > 0) {direction = new Vector2(1, 0); isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation; myBoxCollider.size = new Vector2(1f, 0.5f); childBoxCollider.size = new Vector2(childColliderSizeAtStart.x, childColliderSizeAtStart.y / 2); myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse); movingBlock = this; myAudioSource.clip = moveSFX; myAudioSource.Play(); Invoke(nameof(AddToBlockMovesCount), blockMovesUpdateInterval); boxAnimator.SetBool("IsMovingHorizontally", true); ParticlePlayer(RightParticle); }
        else if(xMoreThan && difference.x < 0) {direction = new Vector2(-1, 0); isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation; myBoxCollider.size = new Vector2(1f, 0.5f); childBoxCollider.size = new Vector2(childColliderSizeAtStart.x, childColliderSizeAtStart.y / 2); myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse); movingBlock = this; myAudioSource.clip = moveSFX; myAudioSource.Play(); Invoke(nameof(AddToBlockMovesCount), blockMovesUpdateInterval); boxAnimator.SetBool("IsMovingHorizontally", true); ParticlePlayer(LeftParticle); }
        else if(yMoreThan && difference.y > 0) {direction = new Vector2(0, 1);isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation; myBoxCollider.size = new Vector2(0.5f, 1f); childBoxCollider.size = new Vector2(childColliderSizeAtStart.x / 2f, childColliderSizeAtStart.y); myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse); movingBlock = this; myAudioSource.clip = moveSFX; myAudioSource.Play(); Invoke(nameof(AddToBlockMovesCount), blockMovesUpdateInterval); boxAnimator.SetBool("IsMovingVertically", true); ParticlePlayer(UpParticle); }
        else if (yMoreThan && difference.y < 0) { direction = new Vector2(0, -1); isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation; myBoxCollider.size = new Vector2(0.5f, 1f); childBoxCollider.size = new Vector2(childColliderSizeAtStart.x / 2f, childColliderSizeAtStart.y); myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse); movingBlock = this; myAudioSource.clip = moveSFX; myAudioSource.Play(); Invoke(nameof(AddToBlockMovesCount), blockMovesUpdateInterval); boxAnimator.SetBool("IsMovingVertically", true); ParticlePlayer(DownParticle); }
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
            ParticleStopper(UpParticle);
            ParticleStopper(DownParticle);
            ParticleStopper(RightParticle);
            ParticleStopper(LeftParticle);
        }
    }

    private void Update()
    {
        if (LevelHasBeenFinished)
        {
            mySpriteMask.enabled = true;
            boxAnimator.SetBool("IsLevelComplete", true);
        }
        else
        {
            mySpriteMask.enabled = false;
            boxAnimator.SetBool("IsLevelComplete", false);
        }
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
            ParticleStopper(UpParticle);
            ParticleStopper(DownParticle);
            ParticleStopper(RightParticle);
            ParticleStopper(LeftParticle);
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

    void ParticlePlayer(ParticleSystem DragBlockParticles)
    {
        currentParticleSystem = DragBlockParticles;
        if (!DragBlockParticles.isEmitting)
        {
            DragBlockParticles.Play();
        }
    }

    void ParticleStopper(ParticleSystem DragBlockParticles)
    {
        currentParticleSystem = DragBlockParticles;

        if (DragBlockParticles.isEmitting)
        {
            DragBlockParticles.Stop();
        }
    }
}
