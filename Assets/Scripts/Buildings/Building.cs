using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
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

    public Building()
    {
    }

    public int Power { get => power;}
    public int Price { get => price; }
    public int MaintenanceCost { get => maintenanceCost; }
    public bool IsPowered { get => isPowered; set => isPowered = value; }
    public bool IsReachable { get => isReachable; set => isReachable = value; }
    public Vector3Int Size { get => size; set => size = value; }
    public int Idx { get => idx; set => idx = value; }
}
