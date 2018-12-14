using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* Summary
 * LoadingSceneDirector is a class which manages all data loading from Azure and after that, moves player into gamescene
 */

public class LoadingSceneDirector : MonoBehaviour
{
    async Task LoadGame()
    {
        // Load playerID from PlayerPrefs
        string loadedID = PlayerPrefs.GetString("playerID");
        Debug.Log("load player ID from playerprefs " + loadedID);

        // GameSettings data loading from Azure Server
        GameSettings.GetInstance().playerID = loadedID;
        await Task.Run(GameSettings.GetInstance().GetData);
        Debug.Log("GameSettings Data loading complete.");

        // if successful loading, update current playerID to PlayerPrefs
        loadedID = GameSettings.GetInstance().GetID();
        if (GameSettings.GetInstance().GetGameSetting() != null)
        {
            PlayerPrefs.SetString("playerID", loadedID);
            Debug.Log("set player ID to playerprefs " + loadedID);
        }
        else Debug.LogError("Failed to load game, try again later");

        // Clover data loading from Azure Server
        CloverDirector.GetInstance().playerID = loadedID;
        await Task.Run(CloverDirector.GetInstance().GetData);
        Debug.Log("Clover data loading complete.");

        // Collection data loading from Azure Server
        Collection.GetInstance().playerID = loadedID;
        await Task.Run(Collection.GetInstance().GetData);
        if (GameSettings.GetInstance().GetLanguage() != string.Empty)
            Collection.GetInstance().SetScript(); // Set Script from .txt file!

        Debug.Log("Collection Data loading complete.");

        // Change to GameScene
        if (GameSettings.GetInstance().GetLanguage() == string.Empty || CloverDirector.GetInstance().GetName() == string.Empty) // language setting needed!
        {
            SceneManager.LoadScene("PrologueScene");
        }
        else SceneManager.LoadScene("GameScene");
    }

    // when game start button is clicked, game start!
    public async void OnGameStartButtonClick()
    {
        Debug.Log("Game Start!");
        GameObject button = GameObject.Find("GameStartButton");
        button.SetActive(false);
        GameObject.Find("LoadingText").GetComponent<Text>().text = "Loading data from server...\nPlease wait...";
        await LoadGame();
    }

    // when playerID reset button is clicked, reset playerID
    public void OnPlayerIDResetButtonClick()
    {
        string newID = Guid.NewGuid().ToString("N"); // make new playerID
        PlayerPrefs.SetString("playerID", newID);
    }
}
