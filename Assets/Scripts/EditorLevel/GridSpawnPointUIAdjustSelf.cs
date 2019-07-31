using UnityEngine;
using System.Collections;
using System;

public class GridSpawnPointUIAdjustSelf : MonoBehaviour,IUIAdjustSelf {
    public RectTransform Self{ get; set; }

    public RectTransform Target{ get; set; }

    public void GetTargetTransform()
    {
        Target = GameObject.Find("Canvas/PlayGame/BG1").GetComponent<RectTransform>();
        Self = GameObject.Find("Canvas/PlayGame/GridController").GetComponent<RectTransform>();
    }
    IEnumerator ISynSelfTransform()
    {
        while (true)
        {
            Vector2 pos = new Vector2(Target.position.x-Target.sizeDelta.x/2+70,Target.position.y-Target.sizeDelta.y/2+50);
            Self.localPosition = pos;
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void SynSelfTransform()
    {
        StartCoroutine(ISynSelfTransform());
    }

    void Start ()
    {
        GetTargetTransform();
        SynSelfTransform();
    }
	
	void Update () {
	
	}
}
