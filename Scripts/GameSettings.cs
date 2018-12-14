using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.WindowsAzure.MobileServices;
using UnityEngine.SceneManagement;

/* Summary
 * GameSettings is a class which manages and game settings(language / tutorial / reset..)
 */
public class GameSettings : MonoBehaviour
{
    private static GameSettings gameSettings;// to use singleton popup
    private static GameSetting myGameSettings;// game setting data which will be updated to Azure
    private static int numberOfAttemptsToLoadData = 3; // number of trying to load data
    public string playerID = string.Empty; // temporary varialbe for check playerID

    void Awake ()
    {
        // when scene is changed, set gamesetting not destroyed
        if (gameSettings == null)
        {
            DontDestroyOnLoad(gameObject);
            gameSettings = this;
        }
        else if (gameSettings != this)
        {
            Destroy(gameObject);
        }
    }

    // return singleton GameSettings
    public static GameSettings GetInstance()
    {
        if (gameSettings == null)
        {
            gameSettings = FindObjectOfType<GameSettings>();
            if (gameSettings == null)
            {
                GameObject container = new GameObject("GameSettings");
                gameSettings = container.AddComponent<GameSettings>();
            }
        }
        return gameSettings;
    }

    // Get data from Azure
    public async Task GetData()
    {
        // Get table from Azure Server
        var table = AzureMobileServiceClient.Client.GetTable<GameSetting>();

        bool validID = false; // Variable for check the ID is valid

        // Load data : try several times to load data
        for (int i = 0; i < numberOfAttemptsToLoadData; i++)
        {
            // if not new player
            if (playerID != string.Empty)
            {
                try
                {
                    Debug.Log("Load gamesetting data from Azure...");
                    myGameSettings = await table.LookupAsync(playerID);
                    Debug.Log("successfully load data from Azure");
                    validID = true;
                    break;
                }
                catch (Exception)
                {
                    Debug.LogError("lookup error, invalid playerID " + playerID);
                }
            }

            // if new player
            if (!validID)
            {
                try
                {
                    Debug.Log("New player! make new instance and insert data to Azure...");
                    if (playerID == string.Empty)
                    {
                        playerID = Guid.NewGuid().ToString("N"); // make new playerID
                        Debug.Log("making new player ID " + playerID);
                    }
                    myGameSettings = new GameSetting { ID = playerID };

                    await table.InsertAsync(myGameSettings);
                    break;
                }
                catch (Exception)
                {
                    Debug.LogError("Insert error.");
                }
            }

            // if try n times
            if (i == numberOfAttemptsToLoadData - 1)
                Debug.LogError("Connection failed. Check logs, try again later.");
            else
                await Task.Delay(500);
        }
    }

    // Set data to Azure
    public async Task SetData()
    {
        // Get table from Azure Server
        var table = AzureMobileServiceClient.Client.GetTable<GameSetting>();
        try
        {
            Debug.Log("update gamesetting data to Azure");
            await table.UpdateAsync(myGameSettings);
        }
        catch (Exception)
        {
            Debug.LogError("Update error");
        }
    }

    // Set language according to input and update it to Azure
    public async void SetLanguage(string lan)
    {
        myGameSettings.language = lan;
        await Task.Run(SetData);
    }

    // Get language setting
    public string GetLanguage()
    {
        Debug.Log("language is " + myGameSettings.language);
        return myGameSettings.language;
    }

    // Get playerID stored in Azure sync data
    public string GetID()
    {
        return myGameSettings.ID;
    }

    // Set playerID into Azure sync data
    public void SetID(string id)
    {
        myGameSettings.ID = id;
    }

    // return myGameSettings to access it from loading scene
    public GameSetting GetGameSetting()
    {
        return myGameSettings;
    }
}
