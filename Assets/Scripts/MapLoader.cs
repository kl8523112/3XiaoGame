using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MapLoader : MonoBehaviour
{
    public Level level;
    public Dictionary<string, Level> LevelDic;
    private List<int> mapPosList;
    private List<int> mapTypeList;
    private GameProps gameProps;
    public string TargetLevelName;
    private LevelsManager levels ; 
    void Start ()
    {
        LevelDic = new Dictionary<string, Level>();
        levels = new LevelsManager();
        PreoadAllLevel();
    }
    void PreoadAllLevel()
    {
        foreach (string levelName in levels.Levels)
        {
#if UNITY_STANDALONE_WIN
            StreamReader sr = new StreamReader(Application.dataPath + "/Levels/" + levelName + ".txt");
#endif
#if UNITY_ANDROID
            StreamReader sr = new StreamReader(Application.persistentDataPath + "/Levels/" + levelName + ".txt");
#endif
            string json = sr.ReadToEnd();
            LevelDic[levelName] = JsonUtility.FromJson<Level>(json);
        }
    }
    public void ReloadAllLevel()
    {
        levels.LoadLevelsName();

        foreach (string levelName in levels.Levels)
        {
#if UNITY_STANDALONE_WIN
            StreamReader sr = new StreamReader(Application.dataPath + "/Levels/" + levelName + ".txt");
#endif
#if UNITY_ANDROID
            StreamReader sr = new StreamReader(Application.persistentDataPath + "/Levels/" + levelName + ".txt");
#endif
            string json = sr.ReadToEnd();
            if(!LevelDic.ContainsKey(levelName))
            LevelDic[levelName] = JsonUtility.FromJson<Level>(json);
            sr.Close();
        }
    }
    public void LoadLevel(string levelName)
    {
        Debug.Log(levelName);
        level = LevelDic[levelName];
        int x = level.X;
        int y = level.Y;
        mapPosList = level.mapPosList;
        mapTypeList = level.MapTypeList;

        if (!Global.instance.SoundManager.isPlayingBGM)
        {
            int order = (int)Random.Range(1f, 4f);
            string BGMName = "GamePlay0" + order.ToString();
            Global.instance.SoundManager.PlayBGM(BGMName);
        }

        GridController.Instance.InitXandY(x, y);
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                GameObject grid = Instantiate(Resources.Load("Prefabs/Grid")) as GameObject;
                GridUnit mgrid = grid.AddComponent<GridUnit>();
                mgrid.GridId = GridController.Instance.GetGridID(i, j);
                grid.transform.SetParent(GridController.Instance.GridControllerTransform, false);
                GridController.Instance.DicGrid[mgrid.GridId] = mgrid;
                mgrid.GridType = GridController.Instance.GetNewType(mgrid.GridId);
                mgrid.Init();

            }
        }
        GridController.Instance.PlayKey = true;
        for (int i = 0;i<mapPosList.Count;i++)
        {
            GridController.Instance.DicGrid[mapPosList[i]].ChangeTypeTo(mapTypeList[i]);
        }

        /*GameObject[] UIsGo = GameObject.FindGameObjectsWithTag("UIElement");
        List<Vector2> UIsPos=level.UIsPos;
        List<Vector2> UIsWH=level.UIsWH;
        for(int i=0;i<UIsPos.Count;i++)
        {
            RectTransform rt = UIsGo[i].GetComponent<RectTransform>();
            rt.localPosition = UIsPos[i];
            rt.sizeDelta = UIsWH[i];
        }*/
        gameProps = GameObject.Find("Tools/GamePropManager").GetComponent<GameProps>();
        gameProps.ShowTioPropCount = level.PropsGetTip;
        gameProps.ResetMapCount = level.PropsMapReset;
    }

    public void SaveLevel(Level level)
    {
#if UNITY_STANDALONE_WIN
        StreamWriter sw = new StreamWriter(Application.dataPath + "/Levels/"+level.levelName+".txt");
#endif

#if UNITY_ANDROID
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/Levels/"+level.levelName+".txt");
#endif
        sw.WriteLine(JsonUtility.ToJson(level));
        sw.Close();
#if UNITY_STANDALONE_WIN
        StreamWriter sw2 = new StreamWriter(Application.dataPath + "/Levels/LevelsManager.txt");
#endif
#if UNITY_ANDROID
        StreamWriter sw2 = new StreamWriter(Application.persistentDataPath + "/Levels/LevelsManager.txt");
#endif
        foreach (string name in LevelDic.Keys)
        {
            sw2.WriteLine(name);
        }
        sw2.WriteLine(level.levelName);
        sw2.Close();
    }

    public void ReloadLevel()
    {
        if (level == null) { Debug.Log("当前地图为空");return; }
        int x = level.X;
        int y = level.Y;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (GridController.Instance.DicGrid.ContainsKey(i*10+j)) { continue; }
                GameObject grid = Instantiate(Resources.Load("Prefabs/Grid")) as GameObject;
                GridUnit mgrid = grid.AddComponent<GridUnit>();
                mgrid.GridId = GridController.Instance.GetGridID(i, j);
                grid.transform.SetParent(GridController.Instance.GridControllerTransform, false);
                GridController.Instance.DicGrid[mgrid.GridId] = mgrid;
                mgrid.GridType = GridController.Instance.GetNewType(mgrid.GridId);
                mgrid.Init();
                grid.transform.SetAsFirstSibling();
            }
        }
    }
}
