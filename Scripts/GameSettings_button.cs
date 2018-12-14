using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
/* Summary
 * GameSettings_button is a class which controls the button related to game settings
 */
public class GameSettings_button : MonoBehaviour
{

    AudioSource buttonClick;
    private static string[] tutorialScripts;
    private static int currentLine = 0;
    private int length;

    void Awake()
    {
        tutorialScripts = Prologue.GetInstance().GetScript("tutorial", GameSettings.GetInstance().GetLanguage());
        this.length = tutorialScripts.Length;
    }

    // on language setting button click, show gamesetting popup
    public void OnGameSettingsButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("GameSettings button clicked!");
        PopUp.GetInstance().ShowPopUp("GameSettings", true);
    }

    // on language setting button click, show language popup
    public void OnLanguageButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("Language button clicked!");
        PopUp.GetInstance().ShowPopUp("Language", true);
    }

    // on All reset button click, close setting popup and reset clover and collection data
    public void OnAllResetButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("All Reset button clicked!");
        PopUp.GetInstance().ShowPopUp("GameSettings", false);
        CloverDirector.GetInstance().Reset();
        Collection.GetInstance().Reset();
    }

    // on gamesettings popup close button click, hide popup
    public void OnGameSettingsPopUpCloseButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("GameSettingPopUp Close button clicked!");
        PopUp.GetInstance().ShowPopUp("GameSettings", false);
    }

    // on language popup close button click, hide popup
    public void OnLanguagePopUpCloseButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("Language PopUp Close button clicked!");
        PopUp.GetInstance().ShowPopUp("Language", false);
    }

    // on tutorial button click, show tutorial popup and load first text
    // text depends on language
    public void OnTutorialButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("Tutorial button clicked!");

        PopUp.GetInstance().ShowPopUp("Tutorial", true);
        tutorialScripts = Prologue.GetInstance().GetScript("tutorial", GameSettings.GetInstance().GetLanguage());
        this.length = tutorialScripts.Length;
        currentLine = 0;
        string T_mark = "<Text>";
        PopUp.GetInstance().FindText("Tutorial").text = tutorialScripts[currentLine].Substring(T_mark.Length);
    }

    // on tutorial next button click, if tutorial script is not last line, load next line of script
    public void OnTutorialNextButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("Tutorial Next button clicked!");
        if (currentLine < this.length - 1)
        {
            string T_mark = "<Text>";
            currentLine += 1;
            PopUp.GetInstance().FindText("Tutorial").text = tutorialScripts[currentLine].Substring(T_mark.Length);
        }
        else
        {
            Debug.Log("Can't get next script");
        }
    }

    // on tutorial close button click, close tutorial popup
    public void OnTutorialPopUpCloseButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("Tutorial PopUp Close button clicked!");
        PopUp.GetInstance().ShowPopUp("Tutorial", false);
    }

    // on music button click, on/off sound
    public void OnMusicOnOffButtonClick()
    {
        GameObject.Find("GameMusic").GetComponent<AudioSource>().mute = !GameObject.Find("GameMusic").GetComponent<AudioSource>().mute;
    }

    // on soundfx button click, on/off sound
    public void OnSoundFxOnOffButtonClick()
    {
        AudioSource rainFx = GameObject.Find("RainSound").GetComponent<AudioSource>();
        rainFx.mute = !rainFx.mute;
        GameObject.Find("ButtonClickSound").GetComponent<AudioSource>().mute = rainFx.mute;
    }

    // on ending popup click, reset clover information
    public void OnEndingPopUpResetClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        CloverDirector.GetInstance().Reset();
        CloverDirector.GetInstance().endingPopupOn = false;
        PopUp.GetInstance().ShowPopUp("Ending", false);
    }

    // on watch prologue button click. move scene to prologue 
    public void OnWatchPrologueButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        SceneManager.LoadScene("PrologueScene");
    }
}