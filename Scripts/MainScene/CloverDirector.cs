using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.WindowsAzure.MobileServices;

/* Summary
 * CloverDirector holds methods about getting & setting clover's variables
 * And methods for changing UIs according to clover's stats
 * And also methods to save and get data from Microsoft Azure
 */

public class CloverDirector : MonoBehaviour
{
    private static CloverDirector myCloverDirector; // singletone CloverDirector

    private static int numberOfAttemptsToLoadData = 3; // number of trying to load data
    public string playerID = string.Empty; // temporary variable for check playerID
    public GameObject rainSoundObject;  // Object that holds raining sound
    private static Clover myClover = null; // Clover Instance
    bool weatherRunning = false; // boolean to show if weather coroutine is running
    public bool endingPopupOn = false;

    // Makes CloverDirector DontDestroyOnLoad
    async void Awake()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            // If clover doesn't have name on GameScene, make NamePopUp
            if (myClover.c_name == string.Empty)
            {
                Debug.Log("name is null");
                await PopUp.GetInstance().ShowNamePopUp();
            }
        }
        CloverDirector.GetInstance().weatherRunning = false; // Initialize weatherRunning variable

        //DontDestroyOnLoad
        if (myCloverDirector == null)
        {
            DontDestroyOnLoad(gameObject);
            myCloverDirector = this;
        }
        else if (myCloverDirector != this)
        {
            Destroy(gameObject);
        }
    }

    // Turn on the Weather function
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (myClover.c_name != string.Empty) WeatherFunctionOn(true);
        }
    }

    // If it's GameScene, update UIs and check if any ending condition is met
    // if it's game scene and weather is not running when it should, makes weather running
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (!CloverDirector.GetInstance().endingPopupOn) UpdateStats();
            if (!CloverDirector.GetInstance().weatherRunning && myClover.c_name != string.Empty && !CloverDirector.GetInstance().endingPopupOn)
            {
                WeatherFunctionOn(true);
                CloverDirector.GetInstance().weatherRunning = true;
            }
        }
    }


    // Get data from Azure
    public async Task GetData()
    {
        // Get table from Azure Server
        var table = AzureMobileServiceClient.Client.GetTable<Clover>();

        bool validID = false; // Variable for check the ID is valid

        // Load data : try several times to load data
        for (int i = 0; i < numberOfAttemptsToLoadData; i++)
        {
            // if not new player
            if (playerID != string.Empty)
            {
                try
                {
                    //Debug.Log("playerID : " + playerID);
                    Debug.Log("Load player data from Azure...");
                    myClover = await table.LookupAsync(playerID);
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
                    //Debug.Log("playerID : " + playerID);
                    Debug.Log("New player! make new instance and insert data to Azure...");
                    if (playerID == string.Empty)
                    {
                        playerID = Guid.NewGuid().ToString("N"); // make new playerID

                        Debug.Log("making new player ID " + playerID);
                    }
                    myClover = new Clover { ID = playerID };

                    await table.InsertAsync(myClover);
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
        var table = AzureMobileServiceClient.Client.GetTable<Clover>();
        try
        {
            Debug.Log("update player data to Azure");
            await table.UpdateAsync(myClover);
        }
        catch (Exception)
        {
            Debug.LogError("Update error");
        }
    }

    // Load clover image according to its level
    void LoadCloverTexture(int level)
    {
        // Find all clover images
        GameObject lv1 = GameObject.Find("cloverLv1");
        GameObject lv2 = GameObject.Find("cloverLv2");
        GameObject lv3 = GameObject.Find("cloverLv3");
        GameObject lv4 = GameObject.Find("cloverLv4");
        GameObject lv5 = GameObject.Find("cloverLv5");

        // Make clovers except current level clover transparent
        // And set current level clover's alpha to 1f
        if (level == 1)
        {
            lv1.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            lv2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            lv3.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            lv4.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            lv5.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        }
        else if (level == 2)
        {
            lv1.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            lv2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            lv3.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            lv4.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            lv5.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        }
        else if (level == 3)
        {
            lv1.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            lv2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            lv3.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            lv4.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            lv5.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        }
        else if (level == 4)
        {
            lv1.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            lv2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            lv3.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            lv4.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            lv5.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        }
        else if (level == 5)
        {
            lv1.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            lv2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            lv3.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            lv4.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            lv5.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
    }

    // Load background & sun/cloud/rain image according to weather
    // By controlling alpha color of every UI images
    // And if raining, load rain sound
    void LoadWeatherTexture(int weather_ID)
    {
        GameObject sunny = GameObject.Find("SunnyBackground");
        GameObject sunny2 = GameObject.Find("SunImage");
        GameObject cloudy = GameObject.Find("CloudyBackground");
        GameObject cloudy2 = GameObject.Find("CloudImage");
        GameObject rainy = GameObject.Find("RainyBackground");
        GameObject rainy2 = GameObject.Find("RainImage");

        if (weather_ID == 0)
        {
            sunny.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            cloudy.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            rainy.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            sunny2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            cloudy2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            rainy2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        }
        else if (weather_ID == 1)
        {
            sunny.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            cloudy.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            rainy.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            sunny2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            cloudy2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            rainy2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        }
        else if (weather_ID == 2)
        {
            sunny.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            cloudy.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            rainy.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            sunny2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            cloudy2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            rainy2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }

        LoadRainSound(CloverDirector.GetInstance().GetWeather() == 2);
    }

    // Find rain audiosource and play
    void LoadRainSound(bool on)
    {
        if (on)
        {
            rainSoundObject = GameObject.Find("RainSound");
            AudioSource rainAudioSource = rainSoundObject.GetComponent<AudioSource>();
            if (!rainAudioSource.isPlaying) rainAudioSource.Play();
        }
        else
        {
            rainSoundObject = GameObject.Find("RainSound");
            AudioSource rainAudioSource = rainSoundObject.GetComponent<AudioSource>();
            rainAudioSource.Pause();
        }
    }

    // Change weather in every weatherCycle seconds during player is in GameScene
    IEnumerator WeatherChange()
    {
        while (true)
        {
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                // Set random weather
                CloverDirector.GetInstance().SetWeather((int)UnityEngine.Random.Range(0, 3));

                // Save changes and load appopriate textures
                LoadWeatherTexture(CloverDirector.GetInstance().GetWeather());
                int weather_ID = CloverDirector.GetInstance().GetWeather();

                yield return new WaitForSeconds(myClover.weatherCycle);
            }
            else yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "GameScene");
        }
    }

    // Change clover stats according to weather every 2 sec
    IEnumerator WeatherStatChange()
    {
        while (true)
        {
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                float sunshine;
                float rain;
                if (myClover.c_weather_ID == 0)
                {
                    sunshine = 3;
                    rain = -2;
                }
                else if (myClover.c_weather_ID == 1)
                {
                    sunshine = 1;
                    rain = -1;
                }
                else
                {
                    sunshine = -2;
                    rain = 1;
                }
                SetStat("sun", sunshine);
                SetStat("water", rain);
                Debug.Log("sunshine: " + sunshine);
                yield return new WaitForSeconds(2.0f);
            }
            else yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "GameScene");
        }
    }

    // Makes weather coroutine running or stop
    // If functionOn is true, make weather coroutines running
    // Else make weather coroutines stop
    public void WeatherFunctionOn(bool functionOn)
    {
        if (!CloverDirector.GetInstance().weatherRunning && functionOn)
        {
            StartCoroutine("WeatherChange");
            StartCoroutine("WeatherStatChange");
            CloverDirector.GetInstance().weatherRunning = true;
        }
        else if (CloverDirector.GetInstance().weatherRunning && !functionOn)
        {
            StopCoroutine("WeatherChange");
            StopCoroutine("WeatherStatChange");
            CloverDirector.GetInstance().weatherRunning = false;
        }
    }

    // Return singleton CloverDirector
    // Used to access CloverDirector outside of this script
    public static CloverDirector GetInstance()
    {
        if (myCloverDirector == null)
        {
            myCloverDirector = FindObjectOfType<CloverDirector>();
            if (myCloverDirector == null)
            {
                GameObject container = new GameObject("CloverDirector");
                myCloverDirector = container.AddComponent<CloverDirector>();
            }
        }
        return myCloverDirector;
    }

    // Return the name of clover
    public string GetName()
    {
        return myClover.c_name;
    }

    // Set clover name
    // Update UIs and get weather coroutines running
    public async void SetName(string name)
    {
        myClover.c_name = name;
        await Task.Run(SetData);
        UpdateStats();
        WeatherFunctionOn(true);
    }

    // Return the ID of the weather
    // 0: sunny, 1: cloudy, 2: rainy
    public int GetWeather()
    {
        return myClover.c_weather_ID;
    }

    // Set weather ID of clover
    public async void SetWeather(int newID)
    {
        myClover.c_weather_ID = newID;
        await Task.Run(SetData);
    }

    // Return water/sun/energy stats or level of the clover
    public float GetStat(string stat)
    {
        if (stat == "water") return myClover.c_water_stat;
        else if (stat == "sun") return myClover.c_sun_stat;
        else if (stat == "energy") return myClover.c_energy_stat;
        else return myClover.c_level;
    }

    // Add amount to clover's stats(sun, water, energy) and save data
    // Check if ending condition is met by stat changes
    public async void SetStat(string statName, float amount)
    {
        if (statName == "water") myClover.c_water_stat += amount;
        else if (statName == "sun") myClover.c_sun_stat += amount;
        else if (statName == "energy")
        {
            myClover.c_energy_stat += amount;
            await Task.Run(SetData);
        }
        LevelUp();
        if (SceneManager.GetActiveScene().name == "GameScene") CheckEnding();
        return;
    }

    // Update text UIs and load clover/weather textures
    // Only works on GameScene
    void UpdateStats()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            // Find Text UIs
            Text WaterStatus = GameObject.Find("WaterStatus").GetComponent<Text>();
            Text SunStatus = GameObject.Find("SunStatus").GetComponent<Text>();
            Text EnergyStatus = GameObject.Find("EnergyStatus").GetComponent<Text>();
            Text Level = GameObject.Find("Level").GetComponent<Text>();
            Text Name = GameObject.Find("Name").GetComponent<Text>();

            // Update Text UIs
            WaterStatus.GetComponent<Text>().text = CloverDirector.GetInstance().GetStat("water").ToString("F1");
            SunStatus.GetComponent<Text>().text = CloverDirector.GetInstance().GetStat("sun").ToString("F1");
            EnergyStatus.GetComponent<Text>().text = CloverDirector.GetInstance().GetStat("energy").ToString("F1");
            Level.GetComponent<Text>().text = "LV." + CloverDirector.GetInstance().GetStat("level").ToString("F0");
            Name.GetComponent<Text>().text = CloverDirector.GetInstance().GetName();

            // Load clover & weather textures
            LoadCloverTexture(myClover.c_level);
            LoadWeatherTexture(myClover.c_weather_ID);
        }
    }

    // See if level up conditions are met
    // If met, decrease energy stats and increase level
    public async void LevelUp()
    {
        if (myClover.c_energy_stat >= myClover.c_energy_requirement)
        {
            if (myClover.c_level != 5)
            {
                myClover.c_energy_stat += -myClover.c_energy_requirement;
                myClover.c_level += 1;
                myClover.c_energy_requirement = (myClover.c_level + 1)* 50;
                await Task.Run(SetData);
                Debug.Log("Level up!");
            }
            else if (myClover.c_bug_ID == 1 || myClover.c_bug_ID == 2)
            {
                myClover.c_energy_stat += -myClover.c_energy_requirement;
                myClover.c_level += 1;
                myClover.c_energy_requirement = (myClover.c_level + 1) * 50;
                await Task.Run(SetData);
                CheckEnding();
            }
        }
    }

    // Set BugID and save data
    // If BugID changes from -1 to nonnegative number, start bug timer
    // -1: don't exist, 0: ladybug, 1: butterfly, 2: bee
    public async void SetBugID(int bugID)
    {
        myClover.c_bug_ID = bugID;
        if (bugID != -1)
        {
            BugTimer.Getbugtimer().StartBugTimer();
            CheckEnding();  // Check if ending conditions are met
        }
        await Task.Run(SetData);
    }

    // Returns bugID of clover
    public int GetBugID()
    {
        return myClover.c_bug_ID;
    }

    // Set BugTime and save data
    // Executed when bug comes and every second when bug is present
    public async void SetBugTime(int time)
    {
        myClover.c_bug_time = time;
        //     await Task.Run(SetData);
    }

    // Return BugTime of the clover
    public int GetBugTime()
    {
        return myClover.c_bug_time;
    }

    // Initialize clover data and corresponding UIs
    // And show name pop up to restart
    public async void Reset()
    {
        myClover.c_level = 1;
        myClover.c_sun_stat = 50;
        myClover.c_water_stat = 50;
        myClover.c_energy_stat = 20;
        myClover.c_name = string.Empty;
        myClover.c_weather_ID = 0;
        myClover.c_energy_requirement = 100;
        SetBugID(-1);
        SetBugTime(60);

        await Task.Run(SetData);
        UpdateStats();
        BugTimer.Getbugtimer().UpdateBugTimer(-1);
        WeatherFunctionOn(false);

        await PopUp.GetInstance().ShowNamePopUp();

        Debug.Log("Reset!");
    }

    // Check if clover met any of ending conditions
    // If ending conditions are met, load EndingPopUp and reset clover
    public void CheckEnding()
    {
        int ending_number = 0;
        if (GetStat("water") <= 0)
        {
            //thirsty
            print("Died of thirst");
            ending_number = 1;
        }
        else if (GetStat("water") >= 100)
        {
            //drown
            print("Drown");
            ending_number = 2;
        }
        else if (GetStat("sun") <= 0)
        {
            //too cold
            print("Freezed");
            ending_number = 3;
        }
        else if (GetStat("sun") >= 100)
        {
            //too hot
            print("Dried");
            ending_number = 4;
        }
        else if (GetStat("level") == 6)
        {
            //survived successfully
            print("Congratulations!");
            ending_number = 5;
        }

        if (ending_number > 0)
        {
            //            EndingDirector.GetInstance().EndingFunctionOn(false); // random event ending off
            UpdateStats();
            WeatherFunctionOn(false); // weather change off 
            CloverDirector.GetInstance().endingPopupOn = true;
            SetBugID(-1); // bug timer off
            Debug.Log("Ending : " + ending_number);
            PopUp.GetInstance().FindText("Ending").text = Collection.GetInstance().GetString("info", ending_number);
            PopUp.GetInstance().FindImage("Ending").sprite = Collection.GetInstance().GetImage(ending_number);
            PopUp.GetInstance().ShowPopUp("Ending", true);
            Collection.GetInstance().SetOpened(ending_number);
        }
    }
}