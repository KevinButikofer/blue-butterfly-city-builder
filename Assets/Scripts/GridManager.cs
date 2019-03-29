using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private readonly int size = 1;
    private List<Building> gridBuilding = new List<Building>();
    private Vector3 TerrainSize;
    [SerializeField]
    private Terrain terrain;
    private Vector3 terrainCenter;
    private List<PowerProviderBuilding> powerProviderBuildings = new List<PowerProviderBuilding>();

    public List<PowerProviderBuilding> PowerProviderBuildings { get => powerProviderBuildings; set => powerProviderBuildings = value; }

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
            if(col.gameObject.tag == "Building")
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
        Collider[] cols = Physics.OverlapBox(pos, buildingSize / 1.95f, new Quaternion());
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "Road" && col.gameObject.GetComponentInParent<Building>().IsReachable)
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
        gridBuilding.Add(b);
    }
    /// <summary>
    /// remove the buiding with the given key
    /// </summary>
    /// <param name="idx">key of the building in the dictonnary</param>
    public void RemoveBuiding(Building b)
    {
        gridBuilding.Remove(b);
    }
    /// <summary>
    /// reset the power status of all non energy provider building
    /// </summary>
    /// <param name="IdToIgnore">Buiding who is going to be destroy</param>
    public void UpdatePower(int IdToIgnore)
    {
        foreach(Building b in gridBuilding)
        {
            if(b is PowerNeedBuilding)
            {
                b.IsPowered = false;
            }            
        }
        for(int i = 0; i < powerProviderBuildings.Count - 1; i++)
        {
            if (i != IdToIgnore)
            {
                powerProviderBuildings[i].UpdatePower();
            }
        }
    }
    /// <summary>
    /// Check the power availability at the buiding place
    /// </summary>
    /// <param name="pos">Position of the building</param>
    /// <returns></returns>
    public bool CheckPowerAvailability(Vector3 pos)
    {
        foreach(PowerProviderBuilding b in PowerProviderBuildings)
        {
            if(b.IsInRange(pos))
            {
                return true;
            }
        }
        return false;
    }

    public void UpdateGameVar(out int nbJobs, out int habitantCapacity, out int money)
    {
        nbJobs = 0;
        habitantCapacity = 0;
        money = 0;
        foreach (Building b in gridBuilding)
        {            
            if (b is WorkPlace)
            {
                nbJobs += (b as WorkPlace).WorkerCapacity;
            }
            if (b is Home)
            {
                habitantCapacity += (b as Home).ResidentCapacity;
            }
            money -= b.MaintenanceCost;
        }
    }

}
