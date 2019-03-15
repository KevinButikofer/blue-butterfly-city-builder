using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkPlace : PowerNeedBuilding
{
    [SerializeField]
    private int workerCapacity;

    public int WorkerCapacity { get => workerCapacity; }
}
