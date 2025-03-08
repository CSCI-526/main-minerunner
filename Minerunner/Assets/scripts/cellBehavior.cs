using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// forward declaration needs to be done to deal with circular dependancy

public class cellBehavior : MonoBehaviour
{
    private bool hasPowerUp, revealed, flagged, isGoal, playerOn;
    public bool hasMine, empty;
    private int numMines;
    private gameMaster gameMaster;
    private PlayerBehavior playerBehavior;
    //private uiMaster uiMaster;
    public GameObject explosionEffect;

    private GameObject[] neighbours;

    // Start is called before the first frame update
    void Start()
    {
        //sets revealed status to false immediately.
        revealed = false;
        gameMaster = FindObjectOfType<gameMaster>();
        playerBehavior = FindObjectOfType<PlayerBehavior>();
        //uiMaster = FindObjectOfType<uiMaster>();
        countMines();
        if (!empty)
        {
            instantiateNumberPrefab();
        }
    }

    public void incrementMineCount() {
        numMines++;
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

            if (playerBehavior.GetComponent<PlayerBehavior>().getLives() > 0)
            {
                playerBehavior.GetComponent<PlayerBehavior>().decreaseLives(1);
            }

             // Remove the Mine
            Transform mineImageTransform = transform.Find("MineImage");
            if (mineImageTransform != null)
            {
                Destroy(mineImageTransform.gameObject, 1f);
            }

            hasMine = false;

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
        if (playerOn == true && hasPowerUp == true)
        {
            //calls player.addPowerup()
        }
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
            gameObject.GetComponent<MeshRenderer>().material = gameMaster.revealedMaterial;

            activateCellItems();
        }
    }

    private void activateCellItems() {
        if (hasMine) {
            Explode();
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
        GameObject cell = gameObject;
        int numMines = cell.GetComponent<cellBehavior>().getNumMines();
        if (numMines == 0) {
            return;
        }

        GameObject numberPrefab = Instantiate(gameMaster.numberPrefabs[numMines - 1]);
        numberPrefab.transform.position = new Vector3(cell.transform.position.x, gameMaster.numberHeight, cell.transform.position.z);
    }
}     