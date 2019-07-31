using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VolSlider : MonoBehaviour
{
    private Slider BGMSlider;
    private Slider AudioSlider; 
	void Start ()
    {
        BGMSlider=transform.Find("BGMSlider").GetComponent<Slider>();
        AudioSlider= transform.Find("AudioSlider").GetComponent<Slider>();


        BGMSlider.value = PlayerPrefs.GetFloat("BGMVol");
        AudioSlider.value = PlayerPrefs.GetFloat("audioVol");

        BGMSlider.onValueChanged.AddListener(delegate { BGMVolChange(); });
        AudioSlider.onValueChanged.AddListener(delegate { AudioVolChange(); });
    }
    public void BGMVolChange()
    {
        Global.instance.SoundManager.ChangeBGMVol(BGMSlider.value);
    }
    public void AudioVolChange()
    {
        Global.instance.SoundManager.ChangeAudioVol(AudioSlider.value);
    }
}
