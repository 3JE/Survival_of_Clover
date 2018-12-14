using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Summary
 * EndingDirector gets random ending and hold coroutine for starting random ending coroutine
 * It also has function to run/stop this random ending coroutine
 */
public class EndingDirector : MonoBehaviour
{
    private int endingFrequency = 120; // the frequency of random ending(unit : sec)

    void Start()
    {
        EndingFunctionOn(true);
    }

    // Generate random ending event
    void RandomEnding()
    {
        // Probability of random ending(unit : %)
        int chickenProbability = 10;
        int rabbitProbability = 10;
        int horseProbability = 10;
        int mowerProbability = 10;
        int tornadoProbability = 10;
        int aphidProbability = 10;
        int boyProbability = 10;

        float[] endingProbabilities = new float[7];
        endingProbabilities[0] = chickenProbability;
        endingProbabilities[1] = endingProbabilities[0] + rabbitProbability;
        endingProbabilities[2] = endingProbabilities[1] + horseProbability;
        endingProbabilities[3] = endingProbabilities[2] + mowerProbability;
        endingProbabilities[4] = endingProbabilities[3] + tornadoProbability;
        endingProbabilities[5] = endingProbabilities[4] + aphidProbability;
        endingProbabilities[6] = endingProbabilities[5] + boyProbability;

        int ending = (int)Random.Range(0, 100);
        int ending_number = 0;
        if (ending < endingProbabilities[0])
        {
            print("Chicken Ending");
            ending_number = 6;
        }
        else if (ending < endingProbabilities[1])
        {
            print("Rabbit Ending");
            ending_number = 7;
        }
        else if (ending < endingProbabilities[2])
        {
            print("Horse Ending");
            ending_number = 8;
        }
        else if (ending < endingProbabilities[3])
        {
            print("Lawn mower Ending");
            ending_number = 9;
        }
        else if (ending < endingProbabilities[4])
        {
            print("Tornado Ending");
            ending_number = 10;
        }
        else if (ending < endingProbabilities[5] && CloverDirector.GetInstance().GetBugID() == 3)
        {
            print("Aphid Ending");
            ending_number = 11;
        }
        else if (ending < endingProbabilities[6])
        {
            print("Boy Ending");
            ending_number = 12;
        }

        if (ending_number > 0)
        {
            EndingFunctionOn(false); // random event ending off
            CloverDirector.GetInstance().endingPopupOn = true;
            CloverDirector.GetInstance().WeatherFunctionOn(false); // weather change off 
            CloverDirector.GetInstance().SetBugID(-1); // bug timer off
            Debug.Log("Ending : " + ending_number);
            PopUp.GetInstance().FindText("Ending").text = Collection.GetInstance().GetString("info", ending_number);
            PopUp.GetInstance().FindImage("Ending").sprite = Collection.GetInstance().GetImage(ending_number);
            PopUp.GetInstance().ShowPopUp("Ending", true);
            Collection.GetInstance().SetOpened(ending_number);
        }
    }

    // ending enumerator stop
    public void EndingFunctionOn(bool functionOn)
    {
        if (functionOn)
        {
            StartCoroutine("EndingEvent");
            print("ending on");
        }
        else
        {
            StopCoroutine("EndingEvent");
            print("ending off");
        }
    }

    // Coroutine to generate ending every endingFrequency seconds
    IEnumerator EndingEvent()
    {
        while (true)
        {
            yield return new WaitForSeconds(endingFrequency);
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                RandomEnding();
                yield return new WaitForSeconds(endingFrequency);
            }
            else
            {
                EndingFunctionOn(false);
            }
        }
    }
}