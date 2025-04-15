using UnityEngine;

public class CameraSystem : MonoBehaviour
{

    private GameObject player;
    public float xMin;
    public float yMin;
    public float xMax;
    public float yMax;

    // This code lets camera follow player wherever it goes.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float x = Mathf.Clamp(player.transform.position.x, xMin, xMax);
        float y = Mathf.Clamp(player.transform.position.y, yMin, yMax);
        gameObject.transform.position = new Vector3 (x, y, gameObject.transform.position.z);
    }
}
