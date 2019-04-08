using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField]
    private string displayName;
    [SerializeField]
    private int power;
    [SerializeField]
    private int price;
    [SerializeField]
    private int maintenanceCost;
    [SerializeField]
    private bool isPowered = false;
    [SerializeField]
    private bool isReachable = false;
    [SerializeField]
    private Vector3Int size;
    private int idx;
    public static int count=0;
    private Building[] connectedBuidings;
    protected GridManager grid;

    private int idxPrefab;

    public Building()
    {
        this.Idx = count;
        count++;
    }
    public void Awake()
    {
        grid = FindObjectOfType<GridManager>();
        buildingPlacer = FindObjectOfType<BuildingPlacer>();
    }
    public int Power { get => power;}
    public int Price { get => price; }
    public int MaintenanceCost { get => maintenanceCost; }
    public bool IsPowered { get => isPowered; set => isPowered = value; }
    public bool IsReachable { get => isReachable; set
        {
            isReachable = value;
        }
    }
    public Vector3Int Size { get => size; set => size = value; }
    public int Idx { get => idx; set => idx = value; }
    public string DisplayName { get => displayName; set => displayName = value; }
    public int IdxPrefab { get => idxPrefab; set => idxPrefab = value; }

    BuildingPlacer buildingPlacer;

    public void OnMouseDown()
    {
        buildingPlacer.OnBuildingClick(this);
    }
    private void OnMouseDrag()
    {
        buildingPlacer.OnBuildingClick(this);
    }

    /// <summary>
    /// Text display in Ui
    /// </summary>
    /// <returns>String with all building Info</returns>
    public virtual List<string> GetBuildingInfo()
    {
        return null;
    }
}
