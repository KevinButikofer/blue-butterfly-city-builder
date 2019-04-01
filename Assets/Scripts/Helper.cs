using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Helper
{
    public static Collider[] CheckConnexity4(Vector3 pos, Vector3 hor, Vector3 ver)
    {
        Collider[] cols = Physics.OverlapBox(pos, hor);
        Collider[] cols2 = Physics.OverlapBox(pos, ver);
        return cols.Concat(cols2).ToArray();
    }
    public static IEnumerable<Building> RandomValues(Dictionary<int, Building> dict, Building notSame=null)
    {
        Random.InitState(System.Environment.TickCount);
        List<Building> values = Enumerable.ToList(dict.Values);
        int size = dict.Count;
        while (true)
        {
            int r;
            do
            {
                r = Random.Range(0, size);
            }
            while (!values[r].IsReachable || values[r].Equals(notSame) || values[r] is Road);
            yield return values[r];
        }
    }
}
