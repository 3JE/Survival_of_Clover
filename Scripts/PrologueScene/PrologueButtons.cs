using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Summary
 * Prologue is a class which manages buttons related to prologuescene
 */
public class PrologueButtons : MonoBehaviour
{
    AudioSource buttonClick;

    // On korean button click, set language to korean, and set collection script into korean and close language popup
    public void OnKoreanButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("Korean Button Click");
        GameSettings.GetInstance().SetLanguage("kor");
        Collection.GetInstance().SetScript();
        PopUp.GetInstance().ShowPopUp("Language", false);
    }


    // On english button click, set language to english, and set collection script into english and close language popup
    public void OnEnglishButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("English Button Click");
        GameSettings.GetInstance().SetLanguage("eng");
        Collection.GetInstance().SetScript();
        PopUp.GetInstance().ShowPopUp("Language", false);
    }

    // On Prev button click, decrease current script number
    public void OnPrevButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("Prev Button Click");
        Prologue.GetInstance().SetCurrentScript(-1);
    }

    // On Next button click, increase current script number
    public void OnNextButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("Next Button Click");
        Prologue.GetInstance().SetCurrentScript(1);
    }

    // On Yes button click, set answer as Yes
    public void OnYesButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("YES Button Click");
        Prologue.GetInstance().GetAnswer("<Yes>");
    }

    // On No button click, set answer as No and MoveMower..
    public void OnNoButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("NO Button Click");
        Prologue.GetInstance().GetAnswer("<No>");
        MowerController mowerController = GameObject.Find("LawnMower").GetComponent<MowerController>();
        mowerController.MoveMower();
    }

    // On GameStartButton click, scene change
    public void OnGameStartButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("GameStart Button Click");
        SceneManager.LoadScene("GameScene");
    }

    // On Quit button click, quit game
    public void OnQuitButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("Quit Button Click");
        Application.Quit();
    }
}
