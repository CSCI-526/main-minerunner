using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cellBehavior : MonoBehaviour
{
    private bool hasMine, hasPowerUp, empty, revealed, flagged, isGoal, playerOn;
    private int numMines;

    // Start is called before the first frame update
    void Start()
    {
        //sets revealed status to false immediately.
        revealed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reveal()
    {
        revealed = true;
    }

    private void Explode()
    {
        if(revealed == true && hasMine == true)
        {
            if (playerOn == true)
            {

            }
            hasMine = false;
            //checks in a range if there are other cells containing mines and explodes them based on distance.
        }
    }

    private void PowerUp()
    {
        if (revealed == true && hasPowerUp == true)
        {
            if (playerOn == true)
            {
                //calls addPowerup()
            }
            //shows powerup if revealed by detonator
        }
    }

    private void LandOn()
    {
        if (playerOn == true && hasPowerUp == true)
        {
            //calls addPowerup()
        }
    }
}
