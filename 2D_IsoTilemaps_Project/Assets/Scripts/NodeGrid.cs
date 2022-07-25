using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeGrid
{
    public Grid tileGrid;
    public string wallTag = "Blocking";
    public string floorTag = "Floor";
    public Vector2Int vGridWorldSize;
    public Node[,] nodeArray;
    public int xOffset;
    public int yOffset;

    public NodeGrid(Grid tileGrid, string wallTag, string floorTag)
    {
        this.tileGrid = tileGrid;
        this.wallTag = wallTag;
        this.floorTag = floorTag;
        if (tileGrid != null && wallTag != "")
            CreateGrid();
    }
    public void CreateGrid()
    {
        // Get all tilemaps
        Tilemap[] allTileMaps = tileGrid.GetComponentsInChildren<Tilemap>();
        // Check for largest tilemap
        int largestX = 0, largestY = 0;
        int lowestX = int.MaxValue, lowestY = int.MaxValue;
        foreach (Tilemap map in allTileMaps)
        {
            map.CompressBounds();
            BoundsInt bounds = map.cellBounds;

            if (bounds.size.x > largestX)
                largestX = bounds.size.x;
            if (bounds.size.y > largestY)
                largestY = bounds.size.y;
            if (map.cellBounds.xMin < lowestX)
                lowestX = bounds.xMin;
            if (map.cellBounds.yMin < lowestY)
                lowestY = bounds.yMin;
        }
        // Setup variables
        nodeArray = new Node[largestX, largestY];
        vGridWorldSize.x = largestX;
        vGridWorldSize.y = largestY;
        xOffset = lowestX * -1;
        yOffset = lowestY * -1;

        // Add nodes
        foreach (Tilemap map in allTileMaps)
        {
            foreach (var pos in map.cellBounds.allPositionsWithin)
            {
                if (!map.HasTile(pos))
                    continue;

                int gridPosX = pos.x + xOffset;
                int gridPosY = pos.y + yOffset;
                // Make new node
                if (nodeArray[gridPosX, gridPosY] == null)
                {
                    // Check tag
                    if (map.tag == wallTag)
                    {
                        nodeArray[gridPosX, gridPosY] = new Node(gridPosX, gridPosY, false);
                        Debug.Log(map.tag);

                    }
                    else if (map.tag == floorTag)
                    {
                        Debug.Log(map.tag);
                        nodeArray[gridPosX, gridPosY] = new Node(gridPosX, gridPosY, true);
                    }
                }
                // Check existing node
                else if (map.tag == wallTag || nodeArray[gridPosX, gridPosY].isWalkable == false || map.tag != floorTag)
                {
                    Debug.Log(map.tag);
                    nodeArray[gridPosX, gridPosY].isWalkable = false;
                }
            }
        }
    }

    public int GetWidth()
    {
        return vGridWorldSize.x;
    }

    public int GetHeight()
    {
        return vGridWorldSize.y;
    }

    public Node GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < vGridWorldSize.x && y < vGridWorldSize.y)
        {
            return nodeArray[x, y];
        }
        else
        {
            return default(Node);
        }
    }
}
