using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class gameMaster : MonoBehaviour
{
    public static int totalMines;
    public GameObject startCell;
    public GameObject endCell;
    public GameObject winPanel;
    public GameObject[] empty;
    public GameObject[] portals;
    private bool hasSentData = false;
    public GameObject losePanel;
    public GameObject[] numberPrefabs;
    public float numberHeight;
    
    sendToGoogle google;
    [SerializeField] public Material revealedMaterial;
    [SerializeField] public Material startMaterial;
    [SerializeField] public Material endMaterial;
    [SerializeField] public Material hiddenMaterial;
    [SerializeField] public Material emptyMaterial;
    [SerializeField] public Material portalMaterial;

    public bool playerDead;
    public bool goalReached;
    public bool isPaused = false;
    public bool recursiveReveal;
    public static int currHighestLevel = 0;
    //private static int permaCurrLevel = -1;
    private float levelStartTime;
    public int powerUpsUsed = 0;  
    public int cellsRevealed = 0; 

    public bool isLevelSelect;
    private string levelName;
    //Change this to work with data structure later
    private int levelNum;

    public Dictionary<string, int> levelNameToNum = new Dictionary<string, int>()
    {
        {"LevelSelectStart", -1},
        {"LevelSelect", 0},
        {"Tutorial", 1},
        {"Detonator Level", 2},
        {"FlagsAreKey", 3},
        {"Test Level", 4},
        {"SeekFind", 5},
        {"LongerStride", 6},
        {"Metamorphosis", 7},
        {"StraightForward",8},
        {"SkyScraper", 9 },
        {"Shift", 10 },
        {"Step Up",11 },
    };
    public Dictionary<int, string> levelNumToName = new Dictionary<int, string>()
    {
        {-1, "LevelSelectStart"},
        {0, "LevelSelect"},
        {1, "Tutorial"},
        {2, "Detonator Level"},
        {3, "FlagsAreKey"},
        {4, "Test Level"},
        {5, "SeekFind"},
        {6, "LongerStride" },
        {7, "Metamorphosis"},
        {8, "StraightForward" },
        {9, "SkyScraper" },
        {10,"Shift" },
        {11,"Step Up" },
    };
    private GameObject[] cells;
    private int totalCells;

    private Dictionary<Vector3, GameObject> cellPositionMap = new Dictionary<Vector3, GameObject>(); // Location of all cells

    public void setMines(int mines)
    {
        totalMines = mines;
    }

    public int getMines()
    {
        return totalMines;
    }

    public void setGoal(bool goal)
    {
        goalReached = goal;
    }

    private void handleGameEnd()
    {
        if (goalReached && !winPanel.activeSelf)
        {
            winPanel.SetActive(true); // Show win screen
            if (levelNum > currHighestLevel)
            {
                currHighestLevel = levelNum;
            }
            
            float levelTime = Time.time - levelStartTime;
            TrySendingData(levelTime);
        
        }
        else if (playerDead && !losePanel.activeSelf)
        {
            losePanel.SetActive(true); // Show lose screen
            float levelTime = Time.time - levelStartTime;
            TrySendingData(levelTime);
        }
    }

    private void TrySendingData(float levelTime)
    {
        if (!hasSentData)
        {
            hasSentData = true;
            StartCoroutine(DelayedSend(levelTime));
        }
    }

    private IEnumerator DelayedSend(float levelTime)
    {
        yield return new WaitForSeconds(1); // Give UI time to update because the screen was not visible
        Debug.Log("Sending data to Google..."); 
        google.Send(playerDead, goalReached, levelTime, powerUpsUsed, cellsRevealed,totalCells, levelName);
    }


    //Awake() is called before Start()
    void Awake()
    {
        detectAllCells();
        createCellAdjacencyMap();
    }

    // Start is called before the first frame update
    void Start()
    {
        google = FindObjectOfType<sendToGoogle>(); 
        levelStartTime = Time.time;
        levelName = SceneManager.GetActiveScene().name;
        levelNum = levelNameToNum[levelName];
        if (google == null)
        {
            Debug.LogError("sendToGoogle script not found");
        }
        totalMines = 0;
        goalReached = false;
        playerDead = false;
        startCell.GetComponent<MeshRenderer>().material = startMaterial; // should be replaced with reveal function in future
        //Debug.Log("START");
        endCell.GetComponent<MeshRenderer>().material = endMaterial;
        for (int i = 0; i < empty.Length; i++)
        {
            empty[i].GetComponent<MeshRenderer>().material  = emptyMaterial;
            //Debug.Log("TEST");
        }
        for (int i = 0; i < portals.Length; i++)
        {
            portals[i].GetComponent<MeshRenderer>().material = hiddenMaterial;
            //Debug.Log("TEST");
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleGameEnd();
    }

    // function to create adjacency map for grid
    private void createCellAdjacencyMap() {

        //store location of all cells
        foreach (GameObject cell in cells) {
            cellPositionMap[cell.transform.position] = cell;
            if (!isLevelSelect)
            {
                cell.GetComponent<MeshRenderer>().material = hiddenMaterial;
            }
        }

        // These are our 8 directions, in the order of what they are stored as in the list
        Vector3[] directions = {
            new Vector3(-1, 0, 1),  // Top-Left
            new Vector3(0, 0, 1),   // Up
            new Vector3(1, 0, 1),   // Top-Right
            new Vector3(-1, 0, 0),  // Left
            new Vector3(1, 0, 0),   // Right
            new Vector3(-1, 0, -1), // Bottom-Left
            new Vector3(0, 0, -1),  // Down
            new Vector3(1, 0, -1)   // Bottom-Right
        };

        foreach (GameObject cell in cells) {
            GameObject[] adjacentCells = new GameObject[8];

            // check 8 directions
            for (int i = 0; i < 8; i++) {
                Vector3 direction = directions[i];
                Vector3 neighborPos = cell.transform.position + direction;

                if (cellPositionMap.TryGetValue(neighborPos, out GameObject neighbor) == true) { // add to map if found
                    adjacentCells[i] = neighbor;
                } else { // else add null
                    adjacentCells[i] = null;
                }
            }

            //Assign list of neighbours to every cell
            cell.GetComponent<cellBehavior>().setNeighbours(adjacentCells);
        }
    }

    private void detectAllCells() {
        cells = GameObject.FindGameObjectsWithTag("Cell");
        totalCells = cells.Length; // Store the total number of cells
        Debug.Log("Total Cells: " + totalCells);

        foreach (GameObject cell in cells) {
            if (cell.GetComponent<cellBehavior>() == null) {
                Debug.LogError("cellBehavior is missing on: " + cell.name);
            }
        }
    }

}
