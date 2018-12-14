using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Summary
 * GameMusic is a class for controlling music in total game
 */
public class GameMusic : MonoBehaviour
{
    private static GameMusic gameMusic;

    // Make GameMusic DontDestroyOnLoad
    void Awake ()
    {
        if (gameMusic == null)
        {
            DontDestroyOnLoad(gameObject);
            gameMusic = this;
        }
        else if (gameMusic != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // On loading scene, starts playing
        if (SceneManager.GetActiveScene().name == "LoadingScene")
        {
            gameMusic.GetComponent<AudioSource>().Play();
        }
        // On other scenes, make GameMusic keep playing
        else
        {
            gameMusic.GetComponent<AudioSource>().UnPause();
        }
    }
}
