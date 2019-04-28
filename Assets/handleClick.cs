using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class handleClick : MonoBehaviour, IPointerClickHandler
{
    public Canvas canvasMenu;
    private bool isMenuActive = false;
    public bool IsMenuActive { get => isMenuActive; set => isMenuActive = value; }


    public void OnPointerClick(PointerEventData eventData)
    {
        if(isMenuActive)
        {
            canvasMenu.gameObject.SetActive(false);
            isMenuActive = false;
        }
        else
        {
            canvasMenu.gameObject.SetActive(true);
            isMenuActive = true;
        }
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
