using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Summary
 * BubbleController controls speech bubble above clover
 * It changes y positions according to clover level and load appopriate image according to clover's state
 */
public class BubbleController : MonoBehaviour
{
    public Sprite[] bubbles;
    RectTransform rectTransform;
    int[] yPositions = new int[5];
    Image bubbleImage;

    // Get bubbleImage and rectTransform
    // Initialize yPositions for each clover level
    void Start () {
        bubbleImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        yPositions[0] = 0;
        yPositions[1] = 100;
        yPositions[2] = 150;
        yPositions[3] = 200;
        yPositions[4] = 250;
    }

    // Load bubble images corresponding to current conditions
    // If aphid is present, load sad bubble.
    // Else if sun and water stats are in 40~60 range, load heart bubble
    // Else if sun&water stats are in 30~70 range, load smile bubble
    void Update()
    {
        if (CloverDirector.GetInstance())
        {
            float sunStat = CloverDirector.GetInstance().GetStat("sun");
            float waterStat = CloverDirector.GetInstance().GetStat("water");
            int level = (int)CloverDirector.GetInstance().GetStat("level");

            rectTransform.anchoredPosition = new Vector2(0, yPositions[level - 1]);

            if (CloverDirector.GetInstance().GetBugID() == 3)
            {
                bubbleImage.color = new Color(1f, 1f, 1f, 1f);
                bubbleImage.sprite = bubbles[2];
            }
            else if (sunStat <= 60 && sunStat >= 40 && waterStat <= 60 && waterStat >= 40)
            {
                bubbleImage.color = new Color(1f, 1f, 1f, 1f);
                bubbleImage.sprite = bubbles[0];
            }
            else if (sunStat <= 70 && sunStat >= 30 && waterStat <= 70 && waterStat >= 30)
            {
                bubbleImage.color = new Color(1f, 1f, 1f, 1f);
                bubbleImage.sprite = bubbles[1];
            }
            else
            {
                bubbleImage.sprite = null;
                bubbleImage.color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }
}
