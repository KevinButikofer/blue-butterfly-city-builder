using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerProviderBuilding : Building
{
    [SerializeField]
    private int powerRange = 1;
    

    public int PowerRange { get => powerRange; set => powerRange = value; }
    
    public void UpdatePower(bool powerStatus = true)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, PowerRange);
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "Building")
            {
                Building b = col.GetComponent<Building>(); 
                if(!(b is PowerProviderBuilding))
                    b.IsPowered = powerStatus;
            }
        }
        if(!powerStatus)
        {
            grid.UpdatePower(grid.PowerProviderBuildings.IndexOf(this));
        }
    }
    private void OnDestroy()
    {
        Debug.Log("Destroy");
        UpdatePower(false);
    }
    public bool IsInRange(Vector3 otherPos)
    {
        if(Vector3.Distance(transform.position, otherPos) - powerRange <= 0.0f)
            return true;
        return false;
    }
}
