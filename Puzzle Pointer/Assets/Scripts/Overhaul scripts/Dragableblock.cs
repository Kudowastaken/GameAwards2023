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
    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnMouseDown()
    {
        mouseStartPosition = Input.mousePosition;// - (Vector2)transform.position;
        moveDirection = Vector2.zero;
    }

    private void OnMouseDrag()
    {
        if (isMoving) { return; }

        difference = (Vector2)Input.mousePosition - mouseStartPosition;
        xMoreThan = Mathf.Abs(difference.x) > Mathf.Abs(difference.y);
        yMoreThan = Mathf.Abs(difference.y) > Mathf.Abs(difference.x);

        if(xMoreThan && Mathf.Abs(difference.x) < dragThreshold){return;}
        if(yMoreThan && Mathf.Abs(difference.y) < dragThreshold){return;}

        if( xMoreThan && difference.x > 0) {direction = new Vector2(1, 0); isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;}
        else if(xMoreThan && difference.x < 0) {direction = new Vector2(-1, 0); isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;}
        else if(yMoreThan && difference.y > 0) {direction = new Vector2(0, 1);isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;}
        else if (yMoreThan && difference.y < 0) { direction = new Vector2(0, -1); isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;}
        else return;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            isMoving = false;
            mouseStartPosition = Vector2.zero;
            moveDirection = Vector2.zero;
            direction = Vector2.zero;
            myRigidBody.velocity = Vector2.zero;
            Debug.Log("Collided with" + collision);
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (!isMoving) { return;}
        
        myRigidBody.AddForce(direction * moveSpeed);
        
        if (myRigidBody.velocity == new Vector2(0, 0))
        {
            isMoving = false;
            direction = Vector2.zero;
            myRigidBody.velocity = Vector2.zero;
        }
    }
}
