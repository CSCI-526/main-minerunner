using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class leftRight : MonoBehaviour
{
    public Button backButton;
    public Button nextButton;
    private KeyCode back = KeyCode.A;
    private KeyCode next = KeyCode.D;
    public GameObject screenCanvas;
    // Start is called before the first frame update
    private gameMaster gameMaster;
    void Start()
    {
        gameMaster = FindObjectOfType<gameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (screenCanvas.activeInHierarchy)
        {
            gameMaster.isPaused = true;
            if (Input.GetKeyDown(back))
            {
                backButton.onClick.Invoke();
            }
            if (Input.GetKeyDown(next))
            {
                nextButton.onClick.Invoke();
            }
        }
    }
}
