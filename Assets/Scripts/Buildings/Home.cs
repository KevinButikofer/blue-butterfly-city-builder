using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : PowerNeedBuilding
{
    [SerializeField]
    private int residentCapacity;

    public int ResidentCapacity { get => residentCapacity; }

    override public List<string> getBuildingInfo()
    {
        List<string> list = new List<string>();
        list.Add("Building: " + this.DisplayName);
        list.Add("Price: " + this.Price);
        list.Add("Resident capacity: " + this.residentCapacity);
        list.Add("Power consumption: " + this.Power);
        list.Add("Maintenance cost: " + this.MaintenanceCost);
        list.Add("Description: Small cozy house for happy moderately rich people");
        return list;
    }

}
