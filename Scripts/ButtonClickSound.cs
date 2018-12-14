using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Summary
 * button click sound is a class which is not destroyed when scene is changed
 */
public class ButtonClickSound : MonoBehaviour
{
    private static ButtonClickSound buttonClickSound;

    void Awake()
    {
        if (buttonClickSound == null)
        {
            DontDestroyOnLoad(gameObject);
            buttonClickSound = this;
        }
        else if (buttonClickSound != this)
        {
            Destroy(gameObject);
        }
    }
}
