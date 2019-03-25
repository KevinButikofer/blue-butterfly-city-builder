using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class imageBuildingClick : MonoBehaviour, IPointerClickHandler
{

    public Canvas canvasBuildings;
    public int buildingIndex;
    public Image imageCurrentBuilding;
    public void OnPointerClick(PointerEventData eventData)
    {

        canvasBuildings.gameObject.SetActive(false);
        imageCurrentBuilding.sprite = this.GetComponent<Image>().sprite;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
