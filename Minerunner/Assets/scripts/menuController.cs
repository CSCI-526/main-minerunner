using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour
{
    public Button[] menuButtons; 
    private int selectedIndex = 0; 

    void Start()
    {
        HighlightButton();
    }

    void Update()
    {
        HandleKeyboardInput();
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) //UP OR W
        {
            selectedIndex = (selectedIndex - 1 + menuButtons.Length) % menuButtons.Length;
            HighlightButton();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) // UP OR S
        {
            selectedIndex = (selectedIndex + 1) % menuButtons.Length;
            HighlightButton();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) // ENTER OR SPACE
        {
            menuButtons[selectedIndex].onClick.Invoke();
            //Debug.Log("TEST!");
        }
    }

    private void HighlightButton()
    {
        foreach (Button btn in menuButtons)
        {
            ColorBlock colors = btn.colors;
            colors.normalColor = Color.white;
            btn.colors = colors;
        }

        ColorBlock selectedColors = menuButtons[selectedIndex].colors;
        selectedColors.normalColor = Color.yellow; //HIGHLIGHT IN YELLOW FOR NOW CAN CHANGE COLOUR LATER
        menuButtons[selectedIndex].colors = selectedColors;
    }
}
