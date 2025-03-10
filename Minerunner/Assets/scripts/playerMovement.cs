using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private float cursorHeight = 1f;
    private GameObject cursorCell;
    private gameMaster gameMaster;
    private GameObject playerCursor = null;
    private bool cursorMoving = false;
    private bool playerMoving = false;
    private float moveDuration = 2f;
    private float elapsedTime = 0f;
    private Vector3 startPos;
    private Vector3 endPos;
    public double movementRange;
    public GameObject playerCell;
    public GameObject playerCursorPrefab;

    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = FindObjectOfType<gameMaster>();
        movementRange = Math.Sqrt(2*(movementRange * movementRange));
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;
        // if (playerMoving)
        // {
        //     elapsedTime += Time.deltaTime;
        //     float movePercent = elapsedTime / moveDuration;
        //     lerpMove(gameObject, movePercent);
        // }
        // else if (cursorMoving)
        // {
        //     elapsedTime += Time.deltaTime;
        //     float movePercent = elapsedTime / moveDuration;
        //     lerpMove(playerCursor, movePercent);
        // }
        // else
        // {
        //     elapsedTime = 0;
        //     handlePlayerMovement();
        // }
        handlePlayerMovement();
    }

    private void lerpMove(GameObject obj, float movePercent)
    {
        obj.transform.position = Vector3.Lerp(startPos, endPos, movePercent);
    }

    private void handlePlayerMovement() {
        if (gameMaster.goalReached || gameMaster.playerDead) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W)) moveCursor(1); // Up
        if (Input.GetKeyDown(KeyCode.A)) moveCursor(3); // Left
        if (Input.GetKeyDown(KeyCode.D)) moveCursor(4); // Right
        if (Input.GetKeyDown(KeyCode.S)) moveCursor(6); // Down
        if (Input.GetKeyDown(KeyCode.Return)) movePlayer(-1);
    }

    private void movePlayer(int direction)
    {
        if (playerCursor == null) {
            return;
        }
        
        Destroy(playerCursor);
        if (!cursorCell.GetComponent<cellBehavior>().empty)
        {
            this.transform.position = cursorCell.transform.position;
            playerCell = cursorCell;
            playerCell.GetComponent<cellBehavior>().reveal();
        }
    }

    private void moveCursor(int direction) {
        // These are directions that correspond to indecies in the cellAdjacencyMap in gameMaster
        //   -1    move to cursor
        //    1    up
        //    3    left
        //    4    right
        //    6    down
        
        if (playerCursor == null) {
            instantiatePlayerCursor();
        }

        GameObject targetCell = cursorCell.GetComponent<cellBehavior>().getNeighbours()[direction];

        if (targetCell != null && isInRange(targetCell)) {
            playerCursor.transform.position = new Vector3(targetCell.transform.position.x, cursorHeight, targetCell.transform.position.z);
            cursorCell = targetCell;
        }
    }

    private void instantiatePlayerCursor() {
        playerCursor = Instantiate(playerCursorPrefab);
        playerCursor.transform.position = new Vector3(playerCell.transform.position.x, cursorHeight, playerCell.transform.position.z);
        cursorCell = playerCell;
    }

    private bool isInRange(GameObject targetCell) {
        int xDiff = (int) Mathf.Abs(playerCell.transform.position.x - targetCell.transform.position.x);
        int zDiff = (int) Mathf.Abs(playerCell.transform.position.z - targetCell.transform.position.z);
        double totalDiff = Math.Sqrt(xDiff*xDiff + zDiff*zDiff);
        //Debug.Log("totalDiff: " + totalDiff);

        return totalDiff <= movementRange;
    }
}
