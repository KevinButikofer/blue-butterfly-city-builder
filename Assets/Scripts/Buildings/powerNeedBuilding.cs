using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerNeedBuilding : Building
{
    // Start is called before the first frame update
    public void CheckPowerAvailability()
    {
        grid = FindObjectOfType<GridManager>();
        IsPowered = grid.CheckPowerAvailability(this);
    }
}
