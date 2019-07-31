using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.EventSystems;
using System.Net.Sockets;
using System.Threading;
//using YouKe.Data;

//is MenuManager Script
public class MenuManager : MonoBehaviour , UnityEngine.EventSystems.IPointerDownHandler//,INotifier
{
    public static bool isChooseLevelPanelActive = false;

    public List<Score> scoreList = new List<Score>();

    private Client client;

    private Text mScore;
    public int numScore = 0;

    public Transform GameStartTransform;
    public Transform GameSettingsTransform;
    public Transform RankingTransform;
    public Transform GameAboutTransform;
    public Transform ChooseLevelTransform;

    public Transform Item;

    private List<Transform> ListTopTransform = new List<Transform>();
    public bool RemoveKye = false;

    void Start()
    {
        Global.instance.SoundManager.PlayBGM("GameMenu");
        GameStartTransform.gameObject.SetActive(true);
        RankingTransform.gameObject.SetActive(false);
        GameSettingsTransform.gameObject.SetActive(false);
        GameAboutTransform.gameObject.SetActive(false);
        ChooseLevelTransform.gameObject.SetActive(false);

        Item.gameObject.SetActive(false);

        if (isChooseLevelPanelActive) BtnClickPlayGame();

    }

    public GameObject LevelFilter;
    private List<GameObject> LevelBtnList = new List<GameObject>();
    #region 开始界面
    public void BtnClickQuitGame()
    {
        Global.instance.SoundManager.PlayAudio("BtnClickSound");

        Application.Quit();
    } 
    public void BtnClickPlayGame()
    {
        Global.instance.SoundManager.PlayAudio("BtnClickSound");

        ChooseLevelTransform.gameObject.SetActive(true);

        GameObject btn = Resources.Load("Prefabs/BtnChooseLevel") as GameObject;
        Global.instance.MapLoader.ReloadAllLevel();
        foreach (string name in Global.instance.MapLoader.LevelDic.Keys)
        {
            GameObject btnGo = Instantiate(btn);
            LevelBtnList.Add(btnGo);
            Text btnGoText = btnGo.transform.Find("Text").GetComponent<Text>();
            btnGo.GetComponent<Button>().onClick.AddListener(delegate { this.SetTargetLevel(btnGoText.text); });
            btnGoText.text = name;
            btnGo.transform.SetParent(LevelFilter.transform,false);
        }

        isChooseLevelPanelActive = true;
    }
    void SetTargetLevel(string name)
    {
        Global.instance.MapLoader.TargetLevelName = name;
        LoadLevel(2);
    }

    public void BtnClickGameSettings()
    {
        Global.instance.SoundManager.PlayAudio("BtnClickSound");

        GameStartTransform.gameObject.SetActive(false);
        RankingTransform.gameObject.SetActive(false);
        GameSettingsTransform.gameObject.SetActive(true);
        GameAboutTransform.gameObject.SetActive(false);
        ChooseLevelTransform.gameObject.SetActive(false);

    }

    public void BtnClickAbout()
    {
        Global.instance.SoundManager.PlayAudio("BtnClickSound");

        GameStartTransform.gameObject.SetActive(false);
        RankingTransform.gameObject.SetActive(false);
        GameSettingsTransform.gameObject.SetActive(false);
        GameAboutTransform.gameObject.SetActive(true);
        ChooseLevelTransform.gameObject.SetActive(false);

    }

    public void BtnClickRanking()
    {
        Global.instance.SoundManager.PlayAudio("BtnClickSound");
        //SendMessageGetRankingList(20);
        GameStartTransform.gameObject.SetActive(false);
        RankingTransform.gameObject.SetActive(true);
        GameSettingsTransform.gameObject.SetActive(false);
        GameAboutTransform.gameObject.SetActive(false);
        ChooseLevelTransform.gameObject.SetActive(false);

        ReadRankingScoekt();
        scoreList.Sort();
        //StreamWriter sw = new StreamWriter(Application.dataPath + "/Resources/RankingList.txt");
        // if (scoreList.Count > 10) for (int i = 10; i <= scoreList.Count;i++ ) scoreList.RemoveAt(i);
        //for (int i = 0; i < scoreList.Count; i++) 
        //{ 
        //    sw.WriteLine(JsonUtility.ToJson(scoreList[i]));
        //}
        // sw.Close();
        /*if(scoreList.Count!=0)
        ShowRanking();*/

    }
    public void BtnClickCloseChooseLevel()
    {
        Global.instance.SoundManager.PlayAudio("BtnClickSound");
        ChooseLevelTransform.gameObject.SetActive(false);
        foreach (GameObject go in LevelBtnList)
        {
            Destroy(go);
        }
        LevelBtnList.Clear();

        isChooseLevelPanelActive = false;
    }
    public void LoadLevel(int levelIndex)
    {
        Global.instance.SoundManager.StopBGM();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelIndex);
    }

    #endregion

    #region 排行榜
    void ReadRankingScoekt()
    {
        scoreList.Clear();
        client = gameObject.AddComponent<Client>();
        client.Init();
        client.ConnectToServer(client.SendMessageGet);
        //client.SendMessageToServer("fuck you bug!");
        //client.SendMessageToServer("%get");
        /*StreamReader sr = new StreamReader(Application.dataPath + "/Resources/RankingList.txt");
        string nextLine;
        while ((nextLine = sr.ReadLine()) != null)
        {
            scoreList.Add(JsonUtility.FromJson<Score>(nextLine));
        }
        sr.Close();*/
    }
    public void ShowRanking()
    {
        for (int i = 0; i < scoreList.Count; i++)
        {
            GameObject item = Instantiate(Item.gameObject);
            item.gameObject.SetActive(true);
            ListTopTransform.Add(item.transform);
            item.transform.SetParent(Item.parent, false);
            item.transform.Find("Number").GetComponent<Text>().text = (i + 1).ToString();
            item.transform.Find("Name").GetComponent<Text>().text = scoreList[i].name;
            item.transform.Find("Score").GetComponent<Text>().text = scoreList[i].score.ToString();
        }
    }

    public void BtnClickCloseRanking()
    {
        Global.instance.SoundManager.PlayAudio("BtnClickSound");

        for (int i = 0; i < ListTopTransform.Count; i++)
        {
            Destroy(ListTopTransform[i].gameObject);
        }
        ListTopTransform.Clear();

        GameStartTransform.gameObject.SetActive(true);
        RankingTransform.gameObject.SetActive(false);
        GameSettingsTransform.gameObject.SetActive(false);
        ChooseLevelTransform.gameObject.SetActive(false);
        GameAboutTransform.gameObject.SetActive(false);

        Destroy(client);
        client = null;
    }

    #endregion

    #region 游戏设置
    public void BtnClickCloseGameSettings()
    {
        Global.instance.SoundManager.PlayAudio("BtnClickSound");

        GameStartTransform.gameObject.SetActive(true);
        RankingTransform.gameObject.SetActive(false);
        GameSettingsTransform.gameObject.SetActive(false);
        GameAboutTransform.gameObject.SetActive(false);
        ChooseLevelTransform.gameObject.SetActive(false);
    }
    #endregion

    #region 关于
    public void BtnClickCloseGameAbout()
    {
        Global.instance.SoundManager.PlayAudio("BtnClickSound");

        GameStartTransform.gameObject.SetActive(true);
        RankingTransform.gameObject.SetActive(false);
        GameSettingsTransform.gameObject.SetActive(false);
        GameAboutTransform.gameObject.SetActive(false);
        ChooseLevelTransform.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Global.instance.SoundManager.PlayAudio("BtnClickSound");
    }
    #endregion


}