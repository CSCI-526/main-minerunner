using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class uiMaster : MonoBehaviour
{
    public Image[] lives;  // Assign all GameObjects in the Unity Inspector
    public int livesRemaining;
    //private gameMaster gameMaster;
    private playerMovement player;
    private double currentMoveRange;
    public GameObject instructionsPanel;
    public Button instructionsButton;
    public GameObject powerupsPanel;
    public TextMeshProUGUI powerupText;
    public GameObject detonatorPanel;
    public GameObject rangeUpPanel;
    public TextMeshProUGUI flagModeText;
    public string nextLevelName;

    public GameObject detonatorFlyIconPrefab; 
    public RectTransform detonatorUIIconTarget;
    public GameObject detonatorSlot; 
    public TextMeshProUGUI detonatorCountText; 
    public Camera mainCam;

    public void loseLife()
    {
        livesRemaining --;
        lives[livesRemaining].enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //gameMaster = FindObjectOfType<gameMaster>();

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void toggleObject(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
        EventSystem.current.SetSelectedGameObject(null);
        //Debug.Log("toggleObject");
    }

    public void updatePowerupPanel(Dictionary<string, int> inventory)
    {
        if (inventory.ContainsKey("Detonator") && inventory["Detonator"] > 0)
        {
       StartCoroutine(ShowDetonatorUIWithDelay(inventory["Detonator"]));
        }
        else
        {
        detonatorSlot.SetActive(false);
        powerupsPanel.SetActive(false);
        }
    }

    public void restartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
  
    public void LoadNextLevel()
    {
        //SceneManager.LoadScene(nextLevelName);
        SceneManager.LoadScene("LevelSelect");
    }

    public void LoadstartLevel()
    {
        //SceneManager.LoadScene(nextLevelName);
        SceneManager.LoadScene("LevelSelectStart");
    }

    public void ShowFlagModeText()
    {
        if (flagModeText != null)
        {
            flagModeText.gameObject.SetActive(true);
        }
    }

    public void HideFlagModeText()
    {
        if (flagModeText != null)
        {
            flagModeText.gameObject.SetActive(false);  // Hide the flag mode text when the flag mode is turned off
        }
    }

    
    public void AnimateDetonatorPickupFromWorld(Vector3 worldPos)
    {
    Vector3 screenPos = mainCam.WorldToScreenPoint(worldPos);

    GameObject icon = Instantiate(detonatorFlyIconPrefab, screenPos, Quaternion.identity, powerupsPanel.transform.parent);
    //detonatorFlyIconPrefab.SetActive(true);
    RectTransform iconRect = icon.GetComponent<RectTransform>();

    StartCoroutine(MoveIconToTarget(iconRect, detonatorUIIconTarget));
    }   

    IEnumerator MoveIconToTarget(RectTransform icon, RectTransform target)
    {
    float duration = 0.7f;
    float time = 0f;

    Vector3 start = icon.position;
    Vector3 end = target.position;

    while (time < duration)
    {
        time += Time.deltaTime;
        float t = time / duration;
        icon.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0, 1, t));
        yield return null;
    }

    icon.position = end;
    Destroy(icon.gameObject);

    }

    IEnumerator ShowDetonatorUIWithDelay(int count)
    {
    yield return new WaitForSeconds(0.6f); 

    powerupsPanel.SetActive(true);
    detonatorSlot.SetActive(true);
    detonatorCountText.text = count.ToString();

    }
}
