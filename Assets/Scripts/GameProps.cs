using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameProps : MonoBehaviour {
    private GridController gc;

    private Text ShowTipPropCountText;
    public int ShowTioPropCount=0;

    private Text ResetMapCountText;
    public int ResetMapCount = 0;

    private Transform gameUI;
    private Transform pauseUI;
    void Start () 
    {
        gc = GridController.Instance;
        ShowTipPropCountText = GameObject.Find("Canvas/GameProps/BtnShowTip/PropCount").GetComponent<Text>();
        ShowTipPropCountText.text = ShowTioPropCount.ToString();

        ResetMapCountText = GameObject.Find("Canvas/GameProps/ResetMap/PropCount").GetComponent<Text>();
        ResetMapCountText.text = ResetMapCount.ToString();

        gameUI = GameObject.Find("Canvas/PlayGame/BG1/GridController").transform;
        pauseUI = GameObject.Find("Canvas/PauseMenu").transform;
        pauseUI.gameObject.SetActive(false);
    }
    void MinusPropCount(ref Text text,ref int count)
    {
        count--;
        text.text = count.ToString();
    }

    public void BtnShowTip()
    {
        if (ShowTioPropCount - 1 >= 0)
        {
            MinusPropCount(ref ShowTipPropCountText, ref ShowTioPropCount);
            gc.GetTipGo();
        }
    }

    public void ResetMap()
    {
        if (ResetMapCount - 1 >= 0)
        {
           // List<int> delList = new List<int>();
            MinusPropCount(ref ResetMapCountText, ref ResetMapCount);
            /*foreach(KeyValuePair<int,GridIceFream>  kp in GridController.Instance.iceDicGrid)
            {
                //GridController.Instance.iceDicGrid.Remove(kp.Key);
                //Destroy(kp.Value.gameObject);
                delList.Add(kp.Key);
            }*/
            /*for (int i = 0; i < delList.Count; i++)
            {
                Debug.Log(GridController.Instance.iceDicGrid[delList[i]].name);
                Destroy(GridController.Instance.iceDicGrid[delList[i]].gameObject);
                GridController.Instance.iceDicGrid.Remove(delList[i]);
            }*/
            gc.ResetMap();
        }
    }

    public void BtnBackToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void BtnPauseGame()
    {
        if (gc.PlayKey)
        {
            gameUI.gameObject.SetActive(false);
            Time.timeScale = 0;
            pauseUI.gameObject.SetActive(true);
        }
    }
    public void BtnResumeGame()
    {
        gameUI.gameObject.SetActive(true);
        Time.timeScale = 1;
        pauseUI.gameObject.SetActive(false);
    }
}
