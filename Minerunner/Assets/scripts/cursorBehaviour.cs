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
    private GameObject lastMovementCell;
    public enum CursorMode { Movement, Flagging }
    private CursorMode currentMode = CursorMode.Movement;
    [SerializeField] private GameObject flagModeTextObject; 

    [SerializeField] private Material movementMaterial;
    [SerializeField] private Material flagMaterial;

    private uiMaster uiMaster;  // Reference to the uiMaster script

    void Start()
    {
        gameMaster = FindObjectOfType<gameMaster>();
        uiMaster = FindObjectOfType<uiMaster>();  // Get reference to the UI master
        UpdateCursorVisual();
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle mode
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleMode();
        }

        // Always allow movement
        if (!cursorMoving)
        {
            handleCursorMovement();
        }
        else
        {
            HandleSmoothMovement();
        }

        // Handle flagging only in Flag mode
        if (currentMode == CursorMode.Flagging && Input.GetKeyDown(KeyCode.Return))
        {
            ToggleFlagOnCell();
        }
    }

    private void HandleSmoothMovement()
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

    public CursorMode GetCursorMode()
    {
        return currentMode;
    }

    private void handleCursorMovement()
    {
        if (gameMaster.goalReached || gameMaster.playerDead || gameMaster.isPaused) return;

        if (Input.GetKeyDown(KeyCode.W)) moveCursor(1);
        if (Input.GetKeyDown(KeyCode.A)) moveCursor(3);
        if (Input.GetKeyDown(KeyCode.D)) moveCursor(4);
        if (Input.GetKeyDown(KeyCode.S)) moveCursor(6);
    }

    private void moveCursor(int direction)
    {
        GameObject targetCell = cursorCell.GetComponent<cellBehavior>().getNeighbours()[direction];

        // Restrict by range in Movement mode
        if (targetCell != null)
        {
            bool inMovementMode = currentMode == CursorMode.Movement;
            bool isValidMove = !inMovementMode || player.GetComponent<playerMovement>().isInRange(targetCell);

            if (isValidMove)
            {
                startPos = transform.position;
                endPos = new Vector3(targetCell.transform.position.x, cursorHeight + targetCell.transform.position.y, targetCell.transform.position.z);
                cursorMoving = true;
                cursorCell = targetCell;
            }
        }
    }

    private void ToggleFlagOnCell()
    {
        if (cursorCell != null)
        {
            cellBehavior cb = cursorCell.GetComponent<cellBehavior>();
            if (cb != null)
            {
                cb.setFlagged(!cb.isFlagged());
            }
        }
    }

    private void ToggleMode()
    {
        if (currentMode == CursorMode.Movement)
        {
            lastMovementCell = cursorCell;
            currentMode = CursorMode.Flagging;

            // Show Flag Mode Text from uiMaster
            if (uiMaster != null)
            {
                uiMaster.ShowFlagModeText(); // Call ShowFlagModeText on uiMaster
            }
        }
        else
        {
            currentMode = CursorMode.Movement;

            if (lastMovementCell != null)
            {
                setCursorCell(lastMovementCell);
            }

            // Hide Flag Mode Text from uiMaster
            if (uiMaster != null)
            {
                uiMaster.HideFlagModeText(); // Call HideFlagModeText on uiMaster
            }
        }

        UpdateCursorVisual();
    }

    private void UpdateCursorVisual()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            // Set the material based on the current cursor mode (Movement or Flagging)
            if (currentMode == CursorMode.Movement)
            {
                renderer.material = movementMaterial;  // Movement mode material
            }
            else if (currentMode == CursorMode.Flagging)
            {
                renderer.material = flagMaterial;  // Flag mode material (the flag material)
            }
        }
    }

    public void setPlayer(GameObject obj)
    {
        player = obj;
    }

    public void setCursorCell(GameObject obj)
    {
        cursorCell = obj;
        transform.position = new Vector3(cursorCell.transform.position.x, cursorHeight + cursorCell.transform.position.y, cursorCell.transform.position.z);
    }

    public GameObject getCursorCell()
    {
        return cursorCell;
    }
}
