using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

/* Summary
 * If exit button is clicked, save data and close application
 */
public class Exit_button : MonoBehaviour
{
    AudioSource buttonClick;

    public async void OnExitButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("Exit button click");
        //저장하고 종료하기
        await Task.Run(CloverDirector.GetInstance().SetData);
        Application.Quit();
    }
}
