using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Summary
 * Ending_button holds methods to change scene to EndingCollectionScene
 */
public class Ending_button : MonoBehaviour
{
    AudioSource buttonClick;
    
    public void OnEndingButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("Ending button clicked!");
        SceneManager.LoadScene("EndingCollectionScene");
    }
}
