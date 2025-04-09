using UnityEngine;

public class teleportPoint : MonoBehaviour
{
    public Transform endPoint;
    private playerMovement playerMovementScript;
    private cursorBehaviour cursorScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovementScript = other.GetComponent<playerMovement>();
            cursorScript = playerMovementScript.getCursor();

            if (playerMovementScript != null && cursorScript != null)
            {
                GameObject endCell = endPoint.gameObject;
                playerMovementScript.TeleportToCell(endCell);
            }
        }
    }
}
