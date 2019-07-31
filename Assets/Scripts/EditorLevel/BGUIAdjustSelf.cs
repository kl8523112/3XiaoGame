using UnityEngine;
using System.Collections;
using System;

public interface IUIAdjustSelf
{
    RectTransform Target { get; set; }
    RectTransform Self { get; set; }
    void GetTargetTransform();
    void SynSelfTransform();
}
public class BGUIAdjustSelf : MonoBehaviour,IUIAdjustSelf {
    public RectTransform Self{ get; set; }

    public RectTransform Target{get;set;}

    private Vector2 wh;
    public void GetTargetTransform()
    {
        Self = this.gameObject.GetComponent<RectTransform>();
    }

    IEnumerator ISynSelfTransform()
    {
        while (true)
        {
            Self.sizeDelta = new Vector2(EditorManager.Instance.xl * 103,EditorManager.Instance.yl*103);
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
