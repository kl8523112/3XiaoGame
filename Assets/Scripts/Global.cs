using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour
{
    public static Global instance;
    private SoundManager soundManager;
    private MapLoader mapLoader;
    private GridAnimation gridAnimation;
    void Awake()
    {
        instance=this;
    }
    void InitComponent()
    {
        soundManager = gameObject.AddComponent<SoundManager>()as SoundManager;
        mapLoader = gameObject.AddComponent<MapLoader>();
        gridAnimation = gameObject.AddComponent<GridAnimation>();
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        InitComponent();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
    }
    public SoundManager SoundManager
    {
        get { return soundManager; }
    }
    public GridAnimation GridAnimation
    {
        get { return gridAnimation; }
    }
    public MapLoader MapLoader
    {
        get { return mapLoader; }
    }
}