using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;


public class BuildingPlacer : MonoBehaviour
{
    readonly WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
    private List<GameObject> buildingPrefabs = new List<GameObject>();
    private int currentPrefabIdx = 0;
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
    private ImageBuildingClick[] ImageBuildingClicks;

    private GridManager grid;
    private bool isUsingDestroyTool = false;
    [SerializeField]
    private MyGameManager myGameManager;

    List<TreeInstance> trees;
    List<TreeInstance> toRemove;

    public MyGameManager MyGameManager { get => myGameManager; set => myGameManager = value; }
    public List<GameObject> BuildingPrefabs { get => buildingPrefabs; set => buildingPrefabs = value; }

    

    private void Start()
    {        
        //Load all building prefab in ressource
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
        trees = new List<TreeInstance>(Terrain.activeTerrain.terrainData.treeInstances);
        TryLoadSave();
        SwitchBuilding(0);               
    }

    private void TryLoadSave()
    {
        LoadMyGame loadMyGame = FindObjectOfType<LoadMyGame>();
        if (loadMyGame!=null && loadMyGame.isSaveLoad)
        {
            for (int i = 0; i < loadMyGame.indicesPrefabs.Count(); i++)
            {
                GameObject g = Instantiate(BuildingPrefabs[loadMyGame.indicesPrefabs[i]], buildingsContainer.transform);
                g.transform.position = loadMyGame.pos[i];
                InitBuilding(g, loadMyGame.indicesPrefabs[i]);
            }
            grid.PowerProviderBuildings.ForEach(x => x.UpdatePower());
            cityCenter.UpdateReachability();
        }
    }

    public void OnMouseDown()
    {
        if ((!EventSystem.current.IsPointerOverGameObject() || isUsingDestroyTool) && !myGameManager.isGamePaused)
        {
            HandleMouse();
        }   
    }

    public void OnMouseDrag()
    {
        if ((!EventSystem.current.IsPointerOverGameObject() || isUsingDestroyTool) && !myGameManager.isGamePaused)
        {
            if (curentBuilding is Road || isUsingDestroyTool)
            {
                HandleMouse();
            }
        }
    }

    void HandleMouse()
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
    /// <summary>
    /// Destroy building if using destoy tools
    /// </summary>
    /// <param name="b"> clicked building</param>
    public void OnBuildingClick(Building b)
    {
        if (isUsingDestroyTool && !myGameManager.isGamePaused)
            DestroyClickedBuiding(b);
    }
    private void DestroyClickedBuiding(Building b)
    {
        grid.RemoveBuiding(b);
        if (b is Road)
        {
            GameObject.Destroy(b.transform.parent.gameObject);
            grid.ResetReacheabillity();
            cityCenter.UpdateReachability(b as Road);
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
        if (!EventSystem.current.IsPointerOverGameObject() && !myGameManager.isGamePaused)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                //on empty space with building tool
                if (hitInfo.transform.tag == "terrain" && !isUsingDestroyTool)
                {
                    placementZone.SetActive(true);
                    placementZone.transform.localScale = new Vector3(curentBuilding.Size.x, 0.2f, curentBuilding.Size.z);

                    //Get the grid position
                    if (grid.GetNearestPointOnGrid(hitInfo.point, curentBuilding.Size, out Vector3 resultPos))
                    {
                        placementZone.GetComponent<Renderer>().material.color = correctPlacementColor;

                        //if building provide energy we show the range
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
                SwitchBuilding(currentPrefabIdx, true);
                if(ImageBuildingClicks == null)
                {
                    ImageBuildingClicks = Resources.FindObjectsOfTypeAll<ImageBuildingClick>();
                }
                foreach(ImageBuildingClick image in ImageBuildingClicks)
                {
                    if(image.buildingIndex == currentPrefabIdx)
                    {
                        myGameManager.currentImageBuilding.sprite = image.GetComponent<Image>().sprite;
                    }
                }
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
                    grid.PowerProviderBuildings.ForEach(x => x.ShowPowerZone(false));
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
                InitBuilding(obj);
            }
        }
    }

    private void InitBuilding(GameObject obj, int prefabIdx=-1)
    {
        Building b = obj.GetComponentInChildren<Building>();

        //use when loading save
        if (prefabIdx == -1)
            b.IdxPrefab = currentPrefabIdx;
        else
            b.IdxPrefab = prefabIdx;

        PowerNeedBuilding powerNeedBuilding = b as PowerNeedBuilding;
        powerNeedBuilding?.CheckPowerAvailability();

        Road road = b as Road;
        if (road != null)
        {
            road.UpdateReachable();
        }
        else
        {
            grid.GetStatus(b.Size, b.transform.position, b);
        }

        if (b is PowerProviderBuilding)
        {
            PowerProviderBuilding powerBuilding = b as PowerProviderBuilding;
            powerBuilding.UpdatePower();
            grid.PowerProviderBuildings.Add(powerBuilding);
            powerBuilding.ShowPowerZone(true);
        }

        grid.AddBuiding(b);

        //Remove tree where the building placed
        if(trees == null)
            trees = new List<TreeInstance>(Terrain.activeTerrain.terrainData.treeInstances);
        toRemove = new List<TreeInstance>();
        foreach (TreeInstance tree in trees)
        {
            Vector3 treePosition = Vector3.Scale(tree.position, Terrain.activeTerrain.terrainData.size);
            float treeX = treePosition.x - Terrain.activeTerrain.terrainData.size.x / 2;
            float treeZ = treePosition.z - Terrain.activeTerrain.terrainData.size.z / 2;

            if (treeX <= b.transform.parent.position.x + b.Size.x / 1.8 && treeX >= b.transform.parent.position.x - b.Size.x / 2.2)
            {
                if (treeZ <= b.transform.parent.position.z + b.Size.z / 1.8 && treeZ >= b.transform.parent.position.z - b.Size.z / 2.2)
                {
                    toRemove.Add(tree);
                }
            }
        }
        foreach (TreeInstance item in toRemove) trees.Remove(item);

        //update terrain trees
        Terrain.activeTerrain.terrainData.treeInstances = trees.ToArray();
    }


    /// <summary>
    /// Change the current building to place
    /// </summary>
    /// <param name="index">index of the building in the list</param>
    /// <param name="isQInput"> if building switch with the q key</param>
    public void SwitchBuilding(int index, bool isQInput = false)
    {
        if (isQInput)
        {
            if (index < BuildingPrefabs.Count - 1)
            {
                currentPrefabIdx++;
            }
            else
            {
                currentPrefabIdx = 0;
            }
        }
        else
        {
            currentPrefabIdx = index;
        }
        
        curentPrefab = BuildingPrefabs[currentPrefabIdx];
        curentBuilding = BuildingPrefabs[currentPrefabIdx].GetComponentInChildren<Building>();
        if (curentBuilding is PowerProviderBuilding && !isUsingDestroyTool)
        {
            grid.PowerProviderBuildings.ForEach(x => x.ShowPowerZone(true));
        }    
        else
        {
            grid.PowerProviderBuildings.ForEach(x => x.ShowPowerZone(false));
        }
    }
    
}
