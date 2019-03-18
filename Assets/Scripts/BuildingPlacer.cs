using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    private Dictionary<string, GameObject> buidingPrefabs = new Dictionary<string, GameObject>();
    [SerializeField]
    private GameObject housePrefab;
    [SerializeField]
    private GameObject roadPrefab;
    [SerializeField]
    private GameObject powerPrefab;
    [SerializeField]
    private GameObject placementZonePrefab;
    private GameObject placementZone;
    [SerializeField]
    private GameObject powerZonePrefab;
    private GameObject powerZone;

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

    public Dictionary<string, GameObject> BuidingPrefabs { get => buidingPrefabs; set => buidingPrefabs = value; }

    private void Awake()
    {
        
        Object[] prefabs = Resources.LoadAll("BuildingPrefabs", typeof(GameObject));
        foreach(Object o in prefabs)
        {
            Building b = (o as GameObject).GetComponent<Building>();
            buidingPrefabs.Add(b.DisplayName, b.gameObject);
        }

       grid = FindObjectOfType<GridManager>();
        curentPrefab = housePrefab;
        curentBuilding = curentPrefab.GetComponent<Building>();

        placementZone = Instantiate(placementZonePrefab, new Vector3(0, -1, 0), new Quaternion());
        powerZone = Instantiate(powerZonePrefab, new Vector3(0, -1, 0), new Quaternion());
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse position
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
                placementZone.transform.localScale = new Vector3(curentBuilding.Size.x, 0.2f, curentBuilding.Size.z);
                
                if (grid.GetNearestPointOnGrid(hitInfo.point, curentBuilding.Size, out Vector3 resultPos))
                {
                    placementZone.GetComponent<Renderer>().material.color = correctPlacementColor;
                    PowerProviderBuilding building = curentBuilding.GetComponent<PowerProviderBuilding>();
                    if(building != null)
                    {
                        powerZone.SetActive(true);
                        powerZone.transform.position = resultPos;
                        powerZone.transform.localScale = new Vector3(building.PowerRange, 0.5f, building.PowerRange);
                    }
                    else
                    {
                        powerZone.SetActive(false);
                    }
                }
                else
                {
                    placementZone.GetComponent<Renderer>().material.color = unCorrectPlacementColor;
                }
                placementZone.transform.position = resultPos;
                placementZone.transform.Translate(new Vector3(0.0f, 0.1f, 0.0f));

            }
            else if (hitInfo.transform.tag == "Building" && Input.GetMouseButtonDown(0))
            {
                Debug.Log(hitInfo.transform.gameObject.GetComponent<Building>().IsPowered);
            }
            else
            {
                placementZone.SetActive(false);
                powerZone.SetActive(false);
            }
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (curentPrefab == housePrefab)
                curentPrefab = roadPrefab;
            else if (curentPrefab == roadPrefab)
                curentPrefab = powerPrefab;
            else
                curentPrefab = housePrefab;

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

    /// <summary>
    /// Place the currnet buiding near the mouse position
    /// </summary>
    /// <param name="clickPoint"></param>
    public void PlaceNearCube(Vector3 clickPoint)
    {
        Building b = curentPrefab.GetComponent<Building>();
        if (grid.GetNearestPointOnGrid(clickPoint, b.Size, out Vector3 finalPosition))
        {
            GameObject obj = Instantiate(curentPrefab);
            obj.transform.position = finalPosition;

            PowerNeedBuilding powerNeedBuilding = b as PowerNeedBuilding;
            powerNeedBuilding?.CheckPowerAvailability();

            Road road = b as Road;
            road?.UpdateReacheable();

            if(b is PowerProviderBuilding)
            { 
                PowerProviderBuilding powerBuilding = b as PowerProviderBuilding;
                powerBuilding.UpdatePower();
                grid.PowerProviderBuildings.Add(powerBuilding);
            }
        }        
    }
    
}
