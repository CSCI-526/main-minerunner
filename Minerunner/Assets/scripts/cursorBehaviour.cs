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
    [SerializeField] private float flagTextDuration = 1.5f;
    public Material movementMaterial;
    public Material flagMaterial;

    void Start()
    {
        gameMaster = FindObjectOfType<gameMaster>();
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
        if (gameMaster.goalReached || gameMaster.playerDead) return;

        if (Input.GetKeyDown(KeyCode.W)) moveCursor(1);
        if (Input.GetKeyDown(KeyCode.A)) moveCursor(3);
        if (Input.GetKeyDown(KeyCode.D)) moveCursor(4);
        if (Input.GetKeyDown(KeyCode.S)) moveCursor(6);
    }

    private void moveCursor(int direction)
    {
        GameObject targetCell = cursorCell.GetComponent<cellBehavior>().getNeighbours()[direction];
        //Debug.Log("targetCell: " + targetCell);

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

            ShowFlagModeText();
        }
        else
        {
            currentMode = CursorMode.Movement;

            if (lastMovementCell != null)
            {
                setCursorCell(lastMovementCell);
            }
        }

        UpdateCursorVisual();
    }

    private void ShowFlagModeText()
    {
        if (flagModeTextObject != null)
        {
            flagModeTextObject.SetActive(true);
            CancelInvoke(nameof(HideFlagModeText));
            Invoke(nameof(HideFlagModeText), flagTextDuration);
        }
    }

    private void HideFlagModeText()
    {
        if (flagModeTextObject != null)
        {
            flagModeTextObject.SetActive(false);
        }
    }

    private void UpdateCursorVisual()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = currentMode == CursorMode.Movement ? movementMaterial : flagMaterial;
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
