using UnityEngine;
using System.Collections;

public class MapInit : MonoBehaviour
{
	void Awake ()
    {
        string name = Global.instance.MapLoader.TargetLevelName;
        Global.instance.MapLoader.LoadLevel(name);
        /*Level newLevel = new Level("Level111",9,9,"GamePlay03");
        newLevel.GetUIsRecttransform();
        newLevel.SetPropsCount(10, 3);
        Global.instance.MapLoader.SaveLevel(newLevel);*/
    }
    void Start()
    {

    }
}
