using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Summary
 * Back_button is a class which controls functions on back button click
 */
public class Back_button : MonoBehaviour
{
    AudioSource buttonClick;

    // On Back_button click, move back to gamescene from collection scene
    public void OnBackButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("Back button click");
        SceneManager.LoadScene("GameScene");
    }
}
