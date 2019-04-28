using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityCenter : MonoBehaviour
{
    private readonly float size = 8;
    private Vector3 hor, ver;
    public List<List<Road>> roadsList;

    // Start is called before the first frame update
    void Start()
    {
        hor = new Vector3(size, 0.1f, 0.1f);
        ver = new Vector3(0.1f, 0.1f, size);
    }

    // Update reachability of all building
    public void UpdateReachability(Road objectToIgnore=null)
    {
        roadsList = new List<List<Road>>();
        foreach (Collider c in Helper.CheckConnexity4(transform.position, hor, ver))
        {
            Building b = c.gameObject.GetComponent<Building>();
            List<Road> roads = new List<Road>();
            if (b is Road && !b.Equals(objectToIgnore))
            {
                Road r = b as Road;
                roads.Add(r);
                foreach (Collider col in Helper.CheckConnexity4(r.transform.position, Road.horVect / 1f, Road.verVect / 1f))
                {
                    Building b2 = col.gameObject.GetComponent<Building>();
                    if (b2 is Road && !roads.Contains(b2 as Road) && !(b2 as Road).Equals(objectToIgnore))
                    {
                        r.IsReachable = true;
                        roads.Add(b2 as Road);
                    }
                }
                roadsList.Add(roads);
            }
        }
        foreach(List<Road> r in roadsList)
        {
            foreach(Road road in r)
            {
                road.UpdateReachability2();
                road.UpdateReachable();
            }
        }
    }
}
