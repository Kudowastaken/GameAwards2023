using System;
using System.Collections;
using System.Threading;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.Audio;

// ReSharper disable InconsistentNaming
public class Dragableblock : MonoBehaviour
{
    [SerializeField] private BoxCollider2D childBoxCollider;
    [SerializeField] private float dragThreshold;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float portalResetDelay;
    [SerializeField] private float blockMovesUpdateInterval;
    [SerializeField] private AudioClip moveSFX;
    [SerializeField] private AudioClip wallHitSFX;
    [SerializeField] private AudioClip portalSFX;
    [SerializeField] private AudioMixerGroup wallMixerGroup;
    [SerializeField] private AudioMixer wallMixer;
    [SerializeField] private AudioMixerGroup SFXMixer;
    [SerializeField] private SpriteRenderer eyesVisualRenderer;
    [SerializeField] private SpriteRenderer glowVisualRenderer;
    [SerializeField] private Sprite startingSprite;
    [SerializeField] private Sprite whenOnButtonSprite;
    [SerializeField] private float shakeIntensity;
    [SerializeField] private float shakeTime;
    [SerializeField] private AudioSource wallHitSource;
    [SerializeField] private AudioSource blockHitSource;
    [SerializeField] private AudioSource portalAudioSource;
    [SerializeField] private float overlapSoundVolume;
    [SerializeField] private SpriteRenderer onMouseHoverSpriteRenderer;

    [Space(10)] [Header("Particles")] [SerializeField]
    private ParticleSystem UpGrassParticle;

    [SerializeField] private ParticleSystem DownGrassParticle;
    [SerializeField] private ParticleSystem LeftGrassParticle;
    [SerializeField] private ParticleSystem RightGrassParticle;
    [SerializeField] private ParticleSystem UpDustParticle;
    [SerializeField] private ParticleSystem DownDustParticle;
    [SerializeField] private ParticleSystem LeftDustParticle;
    [SerializeField] private ParticleSystem RightDustParticle;
    [SerializeField] private ParticleSystem RightWallParticle;
    [SerializeField] private ParticleSystem LeftWallParticle;
    [SerializeField] private ParticleSystem UpWallParticle;
    [SerializeField] private ParticleSystem DownWallParticle;


    private Rigidbody2D myRigidBody;
    private BoxCollider2D myBoxCollider;
    private AudioSource myAudioSource;
    private Animator boxAnimator;
    private SpriteMask mySpriteMask;

    private bool xMoreThan;
    private bool yMoreThan;
    [SerializeField] private bool isMoving;
    private bool mouseReleased = true;
    private bool mouseHover;
    private bool isUsingPortal;
    private bool portalGotSentBack;

    private float collisionTimer
    {
        get { return collisionTimerBacker; }
        set { collisionTimerBacker = value; }
    }

    private float collisionTimerBacker;

    private float insidePortalTimer;
    private Vector2 direction;
    private Vector2 childColliderSizeAtStart;
    private Vector2 difference;
    private Vector2 mouseStartPosition = Vector2.zero;
    private ParticleSystem currentParticleSystem;
    private PortalScript currentPortalScript;
    private PortalScript lastPortalUsed;

