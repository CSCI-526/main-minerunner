using UnityEngine;
using System.Collections;

public class teleportPoint : MonoBehaviour
{
    public Transform endPoint; 
    public float moveSpeed = 5f; // change later accordingly
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
                StartCoroutine(MovePlayer(other.transform));
            }
        }
    }

    IEnumerator MovePlayer(Transform player)
    {
        Vector3 startPos = player.position;
        Vector3 endPos = endPoint.position;
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * moveSpeed;
            player.position = Vector3.Lerp(startPos, endPos, time);
            yield return null;
        }

        player.position = endPos; 

        // update cursor and player position
        GameObject endCell = endPoint.gameObject;
        playerMovementScript.playerCell = endCell; 
        cursorScript.setCursorCell(endCell); 

        //reveal end cell after jumping
        cellBehavior endCellBehavior = endPoint.GetComponent<cellBehavior>();
        if (endCellBehavior != null)
        {
            endCellBehavior.reveal();
        }
    }
}
