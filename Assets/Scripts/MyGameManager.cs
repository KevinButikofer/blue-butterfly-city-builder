using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class MyGameManager : MonoBehaviour
{
    private float money = 100000;
    private int population = 0;
    private float populationSatisfaction;
    private float unemploymentRate;
    private int jobNumber;
    private float taxes = 1;
    private int populationCapacity;
    [SerializeField]
    private GridManager gridManager;
    private BuildingPlacer buildingPlacer;
    readonly WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
    [SerializeField]
    private GameObject carContainer;
    [SerializeField]
    private GameObject pauseCanvas;
    [SerializeField]
    private GameObject optionCanvas;
    [SerializeField]
    private GameObject UiCanvas;

    [SerializeField]
    private List<GameObject> listPrefabCars;
    private List<GameObject> cars = new List<GameObject>();
    private float nextSpawnTime;

    public bool isGamePaused = false;

    public int Population { get => population; set => population = value; }
    public float PopulationSatisfaction { get => populationSatisfaction; set => populationSatisfaction = value; }
    public float UnemploymentRate { get => unemploymentRate; set => unemploymentRate = value; }
    public int JobNumber { get => jobNumber; set => jobNumber = value; }
    public float Money { get => money; set => money = value; }
    public float Taxes { get => taxes; set => taxes = value; }
    public List<GameObject> Cars { get => cars; set => cars = value; }
    

    private bool isPopulationGrowing;
    private bool isWinningMoney;

    public Image currentImageBuilding;
    public UI_InformationDetailsHandler uI_InformationDetails;


    // Start is called before the first frame update
    void Start()
    {
        buildingPlacer = FindObjectOfType<BuildingPlacer>();
        TryLoadSave();
        gridManager = GetComponent<GridManager>();
        StartCoroutine("UpdateGame");
        pauseCanvas.SetActive(false);
        isGamePaused = false;
        PauseGame();
        PauseGame();
    }
    private void Awake()
    {
        isGamePaused = false;
    }
    private void TryLoadSave()
    {
        LoadMyGame l = FindObjectOfType<LoadMyGame>();
        if(l != null && l.isSaveLoad)
        {
            Money = l.money;
            Population = l.population;
            Taxes = l.taxes;
            buildingPlacer.TryLoadSave();
            UpdateGameUI();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGamePaused)
        { 
            if (gridManager.BuildingExceptRoadCount() > 5)
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();          
        }
    }
    IEnumerator UpdateGame()
    {
        while (true)
        {
            UpdateGameOneSec();
            UpdateGameUI();
            yield return waitForSeconds;
        }
    }
    public void UpdateGameOneSec()
    {
        UpdateGameVar();
        if (populationCapacity > population)
        {
            if (PopulationSatisfaction > 0)
            {
                population += 5;
                isPopulationGrowing = true;
            }               
            else
            {
                population = Mathf.Max(0, population -= 2);
                isPopulationGrowing = false;
            }
        }
        else if(populationCapacity != 0)
            population = populationCapacity;
    }
    /// <summary>
    /// Update game var
    /// </summary>
    public void UpdateGameVar()
    {
        gridManager.UpdateGameVar(out int jobs, out int populationNumber, out int money, out int happyness);
        JobNumber = jobs;        
        populationCapacity = populationNumber;
        unemploymentRate = JobNumber > population ? 0 : 1 - JobNumber / Mathf.Max(population, 1);
        populationSatisfaction = ((-unemploymentRate == 0 ? 0 : -unemploymentRate) * 10) - Taxes * 10f + happyness * 2 + 20;
        float moneyWin = population * Taxes  + Mathf.Min(JobNumber, population) + money;
        Money += moneyWin;
        if (moneyWin > 0)
            isWinningMoney = true;
        else
            isWinningMoney = false;

        if (Money < -10000)
        {
            Debug.Log("You loose git gud Noob");
        }


       // Debug.Log("population : " + population + " on " + populationCapacity + " capacity jobs number : " + JobNumber + " money : " + Money + " Happyness: " + PopulationSatisfaction);
    }
    /// <summary>
    /// Save current game state
    /// </summary>
    public void SaveGame()
    {
        SaveMyGame s = new SaveMyGame(money, population, taxes, gridManager.GridBuilding);       
    }

    public void UpdateGameUI()
    {
        uI_InformationDetails.UpdateGameUI(Money, Population, populationSatisfaction, isWinningMoney, isPopulationGrowing);
    }
    
    /// <summary>
    /// Pause the game and shoe pause menu
    /// </summary>
    public void PauseGame()
    {
        optionCanvas.SetActive(false);
        UiCanvas.SetActive(pauseCanvas.activeSelf);
        pauseCanvas.SetActive(!pauseCanvas.activeSelf);
        isGamePaused = pauseCanvas.activeSelf;
    }

    /// <summary>
    /// Increase taxes
    /// </summary>
    public void IncreaseTaxes()
    {
        taxes += 1;
        uI_InformationDetails.textTaxes.text = taxes.ToString();
    }

    /// <summary>
    /// Decrease taxes
    /// </summary>
    public void DecreaseTaxes()
    {
        taxes -= 1;
        uI_InformationDetails.textTaxes.text = taxes.ToString();
    }
}
