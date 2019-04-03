using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MyGameManager : MonoBehaviour
{
    private float money = 1000000;
    private int population = 0;
    private int power = 0;
    private float populationSatisfaction;
    private float unemploymentRate;
    private int jobNumber;
    private float taxes = 1;
    private int populationCapacity;
    [SerializeField]
    private GridManager gridManager;
    readonly WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
    [SerializeField]
    private GameObject carContainer;

    [SerializeField]
    private List<GameObject> listPrefabCars;
    [SerializeField]
    private GameObject prefabCar;
    private List<GameObject> cars = new List<GameObject>();
    private float nextSpawnTime;

    public int Population { get => population; set => population = value; }
    public int Power { get => power; set => power = value; }
    public float PopulationSatisfaction { get => populationSatisfaction; set => populationSatisfaction = value; }
    public float UnemploymentRate { get => unemploymentRate; set => unemploymentRate = value; }
    public int JobNumber { get => jobNumber; set => jobNumber = value; }
    public float Money { get => money; set => money = value; }
    public float Taxes { get => taxes; set => taxes = value; }
    public List<GameObject> Cars { get => cars; set => cars = value; }

    // Start is called before the first frame update
    void Start()
    {
        TryLoadSave();
        gridManager = GetComponent<GridManager>();
        Debug.Log("Data location : " + Application.persistentDataPath);
        StartCoroutine("UpdateGame");        
    }
    private void TryLoadSave()
    {
        LoadMyGame loadMyGame = new LoadMyGame();
        if(loadMyGame.Load())
        {
            Money = loadMyGame.money;
            Population = loadMyGame.population;
            Taxes = loadMyGame.taxes;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (gridManager.BuildingCount() > 5)
        {
            if (nextSpawnTime < 0 && Population / 2 > Cars.Count)
            {
                Building b = Helper.RandomValues(gridManager.GridBuilding).First();
                if (b != null)
                {
                    Random.InitState(System.Environment.TickCount);
                    int r = Random.Range(0, 5);
                    GameObject car = GameObject.Instantiate(listPrefabCars[r], b.transform.position, new Quaternion(), carContainer.transform);
                    car.GetComponent<FindPath>().dest = Helper.RandomValues(gridManager.GridBuilding, b).First();
                    Cars.Add(car);
                    nextSpawnTime = Random.Range(0.05f, 0.3f);
                }
            }
            else
            {
                nextSpawnTime -= Time.deltaTime;
            }
        }
    }
    IEnumerator UpdateGame()
    {
        while (true)
        {
            UpdatePopulation();
            yield return waitForSeconds;
        }
    }
    public void UpdatePopulation()
    {
        UpdateGameVar();
        if (populationCapacity > population)
        {
            if (PopulationSatisfaction > 50)
                population++;
            else
                population = Mathf.Min(0, population--);
        }
        else
            population = populationCapacity;
    }
    public void UpdateGameVar()
    {
        gridManager.UpdateGameVar(out int jobs, out int populationNumber, out int money);
        JobNumber = jobs;
        populationCapacity = populationNumber;
        populationSatisfaction = (jobNumber / Mathf.Max(population, 1) - Taxes * 0.10f) * 100;
        Money += population * Taxes / 10 + Mathf.Min(JobNumber, population) / 10 + money;
        if (Money < -10000)
        {
            Debug.Log("You loose git gud Noob");
        }
        Debug.Log("population : " + population + " on " + populationCapacity + " capacity jobs number : " + JobNumber + " money : " + Money + " Happyness: " + PopulationSatisfaction);
    }
    void OnApplicationQuit()
    {
        SaveMyGame s = new SaveMyGame(money, population, taxes, gridManager.GridBuilding);       
    }
}
