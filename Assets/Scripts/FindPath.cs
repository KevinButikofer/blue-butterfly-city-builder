using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPath : MonoBehaviour
{
    public static int moveSpeed=5;
    public Building dest;
    public List<Road> tryRoads;
    public List<Road> roads;
    readonly static WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);
    private static MyGameManager myGameManager;
    // Start is called before the first frame update
    void Start()
    {
        if(myGameManager == null)
            myGameManager = FindObjectOfType<MyGameManager>();
        FindRoadPath();
        StartCoroutine(FollowPath());
    }
    public void FindRoadPath()
    {
        Collider[] cols = Helper.CheckConnexity4(transform.position, Road.horVect * 1.5f, Road.verVect *1.5f);
        foreach (Collider c in cols)
        {
            Building b = c.GetComponent<Building>();
            if (b != null && b is Road && b.gameObject != gameObject)
            {
                tryRoads.Add(b as Road);
                roads = new List<Road>();
                roads.Add(b as Road);
                if (b is Road)
                {
                    Road road = b as Road;
                    if (FindRoadPath(road))
                        return;
                }
                roads.Remove(b as Road);
            }
        }
        if(roads.Count == 0)
            Destroy(transform.gameObject);        
    }
    public bool FindRoadPath(Road r)
    {
        Collider[] cols = Helper.CheckConnexity4(r.transform.position, Road.horVect / 1.5f, Road.verVect / 1.5f);
        foreach (Collider col in cols)
        {
            
            Building b = col.gameObject.GetComponent<Building>();
            if (b is Road && !tryRoads.Contains(b as Road) && b.gameObject != gameObject)
            {
                Road road = b as Road;
                tryRoads.Add(road);                
                roads.Add(road);
                if (FindRoadPath(road))
                    return true;
                else
                    roads.Remove(road);
            }
            else if (b is Building)
            {
                if (col.gameObject == dest.gameObject)
                {
                    return true;
                }
            }            
        }
        return false;
    }

    IEnumerator FollowPath()
    {
        Vector3 nextPos;
        Vector3 startPos = transform.position;
        foreach (Road r in roads)
        {
            Vector3 dest = r.transform.position - transform.position;
            if (dest != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dest);
            nextPos = r.transform.position;
            float t = 0;
            float step = (moveSpeed / (startPos - nextPos).magnitude) * Time.fixedDeltaTime;
            while (t <= 1.0f)
            {
                t += step; // Goes from 0 to 1, incrementing by step each time           
                transform.position = Vector3.Lerp(startPos, nextPos, t); // Move objectToMove closer to b
                yield return waitForSeconds;         // Leave the routine and return here in the next frame            
            }
            transform.position = nextPos;
            startPos = transform.position;            
        }
        myGameManager.Cars.Remove(gameObject);        
        Destroy(gameObject);
    }
}
