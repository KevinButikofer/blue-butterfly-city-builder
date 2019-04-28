using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class handleClickCloseDetails : MonoBehaviour, IPointerClickHandler
{
    public Canvas canvasDetails;
    public void OnPointerClick(PointerEventData eventData)
    {
        canvasDetails.gameObject.SetActive(false);

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
