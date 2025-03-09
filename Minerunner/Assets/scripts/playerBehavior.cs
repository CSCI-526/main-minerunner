using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    //private variables
    private gameMaster gameMaster;
    private uiMaster uiMaster;

    private int lives = 3;  //default
    private Dictionary<string, int> inventory = new Dictionary<string, int>(); //powerup-inventory

    void Start()
    {
        gameMaster = FindObjectOfType<gameMaster>();
        uiMaster = FindObjectOfType<uiMaster>();
    }
    // Update is called once per frame

    void Update()
    {
        
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
        uiMaster.loseLife();
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



    public void UseDetonator()
    {
        if (inventory.ContainsKey("Detonator") && inventory["Detonator"] > 0)
        {
            inventory["Detonator"]--;
            //implement mine reveal
        }
    }
}