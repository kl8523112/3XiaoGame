using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelsManager
{
    public List<string> Levels;

    private static string DefaultLevel1Content = @"{""levelName"":""Level 1"",""BGMname"":""GamePlay04"",""X"":9,""Y"":9,""mapPosList"":[24,34,44,54,64,30,40,50,41,42,31,51,13,2,73,82],""mapTypeList"":[8,8,8,8,8,6,6,6,6,6,6,6,8,8,8,8],""UIsPos"":[{""x"":0.0,""y"":308.0},{""x"":490.0,""y"":282.0},{""x"":-205.0,""y"":336.5999450683594},{""x"":2.5,""y"":-7.0},{""x"":-132.5,""y"":-7.0},{""x"":490.0,""y"":374.0},{""x"":-6.0,""y"":-32.5},{""x"":0.0,""y"":353.79998779296877}],""UIsWH"":[{""x"":410.0,""y"":40.0},{""x"":188.0,""y"":51.0},{""x"":188.0,""y"":51.0},{""x"":116.0,""y"":51.0},{""x"":116.0,""y"":51.0},{""x"":188.0,""y"":51.0},{""x"":541.0,""y"":100.0},{""x"":89.0,""y"":-65.5999984741211}],""PropsGetTip"":5,""PropsMapReset"":5}
";
    private static string DefaultLevel2Content = @"{""levelName"":""Level 2"",""BGMname"":""GamePlay04"",""X"":8,""Y"":7,""mapPosList"":[34,33,43,44,4,3,2,74,73,72],""mapTypeList"":[8,8,8,8,6,6,6,6,6,6],""UIsPos"":[{""x"":0.0,""y"":308.0},{""x"":490.0,""y"":282.0},{""x"":-205.0,""y"":336.5999450683594},{""x"":2.5,""y"":-7.0},{""x"":-132.5,""y"":-7.0},{""x"":490.0,""y"":374.0},{""x"":-6.0,""y"":-32.5},{""x"":0.0,""y"":353.79998779296877}],""UIsWH"":[{""x"":410.0,""y"":40.0},{""x"":188.0,""y"":51.0},{""x"":188.0,""y"":51.0},{""x"":116.0,""y"":51.0},{""x"":116.0,""y"":51.0},{""x"":188.0,""y"":51.0},{""x"":541.0,""y"":100.0},{""x"":89.0,""y"":-65.5999984741211}],""PropsGetTip"":4,""PropsMapReset"":6}
";
    private static string DefaultLevel3Content = @"{""levelName"":""Level 3"",""BGMname"":""GamePlay04"",""X"":6,""Y"":6,""mapPosList"":[3,13,23,33,43,43,53,22,32],""mapTypeList"":[8,8,8,8,8,8,8,6,6],""UIsPos"":[{""x"":0.0,""y"":308.0},{""x"":490.0,""y"":282.0},{""x"":-205.0,""y"":336.5999450683594},{""x"":2.5,""y"":-7.0},{""x"":-132.5,""y"":-7.0},{""x"":490.0,""y"":374.0},{""x"":-6.0,""y"":-32.5},{""x"":0.0,""y"":353.79998779296877}],""UIsWH"":[{""x"":410.0,""y"":40.0},{""x"":188.0,""y"":51.0},{""x"":188.0,""y"":51.0},{""x"":116.0,""y"":51.0},{""x"":116.0,""y"":51.0},{""x"":188.0,""y"":51.0},{""x"":541.0,""y"":100.0},{""x"":89.0,""y"":-65.5999984741211}],""PropsGetTip"":4,""PropsMapReset"":4}
";
    private static string DefaultLevel4Content = @"{""levelName"":""Level 4"",""BGMname"":""GamePlay04"",""X"":6,""Y"":6,""mapPosList"":[],""mapTypeList"":[],""UIsPos"":[{""x"":0.0,""y"":308.0},{""x"":490.0,""y"":282.0},{""x"":-205.0,""y"":336.5999450683594},{""x"":2.5,""y"":-7.0},{""x"":-132.5,""y"":-7.0},{""x"":490.0,""y"":374.0},{""x"":-6.0,""y"":-32.5},{""x"":0.0,""y"":353.79998779296877}],""UIsWH"":[{""x"":410.0,""y"":40.0},{""x"":188.0,""y"":51.0},{""x"":188.0,""y"":51.0},{""x"":116.0,""y"":51.0},{""x"":116.0,""y"":51.0},{""x"":188.0,""y"":51.0},{""x"":541.0,""y"":100.0},{""x"":89.0,""y"":-65.5999984741211}],""PropsGetTip"":0,""PropsMapReset"":0}
";
#if UNITY_STANDALONE_WIN
    private string DataPath = Application.dataPath + "/Levels/";
#endif
#if UNITY_ANDROID
    private string DataPath = Application.persistentDataPath + "/Levels/";
#endif
    public LevelsManager()
    {
        Debug.Log(DataPath);
        Levels = new List<string>();
        LoadLevelsName();
    }

    void CreateAndWriteDefaultTxt(string fileName,string fileContent)
    {
        FileStream fs = new FileStream(DataPath + fileName, FileMode.Create, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs);
        sw.WriteLine(fileContent);
        sw.Flush();
        sw.Close();
        fs.Close();
    }
    public void LoadLevelsName()
    {
#if UNITY_STANDALONE_WIN
        if (!Directory.Exists(Application.dataPath + "/Levels"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Levels");
        }
#endif
#if UNITY_ANDROID
         if (!Directory.Exists(Application.persistentDataPath + "/Levels"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Levels");
        }
#endif
        //FileStream fs;
        if (!File.Exists(DataPath + "LevelsManager.txt"))
        {
            Debug.Log("不存在");
            CreateAndWriteDefaultTxt("Level 1.txt", DefaultLevel1Content);
            CreateAndWriteDefaultTxt("Level 2.txt", DefaultLevel2Content);
            CreateAndWriteDefaultTxt("Level 3.txt", DefaultLevel3Content);
            CreateAndWriteDefaultTxt("Level 4.txt", DefaultLevel4Content);
            CreateAndWriteDefaultTxt("LevelsManager.txt", "Level 1\nLevel 2\nLevel 3\nLevel 4\n");
            //fs = new FileStream(DataPath + "LevelsManager.txt", FileMode.Create, FileAccess.ReadWrite);
        }
        //else
        //{
        //    fs = new FileStream(DataPath + "LevelsManager.txt", FileMode.Open, FileAccess.ReadWrite);
        //}
        //StreamReader sr = new StreamReader(fs);
        StreamReader sr = new StreamReader(DataPath+ "LevelsManager.txt");
        while (sr.Peek() >= 0)
        {
            string levelName = sr.ReadLine();
            if (File.Exists(DataPath + levelName + ".txt") && !Levels.Contains(levelName))
            {
                Levels.Add(levelName);
            }
        }
        sr.Close();
    }
    public void Add(string name)
    {
        Levels.Add(name);
    }
}
