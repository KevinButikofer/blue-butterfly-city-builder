using System.Collections.Generic;
using UnityEngine;

public class HappinessBuilding : PowerNeedBuilding
{
    [SerializeField]
    private int happiness;

    public int Happiness { get => happiness; set => happiness = value; }

    override public List<string> GetBuildingInfo()
    {
        List<string> list = new List<string>();
        list.Add("Building: " + this.DisplayName);
        list.Add("Price: " + this.Price);
        list.Add("Happiness: " + this.Happiness);
        list.Add("Maintenance cost: " + this.MaintenanceCost);
        list.Add("Bring happiness to your people");
        return list;
    }

}
