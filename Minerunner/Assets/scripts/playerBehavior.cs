using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerCell;
    public GameObject playerCursorPrefab;
    public float cursorHeight;
    public int movementRange;
    //private variables
    private gameMaster gameMaster;
    private GameObject playerCursor = null;
    private GameObject cursorCell;
    private int lives = 3;  //default
    private Dictionary<string, int> inventory = new Dictionary<string, int>(); //powerup-inventory

    //move range
    //private int moveRange = 1;
    void Start()
    {
        gameMaster = FindObjectOfType<gameMaster>();
    }
    // Update is called once per frame

    void Update()
    {
        handlePlayerMovement();
        //handle player movement
    }

    //get player's lives
    public int getLives()
    {
        return lives;
    }

    //set players lives on mine hit
    public void setLives(int newLives)
    {
        lives = Mathf.Max(0, newLives); //to make lives not go negative
        if (lives == 0)
        {
            //FindObjectOfType<GameMaster>().EndGame();
        }
    }

    public void decreaseLives(int damage)
    {
        lives -= damage;
        lives = Mathf.Max(0, lives);
        if (lives == 0)
        {
            gameMaster.playerDead = true;
        }
    }


    //add powerup to inventory
    public void addPowerup(string powerupType)
    {
        if (inventory.ContainsKey(powerupType))
        {
            inventory[powerupType]++;
        }
        else
        {
            inventory[powerupType] = 1;
        }
    }

    private void handlePlayerMovement() {
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
        
        removeObject(playerCursor);
        this.transform.position = cursorCell.transform.position;
        playerCell = cursorCell;
        playerCell.GetComponent<cellBehavior>().reveal();
    }

    private void moveCursor(int direction) {
        // These are directions that correspond to indecies in the cellAdjacencyMap in gameMaster
        //   -1    move to cursor
        //    1    up
        //    3    left
        //    4    right
        //    6    down
        
        if (playerCursor == null) {
            InstantiatePlayerCursor();
        }

        
        Dictionary<GameObject, GameObject[]> cellAdjacencyMap = gameMaster.getCellAdjacencyMap();

        if (cellAdjacencyMap.TryGetValue(cursorCell, out GameObject[] adjacentCells)) {
            Debug.Log("Reached!");
            GameObject targetCell = adjacentCells[direction];

            if (targetCell != null && isInRange(targetCell)) {
                playerCursor.transform.position = new Vector3(targetCell.transform.position.x, cursorHeight, targetCell.transform.position.z);
                cursorCell = targetCell;
            }
        }
    }

    private void removeObject(GameObject obejct) {
        Destroy(obejct);
        obejct = null;
    }

    private void InstantiatePlayerCursor() {
        playerCursor = Instantiate(playerCursorPrefab);
        playerCursor.transform.position = new Vector3(playerCell.transform.position.x, cursorHeight, playerCell.transform.position.z);
        cursorCell = playerCell;
    }

    private bool isInRange(GameObject targetCell) {
        int xDiff = (int) Mathf.Abs(playerCell.transform.position.x - targetCell.transform.position.x);
        int zDiff = (int) Mathf.Abs(playerCell.transform.position.z - targetCell.transform.position.z);
        int totalDiff = xDiff + zDiff;

        return totalDiff <= movementRange;
    }

    /*private void reveal(GameObject cell) {
        cell.GetComponent<MeshRenderer>().material = gameMaster.revealedMaterial;
    }*/


    public void UseDetonator()
    {
        if (inventory.ContainsKey("Detonator") && inventory["Detonator"] > 0)
        {
            inventory["Detonator"]--;
            //implement mine reveal
        }
    }
}