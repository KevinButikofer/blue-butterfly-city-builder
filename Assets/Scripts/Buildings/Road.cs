using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Building
{
    private float range = 2.1f;
    public  static Vector3 horVect = new Vector3(0.1f, 0.1f, 2.1f);
    public static Vector3 verVect = new Vector3(2.1f, 0.1f, 0.1f);
    public void Start()
    {
        foreach (Collider col in Helper.CheckConnexity4(transform.position, horVect / 1.5f, verVect / 1.5f))
        {
            if (col.transform.parent != null && col.transform.parent.name == "CityCenter")
            {
                IsReachable = true;
            }
            else if (col.gameObject.GetComponent<Building>() != null && col.gameObject.GetComponent<Building>().IsReachable && col.gameObject.GetComponent<Building>() is Road)
            {
                IsReachable = true;
            }
        }
        if(IsReachable)
        {
            UpdateReacheable2();
        }
    }
    public void UpdateReacheable2()
    {
        Collider[] cols = Helper.CheckConnexity4(transform.position, horVect / 1.5f, verVect / 1.5f);
        foreach (Collider col in cols)
        {
            Building b = col.gameObject.GetComponent<Building>();
            if (b is Road)
            {
                Road r = b as Road;
                if (!r.IsReachable)
                {
                    r.IsReachable = true;
                    r.UpdateReacheable2();                   
                }
            }
        }
    }
    public void UpdateReacheable()
    {
        if (IsReachable)
        {
            Collider[] cols = Helper.CheckConnexity4(transform.position, horVect / 1.5f, verVect / 1.5f);
            foreach (Collider col in cols)
            {
                if (col.gameObject.tag == "Building")
                {
                    Building b = col.GetComponent<Building>();
                    if (!(b is Road))
                    {
                        b.IsReachable = true;
                    }
                }
               
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
