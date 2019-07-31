using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class EditorGrid : MonoBehaviour,IPointerClickHandler {

    public int EditorGridType;
    public int GridID;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (EditorManager.Instance.CurrentGridType != 0)
        {
            this.EditorGridType = EditorManager.Instance.CurrentGridType;
            ChangeType(EditorGridType);
        }
    }

    void ChangeType(int type)
    {
        Image img = gameObject.GetComponent<Image>();
        EditorGridType = type;
        switch (EditorGridType)
        {
            case 6:img.overrideSprite = EditorManager.Instance.s6;break;
            case 8: img.overrideSprite = EditorManager.Instance.s8; break;
        }
        EditorManager.Instance.newLevel.Add(GridID, EditorGridType);
    }

    void Start ()
    { 
	    
	}
	void Update ()
    { 
	    
	}
}
