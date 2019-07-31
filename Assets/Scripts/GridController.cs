using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//is GridController Script
public class GridController : MonoBehaviour //,INotifier
{
    private static GridController mInstance;
    public static GridController Instance
    {
        get { return mInstance; }
    }
    private int typeNum = 6;

    //List<Score> scoreList = new List<Score>();

    [HideInInspector]public Dictionary<int, GridUnit> DicGrid = new Dictionary<int, GridUnit>();  //存放基础格子
    [HideInInspector]public Dictionary<int, GridIceFream> iceDicGrid = new Dictionary<int, GridIceFream>();//存放冰框格子
    public GridUnit LastClickGrid = null;
    public GridUnit CurrentClickGrid = null;
    public List<int> IsMove = new List<int>();
    public List<GridUnit> RemoveGridList = new List<GridUnit>();
    private Text mScore;
    public int numScore = 0;

    public int xl;
    public int yl;

    public Sprite s1;
    public Sprite s2;
    public Sprite s3;
    public Sprite s4;
    public Sprite s5;
    public Sprite s7;
    public Sprite s8;

    public Transform GridControllerTransform;
    public Transform GamePlayTransform;
    public Transform ScoreUpdateTransform;

    // private List<protos.integral.TopInfo> listinfo = new List<protos.integral.TopInfo>();
    public bool RemoveKye = false;

    private Client client;
    void Awake()
    {
        mInstance = this;
    }

    void Start()
    {
        // GameServerMgr.GetInstance().RegisterNotifier(NetMessageDef.ResGetTopList, this);

        GamePlayTransform.gameObject.SetActive(true);
        ScoreUpdateTransform.gameObject.SetActive(false);

        mScore = GameObject.Find("Canvas/PlayGame/Score").GetComponent<Text>();
        //TimeText = GameObject.Find("Canvas/PlayGame/Timebar/TimeText ").GetComponent<Text>();
        TimeBar = GameObject.Find("Canvas/PlayGame/Timebar").GetComponent<Scrollbar>();
        //ScoreUp = ScoreUpdateTransform.Find("ScoreUp").GetComponent<Text>();
        input = ScoreUpdateTransform.Find("InputField").GetComponent<InputField>();
        AverCombo = ScoreUpdateTransform.Find("AverCombo").GetComponent<Text>();
        ComboCommit = ScoreUpdateTransform.Find("ComboCommit").GetComponent<Text>();

        timeCounter = GameObject.Find("Tools/TimeCounter").GetComponent<TimeCounter>();

        GamePlay();
    }

    void Update()
    {
        if (RemoveKye)
        {
            if (IsMove.Count == 0)
            {
                RemoveGrid();
                RemoveKye = false;
            }
        }

        if (IsDownKey)
        {
            if (IsMove.Count == 0)
            {
                IsDownKey = false;
                ReSetNullGrid();
            }
        }

        if (PlayKey)
        {
            mTime -= Time.deltaTime* factor;
            if (mTime < 0)
            {
                totalSeconds++;
                mTime = 1;
                GameTime -= 1;
                TimeText.text = GameTime.ToString();
                timeCounter.UpdateCombo();
                TimeBar.size = GameTime / GameTimeAll;
            }
            if (totalSeconds==60 )
            {
                factor+=0.5f;
                totalSeconds = 0;
            }

            if (GameTime == 0)
            {
                Global.instance.SoundManager.PlayBGM("ScoreSubmit");
                PlayKey = false;
                ScoreUpdateTransform.gameObject.SetActive(true);
                ScoreUp.text = numScore.ToString();
                AverCombo.text = timeCounter.CalculateAverCombo().ToString("f2");
                ComboCommit.text = timeCounter.GetComboCommit();
                timeCounter.InitTimeCounter();

                foreach (int id in DicGrid.Keys)
                {
                    Destroy(DicGrid[id].transform.gameObject);
                }
                DicGrid.Clear();
            }
        }

    }

    #region 棋盘初始化
    public void InitXandY(int xll, int yll)
    {
        xl = xll;
        yl = yll;
    }
    #endregion

