using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Helper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static Collider[] CheckConnexity4(Vector3 pos, Vector3 hor, Vector3 ver)
    {
        Collider[] cols = Physics.OverlapBox(pos, hor);
        Collider[] cols2 = Physics.OverlapBox(pos, ver);
        return cols.Concat(cols2).ToArray();
    }
}
