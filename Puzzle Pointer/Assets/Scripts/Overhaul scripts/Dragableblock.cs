using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragableblock : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 difference = Vector2.zero;
    private Vector2 moveDirection = Vector2.zero;
    private bool isMoving = false;
    private Rigidbody2D myRigidBody;

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        difference = (Vector2)Input.mousePosition - (Vector2)transform.position;
    }

    private void OnMouseDrag()
    {
        if (isMoving) { return; }
        moveDirection = (Vector2)Input.mousePosition - difference;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            isMoving = false;
            difference = Vector2.zero;
            moveDirection = Vector2.zero;
            myRigidBody.velocity = Vector2.zero;
        }
    }

    private void Update()
    {
        Debug.Log(isMoving);
        Movement();
    }

    private void Movement()
    {
        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            switch (moveDirection.normalized.x)
            {
                case 0:
                    difference = Vector2.zero;
                    break;
                case 1:
                    difference = new Vector2(1, 0);
                    isMoving = true;
                    break;
                case -1:
                    difference = new Vector2(-1, 0);
                    isMoving = true;
                    break;
            }
        }
        else if(Mathf.Abs(moveDirection.x) < Mathf.Abs(moveDirection.y))
        {
            switch (moveDirection.normalized.y)
            {
                case 0:
                    difference = Vector2.zero;
                    break;
                case 1:
                    difference = new Vector2(0, 1);
                    isMoving = true;
                    break;
                case -1:
                    difference = new Vector2(0, -1);
                    isMoving = true;
                    break;
            }
        }
        if (isMoving)
        {
            myRigidBody.AddForce(difference * moveSpeed);
        }
    }
}
