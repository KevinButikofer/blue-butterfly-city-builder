using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerProviderBuilding : Building
{
    [SerializeField]
    private int powerRange = 1;
    private int remainingPower;
    
    public int PowerRange { get => powerRange; set => powerRange = value; }
    public int RemainingPower { get => remainingPower; set => remainingPower = value; }

    /// <summary>
    /// Give power in the range of the buiding
    /// </summary>
    /// <param name="powerStatus"></param>
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
}
