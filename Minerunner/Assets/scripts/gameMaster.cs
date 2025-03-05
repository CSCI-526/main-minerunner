using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class gameMaster : MonoBehaviour
{
    public static int totalMines;
    [SerializeField] public GameObject[] cells;  // Manually assigned thorugh Unity
    public GameObject startCell;
    public GameObject endCell;
    public GameObject winPanel;
     private bool hasSentData = false;
    public GameObject losePanel;
    public GameObject[] numberPrefabs;
    public float numberHeight;
    sendToGoogle google;
    [SerializeField] public Material revealedMaterial;
    [SerializeField] public Material startMaterial;
    [SerializeField] public Material endMaterial;
    [SerializeField] public Material hiddenMaterial;

    public bool playerDead;
    public bool goalReached;
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
        TrySendingData();
    }
    else if (playerDead && !losePanel.activeSelf)
    {
        losePanel.SetActive(true); // Show lose screen
        TrySendingData();
    }
}

private void TrySendingData()
{
    if (!hasSentData) // Prevents multiple sends to avoid the http 429 error
    {
        hasSentData = true;
        StartCoroutine(DelayedSend());
    }
}

private IEnumerator DelayedSend()
{
    yield return new WaitForSeconds(1); // Give UI time to update because the screen was not visible
    Debug.Log("Sending data to Google..."); 
    google.Send(playerDead, goalReached); // Now send the data
}


    //Awake() is called before Start()
    void Awake()
    {
        createCellAdjacencyMap();
    }

    // Start is called before the first frame update
    void Start()
    {
        google = FindObjectOfType<sendToGoogle>(); 
        if (google == null)
        {
            Debug.LogError("sendToGoogle script not found");
        }
        totalMines = 0;
        goalReached = false;
        playerDead = false;
        startCell.GetComponent<MeshRenderer>().material = startMaterial; // should be replaced with reveal function in future
        endCell.GetComponent<MeshRenderer>().material = endMaterial;
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
            cell.GetComponent<MeshRenderer>().material = hiddenMaterial;
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
}
