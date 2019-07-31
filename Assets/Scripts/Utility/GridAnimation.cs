using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class GridAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {

	}
    public void FadeCross(Image img1,Image img2)
    {
        StartCoroutine(IFadeCross(img1, img2));
        Global.instance.SoundManager.PlayAudio("ShowTip");
    }
    IEnumerator IFadeCross(Image img1, Image img2)
    {
        img1.DOFade(0, 1f);
        img2.DOFade(0, 1f);
        yield return new WaitForSeconds(0.5f);
        img1.DOFade(1, 1f);
        img2.DOFade(1, 1f);
        yield return 0;
    }
    public void HighLightText(Text text,int maxSize,float duration)
    {
        StartCoroutine(IHighLightText(text,maxSize,duration));
    }
    IEnumerator IHighLightText(Text text, int maxSize, float duration)
    {
        int size = text.fontSize;
        for (int i = size; i<= maxSize; i++)
        {
            text.fontSize = i;
            yield return new WaitForSeconds(duration);
        }
        for (int i = maxSize; i >= size; i--)
        {
            text.fontSize = i;
            yield return new WaitForSeconds(duration);
        }
    }
}
