using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//1棕熊 2紫Owl 3黄鸡 4绿蛤 5蓝猪 6空气墙 7小冰框 8冰墙 
//is Grid Script
public class GridUnit : MonoBehaviour, IPointerClickHandler
{
    public int GridId;
    private int GridSize = 100;
    private int Gridx;
    private int Gridy;
    public int GridType = 0;
    private int Speed = 200;

    public bool isBomb = false;
    private Image img;
    public Vector2 TragetPos = Vector2.zero;
    public void Init()
    {
        name = GridId.ToString();
        Gridx = GridController.Instance.GetGridPosX(GridId);
        Gridy = GridController.Instance.GetGridPosY(GridId);
        transform.localPosition = new Vector3(Gridx * GridSize, Gridy * GridSize, 0);
        TragetPos = transform.localPosition;
        img = this.GetComponent<Image>();
        if(GridType==1)img.overrideSprite = GridController.Instance.s1;
        else if(GridType==2)img.overrideSprite = GridController.Instance.s2;
        else if(GridType==3)img.overrideSprite = GridController.Instance.s3;
        else if(GridType==4)img.overrideSprite = GridController.Instance.s4;
        else if(GridType==5)img.overrideSprite = GridController.Instance.s5;
    }
    public void ChangeTypeTo(int newType)
    {
        GridType=newType;
        Image img = this.GetComponent<Image>();
        if (GridType == 1) img.overrideSprite = GridController.Instance.s1;
        else if (GridType == 2) img.overrideSprite = GridController.Instance.s2;
        else if (GridType == 3) img.overrideSprite = GridController.Instance.s3;
        else if (GridType == 4) img.overrideSprite = GridController.Instance.s4;
        else if (GridType == 5) img.overrideSprite = GridController.Instance.s5;
        else if (GridType == 6) Destroy(img);
        else if (GridType == 7) doFrozen(true);
        else if (GridType == 8) img.overrideSprite = GridController.Instance.s8;
        GridController.Instance.DicGrid[GridId]=this;
    }
    void Update()
    {
        if (transform.localPosition.x == TragetPos.x && transform.localPosition.y == TragetPos.y)
        {
            if (GridController.Instance.IsMove.Contains(GridId))
            {
                GridController.Instance.IsMove.Remove(GridId);
            }
        }
        if (transform.localPosition.x != TragetPos.x)
        {
            if (transform.localPosition.x > TragetPos.x)
            {
                Vector2 pos = transform.localPosition;
                pos.x -= (Speed * Time.deltaTime);
                pos.x = pos.x < TragetPos.x ? TragetPos.x : pos.x;
                transform.localPosition = pos;
            }
            else
            {
                Vector2 pos = transform.localPosition;
                pos.x += (Speed * Time.deltaTime);
                pos.x = pos.x > TragetPos.x ? TragetPos.x : pos.x;
                transform.localPosition = pos;
            }
        }
        if (transform.localPosition.y != TragetPos.y)
        {
            if (transform.localPosition.y > TragetPos.y)
            {
                Vector2 pos = transform.localPosition;
                pos.y -= (Speed * Time.deltaTime);
                pos.y = pos.y < TragetPos.y ? TragetPos.y : pos.y;
                transform.localPosition = pos;
            }
            else
            {
                Vector2 pos = transform.localPosition;
                pos.y += (Speed * Time.deltaTime);
                pos.y = pos.y > TragetPos.y ? TragetPos.y : pos.y;
                transform.localPosition = pos;
            }
        }
    }

    void doFrozen(bool b)
    {
        if(b)
        {
            GameObject iceFrameGo = Instantiate(Resources.Load("Prefabs/GridIceFream")) as GameObject;
            GridIceFream gif = iceFrameGo.AddComponent<GridIceFream>();
            iceFrameGo.transform.SetParent(this.transform, false);   //作为父节点
            iceFrameGo.transform.SetAsFirstSibling();
            GridController.Instance.iceDicGrid[GridId] = gif;    //存到容器中
            this.transform.DetachChildren();
            iceFrameGo.transform.SetParent(GridController.Instance.GridControllerTransform,true);
            iceFrameGo.transform.position = this.transform.position;
            this.transform.SetParent(iceFrameGo.transform, true);
            iceFrameGo.name = GridId.ToString()+"IceFream";
            GridController.Instance.RemoveGridList.Add(this);
            Debug.Log("加载"+ GridController.Instance.iceDicGrid[GridId].name);
            //GridController.Instance.DicGrid.Remove(this.GridId);
            //Destroy(this.gameObject);
            // GridController.Instance.AllGridMoveDown();
        }
    }
    public void MoveGrid(GridUnit last, GridUnit current)
    {
        if (GridController.Instance.GetGridMoveRange(last.GridId, current.GridId))
        {
            last.TragetPos = current.transform.localPosition;
            current.TragetPos = last.transform.localPosition;
            if (!GridController.Instance.IsMove.Contains(GridId))
            {
                GridController.Instance.IsMove.Add(GridId);
            }
            GridController.Instance.MoveGridDate(last, current);
            GridController.Instance.RemoveKye = true;
        }
        else
        {
            GridController.Instance.LastClickGrid = null;
            GridController.Instance.CurrentClickGrid = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)//
    {
        if (GridController.Instance.IsMove.Count != 0
            ||this.GridType==8
            )
        {
            return;
        }
        //Debug.Log("Click:"+name);

        Global.instance.SoundManager.PlayAudio("GridClickSound");

        if (GridController.Instance.LastClickGrid != this)
        {
            if (GridController.Instance.LastClickGrid != null)
            {
                MoveGrid(GridController.Instance.LastClickGrid, this);
                GridController.Instance.CurrentClickGrid = this;
                return;
            }
            GridController.Instance.LastClickGrid = this;
        }
    }
    public void SetBomb()
    {
        isBomb = true;
        img.color = Color.red;
    }

    void OnDestroy()
    {
        //Debug.Log(GridType);
    }
}
