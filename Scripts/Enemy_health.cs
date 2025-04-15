using UnityEngine;

public class Enemy_health : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    { 
        if (gameObject.transform.position.y < -10){
            Destroy (gameObject); // Enemy object gets destroyed when it goes below -10 units of y axis.
        }
    }

}