    #region 游戏道具
    void Swap(ref int a,ref int b)
    {
        int t = a;
        a = b;
        b = t;
    }
    public void ResetMap()
    {
        List<int> delList = new List<int>();
        foreach (int id in DicGrid.Keys)
        {
            //Destroy(DicGrid[id].transform.gameObject);
            if(DicGrid[id].GridType!=6&& DicGrid[id].GridType != 8)
            delList.Add(id);
        }
        for (int i = 0; i < delList.Count; i++)
        {
            Destroy(DicGrid[delList[i]].transform.gameObject);
            DicGrid.Remove(delList[i]);
        }

        //DicGrid.Clear();

        Global.instance.MapLoader.ReloadLevel();
    }
    public void GetTipGo()
    {
        StartCoroutine("IGetTipGo");
    }
    IEnumerator IGetTipGo()
    {
        GameObject go1, go2;
        int[] way = { 1, -1, -10, 10 };
        int max = (xl - 1) * 10 + yl - 1;
        for (int x = 0; x < xl; x += 2)
        {
            for (int y = 0; y < yl; y++)
            {
                int gid = GetGridID(x, y);
                for (int i = 0; i <= 3; i++)
                {
                    int newGid = gid + way[i];
                    if (DicGrid[gid].GridType >= 6) continue;
                    if (newGid <= max && newGid >= 0 && newGid % 10 < yl&&DicGrid[newGid].GridType<=5)
                    {
                        yield return 0;
                        Swap(ref DicGrid[newGid].GridType, ref DicGrid[gid].GridType);
                        if (ComparatorTip(newGid))
                        {
                            yield return 0;
                            go1 = DicGrid[gid].gameObject;
                            go2 = DicGrid[newGid].gameObject;
                            Global.instance.GridAnimation.FadeCross(go1.GetComponent<Image>(),go2.GetComponent<Image>());
                            Swap(ref DicGrid[newGid].GridType, ref DicGrid[gid].GridType);
                            StopCoroutine("IGetTipGo");
                        }
                        else Swap(ref DicGrid[newGid].GridType, ref DicGrid[gid].GridType);
                    }
                }
            }
        }
        StopCoroutine("IGetTipGo");
    }

    bool ComparatorTip(int newgid)
    {
        if (Comparator(newgid, GetLeft(newgid), GetLeft(GetLeft(newgid))))
        {
             return true;
        }
        if (Comparator(newgid, GetRight(newgid), GetRight(GetRight(newgid))))
        {
             return true;
        }
        if (Comparator(newgid, GetDown(newgid), GetDown(GetDown(newgid))))
        {
             return true;
        }
        if (Comparator(newgid, GetUp(newgid), GetUp(GetUp(newgid))))
        {
             return true;
        }
        if (Comparator(newgid, GetUp(newgid), GetDown(newgid)))
        {
             return true;
        }
        if (Comparator(newgid, GetRight(newgid), GetLeft(newgid)))
        {
             return true;
        }
        return false;
    }
    #endregion

    #region 游戏进行

    public Text TimeText = null;
    private Scrollbar TimeBar = null;
    private float mTime = 1f;
    private float GameTime;

    private float totalSeconds = 0;
    private float factor=1;

    public float GameTimeAll=15;
    public float timeIncrement;
    [HideInInspector]public bool PlayKey = false;
    private TimeCounter timeCounter;
    public GameObject MapInit;
    public Transform ToolsTransform;

	private int loopTimes = 0;
    private void GamePlay()
    {
        timeCounter.InitTimeCounter();
        timeCounter.StartCount();

        ScoreUp.text = numScore.ToString();
        GameTime = GameTimeAll;
        numScore = 0;
        TimeBar.size = 1;
        TimeText.text = GameTime.ToString();
		if (Global.instance == null&&!DicGrid.ContainsKey(0))
        {
            Debug.Log("缺少地图加载器！");
            return;
        }
		if (!ReStart())
        {
            
			loopTimes++;
			if (loopTimes >= 50) 
			{
				Debug.Log ("死循环！");
                loopTimes = 0;

                return;
			}
            ResetMap();
            GamePlay();
        }
    }
    public void MoveGridDate(GridUnit last, GridUnit current)
    {
        DicGrid.Remove(last.GridId);
        DicGrid.Remove(current.GridId);

        DicGrid.Add(last.GridId, current);
        DicGrid.Add(current.GridId, last);

        int t = last.GridId;
        last.GridId = current.GridId;
        current.GridId = t;
    }

