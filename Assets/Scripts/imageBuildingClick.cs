using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class imageBuildingClick : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Canvas canvasBuildings;
    public int buildingIndex;
    public Image imageCurrentBuilding;
    public BuildingPlacer buildingPlacer;
    private Building building;
    public Canvas canvasDescription;
    public void OnPointerClick(PointerEventData eventData)
    {

        canvasBuildings.gameObject.SetActive(false);
        canvasDescription.gameObject.SetActive(false);

        imageCurrentBuilding.sprite = this.GetComponent<Image>().sprite;
        buildingPlacer.switchBuilding(buildingIndex);
        
    }

    void Start()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        building = buildingPlacer.BuildingPrefabs[buildingIndex].GetComponentInChildren<Building>();

        //Text newText = transform.gameObject.AddComponent<Text>();
        string text = "";
        foreach(string s in building.getBuildingInfo())
        {
            text += s  + "\n";
        }

        canvasDescription.gameObject.SetActive(true);
        canvasDescription.gameObject.transform.Find("TextBuildingDescription").GetComponent<Text>().text = text;
        


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canvasDescription.gameObject.SetActive(false);
    }
}