    private static Dragableblock movingBlock;
    public static float BlockMoves { get; set; }
    public bool LevelHasBeenFinished { get; set; }
    public bool BlockIsOnButton { get; set; }

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
        eyesVisualRenderer.sprite = whenOnButtonSprite;
    }

    private void Update()
    {
        //Timers
        if (collisionTimer > 0)
        {
            float deltaTime = Time.deltaTime;
            float result = collisionTimer - deltaTime;
            collisionTimer = result;
        }

        if (insidePortalTimer > 0)
        {
            insidePortalTimer -= Time.deltaTime;
        }

        eyesVisualRenderer.sprite = BlockIsOnButton ? whenOnButtonSprite : startingSprite;
        glowVisualRenderer.enabled = BlockIsOnButton;
        if (!LevelHasBeenFinished)
        {
            onMouseHoverSpriteRenderer.enabled = mouseHover && !isMoving;
        }

        if (mouseHover && BlockIsOnButton)
        {
            if (LevelHasBeenFinished)
            {
                return;
            }

            glowVisualRenderer.enabled = false;
            onMouseHoverSpriteRenderer.enabled = true;
        }


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

        blockHitSource.volume = overlapSoundVolume;
        StopMoving();
    }

    private void OnMouseEnter()
    {
        mouseHover = true;
    }

    private void OnMouseExit()
    {
        mouseHover = false;
    }

    private void OnMouseDown()
    {
        if (LevelHasBeenFinished)
        {
            return;
        }

        mouseStartPosition = Input.mousePosition;
        mouseReleased = false;
    }

    private void OnMouseUp()
    {
        mouseReleased = true;
    }

    private void OnMouseDrag()
    {
        if (isMoving)
        {
            return;
        }

        if (mouseReleased)
        {
            return;
        }

        if (movingBlock != null)
        {
            return;
        }

        if (LevelHasBeenFinished)
        {
            return;
        }

        if (PauseMenu.isPaused)
        {
            return;
        }

        if (portalGotSentBack)
        {
            return;
        }

        if (isUsingPortal)
        {
            return;
        }

        difference = (Vector2)Input.mousePosition - mouseStartPosition;
        xMoreThan = Mathf.Abs(difference.x) > Mathf.Abs(difference.y);
        yMoreThan = Mathf.Abs(difference.y) > Mathf.Abs(difference.x);

        if (xMoreThan && Mathf.Abs(difference.x) < dragThreshold)
        {
            return;
        }

        if (yMoreThan && Mathf.Abs(difference.y) < dragThreshold)
        {
            return;
        }

        myAudioSource.outputAudioMixerGroup = SFXMixer;

        if (xMoreThan && difference.x > 0)
        {
            direction = new Vector2(1, 0);
            isMoving = true;
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            myBoxCollider.size = new Vector2(1f, 0.5f);
            childBoxCollider.size = new Vector2(childColliderSizeAtStart.x, childColliderSizeAtStart.y / 2);
            myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse);
            movingBlock = this;
            myAudioSource.clip = moveSFX;
            myAudioSource.Play();
            Invoke(nameof(AddToBlockMovesCount), blockMovesUpdateInterval);
            boxAnimator.SetBool("IsMovingHorizontally", true);
            ParticlePlayer(RightGrassParticle, RightDustParticle);
        }
        else if (xMoreThan && difference.x < 0)
        {
            direction = new Vector2(-1, 0);
            isMoving = true;
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            myBoxCollider.size = new Vector2(1f, 0.5f);
            childBoxCollider.size = new Vector2(childColliderSizeAtStart.x, childColliderSizeAtStart.y / 2);
            myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse);
            movingBlock = this;
            myAudioSource.clip = moveSFX;
            myAudioSource.Play();
            Invoke(nameof(AddToBlockMovesCount), blockMovesUpdateInterval);
            boxAnimator.SetBool("IsMovingHorizontally", true);
            ParticlePlayer(LeftGrassParticle, LeftDustParticle);
        }
        else if (yMoreThan && difference.y > 0)
        {
            direction = new Vector2(0, 1);
            isMoving = true;
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            myBoxCollider.size = new Vector2(0.5f, 1f);
            childBoxCollider.size = new Vector2(childColliderSizeAtStart.x / 2f, childColliderSizeAtStart.y);
            myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse);
            movingBlock = this;
            myAudioSource.clip = moveSFX;
            myAudioSource.Play();
            Invoke(nameof(AddToBlockMovesCount), blockMovesUpdateInterval);
            boxAnimator.SetBool("IsMovingVertically", true);
            ParticlePlayer(UpGrassParticle, UpDustParticle);
        }
        else if (yMoreThan && difference.y < 0)
        {
            direction = new Vector2(0, -1);
            isMoving = true;
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            myBoxCollider.size = new Vector2(0.5f, 1f);
            childBoxCollider.size = new Vector2(childColliderSizeAtStart.x / 2f, childColliderSizeAtStart.y);
            myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse);
            movingBlock = this;
            myAudioSource.clip = moveSFX;
            myAudioSource.Play();
            Invoke(nameof(AddToBlockMovesCount), blockMovesUpdateInterval);
            boxAnimator.SetBool("IsMovingVertically", true);
            ParticlePlayer(DownGrassParticle, DownDustParticle);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.transform.CompareTag("Portal")) return;
        if (portalGotSentBack)
        {
            return;
        }

        currentPortalScript = other.transform.GetComponent<PortalScript>();

        if (lastPortalUsed != null)
        {
            if (currentPortalScript.connectedPortal == lastPortalUsed)
            {
                return;
            }
        }

        lastPortalUsed = currentPortalScript;
        isUsingPortal = true;
        StartCoroutine(BlockUsedPortalRoutine());
        //transform.position = currentPortalScript.connectedPortal.transform.position;
    }

    private void UsingPortalFalse()
    {
        isUsingPortal = false;
    }

    private IEnumerator BlockUsedPortalRoutine()
    {
        myRigidBody.velocity = Vector2.zero;
        myRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        boxAnimator.SetBool("IsMovingHorizontally", false);
        boxAnimator.SetBool("IsMovingVertically", false);
        ParticleStopper(UpGrassParticle, UpDustParticle);
        ParticleStopper(DownGrassParticle, DownDustParticle);
        ParticleStopper(RightGrassParticle, RightDustParticle);
        ParticleStopper(LeftGrassParticle, LeftDustParticle);
        myAudioSource.Stop();
        transform.position = currentPortalScript.transform.position;
        boxAnimator.SetBool("BoxShrink", true);

        portalAudioSource.clip = portalSFX;
        portalAudioSource.Play();

        yield return new WaitForSeconds(0.4f);

        boxAnimator.SetBool("BoxShrink", false);
        transform.position = currentPortalScript.connectedPortal.transform.position;
        StartCoroutine(BlockLeftPortalRoutine());
    }

    private IEnumerator BlockLeftPortalRoutine()
    {
        boxAnimator.SetBool("BoxGrow", true);

        yield return new WaitForSeconds(0.4f);

        boxAnimator.SetBool("BoxGrow", false);
        myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse);
        if (direction == new Vector2(1, 0))
        {
            boxAnimator.SetBool("IsMovingHorizontally", true);
            ParticlePlayer(RightGrassParticle, RightDustParticle);
            myAudioSource.clip = moveSFX;
            myAudioSource.Play();
        }
        else if (direction == new Vector2(-1, 0))
        {
            boxAnimator.SetBool("IsMovingHorizontally", true);
            ParticlePlayer(LeftGrassParticle, LeftDustParticle);
            myAudioSource.clip = moveSFX;
            myAudioSource.Play();
        }
        else if (direction == new Vector2(0, 1))
        {
            boxAnimator.SetBool("IsMovingVertically", true);
            ParticlePlayer(UpGrassParticle, UpDustParticle);
            myAudioSource.clip = moveSFX;
            myAudioSource.Play();
        }
        else if (direction == new Vector2(0, -1))
        {
            boxAnimator.SetBool("IsMovingVertically", true);
            ParticlePlayer(DownGrassParticle, DownDustParticle);
            myAudioSource.clip = moveSFX;
            myAudioSource.Play();
        }

        Invoke(nameof(UsingPortalFalse), portalResetDelay);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isUsingPortal) { return; }
        if (collisionTimer > 0) { return; }
        if (movingBlock != this) { return; }
        
        var wallCollision = collision.transform.CompareTag("Wall");
        var blockCollision = collision.transform.CompareTag("DragableBlock");

        if (wallCollision)
        {
            HandleWallCollision();
        }

        if (blockCollision)
        {
            HandleBlockCollision();
        }
    }

    private void HandleWallCollision()
    {
        currentPortalScript = null;
        lastPortalUsed = null;

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
        ParticleStopper(UpGrassParticle, UpDustParticle);
        ParticleStopper(DownGrassParticle, DownDustParticle);
        ParticleStopper(RightGrassParticle, RightDustParticle);
        ParticleStopper(LeftGrassParticle, LeftDustParticle);
        myAudioSource.Stop();
        if (!blockHitSource.isPlaying)
        {
            Debug.Log("Starting wall collision timer");
            const float collisionDelay = .2f;
            collisionTimer = collisionDelay;
            wallHitSource.clip = wallHitSFX;
            wallHitSource.Play();
        }

        CameraShake.Instance.ShakeCamera(shakeIntensity, shakeTime);
        if (LevelHasBeenFinished)
        {
            return;
        }

        if (xMoreThan && difference.x > 0)
        {
            RightWallParticle.Play();
        }

        if (xMoreThan && difference.x < 0)
        {
            LeftWallParticle.Play();
        }

        if (yMoreThan && difference.y > 0)
        {
            UpWallParticle.Play();
        }

        if (yMoreThan && difference.y < 0)
        {
            DownWallParticle.Play();
        }
    }

    private void HandleBlockCollision()
    {
        currentPortalScript = null;
        lastPortalUsed = null;
        myAudioSource.Stop();
        
        if (!blockHitSource.isPlaying)
        {
            const float collisionDelay = .2f;
            collisionTimer = collisionDelay;
            wallHitSource.clip = wallHitSFX;
            wallHitSource.Play();
            /*
            Debug.Log("Starting block collision timer");
            const float collisionDelay = .2f;
            collisionTimer = collisionDelay;
            blockHitSource.outputAudioMixerGroup = wallMixerGroup;
            blockHitSource.clip = wallHitSFX;
            blockHitSource.volume = overlapSoundVolume;
            blockHitSource.Play();
            Debug.Log(gameObject);
            */
        }
    }

    private void StopMoving()
    {
        if (!isMoving)
        {
            return;
        }

        if (isUsingPortal)
        {
            return;
        }

        if (myRigidBody.velocity == new Vector2(0, 0) ||
            Mathf.Abs(myRigidBody.velocity.x) < 0.1f && myRigidBody.velocity.y == 0f ||
            Mathf.Abs(myRigidBody.velocity.y) < 0.1f && myRigidBody.velocity.x == 0f)
        {
            Debug.Log("Stop moving got called");
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
            ParticleStopper(UpGrassParticle, UpDustParticle);
            ParticleStopper(DownGrassParticle, DownDustParticle);
            ParticleStopper(RightGrassParticle, RightDustParticle);
            ParticleStopper(LeftGrassParticle, LeftDustParticle);
            myAudioSource.Stop();
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

    void ParticlePlayer(ParticleSystem GrassDragBlockParticles, ParticleSystem DustDragBlockParticles)
    {
        currentParticleSystem = GrassDragBlockParticles;
        currentParticleSystem = DustDragBlockParticles;
        if (!GrassDragBlockParticles.isEmitting && !DustDragBlockParticles.isEmitting)
        {
            GrassDragBlockParticles.Play();
            DustDragBlockParticles.Play();
        }
    }

    void ParticleStopper(ParticleSystem GrassDragBlockParticles, ParticleSystem DustDragBlockParticles)
    {
        currentParticleSystem = GrassDragBlockParticles;
        currentParticleSystem = DustDragBlockParticles;

        if (GrassDragBlockParticles.isEmitting && DustDragBlockParticles.isEmitting)
        {
            GrassDragBlockParticles.Stop();
            DustDragBlockParticles.Stop();
        }
    }
}