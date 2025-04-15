using NUnit.Framework;
using UnityEngine;

public class Player_Movement_Prot : MonoBehaviour
{
    public int playerSpeed = 10;
    private bool facingRight = false;
    public int jumpForce = 1250;
    private float moveX;
    public bool isRunning;
    private bool isGrounded = true;
    private float playerHitDist = 1.1016f;
    private float playerUpDist = 0.45f;
    public bool endZone = false;

    // public float speed = 0;
    private Animator anim;
    public bool isML;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update(){
        Player_Move ();
        PlayerRaycast();
    }
    
    void Player_Move(){
        //CONTROLS
        moveX = Input.GetAxis("Horizontal"); //movement on x-axis
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            Jump();
        }
        //ANIMATION
        isRunning = Mathf.Abs(moveX) > 0.1f;
        if (isML == false){
            anim.SetBool("isRunning", isRunning);
            
        }
        // anim.SetBool("isRunning", isRunning);
        anim.SetBool("isJumping", !isGrounded);
        anim.SetFloat("yVelocity", GetComponent<Rigidbody2D>().linearVelocity.y);
        //PLAYER DIRECTION
        if(moveX < 0.0f && facingRight == false){
            FlipPlayer();
        }
        else if (moveX > 0.0f && facingRight == true){
            FlipPlayer();
        }
        //PHYSICS
        if (!isML)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = new Vector2(moveX * playerSpeed, rb.linearVelocity.y);
        }



    }

    void Jump(){
        //PLAYER JUMP
        GetComponent<Rigidbody2D>().AddForce (Vector2.up *  jumpForce);
        isGrounded = false;

    }
    void FlipPlayer(){
        //PLAYER FLIP
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("End_Zone")){
            Debug.Log("End Level Reached");
            endZone = true;
        }
    }
    void OnCollisionEnter2D ( Collision2D col)
    {
        // Debug.Log("Player has collided with " + col.collider.name);
        if (col.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    void PlayerRaycast()
    {
        // Ray Up - Destroy Breakable Boxes
        RaycastHit2D rayUp = Physics2D.Raycast(transform.position, Vector2.up);
        if (rayUp.collider != null && rayUp.distance < playerUpDist && rayUp.collider.CompareTag("Breakable_Box"))
        {
            Destroy(rayUp.collider.gameObject);
        }

        // Ray Down - Enemy Kill Check
        RaycastHit2D rayDown = Physics2D.Raycast(transform.position, Vector2.down);
        if (rayDown.collider != null && rayDown.distance < 1.105f && rayDown.collider.CompareTag("Enemy_Slime"))
        {
            // Debug.Log("Touched Slime");
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.up * 300);
            Rigidbody2D enemyRb = rayDown.collider.gameObject.GetComponent<Rigidbody2D>();
            enemyRb.AddForce(Vector2.right * 300);
            enemyRb.gravityScale = 5;
            enemyRb.freezeRotation = false;
            rayDown.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            rayDown.collider.gameObject.GetComponent<Slime_move>().enabled = false;
        }

        // Ray Down - Ground Check
        if (rayDown.collider != null && rayDown.distance < playerHitDist && !rayDown.collider.CompareTag("Enemy_Slime"))
        {
            isGrounded = true;
        }
    }

}