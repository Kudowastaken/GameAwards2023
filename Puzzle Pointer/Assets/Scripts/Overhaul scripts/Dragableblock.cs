using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dragableblock : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private Vector2 mouseStartPosition = Vector2.zero;
    [SerializeField] private Vector2 moveDirection = Vector2.zero;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private Rigidbody2D myRigidBody;
    [SerializeField] private float dragThreshold;

    public Vector2 difference;
    public bool xMoreThan;
    public bool yMoreThan;

    [SerializeField] private Vector2 direction;
    private BoxCollider2D myBoxCollider;
    [SerializeField] private BoxCollider2D childBoxCollider;
    private Vector2 childColliderSizeAtStart;
    public bool mouseReleased = true;
    public static Dragableblock MovingBlock;
    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        myBoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        childColliderSizeAtStart = childBoxCollider.size;
    }

    private void OnMouseDown()
    {
        mouseStartPosition = Input.mousePosition;// - (Vector2)transform.position;
        moveDirection = Vector2.zero;
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
        if (MovingBlock != null) { return; }
        
        difference = (Vector2)Input.mousePosition - mouseStartPosition;
        xMoreThan = Mathf.Abs(difference.x) > Mathf.Abs(difference.y);
        yMoreThan = Mathf.Abs(difference.y) > Mathf.Abs(difference.x);

        if(xMoreThan && Mathf.Abs(difference.x) < dragThreshold){return;}
        if(yMoreThan && Mathf.Abs(difference.y) < dragThreshold){return;}
        
        if( xMoreThan && difference.x > 0) {direction = new Vector2(1, 0); isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation; myBoxCollider.size = new Vector2(1f, 0.5f); childBoxCollider.size = new Vector2(childColliderSizeAtStart.x, childColliderSizeAtStart.y / 2); myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse); MovingBlock = this;}
        else if(xMoreThan && difference.x < 0) {direction = new Vector2(-1, 0); isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation; myBoxCollider.size = new Vector2(1f, 0.5f); childBoxCollider.size = new Vector2(childColliderSizeAtStart.x, childColliderSizeAtStart.y / 2); myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse); MovingBlock = this;}
        else if(yMoreThan && difference.y > 0) {direction = new Vector2(0, 1);isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation; myBoxCollider.size = new Vector2(0.5f, 1f); childBoxCollider.size = new Vector2(childColliderSizeAtStart.x / 2f, childColliderSizeAtStart.y); myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse); MovingBlock = this;}
        else if (yMoreThan && difference.y < 0) { direction = new Vector2(0, -1); isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation; myBoxCollider.size = new Vector2(0.5f, 1f); childBoxCollider.size = new Vector2(childColliderSizeAtStart.x / 2f, childColliderSizeAtStart.y); myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse); MovingBlock = this;}
        else return;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("DragableBlock") || collision.transform.CompareTag("DragableCollider"))
        {
            isMoving = false;
            mouseReleased = true;
            MovingBlock = null;
            mouseStartPosition = Vector2.zero;
            moveDirection = Vector2.zero;
            direction = Vector2.zero;
            myRigidBody.velocity = Vector2.zero;
            Debug.Log("Collided with" + collision.gameObject);
            myBoxCollider.size = new Vector2(1, 1);
            childBoxCollider.size = childColliderSizeAtStart;
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void Update()
    {
        //StartMovement();
        StopMoving();
    }

    // private void StartMovement()
    // {
    //     if (!isMoving && !mouseReleased) { return;}
    //     if (mouseReleased) { return; }
    //     
    //     Debug.Log("Force gets applied");
    //     myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse);
    // }

    private void StopMoving()
    {
        if (!isMoving) { return;}
        
        if (myRigidBody.velocity == new Vector2(0, 0))
        {
            isMoving = false;
            mouseReleased = true;
            MovingBlock = null;
            mouseStartPosition = Vector2.zero;
            moveDirection = Vector2.zero;
            direction = Vector2.zero;
            myRigidBody.velocity = Vector2.zero;
            myBoxCollider.size = new Vector2(1, 1);
            childBoxCollider.size = childColliderSizeAtStart;
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
