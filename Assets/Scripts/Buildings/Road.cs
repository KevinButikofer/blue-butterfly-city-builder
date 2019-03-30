using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Building
{
    private float range = 2.1f;

    /// <summary>
    /// update reachability of the near building
    /// </summary>
    public void UpdateReacheable()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, range);
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "Building")
            {
                Building b = col.GetComponent<Building>();
                b.IsReachable = true;
            }
        }
    }

    override public List<string> getBuildingInfo()
    {
        List<string> list = new List<string>();
        list.Add("Building: " + this.DisplayName);
        list.Add("Price: " + this.Price);
        list.Add("Power consumption: " + this.Power);
        list.Add("Maintenance cost: " + this.MaintenanceCost);
        list.Add("Description: This is a road dude, what are you expecting?");
        return list;
    }
}
