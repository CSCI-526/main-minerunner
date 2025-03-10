using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorBehaviour : MonoBehaviour
{
    private float cursorHeight = 1f;
    private gameMaster gameMaster;
    private bool cursorMoving = false;
    private float cursorMoveDuration = 0.1f;
    private float elapsedTime = 0f;
    private Vector3 startPos;
    private Vector3 endPos;
    private GameObject cursorCell;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = FindObjectOfType<gameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cursorMoving)
        {
            elapsedTime += Time.deltaTime;
            float movePercent = elapsedTime / cursorMoveDuration;
            transform.position = Vector3.Lerp(startPos, endPos, movePercent);
            if (movePercent >= 1)
            {
                transform.position = endPos;
                cursorMoving = false;
                elapsedTime = 0;
            }
        }
        else
        {
            handleCursorMovement();
        }
    }

    private void handleCursorMovement() {
        if (gameMaster.goalReached || gameMaster.playerDead) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W)) moveCursor(1); // Up
        if (Input.GetKeyDown(KeyCode.A)) moveCursor(3); // Left
        if (Input.GetKeyDown(KeyCode.D)) moveCursor(4); // Right
        if (Input.GetKeyDown(KeyCode.S)) moveCursor(6); // Down
    }

    private void moveCursor(int direction) {
        // These are directions that correspond to indecies in the cellAdjacencyMap in gameMaster
        //   -1    move to cursor
        //    1    up
        //    3    left
        //    4    right
        //    6    down

        GameObject targetCell = cursorCell.GetComponent<cellBehavior>().getNeighbours()[direction];

        if (targetCell != null && player.GetComponent<playerMovement>().isInRange(targetCell)) {
            
            startPos = transform.position;
            endPos = new Vector3(targetCell.transform.position.x, cursorHeight, targetCell.transform.position.z);
            cursorMoving = true;
            cursorCell = targetCell;
        }
    }

    public void setPlayer(GameObject obj)
    {
        player = obj;
    }

    public void setCursorCell(GameObject obj)
    {
        cursorCell = obj;
    }

    public GameObject getCursorCell()
    {
        return cursorCell;
    }
}
