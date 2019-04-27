using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private readonly int size = 1;
    private Dictionary<int, Building> gridBuilding = new Dictionary<int, Building>();
    private Vector3 TerrainSize;
    [SerializeField]
    private Terrain terrain;
    private Vector3 terrainCenter;
    private List<PowerProviderBuilding> powerProviderBuildings = new List<PowerProviderBuilding>();

    public List<PowerProviderBuilding> PowerProviderBuildings { get => powerProviderBuildings; set => powerProviderBuildings = value; }
    public Dictionary<int, Building> GridBuilding { get => gridBuilding; set => gridBuilding = value; }

    public void Start()
    {
        TerrainSize = terrain.GetComponent<Terrain>().terrainData.size;
        terrainCenter = new Vector3(terrain.transform.position.x + TerrainSize.x / 2, 0, terrain.transform.position.z + TerrainSize.z / 2);
    }
    

    /// <summary>
    /// Get the neareast point near of the user click and return is avaiblility 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="buildingSize"></param>
    /// <param name="resultPos"></param>
    /// <returns></returns>
    public bool GetNearestPointOnGrid(Vector3 position, Vector3 buildingSize, out Vector3 resultPos)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        resultPos = new Vector3(
            (float)xCount * size,
            position.y,
            (float)zCount * size);

        if (resultPos.x > TerrainSize.x / 2)
            return false;
        
        resultPos += transform.position;
        Bounds bounds = new Bounds(terrainCenter, TerrainSize / 1.03f);
        if(!bounds.Contains(resultPos))
            return false;

        Collider[] cols = Physics.OverlapBox(resultPos, buildingSize/2.05f, new Quaternion());
        foreach(Collider col in cols)
        {
            if(col.gameObject.tag == "Building" || col.gameObject.tag == "Mountain" )
            {
                return false;
            }
            if(col.gameObject.transform.parent != null && col.gameObject.transform.parent.name == "CityCenter")
            {
                return false;
            }
        }       
        return true;        
    }
    /// <summary>
    /// Check if the buiding is recheable by the road
    /// </summary>
    /// <param name="buildingSize"></param>
    /// <param name="pos"></param>
    /// <param name="b"></param>
    public void GetStatus(Vector3 buildingSize, Vector3 pos, Building b)
    {
        buildingSize = new Vector3(buildingSize.x + 1, buildingSize.y + 1, buildingSize.z + 1);
        foreach (Collider col in Helper.CheckConnexity4(pos, new Vector3(buildingSize.x, 0.1f, 0.1f), new Vector3(0.1f, 0.1f, buildingSize.z)))
        {
            if (col.gameObject.GetComponent<Building>() != null && col.gameObject.GetComponent<Building>().IsReachable && col.gameObject.GetComponent<Building>() is Road)
            {                
               b.IsReachable = true;
            }
        }
    }
    /// <summary>
    /// Add buiding to the buiding dictionnary
    /// </summary>
    /// <param name="b"></param>
    public void AddBuiding(Building b)
    {
        GridBuilding.Add(b.Idx, b);
    }
    /// <summary>
    /// remove the buiding with the given key
    /// </summary>
    /// <param name="idx">key of the building in the dictonnary</param>
    public void RemoveBuiding(Building b)
    {
        GridBuilding.Remove(b.Idx);
    }
    /// <summary>
    /// reset the power status of all non energy provider building
    /// </summary>
    /// <param name="IdToIgnore">Buiding who is going to be destroy</param>
    public void UpdatePower(int IdToIgnore)
    {
        foreach(KeyValuePair<int, Building> item  in GridBuilding)
        {
            if(item.Value is PowerNeedBuilding)
            {
                item.Value.IsPowered = false;
            }            
        }
        for(int i = 0; i < powerProviderBuildings.Count - 1; i++)
        {
            if (i != IdToIgnore && powerProviderBuildings[i]!=null)
            {
                powerProviderBuildings[i].UpdatePower();
            }
        }
    }
    public void ResetReacheabillity()
    {
        int i = 0;
        foreach (KeyValuePair<int, Building> item in GridBuilding)
        {
            i++;
            item.Value.IsReachable = false;
        }
    }
    /// <summary>
    /// Check the power availability at the buiding place
    /// </summary>
    /// <param name="pos">Position of the building</param>
    /// <returns></returns>
    public bool CheckPowerAvailability(Building b)
    {
        foreach(PowerProviderBuilding powerProvider in PowerProviderBuildings)
        {
            if(powerProvider.IsInRange(b.transform.position))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Update the game variable
    /// </summary>
    /// <param name="nbJobs">out total number of job</param>
    /// <param name="habitantCapacity">out total number of rensident capacity</param>
    /// <param name="money">out money win or loose</param>
    public void UpdateGameVar(out int nbJobs, out int habitantCapacity, out int money)
    {
        nbJobs = 0;
        habitantCapacity = 0;
        money = 0;
        foreach (KeyValuePair<int, Building> item in GridBuilding)
        {
            if (item.Value.IsReachable && item.Value.IsPowered)
            {
                if (item.Value is WorkPlace)
                {
                    nbJobs += (item.Value as WorkPlace).WorkerCapacity;
                }
                if (item.Value is Home)
                {
                    habitantCapacity += (item.Value as Home).ResidentCapacity;
                }
            }
            money -= item.Value.MaintenanceCost;
        }
    }

    /// <summary>
    /// return number of building that aren't road
    /// </summary>
    /// <returns></returns>
    public int BuildingExceptRoadCount()
    {
        return gridBuilding.Where(x => !(x.Value is Road)).Count();
    }

}
