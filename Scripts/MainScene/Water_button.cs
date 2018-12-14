using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/* Summary
 * Water_button changes to water game scene when water button is clicked
 */
public class Water_button : MonoBehaviour
{
    AudioSource buttonClick;
    
	public void OnWaterGameClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        Debug.Log("WaterGame clicked!");
        SceneManager.LoadScene("mineWaterScene");
    }
}
