using UnityEngine;
using UnityEngine.SceneManagement;

public class levelMove : MonoBehaviour
{

    public int levelToChange ; //This will change according to the Build Index of the current scene.

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu"); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player"){
            Debug.Log("Player Touched End Zone. Changing to Level " + (levelToChange));
            levelToChange += 1;
            SceneManager.LoadScene(levelToChange, LoadSceneMode.Single);
        }
    }
}
