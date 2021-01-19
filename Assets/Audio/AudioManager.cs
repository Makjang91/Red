using UnityEngine;


public class AudioManager : MonoBehaviour
{

    public AudioClip[] playlist;
    public AudioSource audiosource;
    private static AudioManager instance;
    public int musicIndex = 0;

    private void Awake()
    {
        instance = FindObjectOfType<AudioManager>();

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audiosource.clip = playlist[0];
        audiosource.Play();
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(!audiosource.isPlaying)
        {
            PlayNextSong();
        }
    }

    void PlayNextSong()
    {
        musicIndex = (musicIndex + 1) % playlist.Length;
        audiosource.clip = playlist[musicIndex];
        audiosource.Play();
    }
}
