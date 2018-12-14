using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

/* Summary
 * Prologue is a class which manages text scripts and prologue scene
 */
public class Prologue : MonoBehaviour
{
    private static Prologue prologue; // to use singleton prologue
    private string[] script; // prologue scripts
    private int length; // length of prologue
    private int current_script = 0; // variables for storing current script
    private bool needAnswer = false; // boolean variable which determine which popup to appear (Prologue / Answer)
    private string answer = string.Empty; // string variable which stores answer(Yes / No)

    async void Start()
    {
        if (SceneManager.GetActiveScene().name == "PrologueScene")
        {
            Button skipButton = GameObject.Find("SkipButton").GetComponent<Button>();
            
            // close all popups
            PopUp.GetInstance().ShowPopUp("Prologue", false);
            PopUp.GetInstance().ShowPopUp("Answer", false);
            PopUp.GetInstance().ShowPopUp("GameStart", false);
            
            // when language setting needed, set language
            if (GameSettings.GetInstance().GetLanguage() == string.Empty)
            {
                PopUp.GetInstance().ShowPopUp("Language", true);
                skipButton.interactable = false; // since language setting is mendatory, skip is not permitted during language setting
                while (GameSettings.GetInstance().GetLanguage() == string.Empty)
                {
                    await Task.Delay(300);
                }
            }
            // open prologue popup
            PopUp.GetInstance().ShowPopUp("Language", false);
            skipButton.interactable = true;
            PopUp.GetInstance().ShowPopUp("Prologue", true);

            // Get script according to language
            this.script = GetScript("prologue", GameSettings.GetInstance().GetLanguage());
            this.length = this.script.Length;
            
            // Show prologue script
            await ShowScript();
        }
    }

    // return singleton prologue
    public static Prologue GetInstance()
    {
        if (prologue == null)
        {
            prologue = FindObjectOfType<Prologue>();
            if (prologue == null)
            {
                GameObject container = new GameObject("PopUps");
                prologue = container.AddComponent<Prologue>();
            }
        }
        return prologue;
    }

    // before using this, check the encoding of .txt file is UTF-8
    public string[] GetScript(string script, string type)
    {
        Debug.Log("loading " + script + " in " + type);
        string path = string.Empty;
        path = "Text/" + script + "_" + type;

        // Read .txt data
        TextAsset _txtFile = Resources.Load(path, typeof(TextAsset)) as TextAsset;
        string str = _txtFile.text;
        byte[] byteStr = Encoding.Unicode.GetBytes(str); // need binary transform!
        string Text = Encoding.Unicode.GetString(byteStr);
        
        return Text.Split('\n');
    }

    // Task which Shows prologue script
    private async Task ShowScript()
    {
        // our text files are splited into line by line, which has a mark at the beginning of sentence
        // there are 4 marks in here
        string T_mark = "<Text>"; // common script mark
        string Q_mark = "<Q>"; // question mark
        string Y_mark = "<Yes>"; // yes mark
        string N_mark = "<No>"; // no mark
        
        while (needAnswer == false)
        {
            // if this script is first script, hide prev button
            if (current_script == 0)
            {
                Debug.Log("prev button false");
                PopUp.GetInstance().ShowButton("Prev", false);
            }
            else
            {
                Debug.Log("prev button true");
                PopUp.GetInstance().ShowButton("Prev", true);
            }

            if (this.script[current_script].StartsWith(T_mark,false,null)) // if this script is not question
            {
                PopUp.GetInstance().FindText("Prologue").text = this.script[current_script].Substring(T_mark.Length);
            }
            else if (this.script[current_script].StartsWith(Q_mark,false,null)) // if this script is question
            {
                needAnswer = true;
                PopUp.GetInstance().ShowPopUp("Prologue", false); // close prologue popup
                PopUp.GetInstance().ShowPopUp("Answer", true); // open answer popup
                PopUp.GetInstance().FindText("Answer").text = this.script[current_script].Substring(Q_mark.Length);
            }
            await Task.Delay(100);
        }

        // question answer handling
        while (answer == string.Empty)
        {
            await Task.Delay(100);
        }
        current_script++;

        // after get answer, show gamestart popup
        PopUp.GetInstance().ShowPopUp("Answer", false);
        PopUp.GetInstance().ShowPopUp("GameStart", true);

        // current script differs according to answer
        if (answer == Y_mark)
        {
            while (current_script < length && !this.script[current_script].StartsWith(Y_mark,false,null))
            {
                current_script++;
            }
            PopUp.GetInstance().ShowButton("Quit", false);
        }
        else if (answer == N_mark)
        {
            while (current_script < length && !this.script[current_script].StartsWith(N_mark, false, null))
            {
                current_script++;
            }
        }
        else Debug.Log("invalid answer");
        // Set Popup text
        PopUp.GetInstance().FindText("GameStart").text = this.script[current_script].Substring(answer.Length);
    }

    // set current script. to access it from button click, this method(ftn) is public
    // the value of current script should be between 0 ~ length - 1
    public void SetCurrentScript(int value)
    {
        if (this.current_script + value >= 0 && this.current_script + value < length)
        {
            Debug.Log("current script : " + current_script);
            this.current_script += value;
        }
        else Debug.Log("invalid script number");
    }
    // Set Answer from player Input
    public void GetAnswer(string ans)
    {
        this.answer = ans;
    }

}
