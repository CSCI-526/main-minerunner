using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class uiMaster : MonoBehaviour
{
    public Image[] lives;  // Assign all GameObjects in the Unity Inspector
    public int livesRemaining;
    //private gameMaster gameMaster;
    private playerMovement player;
    private double currentMoveRange;
    public GameObject instructionsPanel;
    public Button instructionsButton;
    public GameObject powerupsPanel;
    public TextMeshProUGUI powerupText;
    public GameObject detonatorPanel;
    public GameObject rangeUpPanel;
    public TextMeshProUGUI flagModeText;
    public string nextLevelName;
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

    public void toggleObject(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }

    public void updatePowerupPanel(Dictionary<string, int> inventory)
    {
        powerupsPanel.SetActive(!powerupsPanel.activeSelf);
        if (inventory.Count > 0)
        {
            powerupText.text = "";

            foreach (var powerup in inventory)
            {
                powerupText.text += powerup.Key + " : " + powerup.Value + "\n";
            }
        }
        else
        {
            powerupsPanel.SetActive(false);
        }
    }

    public void restartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
  
    public void LoadNextLevel()
    {
        //SceneManager.LoadScene(nextLevelName);
        SceneManager.LoadScene("LevelSelectStart");
    }

    public void ShowFlagModeText()
    {
        if (flagModeText != null)
        {
            flagModeText.gameObject.SetActive(true);
        }
    }

    public void HideFlagModeText()
    {
        if (flagModeText != null)
        {
            flagModeText.gameObject.SetActive(false);  // Hide the flag mode text when the flag mode is turned off
        }
    }


}
