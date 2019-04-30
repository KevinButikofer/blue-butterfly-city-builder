using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerProviderBuilding : Building
{
    [SerializeField]
    private int powerRange = 1;
    
    public int PowerRange { get => powerRange; set => powerRange = value; }

    [SerializeField]
    private int workerCapacity;

    public int WorkerCapacity { get => workerCapacity; }

    /// <summary>
    /// Give power in the range of the buiding
    /// </summary>
    /// <param name="powerStatus"></param>
    public void UpdatePower(bool powerStatus = true)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, PowerRange);
        foreach (Collider col in cols)
        {
            if (col.gameObject != null && col.gameObject.tag == "Building")
            {
                print("power");
                Building b = col.GetComponent<Building>();
                if(!(b is PowerProviderBuilding))
                    b.IsPowered = true;
            }
        }
        if(!powerStatus)
        {
            grid.UpdatePower(grid.PowerProviderBuildings.IndexOf(this));
        }
    }
    private void OnDestroy()
    {
        UpdatePower(false);
    }
    /// <summary>
    /// is the pos in the range of the buiding
    /// </summary>
    /// <param name="otherPos"></param>
    /// <returns></returns>
    public bool IsInRange(Vector3 otherPos)
    {
        if(Vector3.Distance(transform.position, otherPos) - powerRange <= 0.0f)
            return true;
        return false;
    }

    override public List<string> GetBuildingInfo()
    {
        List<string> list = new List<string>();
        list.Add("Building: " + this.DisplayName);
        list.Add("Price: " + this.Price);
        list.Add("Worker capacity: " + this.workerCapacity);
        list.Add("Maintenance cost: " + this.MaintenanceCost);
        list.Add("Description: So...you want some of my electricity, do you? Well, for once the rich white man is in control!");
        return list;
    }

    /// <summary>
    /// hide or show the power zone
    /// </summary>
    /// <param name="b">show power zone or not</param>
    public void ShowPowerZone(bool b)
    {
        transform.parent.Find("powerZone").gameObject.SetActive(b && IsReachable);
    }
}
