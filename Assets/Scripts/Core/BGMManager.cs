using UnityEngine;
using System.Collections;

public enum MusicType
{
    main = 1,
    prologue = 2,
    village = 3,
    forrest = 4,
    urgent = 5,
}

public class BGMManager : MonoBehaviour {
    public static BGMManager instance;
    public AudioClip mainBgm;
    public AudioClip prologueBgm;
    public AudioClip villageBgm;
    public AudioClip forestBgm;
    public AudioClip forestUrgentBgm;
    private bool isStage;
    private MusicType currentBGM;

    void Awake()
    {
        // Make sure there is only 1 instance of this class.
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayMain()
    {
        if (currentBGM != MusicType.main)
        {
            currentBGM = MusicType.main;
            isStage = false;
            audio.Stop();
            audio.clip = mainBgm;
            audio.loop = true;
            audio.Play();
        }
    }

    public void PlayPrologue()
    {
        if (currentBGM != MusicType.prologue)
        {
            currentBGM = MusicType.prologue;
            isStage = false;
            audio.Stop();
            audio.clip = prologueBgm;
            audio.loop = false;
            audio.Play();
        }
    }

    public void PlayVillage()
    {
        currentBGM = MusicType.village;
        audio.Stop();
        audio.clip = villageBgm;
        audio.loop = true;
        audio.Play();
    }

    public void PlayForest()
    {
        currentBGM = MusicType.forrest;
        audio.Stop();
        audio.clip = forestBgm;
        audio.loop = true;
        audio.Play();
    }

    public void PlayStage()
    {
        isStage = true;
        if ((NavigationManager.instance.chapter == 0 || NavigationManager.instance.chapter == 7) && currentBGM != MusicType.village)
        {
            PlayVillage();
        }
        else if ((NavigationManager.instance.chapter == 1 || NavigationManager.instance.chapter == 8) && currentBGM != MusicType.forrest)
        {
            PlayForest();
        } 
    }

    public void PlayUrgent()
    {
        if (currentBGM != MusicType.urgent) { 
        currentBGM = MusicType.urgent;
        isStage = true;
        audio.Stop();
        if (NavigationManager.instance.chapter == 0 
            || NavigationManager.instance.chapter == 1
            || NavigationManager.instance.chapter == 7
            || NavigationManager.instance.chapter == 8)
        {
            audio.clip = forestUrgentBgm;
        }
        audio.loop = true;
        audio.Play();
            }
    }

    public bool isPlaying()
    {
        return audio.isPlaying;
    }

    public bool IsStage()
    {
        return isStage;
    }

	// Use this for initialization
	void Start () {
        GetComponent<AudioSource>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
