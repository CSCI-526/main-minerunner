using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    public Button closeButton;
    public GameObject screenCanvas;
    public GameObject Movement;
    public GameObject Mines;
    public GameObject FlagMode;
    public GameObject Range;
    public GameObject Detonator;
    //public GameObject worldCanvas;
    private gameMaster gameMaster;

    void Start()
    {
        gameMaster = FindObjectOfType<gameMaster>();
    }

    void Update()
    {
        if (screenCanvas.activeInHierarchy) {
            gameMaster.isPaused = true;

            if (Input.GetKeyDown(KeyCode.Return)) {
                closeButton.onClick.Invoke();
                Debug.Log("ENTER!");
                return;
            }
       
        }
    }

    public void toggleTutorialPanel() {
        screenCanvas.SetActive(!screenCanvas.activeSelf);
        Debug.Log("Unpause!");
        //worldCanvas.SetActive(!worldCanvas.activeSelf);
        gameMaster.isPaused = false;
    }

    public void toggleMovementPanel()
    {
        Movement.SetActive(!Movement.activeSelf);
        //worldCanvas.SetActive(!worldCanvas.activeSelf);
        //gameMaster.isPaused = false;
    }

    public void toggleFlagModePanel()
    {
        FlagMode.SetActive(!FlagMode.activeSelf);
        //worldCanvas.SetActive(!worldCanvas.activeSelf);
        //gameMaster.isPaused = false;
    }

    public void toggleMinesPanel()
    {
        Mines.SetActive(!Mines.activeSelf);
        //worldCanvas.SetActive(!worldCanvas.activeSelf);
        //gameMaster.isPaused = false;
    }

    public void toggleRangePanel()
    {
        Range.SetActive(!Range.activeSelf);
        //worldCanvas.SetActive(!worldCanvas.activeSelf);
        //gameMaster.isPaused = false;
    }

    public void toggleDetonatorPanel()
    {
        Detonator.SetActive(!Detonator.activeSelf);
        //worldCanvas.SetActive(!worldCanvas.activeSelf);
        //gameMaster.isPaused = false;
    }


}