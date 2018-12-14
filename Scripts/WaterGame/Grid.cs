using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Summary
 * Grid holds all Elements on mineWaterGameScene
 * It shows adjacent mine count of certain element and whether certain element is mine
 * It also holds method to show all mines in Grid and uncover near mines
 */
public class Grid
{
    // Width and height of the Grid
    public static int w = 12;
    public static int h = 20;

    // Hold all elements. Index starts from upper left
    public static Element[,] elements = new Element[w, h];
    // Shows if certain element is clickable or not
    public static bool[,] visited = new bool[w, h];

    // Uncover all mines on grid
    public static void UncoverMines()
    {
        foreach (Element elem in elements)
        {
            if (elem.mine) elem.LoadButtonTexture(0);
        }
    }

    // Return whether certain element is mine or not
    public static bool MineAt(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < w && y < h)
        {
            return elements[x, y].mine;
        }
        return false;
    }

    // Return adjacent mine count of certain element
    public static int AdjacentMines(int x, int y)
    {
        int count = 0;
        if (MineAt(x, y + 1)) ++count; // top
        if (MineAt(x + 1, y + 1)) ++count; // top-right
        if (MineAt(x + 1, y)) ++count; // right
        if (MineAt(x + 1, y - 1)) ++count; // bottom-right
        if (MineAt(x, y - 1)) ++count; // bottom
        if (MineAt(x - 1, y - 1)) ++count; // bottom-left
        if (MineAt(x - 1, y)) ++count; // left
        if (MineAt(x - 1, y + 1)) ++count; // top-left
        return count;
    }

    // Uncover near elements until there are elements near mine
    public static void UncoverNearElems(int x, int y, bool[,] visited)
    {
        // Coordinates in Range?
        if (x >= 0 && y >= 0 && x < w && y < h)
        {
            // visited already?
            if (visited[x, y])
                return;

            // uncover element
            elements[x, y].LoadButtonTexture(AdjacentMines(x, y));

            // set visited flag
            visited[x, y] = true;

            // close to a mine? then no more work needed here
            if (AdjacentMines(x, y) > 0)
                return;

            // recursion
            UncoverNearElems(x - 1, y, visited);
            UncoverNearElems(x - 1, y - 1, visited);
            UncoverNearElems(x - 1, y + 1, visited);
            UncoverNearElems(x + 1, y, visited);
            UncoverNearElems(x + 1, y - 1, visited);
            UncoverNearElems(x + 1, y + 1, visited);
            UncoverNearElems(x, y - 1, visited);
            UncoverNearElems(x, y + 1, visited);
        }
    }

    // Return total count of mines in Grid
    public static int RockCount()
    {
        int count = 0;
        foreach (Element elem in elements)
        {
            if (elem.mine) count++;
        }
        return count;
    }

    // Return total count of found water in Grid
    public static int WaterCount()
    {
        int count = 0;
        foreach (Element elem in elements)
        {
            if (visited[elem.x, elem.y] && !elem.mine) count++;
        }
        return count;
    }

    // Return if all water on Grid is discovered or not
    public static bool IsFinished()
    {
        foreach(Element elem in elements)
        {
            if (elem.IsCovered() && !elem.mine) return false;
        }
        return true;
    }
}
