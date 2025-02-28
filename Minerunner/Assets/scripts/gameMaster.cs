using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameMaster : MonoBehaviour
{
    public static int totalMines;
    private bool goalReached;
    private bool playerDead;

    public void setMines(int mines)
    {
        totalMines = mines;
    }

    public int getMines()
    {
        return totalMines;
    }

    public bool endGame()
    {
        //Maybe split this into 2 cases later
        if (goalReached || playerDead)
        {
            return true;
        }
        return false;
    }
    // Start is called before the first frame update
    void Start()
    {
        totalMines = 0;
        goalReached = false;
        playerDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Might call getLives from player to set playerDead here
    }
}