    public bool GetGridMoveRange(int last, int current)
    {
        if (GetGridPosX(last) == GetGridPosX(current))
        {
            if (GetGridPosY(last) == GetGridPosY(current) + 1 || GetGridPosY(last) == GetGridPosY(current) - 1)
            {
                return true;
            }
        }
        if (GetGridPosY(last) == GetGridPosY(current))
        {
            if (GetGridPosX(last) == GetGridPosX(current) + 1 || GetGridPosX(last) == GetGridPosX(current) - 1)
            {
                return true;
            }
        }
        return false;
    }

    public int GetGridID(int x, int y)
    {
        return x * 10 + y;
    }
    public int GetGridPosX(int id)
    {
        return id / 10;
    }
    public int GetGridPosY(int id)
    {
        return id % 10;
    }
    public int GetLeft(int id)
    {
        return id - 10;
    }
    public int GetDown(int id)
    {
        return id - 1;
    }
    public int GetRight(int id)
    {
        return id+10;
    }
    public int GetUp(int id)
    {
        return id + 1;
    }
    void CheckIceColumn(int id)
    {
        if (DicGrid.ContainsKey(GetUp(id)))
        {
            if (DicGrid[GetUp(id)].GridType == 8)
            {
                DicGrid[GetUp(id)].ChangeTypeTo(7);
            }
        }
        if (DicGrid.ContainsKey(GetDown(id)))
        {
            if (DicGrid[GetDown(id)].GridType == 8)
            {
                DicGrid[GetDown(id)].ChangeTypeTo(7);
            }
        }
        if (DicGrid.ContainsKey(GetLeft(id)))
        {
            if (DicGrid[GetLeft(id)].GridType == 8)
            {
                DicGrid[GetLeft(id)].ChangeTypeTo(7);
            }
        }
        if (DicGrid.ContainsKey(GetRight(id)))
        {
            if (DicGrid[GetRight(id)].GridType == 8)
            {
                DicGrid[GetRight(id)].ChangeTypeTo(7);
            }
        }

    }
    bool Comparator(int a, int b, int c)
    {
        if (!DicGrid.ContainsKey(a) || !DicGrid.ContainsKey(b) || !DicGrid.ContainsKey(c))
        {
            return false;
        }
        if (DicGrid[a].GridType == DicGrid[b].GridType
            && DicGrid[b].GridType == DicGrid[c].GridType
            && DicGrid[a].GridType!=6
            && DicGrid[a].GridType!=8)
        {
            return true;
        }

        return false;
    }
    bool Comparator(int a, int b, int c, int moveTo)
    {
        if (!DicGrid.ContainsKey(a) || !DicGrid.ContainsKey(b) || !DicGrid.ContainsKey(c)|| !DicGrid.ContainsKey(moveTo))
        {
            return false;
        }
        if (DicGrid[moveTo].GridType == 6 || DicGrid[moveTo].GridType == 8) return false;

        if (DicGrid[a].GridType == DicGrid[b].GridType
            && DicGrid[b].GridType == DicGrid[c].GridType
            && DicGrid[a].GridType != 6
            && DicGrid[a].GridType != 8)
        {
            return true;
        }

        return false;
    }

