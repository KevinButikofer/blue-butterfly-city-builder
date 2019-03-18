using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private readonly int size = 1;
    private Dictionary<int, Building> gridBuilding;
    private Vector3 TerrainSize;
    [SerializeField]
    private Terrain terrain;
    private List<PowerProviderBuilding> powerProviderBuildings = new List<PowerProviderBuilding>();

    public List<PowerProviderBuilding> PowerProviderBuildings { get => powerProviderBuildings; set => powerProviderBuildings = value; }

    public void Start()
    {
        gridBuilding = new Dictionary<int, Building>();
        TerrainSize = terrain.GetComponent<Terrain>().terrainData.size; 
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
        
        resultPos += transform.position;

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
            if (col.gameObject.tag == "Road" && col.gameObject.GetComponent<Building>().IsReachable)
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
        b.Idx = gridBuilding.Count;
        gridBuilding.Add(gridBuilding.Count, b);
    }
    /// <summary>
    /// remove the buiding with the given key
    /// </summary>
    /// <param name="idx">key of the building in the dictonnary</param>
    public void RemoveBuiding(int idx)
    {
        gridBuilding.Remove(idx);
    }
    /// <summary>
    /// reset the power status of all non energy provider building
    /// </summary>
    /// <param name="IdToIgnore">Buiding who is going to be destroy</param>
    public void UpdatePower(int IdToIgnore)
    {
        foreach(KeyValuePair<int, Building> item in gridBuilding)
        {
            if(item.Value is PowerNeedBuilding)
            {
                item.Value.IsPowered = false;
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
}
