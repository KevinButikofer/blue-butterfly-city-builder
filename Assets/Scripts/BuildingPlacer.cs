using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;


public class BuildingPlacer : MonoBehaviour
{
    readonly WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
    private List<GameObject> buildingPrefabs = new List<GameObject>();
    private int currentIdx = 0;
    //private List<string> buildingLabels = new List<string>();
    [SerializeField]
    private GameObject buildingsContainer;
    [SerializeField]
    private GameObject placementZonePrefab;
    [SerializeField]
    private CityCenter cityCenter;
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
    [SerializeField]
    private MyGameManager myGameManager;
    public MyGameManager MyGameManager { get => myGameManager; set => myGameManager = value; }
    public List<GameObject> BuildingPrefabs { get => buildingPrefabs; set => buildingPrefabs = value; }

    

    private void Start()
    {
        Object[] prefabs = Resources.LoadAll("BuildingPrefabs", typeof(GameObject));
        foreach(Object o in prefabs)
        {
            GameObject b = o as GameObject;
            BuildingPrefabs.Add(b);
        }

        grid = FindObjectOfType<GridManager>();
        curentPrefab = BuildingPrefabs[0];
        curentBuilding = curentPrefab.GetComponentInChildren<Building>();

        placementZone = Instantiate(placementZonePrefab, new Vector3(0, -1, 0), new Quaternion());
        powerZone = Instantiate(powerZonePrefab, new Vector3(0, -1, 0), new Quaternion());
    }
    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject() || isUsingDestroyTool)
        {
            handleMouse();
        }   
    }

    public void OnMouseDrag()
    {
        if (!EventSystem.current.IsPointerOverGameObject() || isUsingDestroyTool)
        {
            if (curentBuilding is Road || isUsingDestroyTool)
            {
                handleMouse();
            }
        }
    }

    void handleMouse()
    {
        //Debug.Log("handlemouse, isusingdestroytools: "+isUsingDestroyTool);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.transform.tag == "terrain" && !isUsingDestroyTool)
            {
                PlaceNearCube(hitInfo.point);
            }
            if (hitInfo.transform.tag == "Building" && isUsingDestroyTool)
            {
                DestroyClickedBuiding(hitInfo.transform.GetComponent<Building>());
            }
        }
    }
    public void OnBuildingClick(Building b)
    {
        if (isUsingDestroyTool)
            DestroyClickedBuiding(b);
    }
    private void DestroyClickedBuiding(Building b)
    {
        grid.RemoveBuiding(b);
        if (b is Road)
        {
            GameObject.Destroy(b.transform.parent.gameObject);
            grid.ResetReacheabillity();
            cityCenter.UpdateReacheability(b as Road);
        }
        else
        {
            GameObject.Destroy(b.transform.parent.gameObject);
        }
       
    }
    // Update is called once per frame
    void Update()
    {
       
        //Mouse position
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.transform.tag == "terrain" && !isUsingDestroyTool)
                {
                    placementZone.SetActive(true);
                    placementZone.transform.localScale = new Vector3(curentBuilding.Size.x, 0.2f, curentBuilding.Size.z);

                    if (grid.GetNearestPointOnGrid(hitInfo.point, curentBuilding.Size, out Vector3 resultPos))
                    {
                        placementZone.GetComponent<Renderer>().material.color = correctPlacementColor;
                        PowerProviderBuilding building = curentBuilding.GetComponent<PowerProviderBuilding>();
                        if (building != null)
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
                    Debug.Log(hitInfo.transform.gameObject.GetComponent<Building>().IsReachable);
                }
                else
                {
                    placementZone.SetActive(false);
                    powerZone.SetActive(false);
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                switchBuilding(currentIdx, true);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
               
                /*Building b = Helper.RandomValues(grid.GridBuilding).First();
                if (b != null)
                {
                    GameObject car = GameObject.Instantiate(prefabCar, b.transform.position, new Quaternion());
                    car.GetComponent<FindPath>().dest = Helper.RandomValues(grid.GridBuilding, b).First();
                    cars.Add(car);
                }  */
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isUsingDestroyTool)
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

        else
        {
            placementZone.SetActive(false);
        }
    }
    
    

    /// <summary>
    /// Place the currnet buiding near the mouse position
    /// </summary>
    /// <param name="clickPoint"></param>
    public void PlaceNearCube(Vector3 clickPoint)
    {

        Building b = curentPrefab.GetComponentInChildren<Building>();
        if (myGameManager.Money - b.Price > 0)
        {
            if (grid.GetNearestPointOnGrid(clickPoint, b.Size, out Vector3 finalPosition))
            {
                GameObject obj = Instantiate(curentPrefab, buildingsContainer.transform);
                obj.transform.position = finalPosition;

                b = obj.GetComponentInChildren<Building>();

                PowerNeedBuilding powerNeedBuilding = b as PowerNeedBuilding;
                powerNeedBuilding?.CheckPowerAvailability();

                Road road = b as Road;
                if (road != null)
                {
                    road.UpdateReacheable();
                }
                else
                {
                    grid.GetStatus(b.Size, b.transform.position,b);
                }

                if (b is PowerProviderBuilding)
                {
                    PowerProviderBuilding powerBuilding = b as PowerProviderBuilding;
                    powerBuilding.UpdatePower();
                    grid.PowerProviderBuildings.Add(powerBuilding);
                    powerBuilding.ShowPowerZone(true);
                    
                }
                
                grid.AddBuiding(b);
                List<TreeInstance> trees = new List<TreeInstance>(Terrain.activeTerrain.terrainData.treeInstances);
                List<TreeInstance> toRemove = new List<TreeInstance>();
                foreach (TreeInstance tree in trees)
                {
                    Vector3 treePosition = Vector3.Scale(tree.position, Terrain.activeTerrain.terrainData.size);
                    float treeX = treePosition.x - Terrain.activeTerrain.terrainData.size.x / 2;
                    float treeZ = treePosition.z - Terrain.activeTerrain.terrainData.size.z / 2;

                    if ( treeX <= b.transform.parent.position.x + b.Size.x/1.8  && treeX >= b.transform.parent.position.x - b.Size.x / 2.2)
                    {
                        if(treeZ <= b.transform.parent.position.z + b.Size.z/1.8 && treeZ >= b.transform.parent.position.z - b.Size.z /2.2)
                        {
                            Debug.Log("in if");
                            toRemove.Add(tree);
                        }
                        

                    }
                }
                foreach (TreeInstance item in toRemove) trees.Remove(item);
                Terrain.activeTerrain.terrainData.treeInstances = trees.ToArray();


            }
        }
    }

    public void switchBuilding(int index, bool isQInput = false)
    {
        if (isQInput)
        {
            if (index < BuildingPrefabs.Count - 1)
            {
                currentIdx++;
            }
            else
            {
                currentIdx = 0;
            }
        }
        else
        {
            currentIdx = index;
        }

        /*if (curentPrefab == housePrefab)
            curentPrefab = roadPrefab;
        else if (curentPrefab == roadPrefab)
            curentPrefab = powerPrefab;
        else
            curentPrefab = housePrefab;*/

        curentPrefab = BuildingPrefabs[currentIdx];
        curentBuilding = BuildingPrefabs[currentIdx].GetComponentInChildren<Building>();
        if (curentBuilding is PowerProviderBuilding)
        {
            grid.PowerProviderBuildings.ForEach(x => x.ShowPowerZone(true));
        }    
        else
        {
            grid.PowerProviderBuildings.ForEach(x => x.ShowPowerZone(false));
        }
    }
    
}
