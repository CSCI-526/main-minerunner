using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// forward declaration needs to be done to deal with circular dependancy

public class cellBehavior : MonoBehaviour
{
    private bool isGoal, playerOn;
    public bool hasPowerUp, hasMine, empty, revealed;
    public int powerUp;
    public GameObject powerUpImage;
    private int numMines;
    private gameMaster gameMaster;
    private playerBehavior playerBehavior;
    private playerMovement playerMovement;
    //private detonator detonatorObj;
    private uiMaster uiMaster;
    private GameObject numberPrefab;
    public GameObject explosionEffect;
    public GameObject flagIcon;
    private bool flagged = false;
    private GameObject[] neighbours;

    // Start is called before the first frame update
    void Start()
    {
        //sets revealed status to false immediately.
        revealed = false;
        gameMaster = FindObjectOfType<gameMaster>();
        playerBehavior = FindObjectOfType<playerBehavior>();
        playerMovement = FindObjectOfType<playerMovement>();
        //detonatorObj = FindObjectOfType<detonator>();
        uiMaster = FindObjectOfType<uiMaster>();
        countMines();
        if (!empty)
        {
            instantiateNumberPrefab();
        }
        
        
        if (powerUp == 2 && playerMovement.movementRange >= 2)
        {
            //Debug.Log("Test: " + playerMovement.movementRange);
            uiMaster.toggleObject(powerUpImage);
        }
    }

    public void incrementMineCount() {
        numMines++;
    }

    public void decrementMineCount() {
        numMines--;
    }

    public int getNumMines() {
        return numMines;
    }

    public bool gethasMine()
    {
        return hasMine;
    }

    public void setNeighbours(GameObject[] neighbours)
    {
        this.neighbours = neighbours;
    }

    public GameObject[] getNeighbours()
    {
        return neighbours;
    }

    // Update is called once per frame
    private void Explode()
    {
        if (explosionEffect != null)
        {
            GameObject explosionInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosionInstance, 2f);
        }

            if (playerBehavior.getLives() > 0 && playerOn == true)
            {
                playerBehavior.decreaseLives(1);
            }

             // Remove the Mine
            Transform mineImageTransform = transform.Find("MineImage");
            if (mineImageTransform != null)
            {
                Destroy(mineImageTransform.gameObject, 1f);
            }

            hasMine = false;
            updateNeighborNums();

            Debug.Log("Exploded!");

        //TODO: checks in a range if there are other cells containing mines and explodes them based on distance. 
    }
    private void PowerUp()
    {
        if (playerOn == true)
        {
            //calls player.addPowerup()
        }
        //shows powerup if revealed by detonator
    }
    private void LandOn()
    {
        if (!playerOn)
        {
            //calls player.addPowerup()
        }
    }

    public void setPlayerOn(bool on)
    {
        playerOn = on;
    }

    public void reveal()
{
    if (gameMaster.startCell == gameObject)
    {
        return;
    }
    else if (gameMaster.endCell == gameObject)
    {
        gameMaster.setGoal(true);
        return;
    }
    else if (!empty)
    {
        
        if (!revealed)
        {
            gameMaster.cellsRevealed++; // Track revealed cells
        }

        gameObject.GetComponent<MeshRenderer>().material = gameMaster.revealedMaterial;
        this.revealed = true;

        activateCellItems();

        if (gameMaster.recursiveReveal == true && numMines == 0)
        {
            recursiveReveal(); 
        }
    }
}


    public void recursiveReveal() {
        gameObject.GetComponent<MeshRenderer>().material = gameMaster.revealedMaterial;
        this.revealed = true;

        
        if (numMines > 0) {
            return;
        }

        foreach (GameObject neighbor in neighbours) {
            if (neighbor == null) {
                continue;
            }

            cellBehavior neighborCellBehavior = neighbor.GetComponent<cellBehavior>();

            if (neighborCellBehavior.empty == true
                || neighbor == gameMaster.startCell || neighbor == gameMaster.endCell
                || neighborCellBehavior.revealed == true
                || neighborCellBehavior.gethasMine() == true) {
                continue;
            }
            // Debug.Log(neighborCellBehavior.getNumMines());
            neighborCellBehavior.recursiveReveal();
        }
    }

    private void activateCellItems() 
    {
        if (hasMine) 
        {
            Explode();
        }
        else if (hasPowerUp) 
        {

            if (powerUp == 1) 
            {
                uiMaster.toggleObject(powerUpImage);
                uiMaster.toggleObject(uiMaster.detonatorPanel);
                playerBehavior.addPowerup("Detonator");
            }
            else if (powerUp == 2 && playerMovement.movementRange <= 2)
            {
                //Debug.Log("Test: " + playerMovement.movementRange);
                uiMaster.toggleObject(powerUpImage);
                uiMaster.toggleObject(uiMaster.rangeUpPanel);
                playerMovement.addMoveRange(1);
            }
        
            hasPowerUp = false; // Mark power-up as collected
        }
}


    private void countMines() {
        GameObject cell = gameObject;
        foreach (GameObject neighbor in neighbours) {
            if (neighbor == null) {
                continue;
            }

            if (neighbor.GetComponent<cellBehavior>().gethasMine() == true) {
                cell.GetComponent<cellBehavior>().incrementMineCount();
            }
        }
    }

    private void instantiateNumberPrefab() {
        if (numberPrefab != null) {
            Destroy(numberPrefab);
        }

        int numMines = getNumMines();
        if (numMines == 0) {
            return;
        }

        numberPrefab = Instantiate(gameMaster.numberPrefabs[numMines - 1]);
        numberPrefab.transform.position = new Vector3(this.transform.position.x, gameMaster.numberHeight, this.transform.position.z);
    }

    private void updateNeighborNums() {
        foreach (GameObject neighbor in neighbours) {
            if (neighbor == null || neighbor.GetComponent<cellBehavior>().empty == true) {
                continue;
            }

            neighbor.GetComponent<cellBehavior>().decrementMineCount();
            neighbor.GetComponent<cellBehavior>().instantiateNumberPrefab();
        }
    }

    public void setFlagged(bool value)
    {
        flagged = value;

        if (flagIcon != null)
        {
            flagIcon.SetActive(value); // This toggles the image
        }
    }

    public bool isFlagged()
    {
        return flagged;
    }
}