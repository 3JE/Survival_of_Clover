using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Summary
 * If flag button is clicked, click on element is used to mark/unmark rocks
 */
public class FlagButton : MonoBehaviour
{
    public static bool isClicked;
    public Sprite clickedTexture;
    public Sprite unclickedTexture;

    AudioSource buttonClick;

    void Start()
    {
        isClicked = false;
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
    }

    public void OnFlagButtonClick()
    {
        buttonClick.Play();
        isClicked = !isClicked;
        if (isClicked) GetComponent<Image>().sprite = clickedTexture;
        else GetComponent<Image>().sprite = unclickedTexture;
    }
}
