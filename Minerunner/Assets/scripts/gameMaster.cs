using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class gameMaster : MonoBehaviour
{
    public static int totalMines;
    public GameObject[] cells;  // Manually assigned thorugh Unity
    public GameObject startCell;
    public Material revealedMaterial;
    private bool goalReached;
    private bool playerDead;
    private Dictionary<Vector3, GameObject> cellPositionMap = new Dictionary<Vector3, GameObject>(); // Location of all cells
    private Dictionary<GameObject, GameObject[]> cellAdjacencyMap = new Dictionary<GameObject, GameObject[]>();
    
    public Dictionary<GameObject, GameObject[]> getCellAdjacencyMap() {
        return cellAdjacencyMap;
    }

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
        startCell.GetComponent<MeshRenderer>().material = revealedMaterial; // should be replaced with reveal function in future
        createCellAdjacencyMap();
    }

    // Update is called once per frame
    void Update()
    {
        //Might call getLives from player to set playerDead here
    }

    // function to create adjacency map for grid
    private void createCellAdjacencyMap() {

        //store locaiton of all cells
        foreach (GameObject cell in cells) {
            cellPositionMap[cell.transform.position] = cell;
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

            cellAdjacencyMap[cell] = adjacentCells;
        }
        
        // print cellAdjacencyMap to test
        foreach (var entry in cellAdjacencyMap) {
            Debug.Log($"Cell: {entry.Key.name} -> Neighbors: " + 
            string.Join(", ", entry.Value.Select(n => n ? n.name : "null")));
        }
    }
}
