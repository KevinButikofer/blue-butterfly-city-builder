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
}
