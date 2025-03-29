using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;     
    public Vector3 offset;       // customize this per level
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothed;
    }
}
