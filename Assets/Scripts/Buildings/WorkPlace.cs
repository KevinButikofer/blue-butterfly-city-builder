using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkPlace : PowerNeedBuilding
{
    [SerializeField]
    private int workerCapacity;

    public int WorkerCapacity { get => workerCapacity; }

    override public List<string> GetBuildingInfo()
    {
        List<string> list = new List<string>();
        list.Add("Building: " + this.DisplayName);
        list.Add("Price: " + this.Price);
        list.Add("Worker capacity: " + this.workerCapacity);
        list.Add("Power consumption: " + this.Power);
        list.Add("Maintenance cost: " + this.MaintenanceCost);
        list.Add("Description: A place where you can work to your heart content for your obviously wonderful and not boring job");
        return list;
    }
}
