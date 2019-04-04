using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;
using System.Linq;

public class LoadMyGame : MonoBehaviour
{
    public float money;
    public int population;
    public float taxes;

    public int[] indices;
    public int[] indicesPrefabs;
    public Vector3[] pos;
    public bool isSaveLoad;
    
    public bool Load()
    {
        try
        {
            if (SaveGame.Exists("money"))
            {
                isSaveLoad = true;
                SaveGame.SavePath = SaveGamePath.PersistentDataPath;

                money = SaveGame.Load<float>("money");
                population = SaveGame.Load<int>("population");
                taxes = SaveGame.Load<float>("taxes");

                indices = SaveGame.Load<int[]>("ids");
                Debug.Log(indices.Count());
                indicesPrefabs = SaveGame.Load<int[]>("indexPrefabs");
                Debug.Log(indicesPrefabs.Count());
                pos = SaveGame.Load<Vector3[]>("pos");
                Debug.Log(pos.Count());
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
}
