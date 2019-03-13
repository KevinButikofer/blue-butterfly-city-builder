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

    public int Power { get => power;}
    public int Price { get => price; }
    public int MaintenanceCost { get => maintenanceCost; }
    public bool IsPowered { get => isPowered; }
    public bool IsReachable { get => isReachable; }
}
