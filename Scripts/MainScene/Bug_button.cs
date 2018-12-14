using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Summary
 * Bug_button controlls behaviours of bug button and bug popup
 * It loads bug popup when bug button is clicked
 * And set random bug id when generate button is clicked
 */


public class Bug_button : MonoBehaviour
{
    float energy_convert_ratio = 5;// Energy needed to call bug
    AudioSource buttonClick;

    // On Bug Button Click, load bug popup
    public void OnBugButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("Bug Button Click");
        PopUp.GetInstance().ShowPopUp("Bug",true);
    }

    // If clover can generate molecules, set random bugID
    // Close bug popup
    public void OnGenerateMoleculeButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();

        Debug.Log("Generate Molecule Button Click");
        // code for molecule generation
        bool canGenerate = CloverDirector.GetInstance().GetStat("energy") > energy_convert_ratio;
        canGenerate = canGenerate && CloverDirector.GetInstance().GetBugID() == -1;

        if (canGenerate)
        {
            Debug.Log("use energy to generate molecule");
            CloverDirector.GetInstance().SetStat("energy", -energy_convert_ratio);
            int RandomBug = (int)Random.Range(0, 4); // 0~3
            CloverDirector.GetInstance().SetBugID(RandomBug);
        }
        PopUp.GetInstance().ShowPopUp("Bug", false);
    }

    // On cancel button , close bug popup
    public void OnCancelButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("Cancel Button Click");
        PopUp.GetInstance().ShowPopUp("Bug", false);
    }

}
