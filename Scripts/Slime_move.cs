using UnityEngine;
using UnityEngine.SceneManagement;

public class Slime_move : MonoBehaviour
{
    public int Enemyspeed = 2;
    public int xMoveDirection = -1;
    private bool facingRight = false;


    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast (transform.position, new Vector2 (xMoveDirection, 0));
        gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2 (xMoveDirection, 0) * Enemyspeed;
        if (hit.distance < 0.6f){
            Flip();
            if(hit.collider.tag == "Player"){
                //player death by slime
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single); // reloads the current level.
            }
        }
    }

    void Flip(){
        if (xMoveDirection > 0){
            xMoveDirection = -1;
        }
        else{
            xMoveDirection = 1;
        }
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
