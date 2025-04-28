using UnityEngine;

public class teleportPoint : MonoBehaviour
{
    public Transform endPoint;
    private playerMovement playerMovementScript;
    private cursorBehaviour cursorScript;
    public AudioClip teleportSFXClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovementScript = other.GetComponent<playerMovement>();
            cursorScript = playerMovementScript.getCursor();

            if (playerMovementScript != null && cursorScript != null)
            {
                GameObject endCell = endPoint.gameObject;
                SoundFXManager.instance.PlaySoundFXCLip(teleportSFXClip, this.transform, 0.09f);
                playerMovementScript.TeleportToCell(endCell);
            }
        }
    }
}
