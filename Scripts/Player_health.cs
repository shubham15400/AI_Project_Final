using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_health : MonoBehaviour
{

    private int health = 1;



    // Update is called once per frame
    void Update()
    { 
        if (health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death_zone")) 
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single); // Reloads Prototype_1 scene;
        
    }

}
