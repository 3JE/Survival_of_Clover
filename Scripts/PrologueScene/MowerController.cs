using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Summary
 * MowerController has methods to move lawn mower image on prologue scene
 * When No button is clicked.
 */
public class MowerController : MonoBehaviour
{
    private RectTransform mowerRect;
    /// Get Transform Component
    void Start()
    {
        mowerRect = GetComponent<RectTransform>();
    }

    // Function to make mower move outside of the script
    public void MoveMower()
    {
        StartCoroutine("Move");
    }

    // Make LawnMower Image move to clover position smoothly
    IEnumerator Move()
    {
        float speed = 100.0f;
        print("move!");
        while (mowerRect.anchoredPosition.x < 500)
        {
            mowerRect.Translate(speed, 0, 0);
            if (mowerRect.anchoredPosition.x >= 250) speed *= 0.7f;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}