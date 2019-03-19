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
    private Building[] connectedBuidings;
    protected GridManager grid;

    public void Awake()
    {
        grid = FindObjectOfType<GridManager>();
    }
    public int Power { get => power;}
    public int Price { get => price; }
    public int MaintenanceCost { get => maintenanceCost; }
    public bool IsPowered { get => isPowered; set => isPowered = value; }
    public bool IsReachable { get => isReachable; set => isReachable = value; }
    public Vector3Int Size { get => size; set => size = value; }
    public int Idx { get => idx; set => idx = value; }
    public string DisplayName { get => displayName; set => displayName = value; }

    BuildingPlacer buildingPlacer;

    // Start is called before the first frame update
    void Start()
    {
        buildingPlacer = FindObjectOfType<BuildingPlacer>();
    }    
    public void OnMouseDown()
    {
        buildingPlacer.OnBuildingClick(this);
    }
    private void OnMouseDrag()
    {
        buildingPlacer.OnBuildingClick(this);
    }
}
