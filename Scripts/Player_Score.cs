using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;using TMPro;


public class Player_Score : MonoBehaviour
{
    private int currentSceneIndex;
    public TextMeshProUGUI currentSceneUI;
    private float timeLeft = 200;
    public int playerScore = 0;
    public TextMeshProUGUI timeLeftUI;
    public TextMeshProUGUI playerScoreUI;

    void Awake()
    {
        currentSceneIndex  = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame(second)
    void Update()
    {
        timeLeft -= Time.deltaTime; // updates the time every second.
        // Debug.Log(timeLeft);
        if (timeLeftUI == null)
        {
            Debug.LogError("timeLeftUI is NULL! Assign it in the Inspector.");
        }
        timeLeftUI.GetComponent<TextMeshProUGUI>().text = ("Time Left: " + (int)(timeLeft));

        currentSceneUI.GetComponent<TextMeshProUGUI>().text = ("Level: " + (currentSceneIndex));

        playerScoreUI.GetComponent<TextMeshProUGUI>().text = ("Score: " + playerScore);
        if (timeLeft < 0.1f)
        {
            SceneManager.LoadScene(currentSceneIndex); // Reloads the current scene
        }

    }

    void OnTriggerEnter2D(Collider2D trig){
        // Debug.Log("Touched the end of the Level !!!!");
        // The following if statement adds the points when reaching the end of level.
        if (trig.gameObject.name == "EndLevel"){
            CountScore();
        }

        // the following if statement collects the gem, adds the score, and destroys it's object.
        if (trig.gameObject.tag == "Gem"){
            playerScore += 10;
            Destroy(trig.gameObject);
        }
    }
    void CountScore(){
        playerScore += (int)(timeLeft * 10);
        // Debug.Log (playerScore);
    }
}
