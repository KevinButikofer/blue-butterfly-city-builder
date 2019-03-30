using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    private float money = 100000;
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
    
    public int Population { get => population; set => population = value; }
    public int Power { get => power; set => power = value; }
    public float PopulationSatisfaction { get => populationSatisfaction; set => populationSatisfaction = value; }
    public float UnemploymentRate { get => unemploymentRate; set => unemploymentRate = value; }
    public int JobNumber { get => jobNumber; set => jobNumber = value; }
    public float Money { get => money; set => money = value; }
    public float Taxes { get => taxes; set => taxes = value; }

    // Start is called before the first frame update
    void Start()
    {
        gridManager = GetComponent<GridManager>();
        StartCoroutine("UpdateGame");
    }

    // Update is called once per frame
    void Update()
    {
       
       
    }
    IEnumerator UpdateGame()
    {
        while(true)
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
                population--;
        }
        else
            population = populationCapacity;
    }
    public void UpdateGameVar()
    {
       gridManager.UpdateGameVar(out int jobs, out int populationNumber, out int money);
       JobNumber = jobs;
       populationCapacity = populationNumber;
       populationSatisfaction =  (jobNumber / Mathf.Max(population, 1) - Taxes * 0.10f) * 100;
       Money += population * Taxes / 10 + Mathf.Min(JobNumber, population) /10 + money;
        if(Money < -10000)
        {
            Debug.Log("You loose git gud Noob");
        }
       //Debug.Log("population : " + population + " on " + populationCapacity + " capacity jobs number : " + JobNumber + " money : " + Money);
    }
}
