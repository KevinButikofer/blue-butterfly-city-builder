﻿using System.Collections;
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
    private List<PowerProvidingBuiding> powerProviderBuildings;

    public void Start()
    {
        gridBuilding = new Dictionary<int, Building>();
        TerrainSize = terrain.GetComponent<Terrain>().terrainData.size; 
    }
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
    public void AddBuiding(Building b)
    {
        b.Idx = gridBuilding.Count;
        gridBuilding.Add(gridBuilding.Count, b);
    }
    public void RemoveBuiding(int idx)
    {
        gridBuilding.Remove(idx);
    }
    public void UpdatePower()
    {
        powerProviderBuildings.ForEach(a => a.UpdatePower());
    }

}
