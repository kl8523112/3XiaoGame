using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level
{
    public string levelName;
    public string BGMname;
    public int X=6, Y=6;
    public List<int> mapPosList;   //存储要修改的格子位置
    public List<int> mapTypeList;    //存储要修改的格子的目标类型
    public List<Vector2> UIsPos;
    public List<Vector2> UIsWH;
    public int PropsGetTip;
    public int PropsMapReset;
	public Level(string name,int x,int y,string BGM)
    {
        levelName = name;
		X = x;Y = y;BGMname = BGM;
        mapPosList = new List<int>();
        mapTypeList = new List<int>();
        UIsPos = new List<Vector2>();
        UIsWH = new List<Vector2>();   
    }
    public void GetUIsRecttransform()
    {
        GameObject[] rectTransforms = GameObject.FindGameObjectsWithTag("UIElement");
        foreach (GameObject go in rectTransforms)
        {
            RectTransform rt = go.GetComponent<RectTransform>();
            UIsPos.Add(rt.localPosition);
            UIsWH.Add(rt.sizeDelta);
        }
    }
    public void SetPropsCount(int propsGetTip, int propsMapReset)
    {
        PropsGetTip = propsGetTip;
        PropsMapReset = propsMapReset;
}
    public void Add(int key,int value)
    {
        mapPosList.Add(key);
        mapTypeList.Add(value);
    }
    public List<int> MapPosList
    {
        get { return mapPosList; }
    }
    public List<int> MapTypeList
    {
        get { return mapTypeList; }
    }
}
