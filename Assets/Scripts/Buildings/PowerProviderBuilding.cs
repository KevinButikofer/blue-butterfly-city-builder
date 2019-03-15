using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerProvidingBuiding : Building
{
    [SerializeField]
    private int powerRange = 1;

    public void UpdatePower()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, powerRange);
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "Building")
            {
                Building b = col.GetComponent<Building>();
                b.IsPowered = true;
            }
        }
    }
}
