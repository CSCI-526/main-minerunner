using System.Collections;
using System.Collections.Generic;


using UnityEngine;




public class playerBehavior : MonoBehaviour


public class PlayerBehavior : MonoBehaviour

{


    // Start is called before the first frame update


    //private variables


    private int lives = 3;  //default


    private Dictionary<string, int> inventory = new Dictionary<string, int>(); //powerup-inventory





    //move range


    //private int moveRange = 1;




    void Start()

    {




    }




    // Update is called once per frame

    void Update()

    {





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





    public void usePowerup()


    {





    }





    public void movePlayer()


    {


        //movement logic


        // If cell has power-up, collect it


        //if (targetCell.HasPowerUp)


        //{


        //    AddPowerup();             }


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