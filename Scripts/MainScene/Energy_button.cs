using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Summary
 * Energy_button holds methods for converting sun & water stats to energy when energy button is clicked
 */
public class Energy_button : MonoBehaviour
{
    // Energy_convert_ratio : 1 = Sun & Water stat needed for energy : 5 energy stat
    float energy_convert_ratio = 5;
    AudioSource buttonClick;

    // If clover has enough sun & water stats to generate energy, convert sun & water stats to energy
    // Check if clover can level up
    public void OnEnergyButtonClick()
    {
        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        buttonClick.Play();
        bool canConvert = CloverDirector.GetInstance().GetStat("sun") > energy_convert_ratio && CloverDirector.GetInstance().GetStat("water") > energy_convert_ratio;

        if (canConvert)
        {
            CloverDirector.GetInstance().SetStat("sun", -energy_convert_ratio);
            CloverDirector.GetInstance().SetStat("water", -energy_convert_ratio);
            CloverDirector.GetInstance().SetStat("energy", 5);
            CloverDirector.GetInstance().LevelUp();
        }
    }

}