    void RemoveGrid()//
    {
        Debug.Log("开始消除");
        for (int x = 0; x < xl; x++)
        {
            for (int y = 0; y < yl; y++)
            {
                if (Comparator(GetGridID(x, y), GetGridID(x, y + 1), GetGridID(x, y + 2))
                    &&DicGrid[GetGridID(x,y)].GridType<=5)
                {
                    if (!RemoveGridList.Contains(DicGrid[GetGridID(x, y)]))
                    {
                        RemoveGridList.Add(DicGrid[GetGridID(x, y)]);
                    }
                    if (!RemoveGridList.Contains(DicGrid[GetGridID(x, y + 1)]))
                    {
                        RemoveGridList.Add(DicGrid[GetGridID(x, y + 1)]);
                    }
                    if (!RemoveGridList.Contains(DicGrid[GetGridID(x, y + 2)]))
                    {
                        RemoveGridList.Add(DicGrid[GetGridID(x, y + 2)]);
                    }
                }
                if (Comparator(GetGridID(x, y), GetGridID(x + 1, y), GetGridID(x + 2, y))
                    && DicGrid[GetGridID(x, y)].GridType <= 5)
                {
                    if (!RemoveGridList.Contains(DicGrid[GetGridID(x, y)]))
                    {
                        RemoveGridList.Add(DicGrid[GetGridID(x, y)]);
                    }
                    if (!RemoveGridList.Contains(DicGrid[GetGridID(x + 1, y)]))
                    {
                        RemoveGridList.Add(DicGrid[GetGridID(x + 1, y)]);
                    }
                    if (!RemoveGridList.Contains(DicGrid[GetGridID(x + 2, y)]))
                    {
                        RemoveGridList.Add(DicGrid[GetGridID(x + 2, y)]);
                    }
                }

            }
        }

        if (RemoveGridList.Count > 0)
        {
            Global.instance.SoundManager.PlayAudio("GridRemoveSound");

            Debug.Log("继续消除！");

            timeCounter.AddCombo();   //检查连击计数器

            numScore = RemoveGridList.Count * timeCounter.GetComboMultiple() + numScore;

            GameTime = GameTime + timeIncrement + 1 >= GameTimeAll ? GameTimeAll : GameTime + timeIncrement + 1;

            mScore.text = "积分:" + numScore;
            if (RemoveGridList.Count >= 4)
            {
                GridUnit  bomb = RemoveGridList[0];
                bomb.SetBomb();
                RemoveGridList.RemoveAt(0);
            }
            for (int i = 0; i < RemoveGridList.Count; i++)
            {
                if (RemoveGridList[i].isBomb)
                {
                    bool b = System.Convert.ToBoolean(Random.Range(0f, 1f));
                    int j = RemoveGridList[i].GridId;
                    if (b)
                    {
                        while (DicGrid.ContainsKey(GetUp(j)))
                        {
                            RemoveGridList.Add(DicGrid[GetUp(j)]);
                            j = GetUp(j);
                        }
                        j = RemoveGridList[i].GridId;
                        while (DicGrid.ContainsKey(GetDown(j)))
                        {
                            RemoveGridList.Add(DicGrid[GetDown(j)]);
                            j = GetDown(j);
                        }
                    }
                    else
                    {
                        while (DicGrid.ContainsKey(GetRight(j)))
                        {
                            RemoveGridList.Add(DicGrid[GetRight(j)]);
                            j = GetRight(j);
                        }
                        j = RemoveGridList[i].GridId;
                        while (DicGrid.ContainsKey(GetLeft(j)))
                        {
                            RemoveGridList.Add(DicGrid[GetLeft(j)]);
                            j = GetLeft(j);
                        }
                    }
                }

                if (RemoveGridList[i].GridId == 6) { Debug.Log("跳过删除墙部分"); continue; }

                if (RemoveGridList[i].GridId != 8 && iceDicGrid.ContainsKey(RemoveGridList[i].GridId)) //如果消除的不是冰块
                {
                    iceDicGrid[RemoveGridList[i].GridId].MinusHp();
                    if (iceDicGrid[RemoveGridList[i].GridId].isDead())
                    {
                        Debug.Log(iceDicGrid[RemoveGridList[i].GridId].name);
                        Destroy(iceDicGrid[RemoveGridList[i].GridId].gameObject);
                        iceDicGrid.Remove(RemoveGridList[i].GridId);
                    }
                }
                if (RemoveGridList[i].GridType != 8&& RemoveGridList[i].GridType != 7)
                {
                    CheckIceColumn(RemoveGridList[i].GridId);   //检查冰柱
                }
                DicGrid.Remove(RemoveGridList[i].GridId);
                Destroy(RemoveGridList[i].gameObject);
            }//end for
            CurrentClickGrid = null;
            LastClickGrid = null;
            RemoveGridList.Clear();
        }
        else
        {
            Debug.Log("循环结束！");
            if (LastClickGrid && CurrentClickGrid)
            {
                LastClickGrid.MoveGrid(LastClickGrid, CurrentClickGrid);
            }
            CurrentClickGrid = null;
            LastClickGrid = null;
            return;
        }
        AllGridMoveDown();
    }

