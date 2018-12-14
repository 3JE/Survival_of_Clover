using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Summary
 * OKbutton changes scene from mineWaterClearScene to GameScene
 */
public class OKbutton : MonoBehaviour
{
    AudioSource buttonClick;

	public void OnOKButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        //get back to main scene
        SceneManager.LoadSceneAsync("GameScene");
    }
}
