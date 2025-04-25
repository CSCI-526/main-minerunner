using System;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private float cursorHeight = 1f;
    private gameMaster gameMaster;
    private cursorBehaviour cursorScript;
    public bool playerMoving = false;
    private float playerMoveDuration = 0.2f;
    private float elapsedTime = 0f;
    private Vector3 startPos;
    private Vector3 endPos;

    public static double permaMoveRange;
    public double movementRange = 0;
    public GameObject playerCell;
    public GameObject playerCursorPrefab;

    void Awake()
    {
        gameMaster = FindObjectOfType<gameMaster>();
        if (gameMaster.isLevelSelect && gameMaster.currHighestLevel > 0)
        {
            //Debug.Log("HI: " + (gameMaster.levelCells[gameMaster.currHighestLevel - 1] == null));
            GameObject levelCell = gameMaster.levelCells[gameMaster.currHighestLevel - 1];
            transform.position = levelCell.transform.position;
            playerCell = levelCell;
        }

        instantiatePlayerCursor();
        if (permaMoveRange == 0)
        {
            movementRange = Math.Sqrt(2 * (movementRange * movementRange));
            permaMoveRange = movementRange;
        }
        else
        {
            movementRange = permaMoveRange;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (playerMoving)
        {
            elapsedTime += Time.deltaTime;
            float movePercent = elapsedTime / playerMoveDuration;
            transform.position = Vector3.Lerp(startPos, endPos, movePercent);
            if (movePercent >= 1)
            {
                transform.position = endPos;
                playerMoving = false;
                elapsedTime = 0;
            }
        }
        else
        {
            handlePlayerMovement();
        }
    }

    private void handlePlayerMovement()
    {
        if (gameMaster.goalReached || gameMaster.playerDead)
            return;

        if (Input.GetKeyDown(KeyCode.Return) &&
            cursorScript != null &&
            cursorScript.GetCursorMode() == cursorBehaviour.CursorMode.Movement)
        {
            movePlayer();
        }
    }

    private void movePlayer()
    {
        if (cursorScript == null) return;

        cellBehavior targetCell = cursorScript.getCursorCell().GetComponent<cellBehavior>();

        if (!targetCell.empty && !targetCell.isFlagged())
        {
            startPos = transform.position;
            endPos = targetCell.gameObject.transform.position;
            playerMoving = true;

            playerCell = cursorScript.getCursorCell();
            targetCell.setPlayerOn(true);
            targetCell.reveal();
        }
    }

    private void instantiatePlayerCursor()
    {
        cursorScript = Instantiate(playerCursorPrefab).GetComponent<cursorBehaviour>();
        cursorScript.setPlayer(gameObject);
        cursorScript.transform.position = new Vector3(
            playerCell.transform.position.x,
            cursorHeight,
            playerCell.transform.position.z
        );
        cursorScript.setCursorCell(playerCell);
    }

    public bool isInRange(GameObject targetCell)
    {
        int xDiff = (int)Mathf.Abs(playerCell.transform.position.x - targetCell.transform.position.x);
        int zDiff = (int)Mathf.Abs(playerCell.transform.position.z - targetCell.transform.position.z);
        double totalDiff = Math.Sqrt(xDiff * xDiff + zDiff * zDiff);

        return totalDiff <= movementRange;
    }

    public cursorBehaviour getCursor()
    {
        return cursorScript;
    }

    public void addMoveRange(int i)
    {
        movementRange = Math.Sqrt(movementRange * movementRange / 2);
        movementRange += i;
        movementRange = Math.Sqrt(2 * (movementRange * movementRange));
        permaMoveRange = movementRange;
    }

    //teleport method
    public void TeleportToCell(GameObject destinationCell)
    {
        startPos = transform.position;
        endPos = destinationCell.transform.position;
        playerMoving = true;

        playerCell = destinationCell;

        if (cursorScript != null)
        {
            cursorScript.setCursorCell(destinationCell);
        }

        cellBehavior cell = destinationCell.GetComponent<cellBehavior>();
        if (cell != null)
        {
            cell.setPlayerOn(true);
            cell.reveal();
        }
    }
}