    public void AllGridMoveDown()//
    {
        Debug.Log("下移");
        for (int x = 0; x < xl; x++)
        {
            int NullGridPos = -1;
            bool findWall = false;
            for (int y = 0; y < yl; y++)
            {
                if (DicGrid.ContainsKey(GetGridID(x, y)))
                {
                    if (DicGrid[GetGridID(x, y)].GridType == 6 || DicGrid[GetGridID(x, y)].GridType == 8)
                    {
                        //if (NullGridPos != -1) NullGridPos++;
                        if (findWall == false)
                        {
                            findWall = true;
                        }
                    }
                }
                if (!DicGrid.ContainsKey(GetGridID(x, y)))
                {
                    if (NullGridPos == -1)
                    {
                        NullGridPos = y;

                    }
                }
                else if (NullGridPos != -1&& findWall == false)
                {
                    Vector2 pos = DicGrid[GetGridID(x, y)].transform.localPosition;
                    pos.y = NullGridPos * 100;
                    DicGrid[GetGridID(x, y)].TragetPos = pos;
                    if (!IsMove.Contains(GetGridID(x, NullGridPos)))
                    {
                        IsMove.Add(GetGridID(x, NullGridPos));
                    }
                    GridUnit t = DicGrid[GetGridID(x, y)];
                    DicGrid.Remove(GetGridID(x, y));
                    DicGrid.Add(GetGridID(x, NullGridPos), t);
                    t.GridId = GetGridID(x, NullGridPos);
                    NullGridPos += 1;
                }
            }
        }
        IsDownKey = true;
    }

    void ReSetNullGrid()
    {
        for (int i = 0; i < xl; i++)
        {
            for (int j = 0; j < yl; j++)
            {
                if (!DicGrid.ContainsKey(GetGridID(i, j)))
                {
                    GameObject grid = Instantiate(Resources.Load("Prefabs/Grid")) as GameObject;
                    GridUnit mgrid = grid.AddComponent<GridUnit>();
                    mgrid.GridId = GetGridID(i, j);
                    grid.transform.SetParent(GridControllerTransform, false);
                    DicGrid[mgrid.GridId] = mgrid;
                    mgrid.GridType = GetNewType(mgrid.GridId);
                    mgrid.Init();

                    if (iceDicGrid.ContainsKey(GetGridID(i, j)))
                    {
                        grid.transform.SetAsFirstSibling();
                    }
                }
            }
        }
        foreach (int id in iceDicGrid.Keys) iceDicGrid[id].transform.SetAsLastSibling();
        RemoveGrid();

        //在这里检查有没有还能消的，没有就重新加载地图
        if (!ReStart())
        {

            loopTimes++;
            if (loopTimes >= 50)
            {
                Debug.Log("死循环！");
                loopTimes = 0;
                return;
            }
            ResetMap();
            GamePlay();
        }
    }

