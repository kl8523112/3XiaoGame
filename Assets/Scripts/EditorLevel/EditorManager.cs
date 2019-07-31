using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class EditorManager : MonoBehaviour {
    private static EditorManager mInstance;
    public static EditorManager Instance
    {
        get{ return mInstance; }
    }
    void Awake()
    {
        mInstance = this;
    }
    public int CurrentGridType;

    public InputField LevelNameInputField;
    public Dropdown XlDropdown;
    public Dropdown YlDropdown;
    public Dropdown TipsProps;
    public Dropdown ResetProps;


    public GameObject BtnSaveMap;
    public GameObject MapInitPanel;
    public Transform GridSpawnPoint;

    public Level newLevel;
    public int xl=6;
    public int yl=6;

    public Sprite s6,s8;

    void Start ()
    {
        BtnSaveMap.SetActive(false);
    }

    public void BtnSaveMapInitPanel()
    {
        xl = Convert.ToInt32(XlDropdown.options[XlDropdown.value].text);
        yl = Convert.ToInt32(YlDropdown.options[YlDropdown.value].text);
        newLevel = new Level(LevelNameInputField.text, xl, yl,"GamePlay04");
        MapInitPanel.SetActive(false);

        GameObject GridPrefab = Resources.Load("Prefabs/Grid") as GameObject;
        for (int i = 0; i < xl; i++)
        {
            for (int j = 0; j < yl; j++)
            {
                GameObject grid = Instantiate(GridPrefab);
                EditorGrid editorGrid = grid.AddComponent<EditorGrid>();
                editorGrid.GridID = i * 10 + j;
                grid.transform.SetParent(GridSpawnPoint.transform,false);
                grid.GetComponent<Image>().color = Color.white;
                grid.transform.localPosition = new Vector3(i * 100, j*100, 0);
            }
        }
        /*GameObject[] UIElements = GameObject.FindGameObjectsWithTag("UIElement");
        foreach (GameObject go in UIElements) go.AddComponent<UIDragHandler>();*/
        newLevel.SetPropsCount(Convert.ToInt32(TipsProps.options[TipsProps.value].text),
            Convert.ToInt32(ResetProps.options[ResetProps.value].text));


        BtnSaveMap.SetActive(true);
    }
    public void BtnSetCurGridType(int type)
    {
        CurrentGridType = type;
    }
    public void BtnSaveMapFun()
    {
        newLevel.GetUIsRecttransform();
        Global.instance.MapLoader.SaveLevel(newLevel);
        BtnBackToMenu();
    }
    public void BtnBackToMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
