using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Summary
 * gainedWater get found water count from MineGameDirector and load the information in mineWaterClearScene
 * And changes clover stat according to the result
 */
public class gainedWater : MonoBehaviour
{
    GameObject waterGain;

    // Get final water gain, update text UI, and change clover stat
    void Start () {
        waterGain = GameObject.Find("gainedWater");
        if (MineGameDirector.clear)
        {
            waterGain.GetComponent<Text>().text = (MineGameDirector.water/5).ToString("F1");
            CloverDirector.GetInstance().SetStat("water", MineGameDirector.water / 5);
            CloverDirector.GetInstance().SetStat("energy", 3);
            print(MineGameDirector.water / 5 + " added to water score.");
        }
        else
        {
            waterGain.GetComponent<Text>().text = (MineGameDirector.water / 5).ToString("F1");
            CloverDirector.GetInstance().SetStat("water", MineGameDirector.water / 5);
            print(MineGameDirector.water / 5 + " added to water score.");
        }
    }
}
