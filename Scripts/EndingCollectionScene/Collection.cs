using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.WindowsAzure.MobileServices;

/* Summary
 * Collection is a class which controls collection data(load, update, return collection...) & sync Azure
 */
public class Collection : MonoBehaviour
{
    private static Collection collectionList; // to use singleton
    private static Collections myCollection = null; // Collection instance
    private static int numberOfAttemptsToLoadData = 3; // number of trying to load data
    public string playerID = string.Empty; // temporary varialbe for check playerID
    
    
    private static bool[] opened = null; // stores each collection is opened(true) or not(false) 
    private string[] collection_name = null; // stores each collection name
    private string[] collection_info = null; // stores each collection info
    [SerializeField]
    private Sprite[] collection_image; // stores each collection image

	// initialization when collection scene is loaded
	void Awake ()
    {
        //화면 바뀔 때 컬렉션이 삭제되지 않게 함
        if (collectionList == null)
        {
            DontDestroyOnLoad(gameObject);
            collectionList = this;
        }
        else if (collectionList != this)
        {
            Destroy(gameObject);
        }
    }


    //return singleton collectionList
    public static Collection GetInstance()
    {
        if (collectionList == null)
        {
            collectionList = FindObjectOfType<Collection>();
            if (collectionList == null)
            {
                GameObject container = new GameObject("Collection");
                collectionList = container.AddComponent<Collection>();
            }
        }
        return collectionList;
    }

    // Set Collection Scripts
    public void SetScript()
    {
        this.collection_name = Prologue.GetInstance().GetScript("collectionName", GameSettings.GetInstance().GetLanguage());
        if (myCollection.n_collection != this.collection_name.Length) Debug.Log("collection number error L " + myCollection.n_collection + ", " + this.collection_name.Length);
        this.collection_info = Prologue.GetInstance().GetScript("collectionInfo", GameSettings.GetInstance().GetLanguage());
    }
    

    // Get data from Azure
    public async Task GetData()
    {
        // Get table from Azure Server
        var table = AzureMobileServiceClient.Client.GetTable<Collections>();

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
                    Debug.Log("Load collection data from Azure...");
                    myCollection = await table.LookupAsync(playerID);
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
                    myCollection = new Collections { ID = playerID };
                    // player collection opened info initialization
                    bool[] initializer = new bool[myCollection.n_collection];
                    string openedstring = string.Empty;
                    for (int j = 0; j < myCollection.n_collection; j++)
                    {
                        if (j < myCollection.n_collection - 1)
                            openedstring += (initializer[j] ? 1 : 0) + ",";
                        else
                            openedstring += (initializer[j] ? 1 : 0);
                    }
                    myCollection.opened = openedstring;

                    await table.InsertAsync(myCollection);
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
        if (myCollection != null && myCollection.opened != string.Empty) // if get data : transform data structure from string to boolean array
        {
            string[] openedstring = new string[myCollection.n_collection];
            opened = new bool[myCollection.n_collection];
            openedstring = myCollection.opened.Split(',');
            for (int j = 0; j < myCollection.n_collection; j++)
            {
                opened[j] = (System.Convert.ToInt32(openedstring[j]) == 1) ? true : false;
            }
        }
    }

    // Set data to Azure
    public async Task SetData()
    {
        if (myCollection.n_collection == 0) Debug.LogError("collection number modifyed!!!!!!!!!!!!!!!!!!!!!!!!!!!! : " + myCollection.n_collection);

        // transform data structure from boolean array to string
        string openedstring = string.Empty;
        for (int i = 0; i < myCollection.n_collection; i++)
        {
            if (i < myCollection.n_collection - 1)
                openedstring += (opened[i] ? 1 : 0) + ",";
            else
                openedstring += (opened[i] ? 1 : 0);
        }
        myCollection.opened = openedstring;
        Debug.Log("collection opened : " + openedstring);
        // Get table from Azure Server
        var table = AzureMobileServiceClient.Client.GetTable<Collections>();
        try
        {
            Debug.Log("update collection data to Azure");
            await table.UpdateAsync(myCollection);
        }
        catch (Exception)
        {
            Debug.LogError("Update error");
        }
    }


    // return i-th collection's name or information 
    // i : value between 1 ~ n_collection
    // since script starts at <Text>, this method returns string without it
    public string GetString(string type, int i)
    {
        string T_mark = "<Text>";
        if (myCollection != null)
        {
            if (i <= myCollection.n_collection && i - 1 >= 0)  // valid index of collection
            {
                Debug.Log("vaild number " + i);
                if (type == "name")
                    return this.collection_name[i - 1].Substring(T_mark.Length);
                else return this.collection_info[i - 1].Substring(T_mark.Length);// when type is "info"
            }
            else
            {
                Debug.Log("invalid number");
                return string.Empty;
            }
        }
        else return string.Empty;
    }

    // return i-th collection's image
    // i : value between 1 ~ n_collection
    public Sprite GetImage(int i)
    {
        if (myCollection != null)
        {
            if (i <= myCollection.n_collection && i - 1 >= 0) // valid index of collection
            {
                Debug.Log("vaild number " + i);
                return collection_image[i - 1];
            }
            else
            {
                Debug.Log("invalid number");
                return null;
            }
        }
        else return null;
    }

    // return i-th collection is opened or not
    // i : value between 1 ~ n_collection
    public bool Opened(int i)
    {
        if (myCollection != null)
        {
            if (i <= myCollection.n_collection && i >= 1) // valid index of collection
            {
                return opened[i - 1];
            }
            else
            {
                Debug.Log("invalid number" + i);
                return false;
            }
        }
        else return false;
    }

    // set the collection as opened when ending event is appeared
    public async void SetOpened(int i)
    {
        opened[i - 1] = true;
        Debug.Log("try to update collection opened : " + i);
        await Task.Run(SetData);
    }

    // reset all collection
    public async void Reset()
    {
        for (int i = 0; i < myCollection.n_collection; i++) opened[i] = false;
        await Task.Run(SetData);
        Debug.Log("Reset collection");
    }

    // for test : make all collection opened
    public async void CheatCollection()
    {
        await Task.Run(GetData);
        Debug.Log("Cheat Button clicked");
        for (int i=0;i<myCollection.n_collection;i++)
            opened[i] = true;
        await Task.Run(SetData);
    }

}
    