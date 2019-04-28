using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameCanvas;
    [SerializeField]
    private GameObject optionCanvas;
    [SerializeField]
    private GameObject btns;
    [SerializeField]
    private MyGameManager myGameManager;
    // Start is called before the first frame update
    private void OnEnable()
    {
        //Pause game
        Time.timeScale = 0;
    }
    private void OnDisable()
    {
        //Resume game
        //gameCanvas.SetActive(true);
        Time.timeScale = 1;
    }
    public void Save()
    {
        myGameManager.SaveGame();
    }
    public void ShowOption()
    {
        optionCanvas.SetActive(!optionCanvas.activeSelf);
        btns.SetActive(!btns.activeSelf);
    }
    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ReturnToGame()
    {
        //gameObject.SetActive(false);
        myGameManager.PauseGame();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
