using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Summary
 * BugTimer controlls bug timer on upper left part of GameScene
 * It shows bug image according to clover's bugID
 * and show time left until the bug disappears
 */
public class BugTimer : MonoBehaviour
{
    private static BugTimer bugtimer;

    int timeLeft; //Seconds Overall
    public Text countdown; // UI Text Object to show time left
    public Image bugImage; // UI Image Object to show which bug
    public Sprite[] bugs;  // Sprite arrays for bugs

    void Start()
    {
        StartBugTimer();
        UpdateBugTimer((int)CloverDirector.GetInstance().GetBugID());
    }

    // Return BugTimer Object
    public static BugTimer Getbugtimer()
    {
        if (bugtimer == null)
        {
            bugtimer = FindObjectOfType<BugTimer>();
            if (bugtimer == null)
            {
                GameObject container = new GameObject("BugTimer");
                bugtimer = container.AddComponent<BugTimer>();
            }
        }
        return bugtimer;
    }

    // If time is left and bugID is not -1, start bug timer
    public void StartBugTimer()
    {
        timeLeft = CloverDirector.GetInstance().GetBugTime();
        if (timeLeft < 0)
        {
            timeLeft = 60;
            CloverDirector.GetInstance().SetBugTime(timeLeft);
        }
        if (CloverDirector.GetInstance().GetBugID() != -1) StartCoroutine("BugTimerCoroutine");
        Time.timeScale = 1; //Just making sure that the timeScale is right
    }

    // Update bug timer text and image UIs
    // If no time is left, stop bug timer coroutine
    public void UpdateBugTimer(int bugID)
    {
        print("bugID: " + bugID);
        countdown = GameObject.Find("BugTimerText").GetComponent<Text>();
        bugImage = GameObject.Find("BugTimerImage").GetComponent<Image>();
        if (bugID == -1)
        {
            countdown.text = "";
            bugImage.color = new Color(1f, 1f, 1f, 0f);
            StopCoroutine("BugTimerCoroutine");

        }
        else
        {
            countdown.text = (timeLeft + " sec"); //Showing the Score on the Canvas
            bugImage.color = new Color(1f, 1f, 1f, 1f);
            bugImage.sprite = bugs[bugID];
        }
    }
    //Simple Coroutine
    IEnumerator BugTimerCoroutine()
    {
        while (true)
        {
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                timeLeft = CloverDirector.GetInstance().GetBugTime();
                yield return new WaitForSeconds(1);
                timeLeft--;
                if (timeLeft < 0)
                {
                    CloverDirector.GetInstance().SetBugID(-1);
                }
                UpdateBugTimer((int)CloverDirector.GetInstance().GetBugID());
                CloverDirector.GetInstance().SetBugTime(timeLeft);
                if (CloverDirector.GetInstance().GetBugID() == 3)
                {
                    if (CloverDirector.GetInstance().GetStat("energy") > 0)
                    {
                        CloverDirector.GetInstance().SetStat("energy", -1);
                    }
                    else
                    {
                        CloverDirector.GetInstance().SetStat("water", -1);
                    }
                }
            }
            else yield return null;
        }
    }
}
