using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class uiMaster : MonoBehaviour
{
    public Image[] lives;  // Assign all GameObjects in the Unity Inspector
    public int livesRemaining;
    //private gameMaster gameMaster;
    public GameObject instructionsPanel;
    public Button instructionsButton;

    public void loseLife()
    {
        livesRemaining --;
        lives[livesRemaining].enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //gameMaster = FindObjectOfType<gameMaster>();

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void restartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void toggleInstructions()
    {
        instructionsPanel.SetActive(!instructionsPanel.activeSelf);
    }

    public void closeInstructions()
    {
        instructionsPanel.SetActive(false);
    }
}
