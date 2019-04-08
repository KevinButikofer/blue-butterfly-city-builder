using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;

public class MainMenuUManager : MonoBehaviour
{
    [SerializeField]
    private LoadMyGame load;
    [SerializeField]
    private GameObject aboutCanvas;
    [SerializeField]
    private GameObject optionCanvas;
    [SerializeField]
    private GameObject btns;
    [SerializeField]
    private Button btnLoad;
    [SerializeField]
    private Text MessageText;
    // Start is called before the first frame update
    void Start()
    {        
        //If no save we disable btnLoad
        if(!load.Load())
            btnLoad.interactable = false;        
    }

    public void Load()
    {
        //Load Game with the save
        if(load != null)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
            DontDestroyOnLoad(load.gameObject);
        }
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void ShowAbout()
    {
        aboutCanvas.SetActive(!aboutCanvas.activeSelf);
        btns.SetActive(!btns.activeSelf);
    }
    public void ShowOption()
    {
        optionCanvas.SetActive(!optionCanvas.activeSelf);
        btns.SetActive(!btns.activeSelf);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void OnApplicationQuit()
    {
    }
}
