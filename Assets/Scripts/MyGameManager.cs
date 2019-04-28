using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


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
    private GameObject pauseCanvas;
    [SerializeField]
    private GameObject UiCanvas;

    [SerializeField]
    private List<GameObject> listPrefabCars;
    [SerializeField]
    private GameObject prefabCar;
    private List<GameObject> cars = new List<GameObject>();
    private float nextSpawnTime;

    public bool isGamePaused = false;

    public int Population { get => population; set => population = value; }
    public int Power { get => power; set => power = value; }
    public float PopulationSatisfaction { get => populationSatisfaction; set => populationSatisfaction = value; }
    public float UnemploymentRate { get => unemploymentRate; set => unemploymentRate = value; }
    public int JobNumber { get => jobNumber; set => jobNumber = value; }
    public float Money { get => money; set => money = value; }
    public float Taxes { get => taxes; set => taxes = value; }
    public List<GameObject> Cars { get => cars; set => cars = value; }

    public Text textTaxes;
    public Image imageMoneyDetails;
    public Image imagePopulationDetails;
    public Text textMoneyDetails;
    public Text textPopulationDetails;
    public Text textHappinessDetails;
    public Text textMoney;
    public Text textPopulation;
    public Text textHappiness;
    public Image imageMoney;
    public Image imagePopulation;
    public Sprite upArrow;
    public Sprite downArrow;
    private bool isPopulationGrowing;
    private bool isWinningMoney;
    private Color colorGreen = new Color32(0, 224, 68, 255);
    private Color colorRed = new Color32(255, 0, 0, 255);


    // Start is called before the first frame update
    void Start()
    {        
        TryLoadSave();
        gridManager = GetComponent<GridManager>();
        StartCoroutine("UpdateGame");
        pauseCanvas.SetActive(false);
        isGamePaused = false;
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
            if (PopulationSatisfaction > 50)
            {
                population++;
                isPopulationGrowing = true;
            }               
            else
            {
                population = Mathf.Min(0, population--);
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
        gridManager.UpdateGameVar(out int jobs, out int populationNumber, out int money);
        JobNumber = jobs;
        populationCapacity = populationNumber;
        populationSatisfaction = (jobNumber / Mathf.Max(population, 1) - Taxes * 0.10f) * 100;
        float moneyWin = population * Taxes / 10 + Mathf.Min(JobNumber, population) / 10 + money;
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
        Debug.Log("updating ui");

        textMoney.text = money.ToString();
        textPopulation.text = population.ToString();
        textHappiness.text = populationSatisfaction.ToString();

        textMoneyDetails.text = money.ToString();
        textPopulationDetails.text = population.ToString();
        textHappinessDetails.text = populationSatisfaction.ToString();

        if (isWinningMoney)
        {
            imageMoney.sprite = upArrow;
            imageMoney.color = colorGreen;
            imageMoneyDetails.sprite = upArrow;
            imageMoneyDetails.color = colorGreen;
        }
        else
        {
            imageMoney.sprite = downArrow;
            imageMoney.color = colorRed;
            imageMoneyDetails.sprite = downArrow;
            imageMoneyDetails.color = colorRed;
        }
        if (isPopulationGrowing)
        {
            imagePopulation.sprite = upArrow;
            imagePopulation.color = colorGreen;
            imagePopulationDetails.sprite = upArrow;
            imagePopulationDetails.color = colorGreen;
        }
        else
        {
            imagePopulation.sprite = downArrow;
            imagePopulation.color = colorRed;
            imagePopulationDetails.sprite = downArrow;
            imagePopulationDetails.color = colorRed;

        }
    }

    public void PauseGame()
    {
        UiCanvas.SetActive(pauseCanvas.activeSelf);
        pauseCanvas.SetActive(!pauseCanvas.activeSelf);
        isGamePaused = pauseCanvas.activeSelf;
    }

    public void increaseTaxes()
    {
        taxes += 1;
        textTaxes.text = taxes.ToString();

    }

    public void decreaseTaxes()
    {
        taxes -= 1;
        textTaxes.text = taxes.ToString();

    }
}
