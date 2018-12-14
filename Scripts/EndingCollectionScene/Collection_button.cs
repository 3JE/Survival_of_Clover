using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Linq;

/* Summary
 * Collection_button is a class which controls collection buttons and their popups
 */
public class Collection_button : MonoBehaviour
{
    // member variables are SerialiedField to reference from inspector
    // member variables for Button
    [SerializeField]
    private int CollectionNumber; // the number assigned to each collection buttons
    [SerializeField]
    private Button CollectionButton;// Button assigned to each collection button

    private Text CollectionName;// the text of button when collection opened

    private AudioSource buttonClick;

    // initialization when collection scene is loaded 
    void Start()
    {
        PopUp.GetInstance().ShowPopUp("Collection", false); // hide popup
  
        if (CollectionNumber > 0)
        {
            if (CollectionNumber < 7)
            {
                GameObject content = GameObject.Find("CollectionA");
                CollectionName = content.transform.Find("Collection" + CollectionNumber + "/CollectionName").GetComponent<Text>();
            }
            else
            {
                GameObject content = GameObject.Find("CollectionB");
                CollectionName = content.transform.Find("Collection" + CollectionNumber + "/CollectionName").GetComponent<Text>();
            }
            // check collection is opened
            if (Collection.GetInstance().Opened(CollectionNumber))
            {
                CollectionName.text = Collection.GetInstance().GetString("name", CollectionNumber); // if opened, get collection name
                CollectionButton.interactable = true;
            }
            else // if not opened, collection button is inactivated, and collection name is also unknown
            {   
                CollectionName.text = "???"; 
            }
        }

        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
    }

    // when collection button is clicked
    public void OnCollectionButtonClick()
    {
        buttonClick.Play();
        Debug.Log("Click Collection Button " + CollectionNumber);
        // show popup
        PopUp.GetInstance().ShowPopUp("Collection", true);
        // the window text is changed to collection information
        PopUp.GetInstance().FindText("Collection").text = Collection.GetInstance().GetString("info", CollectionNumber);
        PopUp.GetInstance().FindImage("Collection").sprite = Collection.GetInstance().GetImage(CollectionNumber);
        
    }

    // when ok button in popup window is clicked
    public void OnOKButtonClick()
    {
        buttonClick.Play();
        Debug.Log("OK Button Click");
        // hide popup
        PopUp.GetInstance().ShowPopUp("Collection", false);
    }   
}
