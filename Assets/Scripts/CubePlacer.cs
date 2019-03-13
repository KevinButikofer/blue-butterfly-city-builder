using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject housePrefab;
    private Grid grid;
    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.transform.tag == "terrain")
                {
                    PlaceNearCube(hitInfo.point);
                }
            }
        }
    }
    void PlaceNearCube(Vector3 clickPoint)
    {
        Vector3 finalPosition = grid.GetNearestPointOnGrid(clickPoint);
        GameObject obj = Instantiate(housePrefab);
        obj.transform.position = finalPosition;
        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = finalPosition;
    }
    
}
