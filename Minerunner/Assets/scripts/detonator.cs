using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detonator : MonoBehaviour
{
    private gameMaster gameMaster;
    private playerMovement playerMovement;
    private PlayerBehavior playerBehavior;
    private GameObject detonatorCursor = null;
    private GameObject cursorCell;
    private bool detonatorActive = false;


    public GameObject detonatorPanel;
    public GameObject detonatorCursorPrefab;
    private float cursorHeight = 1f;

    void Start()
    {
        gameMaster = FindObjectOfType<gameMaster>();
        playerMovement = FindObjectOfType<playerMovement>();
        playerBehavior = FindObjectOfType<PlayerBehavior>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && playerBehavior.inventory["Detonator"] > 0)
        {
            ActivateDetonator();
        }

        if (detonatorActive)
        {
            HandleDetonatorInput();
        }
    }

    private void ActivateDetonator()
    {
        if (detonatorActive)
            return;
        playerMovement.canMove = false;
        detonatorActive = true;
        InstantiateDetonatorCursor();
    }

    private void InstantiateDetonatorCursor()
    {
        detonatorCursor = Instantiate(detonatorCursorPrefab);
        cursorCell = playerMovement.playerCell; // Start from player position
        detonatorCursor.transform.position = new Vector3(cursorCell.transform.position.x, cursorHeight, cursorCell.transform.position.z);
    }

    private void HandleDetonatorInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) MoveCursor(1); // Up
        if (Input.GetKeyDown(KeyCode.A)) MoveCursor(3); // Left
        if (Input.GetKeyDown(KeyCode.D)) MoveCursor(4); // Right
        if (Input.GetKeyDown(KeyCode.S)) MoveCursor(6); // Down

        if (Input.GetKeyDown(KeyCode.Return)) RevealSelectedTile();
    }

    private void MoveCursor(int direction)
    {
        GameObject targetCell = cursorCell.GetComponent<cellBehavior>().getNeighbours()[direction];

        if (targetCell != null)
        {
            detonatorCursor.transform.position = new Vector3(targetCell.transform.position.x, cursorHeight, targetCell.transform.position.z);
            cursorCell = targetCell;
        }
    }

    private void RevealSelectedTile()
    {
        if (cursorCell != null)
        {
            cursorCell.GetComponent<cellBehavior>().reveal();
        }

        Destroy(detonatorCursor);
        detonatorActive = false;
        playerMovement.canMove = true;
        playerBehavior.usePowerup("Detonator");

    }



    public void togglePanel()
    {
        detonatorPanel.SetActive(!detonatorPanel.activeSelf);

    }


    public void closeInstructions()
    {
        detonatorPanel.SetActive(false);
    }

}
