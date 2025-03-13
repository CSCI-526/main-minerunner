using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class sendToGoogle : MonoBehaviour
{
    [SerializeField] private string URL;
    private long sessionID;
    private int lives;
    private bool goalReached;
    private bool playerDead;
    private float levelTime;
    private int powerUpsUsed;
    private int cellsRevealed;
    private string levelName;

    playerBehavior player;

    private void Awake() {
        sessionID = DateTime.Now.Ticks; 
    }

    void Start() {
        player = FindObjectOfType<playerBehavior>();
    }

    void Update() { }

    public void Send(bool playerdead, bool goal, float timeTaken, int powerUps, int cells, string level)
    {
        lives = player.getLives();  // Link to the lives of player class
        goalReached = goal;  // From gameMaster
        playerDead = playerdead;  // From gameMaster
        levelTime = timeTaken;
        powerUpsUsed = powerUps;
        cellsRevealed = cells;
        levelName = level;

        // Add delay before sending to prevent spamming
        StartCoroutine(DelayedSend());
    }

    private IEnumerator DelayedSend()
    {
        yield return new WaitForSeconds(2); // Wait 2 seconds before sending
        StartCoroutine(Post(sessionID.ToString(), lives.ToString(), goalReached.ToString(), playerDead.ToString(), levelTime.ToString(), powerUpsUsed.ToString(), cellsRevealed.ToString(), levelName));
    }

    private IEnumerator Post(string sessionNo, string live, string endReached, string playerUnalive, string timeTaken, string powerUps, string cells, string leveln)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1864328021", sessionNo);
        form.AddField("entry.573436752", live);
        form.AddField("entry.2065677343", endReached);
        form.AddField("entry.221428944", playerUnalive);
        form.AddField("entry.300596042", timeTaken);
        form.AddField("entry.979365432", powerUps);
        form.AddField("entry.1869257303", cells);
        form.AddField("entry.1036162266", leveln);

        int maxRetries = 5; // Maximum retry attempts
        int retryCount = 0;
        float waitTime = 2f; // Start with 2 seconds wait time

        while (retryCount < maxRetries)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Form upload completed");
                    yield break; 
                }
                else if (www.responseCode == 429) 
                {
                    Debug.Log($"Rate limited! Retrying in {waitTime} seconds...");
                    yield return new WaitForSeconds(waitTime);
                    waitTime *= 2; // Exponential backoff (2s → 4s → 8s...)
                    retryCount++;
                }
                else
                {
                    Debug.Log("Error: " + www.error);
                    yield break; 
                }
            }
        }

        Debug.LogError("Failed to send data after multiple attempts.");
    }
}