    bool ReStart()
    {
        for (int x = 0; x < xl; x++)
        {
            for (int y = 0; y < yl; y++)
            {
                if (
                    //尖括号种类消除
                    Comparator(GetGridID(x, y), GetGridID(x - 1, y - 1), GetGridID(x - 1, y + 1), GetGridID(x-1,y))  
                    || Comparator(GetGridID(x, y), GetGridID(x + 1, y + 1), GetGridID(x - 1, y + 1),GetGridID(x,y+1))
                    || Comparator(GetGridID(x, y), GetGridID(x + 1, y - 1), GetGridID(x + 1, y + 1),GetGridID(x+1,y))
                    || Comparator(GetGridID(x, y), GetGridID(x - 1, y - 1), GetGridID(x - 1, y + 1),GetGridID(x-1,y))
                    //感叹号种类消除
                    || Comparator(GetGridID(x, y), GetGridID(x - 2, y), GetGridID(x - 3, y),GetGridID(x-1,y))
                    || Comparator(GetGridID(x, y), GetGridID(x + 2, y), GetGridID(x + 3, y),GetGridID(x+1,y))
                    || Comparator(GetGridID(x, y), GetGridID(x, y - 2), GetGridID(x, y - 3),GetGridID(x,y-1))
                    || Comparator(GetGridID(x, y), GetGridID(x, y + 2), GetGridID(x, y + 3),GetGridID(x,y+1))
                    //小拐弯种类
                    || Comparator(GetGridID(x, y), GetGridID(x - 1, y - 1), GetGridID(x - 1, y - 2),GetGridID(x-1,y))
                    || Comparator(GetGridID(x, y), GetGridID(x + 1, y - 1), GetGridID(x + 1, y - 2),GetGridID(x+1,y))

                    || Comparator(GetGridID(x, y), GetGridID(x - 1, y + 1), GetGridID(x - 1, y + 2),GetGridID(x-1,y))
                    || Comparator(GetGridID(x, y), GetGridID(x + 1, y + 1), GetGridID(x + 1, y + 2),GetGridID(x+1,y))

                    || Comparator(GetGridID(x, y), GetGridID(x - 1, y - 1), GetGridID(x - 2, y - 1),GetGridID(x,y-1))
                    || Comparator(GetGridID(x, y), GetGridID(x - 1, y + 1), GetGridID(x - 2, y + 1),GetGridID(x,y+1))

                    || Comparator(GetGridID(x, y), GetGridID(x + 1, y - 1), GetGridID(x + 2, y - 1),GetGridID(x,y-1))
                    || Comparator(GetGridID(x, y), GetGridID(x + 1, y + 1), GetGridID(x + 2, y + 1),GetGridID(x,y+1))
                    )
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsDownKey = false;

    public int GetNewType(int gridID)
    {
        int LeftType = 0;
        int DownType = 0;
        if (GetGridPosY(gridID) > 1)
        {
            if (DicGrid[GetDown(gridID)].GridType == DicGrid[GetDown(GetDown(gridID))].GridType)
            {
                DownType = DicGrid[GetDown(gridID)].GridType;
            }
        }
        if (GetGridPosX(gridID) > 1)
        {
            if (DicGrid[GetLeft(gridID)].GridType == DicGrid[GetLeft(GetLeft(gridID))].GridType)
            {
                LeftType = DicGrid[GetLeft(gridID)].GridType;
            }
        }
        if (GetGridPosY(gridID) > 1 || GetGridPosX(gridID) > 1)
        {
            return Range(DownType, LeftType);
        }
        return Random.Range(1, typeNum);
    }

    int Range(int down, int left)
    {
        int NewType = Random.Range(1, typeNum);
        if (down != NewType && left != NewType)
        {
            return NewType;
        }
        return Range(down, left);
    }
    #endregion

    #region 返回菜单
    public void BtnBackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
        Global.instance.GridAnimation.StopAllCoroutines();
    }
    #endregion

    #region 积分上传
    private Text ComboCommit = null;
    private Text AverCombo = null;
    public Text ScoreUp = null;
    private string Name = "";
    private InputField input = null;
    public void BtnClickSubmit()
    {
        Name = input.text;
        Debug.Log("asd");
        //SendMessageAddRanking(Name, numScore);

        Score score = new Score(Name, numScore);
        SendRankingSocket(score);

        //scoreList.Add(new Score(Name, numScore));
        //Debug.Log(scoreList.Count);
        input.text = null;
        ComboCommit.text = null;

        //scoreList.Sort();
        //StreamWriter sw = new StreamWriter(Application.dataPath + "/Resources/RankingList.txt");
        //if (scoreList.Count > 10) for (int i = 10; i <= scoreList.Count; i++) scoreList.RemoveAt(i);
        //for (int i = 0; i < scoreList.Count; i++)
        //{
        //    sw.WriteLine(JsonUtility.ToJson(scoreList[i]));
        //}
        //sw.Close();

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);

        //

        //与服务器对接上传积分

        // 

    }

    void SendRankingSocket(Score score)
    {
        client = gameObject.AddComponent<Client>();
        client.score = score;
        client.ConnectToServer(client.SendMessageScore);
        //client.SendEndToken();
        //client.CloseSocket();

        Destroy(client);
        client = null;

        //StreamReader sr = new StreamReader(Application.dataPath + "/Resources/RankingList.txt");
        //string nextLine;
        //while ((nextLine = sr.ReadLine()) != null)
        //{
            //scoreList.Add(JsonUtility.FromJson<Score>(nextLine));
        //}
        //sr.Close();
    }

    #endregion

}