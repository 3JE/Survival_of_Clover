using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* Summary
 * Element is class for every block on mineWaterGame Grid
 * When initialized, it decides whether it's going to be a mine or not depending on rock probability and register itself to grid
 * When clicked, it changes textures according to conditions(if it's rock, if flag button is clicked ...)
 */
public class Element : MonoBehaviour
{
    // Is this a mine?
    [HideInInspector]
    public bool mine;

    public int x;
    public int y;

    //mine posibility
    float rockProbability = 0.2f;

    // Different Textures
    public Sprite[] emptyTextures;
    public Sprite rockTexture;
    public Sprite flagTexture;
    public Sprite unknownTexture;

    AudioSource buttonClick;

    // Initialization: get it's location, register to grid, and decide if it's rock or not
    // Find buttonclick sound
    void Start()
    {
        x = transform.GetSiblingIndex();
        // Randomly decide if it's a mine or not
        mine = Random.value < rockProbability;
        Grid.elements[x, y] = this;
        Grid.visited[x, y] = false;
        Debug.Log("Element Start");
        Debug.Log("RockProbability: " + rockProbability);

        buttonClick = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
    }

    // Check if element sprite is default image
    public bool IsCovered()
    {
        return GetComponent<Image>().sprite.name == "tile_unknown";
    }

    // Executed when clicked
    // If flag button was clicked, mark / unmark mine with flag texture
    // Else, discover the element. End if it's a rock, and discover near elements if not.
    public void OnButtonClick()
    {
        Debug.Log(x + ", " + y + " clicked");
        if (Grid.visited[x, y]) return;
        else if (FlagButton.isClicked)
        {
            buttonClick.Play();
            LoadButtonTexture(0);
        }
        else if (mine)
        {
            buttonClick.Play();
            Grid.UncoverMines();
            MineGameDirector.UpdateUIs();
            MineGameDirector.EndOfGame(false);
            print("End of game.");
        }
        else
        {
            buttonClick.Play();
            LoadButtonTexture(Grid.AdjacentMines(x, y));
            Grid.UncoverNearElems(x, y, Grid.visited);
            MineGameDirector.UpdateUIs();
            if (Grid.IsFinished()) MineGameDirector.EndOfGame(true);
        }
    }

    // Load textures according to adjacent count, bool mine, and if flag button is clicked
    public void LoadButtonTexture(int adjacentCount)
    {
        bool isFlag = GetComponent<Image>().sprite.texture.name == "tile_sign";
        if (FlagButton.isClicked)
        {
            if (isFlag) GetComponent<Image>().sprite = unknownTexture;
            else GetComponent<Image>().sprite = flagTexture;
        }
        else if (mine)
        {
            GetComponent<Image>().sprite = rockTexture;
        }
        else
        {
            GetComponent<Image>().sprite = emptyTextures[adjacentCount];
        }
    }
}
