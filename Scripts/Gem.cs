using UnityEngine;

public class Gem : MonoBehaviour
{
    public float rewardValue = 0.5f; // Reward value for collecting the gem

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify the player that a gem has been collected
            // You can also emit an event or use another method to notify the player
            // For now, simply destroy the gem
            Destroy(gameObject);
        }
    }
}