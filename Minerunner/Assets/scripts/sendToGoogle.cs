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

    PlayerBehavior player;

    private void Awake() {
        sessionID = DateTime.Now.Ticks; 
    }

    void Start() {
        
        player = FindObjectOfType<PlayerBehavior>();
     }

    void Update() { }

    public void Send(bool playerdead, bool goal)
    {
        lives        = player.getLives();  // Link to the lives of player class
        goalReached  = goal;  // From gameMaster
        playerDead   = playerdead;  // From gameMaster

    // Add delay before sending to prevent spamming
         StartCoroutine(DelayedSend());
    }

    private IEnumerator DelayedSend()
    {
        yield return new WaitForSeconds(2); // Wait 2 seconds before sending
        StartCoroutine(Post(sessionID.ToString(), lives.ToString(), goalReached.ToString(), playerDead.ToString()));
    }

    private IEnumerator Post(string sessionNo, string live, string endReached, string playerUnalive)
{
    WWWForm form = new WWWForm();
    form.AddField("entry.1864328021", sessionNo);
    form.AddField("entry.573436752", live);
    form.AddField("entry.2065677343", endReached);
    form.AddField("entry.221428944", playerUnalive);

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
                yield break; // Exit the loop if successful
            }
            else if (www.responseCode == 429) // Rate limit exceeded
            {
                Debug.Log($"Rate limited! Retrying in {waitTime} seconds...");
                yield return new WaitForSeconds(waitTime);
                waitTime *= 2; // Exponential backoff (2s → 4s → 8s...)
                retryCount++;
            }
            else
            {
                Debug.Log("Error: " + www.error);
                yield break; // Exit if a non-429 error occurs
            }
        }
    }

    Debug.LogError("Failed to send data after multiple attempts.");
}

}
