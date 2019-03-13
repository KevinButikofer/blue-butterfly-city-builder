using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int money;
    private int population;
    private int power = 0;
    private float populationSatisfaction;
    private float unemploymentRate;

    public int Money { get => money; set => money = value; }
    public int Population { get => population; set => population = value; }
    public int Power { get => power; set => power = value; }
    public float PopulationSatisfaction { get => populationSatisfaction; set => populationSatisfaction = value; }
    public float UnemploymentRate { get => unemploymentRate; set => unemploymentRate = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
