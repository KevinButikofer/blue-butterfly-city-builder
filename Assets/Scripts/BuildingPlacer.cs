using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{

    [SerializeField]
    private GameObject housePrefab;
    [SerializeField]
    private GameObject roadPrefab;
    [SerializeField]
    private GameObject placementZonePrefab;
    private GameObject placementZone;
    [SerializeField]
    private Color correctPlacementColor;
    [SerializeField]
    private Color unCorrectPlacementColor;
    [SerializeField]
    private Texture2D bulldozerCursor;
    private Vector2 hotSpot = Vector2.zero;

    private GameObject curentPrefab;
    private Building curentBuilding;

    private GridManager grid;
    private bool isUsingDestroyTool = false;

    private void Awake()
    {
        grid = FindObjectOfType<GridManager>();
        curentPrefab = housePrefab;
        curentBuilding = curentPrefab.GetComponent<Building>();

        placementZone = Instantiate(placementZonePrefab, new Vector3(0, -1, 0), new Quaternion());
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (Input.GetMouseButton(0))
            {
                if (hitInfo.transform.tag == "terrain" && !isUsingDestroyTool)
                {
                    PlaceNearCube(hitInfo.point);
                }
                if (hitInfo.transform.tag == "Building" && isUsingDestroyTool)
                {
                    grid.RemoveBuiding(hitInfo.transform.GetComponent<Building>().Idx);
                    GameObject.Destroy(hitInfo.transform.gameObject);
                }
            }
            if (hitInfo.transform.tag == "terrain" && !isUsingDestroyTool)
            {
                placementZone.SetActive(true);
                placementZone.transform.localScale = new Vector3(curentBuilding.Size.x, 0.1f, curentBuilding.Size.z);
                if (grid.GetNearestPointOnGrid(hitInfo.point, curentBuilding.Size, out Vector3 resultPos))
                {
                    placementZone.GetComponent<Renderer>().material.color = correctPlacementColor;
                }
                else
                {
                    placementZone.GetComponent<Renderer>().material.color = unCorrectPlacementColor;
                }
                placementZone.transform.position = resultPos;

            }
            else if (hitInfo.transform.tag == "Building" && Input.GetMouseButtonDown(0))
            {
                Debug.Log(hitInfo.transform.gameObject.GetComponent<Building>().IsReachable);
            }
            else
            {
                placementZone.SetActive(false);
            }
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(curentPrefab == roadPrefab)
                curentPrefab = housePrefab;
            else
                curentPrefab = roadPrefab;
            curentBuilding = curentPrefab.GetComponent<Building>();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(isUsingDestroyTool)
            {
                isUsingDestroyTool = false;
                Cursor.SetCursor(null, hotSpot, CursorMode.Auto);
            }
            else
            {
                isUsingDestroyTool = true;
                Cursor.SetCursor(bulldozerCursor, hotSpot, CursorMode.Auto);
            }
        }
    }

    public void PlaceNearCube(Vector3 clickPoint)
    {
        Building b = curentPrefab.GetComponent<Building>();
        if (grid.GetNearestPointOnGrid(clickPoint, b.Size, out Vector3 finalPosition))
        {
            GameObject obj = Instantiate(curentPrefab);
            obj.transform.position = finalPosition;

            Road road = obj.GetComponent<Road>();
            road?.UpdateReacheable();
        }        
    }
    
}
