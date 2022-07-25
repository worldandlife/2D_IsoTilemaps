using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfinderDemo : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    public Grid TileGrid;
    public Pathfinder pathFinder;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    IsometricCharacterRenderer isoRenderer;


    void Start()
    {
        pathFinder = new Pathfinder(TileGrid, "Blocking", "Floor");
    }

    void Update()
    {
        HandleMovement();

        if (Input.GetMouseButtonDown(0))
        {
            SetTargetPosition(GetMouseWorldPosition());
        }
    }

    Rigidbody2D rbody;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
    }
    private void HandleMovement()
    {
        if (pathVectorList != null)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > .5f) {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                isoRenderer.SetDirection(moveDir);
                rbody.MovePosition(Vector2.MoveTowards(rbody.position, targetPosition, speed * Time.deltaTime));
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    StopMoving();
                    isoRenderer.SetDirection(Vector3.zero);
                }
            }
        }
        else
        {
            isoRenderer.SetDirection(Vector3.zero);
        }
    }

    private void StopMoving()
    {
        pathVectorList = null;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = Pathfinder.Instance.FindPath(GetPosition(), targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
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