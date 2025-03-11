using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private float cursorHeight = 1f;
    private gameMaster gameMaster;
    private cursorBehaviour playerCursor;
    public bool playerMoving = false;
    private float playerMoveDuration = 0.2f;
    private float elapsedTime = 0f;
    private Vector3 startPos;
    private Vector3 endPos;
    public double movementRange;
    public GameObject playerCell;
    public GameObject playerCursorPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = FindObjectOfType<gameMaster>();
        movementRange = Math.Sqrt(2*(movementRange * movementRange));
        instantiatePlayerCursor();
    }

    // Update is called once per frame
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

    private void handlePlayerMovement() {
        if (gameMaster.goalReached || gameMaster.playerDead) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Return)) movePlayer();
    }

    private void movePlayer()
    {
        if (playerCursor == null) {
            return;
        }
        
        if (!playerCursor.getCursorCell().GetComponent<cellBehavior>().empty)
        {
            startPos = transform.position;
            endPos = playerCursor.getCursorCell().transform.position;
            playerMoving = true;
            
            playerCell = playerCursor.getCursorCell();
            playerCell.GetComponent<cellBehavior>().setPlayerOn(true);
            playerCell.GetComponent<cellBehavior>().reveal();
        }
    }

    private void instantiatePlayerCursor() {
        playerCursor = Instantiate(playerCursorPrefab).GetComponent<cursorBehaviour>();
        playerCursor.setPlayer(gameObject);
        playerCursor.transform.position = new Vector3(playerCell.transform.position.x, cursorHeight, playerCell.transform.position.z);
        playerCursor.setCursorCell(playerCell);
    }

    public bool isInRange(GameObject targetCell) {
        int xDiff = (int) Mathf.Abs(playerCell.transform.position.x - targetCell.transform.position.x);
        int zDiff = (int) Mathf.Abs(playerCell.transform.position.z - targetCell.transform.position.z);
        double totalDiff = Math.Sqrt(xDiff*xDiff + zDiff*zDiff);

        return totalDiff <= movementRange;
    }
}
