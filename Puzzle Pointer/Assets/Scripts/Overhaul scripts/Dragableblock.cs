using UnityEngine;
// ReSharper disable InconsistentNaming
public class Dragableblock : MonoBehaviour
{
    [SerializeField] private BoxCollider2D childBoxCollider;
    [SerializeField] private float dragThreshold;
    [SerializeField] private float moveSpeed = 5f;
    
    private Rigidbody2D myRigidBody;
    private BoxCollider2D myBoxCollider;
    
    private bool xMoreThan;
    private bool yMoreThan;
    private bool isMoving;
    private bool mouseReleased = true;
    private Vector2 direction;
    private Vector2 childColliderSizeAtStart;
    private Vector2 difference;
    private Vector2 mouseStartPosition = Vector2.zero;
    
    private static Dragableblock movingBlock;
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
        
        difference = (Vector2)Input.mousePosition - mouseStartPosition;
        xMoreThan = Mathf.Abs(difference.x) > Mathf.Abs(difference.y);
        yMoreThan = Mathf.Abs(difference.y) > Mathf.Abs(difference.x);

        if(xMoreThan && Mathf.Abs(difference.x) < dragThreshold){return;}
        if(yMoreThan && Mathf.Abs(difference.y) < dragThreshold){return;}
        
        if( xMoreThan && difference.x > 0) {direction = new Vector2(1, 0); isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation; myBoxCollider.size = new Vector2(1f, 0.5f); childBoxCollider.size = new Vector2(childColliderSizeAtStart.x, childColliderSizeAtStart.y / 2); myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse); movingBlock = this;}
        else if(xMoreThan && difference.x < 0) {direction = new Vector2(-1, 0); isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation; myBoxCollider.size = new Vector2(1f, 0.5f); childBoxCollider.size = new Vector2(childColliderSizeAtStart.x, childColliderSizeAtStart.y / 2); myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse); movingBlock = this;}
        else if(yMoreThan && difference.y > 0) {direction = new Vector2(0, 1);isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation; myBoxCollider.size = new Vector2(0.5f, 1f); childBoxCollider.size = new Vector2(childColliderSizeAtStart.x / 2f, childColliderSizeAtStart.y); myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse); movingBlock = this;}
        else if (yMoreThan && difference.y < 0) { direction = new Vector2(0, -1); isMoving = true; myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation; myBoxCollider.size = new Vector2(0.5f, 1f); childBoxCollider.size = new Vector2(childColliderSizeAtStart.x / 2f, childColliderSizeAtStart.y); myRigidBody.AddForce(direction * moveSpeed, ForceMode2D.Impulse); movingBlock = this;}
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
        }
    }
}
