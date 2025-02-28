using System.Collections.Generic;
using UnityEngine;


// forward declaration needs to be done to deal with circular dependancy

public class cellBehavior : MonoBehaviour
{
    private bool hasMine, hasPowerUp, empty, revealed, flagged, isGoal, playerOn;
    private int numMines;
    // public Player player;
    // public GameMaster gamesmaster;
    // Start is called before the first frame update
    void Start()
    {
        //sets revealed status to false immediately.
        revealed = false;
        // link the player object to the existing object
        //player = FindObjectOfType<Player>();
        // gamemaster = FindObjectOfType<GameMaster>();

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
            }
            hasMine = false;
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
}     