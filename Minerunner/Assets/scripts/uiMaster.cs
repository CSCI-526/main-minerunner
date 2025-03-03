using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class uiMaster : MonoBehaviour
{
    public GameObject[] cells;  // Assign all GameObjects in the Unity Inspector
    //private gameMaster gameMaster;
    private PlayerBehavior playerBehavior;
    [SerializeField]
    private float liveImageWidth = 100f;

    [SerializeField]
    private int maxNumofLives = 3;

    [SerializeField]
    private int numOfLives = 3;

    private RectTransform rect;

    public UnityEvent OutOfLives;
    public int NumOfLives
    {

        get => playerBehavior.GetComponent<PlayerBehavior>().getLives();
        private set
        {
            if (value < 0)
            {
                OutOfLives?.Invoke();
            }
            numOfLives = playerBehavior.GetComponent<PlayerBehavior>().getLives();
            AdjustImageWidth();

        }
    }
    private void Awake()
    {
        rect = transform as RectTransform;
        AdjustImageWidth();
    }

    private void AdjustImageWidth()
    {
        rect.sizeDelta = new Vector2(x: liveImageWidth * numOfLives, rect.sizeDelta.y);
    }

    public void RemoveLife(int num = 1)
    {
        NumOfLives -= num;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        //gameMaster = FindObjectOfType<gameMaster>();
        playerBehavior = FindObjectOfType<PlayerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
