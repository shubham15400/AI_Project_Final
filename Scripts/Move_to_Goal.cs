using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents;
using UnityEngine;

public class Move_to_Goal : Agent
{
    [SerializeField] private Transform targetTransform;
    public Transform slimeTransform;  // Only set for Level 3

    private Animator anim;
    private bool isGrounded, isJumping;
    private Rigidbody2D rb;
    private float lastXPosition;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(-12.6f, -3.734434f, 0f);
        lastXPosition = transform.localPosition.x;

        if (slimeTransform != null)
            slimeTransform.localPosition = new Vector3(6.4f, -3.75f, 0f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
        sensor.AddObservation(isGrounded ? 1f : 0f);

        if (slimeTransform != null)
            sensor.AddObservation(slimeTransform.localPosition - transform.localPosition);
        else
            sensor.AddObservation(Vector2.zero);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        float jump = actions.ContinuousActions[1];

        float moveSpeed = 10f;
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
        anim.SetBool("isRunning", Mathf.Abs(moveX) > 0.1f);

        if (jump > 0.5f && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 25f);
            isGrounded = false;
            anim.SetBool("isJumping", true);
        }

        // Movement reward
        float movementDelta = transform.localPosition.x - lastXPosition;
        AddReward(movementDelta * 0.05f);  // Encourage rightward movement
        if (movementDelta < 0) AddReward(-0.02f);  // Penalize moving left
        lastXPosition = transform.localPosition.x;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetKey(KeyCode.Space) ? 1f : 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("isJumping", false);
        }

        if (collision.gameObject.CompareTag("Enemy_Slime"))
        {
            Vector2 contactPoint = collision.contacts[0].point;
            Vector2 center = transform.position;

            bool hitFromTop = contactPoint.y < center.y - 0.1f;

            if (hitFromTop)
            {
                Destroy(collision.gameObject);
                AddReward(1.0f);  // Bonus for defeating slime
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 20f); // Bounce up
            }
            else
            {
                AddReward(-1.0f); // Penalty for getting hit
                EndEpisode();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            AddReward(2f);  // Goal reward
            EndEpisode();
        }
        else if (other.TryGetComponent<Death_zone>(out Death_zone deathZone))
        {
            AddReward(-1f);  // Penalty for falling
            EndEpisode();
        }
    }
}
