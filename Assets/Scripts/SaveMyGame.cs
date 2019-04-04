using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;
using System.Linq;

public class SaveMyGame 
{
    public SaveMyGame(float money, int population, float taxes, Dictionary<int, Building> gridBuildings)
    {
        SaveGame.SavePath = SaveGamePath.PersistentDataPath;
        SaveGame.Save<float>("money", money);
        SaveGame.Save<int>("population", population);
        SaveGame.Save<float>("taxes", taxes);

        SaveGame.Save<int[]>("ids", gridBuildings.Keys.ToArray());
        SaveGame.Save<int[]>("indexPrefabs", gridBuildings.Values.Select(x => x.IdxPrefab).ToArray());
        SaveGame.Save<Vector3[]>("pos", gridBuildings.Values.Select(x => x.transform.parent.position).ToArray());
    }
}
