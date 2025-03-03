using System.Collections;
using System.Collections.Generic;


using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerCell;
    //private variables
    private gameMaster gameMaster;
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
        if (Input.GetKeyDown(KeyCode.W)) movePlayer(1); // Up
        if (Input.GetKeyDown(KeyCode.A)) movePlayer(3); // Left
        if (Input.GetKeyDown(KeyCode.D)) movePlayer(4); // Right
        if (Input.GetKeyDown(KeyCode.S)) movePlayer(6); // Down
    }

    private void movePlayer(int direction)
    {
        // These are directions that correspond to indecies in the cellAdjacencyMap in gameMaster
        //    1    up
        //    3    left
        //    4    right
        //    6    down


        //movement logic
        Dictionary<GameObject, GameObject[]> cellAdjacencyMap = gameMaster.getCellAdjacencyMap();

        if (cellAdjacencyMap.TryGetValue(playerCell, out GameObject[] adjacentCells)) {
            GameObject targetCell = adjacentCells[direction];

            if (targetCell != null) {
                transform.position = targetCell.transform.position;
                playerCell = targetCell;
                targetCell.GetComponent<cellBehavior>().reveal();
            }
        }

        // If cell has power-up, collect it
        //if (targetCell.HasPowerUp)
        //{
        //    AddPowerup();             }
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