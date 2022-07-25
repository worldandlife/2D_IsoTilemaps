using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public Grid TileGrid;
    public Pathfinder pathFinder;

    void Start()
    {
        pathFinder = new Pathfinder(TileGrid, "Blocking", "Floor");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            var endPoint = TileGrid.WorldToCell(mouseWorldPosition);
            Debug.Log("CELL" + endPoint);
            List<Node> path = pathFinder.FindPath(2, 2, endPoint.x, endPoint.y);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(TileGrid.GetCellCenterWorld(new Vector3Int(path[i].x + 1, path[i].y + 1, 0)), TileGrid.GetCellCenterWorld(new Vector3Int(path[i + 1].x + 1, path[i + 1].y + 1, 0)), Color.green, 5f);
                }
            }

        }
    }

    // Get Mouse Position in World with Z = 0f
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
