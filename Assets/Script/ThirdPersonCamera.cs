using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;  // Player reference
    public Vector3 offset = new Vector3(0, 2, -5);  // Default offset behind the player
    public float smoothSpeed = 10f;
    public LayerMask collisionMask;  // Only the wall layer

    private Vector3 currentOffset;

    void Start()
    {
        currentOffset = offset;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Desired camera position behind player
        Vector3 desiredPosition = player.position + player.TransformDirection(offset);

        // Check if there's any wall between player and camera
        RaycastHit hit;
        if (Physics.Raycast(player.position, (desiredPosition - player.position).normalized, out hit, offset.magnitude, collisionMask))
        {
            // Obstacle detected: bring camera closer
            float distance = hit.distance * 0.9f; // Slightly before the hit point
            desiredPosition = player.position + (desiredPosition - player.position).normalized * distance;
        }

        // Smooth movement to new position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Always look at player
        Vector3 lookDirection = player.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }
}
