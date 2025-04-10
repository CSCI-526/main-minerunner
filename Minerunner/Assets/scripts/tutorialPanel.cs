using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    public Button closeButton;
    private KeyCode key = KeyCode.Return;
    public GameObject screenCanvas;
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

            if (Input.GetKeyDown(key)) {
                closeButton.onClick.Invoke();
            }
        }
    }

    public void toggleTutorialPanel() {
        screenCanvas.SetActive(!screenCanvas.activeSelf);
        //worldCanvas.SetActive(!worldCanvas.activeSelf);
        gameMaster.isPaused = false;
    }
}