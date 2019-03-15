using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : PowerNeedBuilding
{
    [SerializeField]
    private int residentCapacity;

    public int ResidentCapacity { get => residentCapacity; }
}
