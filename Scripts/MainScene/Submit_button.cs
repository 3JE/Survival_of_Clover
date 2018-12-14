using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Summary
 * Submit_button exists on name popup.
 * If clicked, set clover name
 */
public class Submit_button : MonoBehaviour
{
    AudioSource buttonClick;

    void Awake()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
    }
    
    public void OnSubmitButtonClicked()
    {
        buttonClick.Play();
        PopUp.GetInstance().SetName();
    }
}
