using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerNeedBuilding : Building
{
    /// <summary>
    /// Check if în range of one of the power provider
    /// </summary>
    public void CheckPowerAvailability()
    {
        grid = FindObjectOfType<GridManager>();
        IsPowered = grid.CheckPowerAvailability(this);
    }
}
