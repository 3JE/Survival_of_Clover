using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Summary
 * PopUp is a class which manages existing popups(and their text, Image, button component) in current scene 
 */
public class PopUp : MonoBehaviour
{

    private static PopUp myPopUp; // to use singleton popup

    private CanvasGroup[] PopUps; // all popups in that scene
    private Button[] Buttons; // all buttons in that scene
    private int n_OpenedPopUps = 0; // stores number of opened popups
    private string playerName = string.Empty; // temporary variables for save player name from user input

    // when scene is loaded, hide all popup
    public void Awake()
    {
        // find all popups and buttons : required to access them when they are inactivated
        PopUps = FindObjectsOfType<CanvasGroup>();
        Buttons = FindObjectsOfType<Button>();

        // when successfully find popups and buttons
        if (PopUps != null && Buttons != null)
        {
            Debug.Log("Successfully get all popups and buttons");
            // close all popups
            foreach (CanvasGroup popup in PopUps)
            {
                ShowPopUp(popup, false);
            }
            Debug.Log("Successfully close all popups");
        }
    }

    // return singleton PopUp
    public static PopUp GetInstance()
    {
        if (myPopUp == null)
        {
            myPopUp = FindObjectOfType<PopUp>();
            if (myPopUp == null)
            {
                GameObject container = new GameObject("PopUps");
                myPopUp = container.AddComponent<PopUp>();
            }
        }
        return myPopUp;
    }

    // find popup in current scene
    public CanvasGroup FindPopUp(string type)
    {
        if (PopUps != null)
        {
            foreach (CanvasGroup popup in PopUps)
            {
                if (popup.name == type + "PopUp") // if that popup is exist
                {
                    return popup;
                }
            }
        }
        Debug.Log("fail to get " + type + "PopUp");
        return null;
    }

    // find text of specific popup
    public Text FindText(string type)
    {
        foreach (CanvasGroup popup in PopUps)
        {
            if (popup.name == type + "PopUp")
                return popup.transform.Find(type + "Text").GetComponent<Text>();
        }
        return null;
    }

    // find image of specific popup
    public Image FindImage(string type)
    {
        foreach (CanvasGroup popup in PopUps)
        {
            if (popup.name == type + "PopUp")
                return popup.transform.Find(type + "Image").GetComponent<Image>();
        }
        return null;
    }

    // show popup 
    public void ShowPopUp(string type, bool shouldShow)
    {
        // if Ending popup, close all popups first.
        if (type == "Ending")
        {
            foreach (CanvasGroup popup in PopUps)
            {
                if (popup.name != "EndingPopUp") ShowPopUp(popup, false);
            }
        }

        // find popup in current scene
        CanvasGroup PopWindow = FindPopUp(type);
        if (PopWindow != null) // if that popup is exist
        {
            PopWindow.gameObject.SetActive(shouldShow); // activate or inactivate

            // calculate current opened popups
            if (shouldShow) n_OpenedPopUps++;
            else n_OpenedPopUps = n_OpenedPopUps >= 1 ? n_OpenedPopUps - 1 : 0;

            //sorting order settings according to opened popups
            PopWindow.GetComponent<Canvas>().sortingOrder = n_OpenedPopUps;
            if (type == "Bug")
            {
                Text BugText = PopWindow.transform.Find("BugText").GetComponent<Text>();
                if (GameSettings.GetInstance().GetLanguage() == "kor") BugText.text = "곤충을 불러오기 위해 에너지를 사용하겠습니까?";
                else BugText.text = "Are you going to generate molecules to attract bugs?";
            }

            // inactivate other buttons while popup window is opened(only for gameScene)
            if (SceneManager.GetActiveScene().name == "GameScene") InactivateOtherButtons(type, shouldShow);
        }
    }

    // overloaded version of showpopup
    public void ShowPopUp(CanvasGroup PopWindow, bool shouldShow)
    {
        string popupname = PopWindow.name;
        string suffix = "PopUp";
        string type = popupname.Substring(0, popupname.Length - suffix.Length);
        ShowPopUp(type, shouldShow);
    }

    // show buttons according to button name
    public void ShowButton(string name, bool shouldShow)
    {
        // if that button is in current scene
        foreach (Button button in Buttons)
        {
            if (button.name == name + "Button") button.gameObject.SetActive(shouldShow);
        }
    }

    // inactivate buttons except current popup's button
    public void InactivateOtherButtons(string type, bool shouldShow)
    {
        // inactivate all buttons in current scene
        foreach (Button button in Buttons)
        {
            button.interactable = !shouldShow;
        }

        // find current opened popup, and activate their child buttons
        CanvasGroup p = FindPopUp(type);
        int iCount = p.transform.childCount;
        for (int i = 0; i < iCount; i++)
        {
            Transform trChild = p.transform.GetChild(i);
            if (trChild.gameObject.name.Contains("Button"))
            {
                trChild.gameObject.GetComponent<Button>().interactable = shouldShow;
            }
        }
    }

    // show name popup
    public async Task ShowNamePopUp()
    {
        // Wait a bit before showing the popup.
        // This just helps the player experience feel less jarring & some sync error.
        await Task.Delay(1000);
        ShowPopUp("Name", true);

        // Wait until the player enters a name and clicks submit.
        // OnSubmitButtonClicked will set the playerName.
        while (playerName == string.Empty)
        {
            await Task.Delay(100);
        }
        ShowPopUp("Name", false);

        await UpdateNewNameAsync();
    }

    // update name!
    public async Task UpdateNewNameAsync()
    {
        await Task.Delay(100);
        Debug.Log("update name as " + playerName);
        CloverDirector.GetInstance().SetName(playerName);
        playerName = string.Empty; // clear temporary player name 
    }

    // get name from user input
    public void SetName()
    {
        InputField nameInputField = GameObject.Find("NameInputField").GetComponent<InputField>(); // to type user input(name)
        if (nameInputField.text != null) playerName = nameInputField.text;
        nameInputField.text = string.Empty; // clear nameInputField
    }

}