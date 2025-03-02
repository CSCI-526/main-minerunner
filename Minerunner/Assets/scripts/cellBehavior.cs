using System.Collections.Generic;
using UnityEngine;


// forward declaration needs to be done to deal with circular dependancy

public class cellBehavior : MonoBehaviour
{
    private bool hasPowerUp, empty, revealed, flagged, isGoal, playerOn;
    public bool hasMine;
    private int numMines;
    // public Player player;
    private gameMaster gameMaster;
    private PlayerBehavior playerBehavior;
    private uiMaster uiMaster;
    // Start is called before the first frame update
    void Start()
    {
        //sets revealed status to false immediately.
        revealed = false;
        gameMaster = FindObjectOfType<gameMaster>();
        playerBehavior = FindObjectOfType<PlayerBehavior>();
        uiMaster = FindObjectOfType<uiMaster>();
        // link the player object to the existing object
        //player = FindObjectOfType<Player>();
        // gamemaster = FindObjectOfType<GameMaster>();

    }

    public bool gethasMine()
    {
        return hasMine;
    }

    // Update is called once per frame
    private void Explode()
        {
            if (playerOn == true)
            {
                 // if (player.getLives() == 0) {
                 //    gamemaster.end() } 
                 // else {
                 //    player.setLives() }

                if (playerBehavior.GetComponent<PlayerBehavior>().getLives() <= 0)
                {
                    gameMaster.GetComponent<gameMaster>().endGame();
                }
                else
                {
                    playerBehavior.GetComponent<PlayerBehavior>().setLives(playerBehavior.GetComponent<PlayerBehavior>().getLives() - 1);
                    uiMaster.GetComponent<uiMaster>().RemoveLife();
                }
                hasMine = false;
            }
            hasMine = true;
            //checks in a range if there are other cells containing mines and explodes them based on distance. 
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
        else if(gameMaster.endCell == gameObject)
        {
            gameMaster.setGoal(true);
            return;
        }
        gameObject.GetComponent<MeshRenderer>().material = gameMaster.revealedMaterial;
    }
}     