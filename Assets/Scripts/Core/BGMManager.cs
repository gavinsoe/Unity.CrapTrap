using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour {
    public static BGMManager instance;
    public AudioClip mainBgm;
    public AudioClip prologueBgm;
    public AudioClip villageBgm;
    public AudioClip forestBgm;
    public AudioClip forestUrgentBgm;
    private bool isStage;

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
        isStage = false;
        audio.Stop();
        audio.clip = mainBgm;
        audio.loop = true;
        audio.Play();
    }

    public void PlayPrologue()
    {
        isStage = false;
        audio.Stop();
        audio.clip = prologueBgm;
        audio.loop = false;
        audio.Play();
    }

    public void PlayVillage()
    {
        audio.Stop();
        audio.clip = villageBgm;
        audio.loop = true;
        audio.Play();
    }

    public void PlayForest()
    {
        audio.Stop();
        audio.clip = forestBgm;
        audio.loop = true;
        audio.Play();
    }

    public void PlayStage()
    {
        isStage = true;
        audio.Stop();
        if (NavigationManager.instance.chapter == 0 || NavigationManager.instance.chapter == 7)
        {
            audio.clip = villageBgm;
        }
        else if (NavigationManager.instance.chapter == 1 || NavigationManager.instance.chapter == 8)
        {
            audio.clip = forestBgm;
        } 
        audio.loop = true;
        audio.Play();
    }

    public void PlayUrgent()
    {
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
