using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Summary
 * Holds method for changing scene to mineWaterClearScene
 * and method to update Text UIs in mineWaterScene
 * Updates UI every frame
 */
public class MineGameDirector : MonoBehaviour
{
    public static float water = 0;
    static int rock;
    public static bool clear;

    static Text waterCount;
    static Text rockCount;
    static Text unknownCount;

    void Update()
    {
        UpdateUIs();
    }

    // Gets bool cleared which indicates if game was finished after finding all water or not.
    // Change scene to mineWaterClearScene
    public static void EndOfGame(bool cleared)
    {
        //GameObject Generator = GameObject.Find("PopUpGenerator");
        //Generator.GetComponent<PopUpGenerator>().PopUp();
        clear = cleared;
        SceneManager.LoadScene("mineWaterClearScene");
    }

    // Get water, rock, unknown count from class Grid and update Text UIs
    public static void UpdateUIs()
    {
        waterCount = GameObject.Find("WaterCount").GetComponent<Text>();
        rockCount = GameObject.Find("RockCount").GetComponent<Text>();
        unknownCount = GameObject.Find("UnknownCount").GetComponent<Text>();
        water = (float)Grid.WaterCount();
        rock = Grid.RockCount();
        waterCount.text = water.ToString("F0");
        rockCount.text = rock.ToString("F0");
        unknownCount.text = (Grid.w * Grid.h - rock - water).ToString("F0");
    }
}
