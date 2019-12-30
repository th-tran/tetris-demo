using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager _instance;
    public static SoundManager Instance { get { return _instance; } }
    public AudioSource MusicSource;
    public AudioClip bgMusic;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MusicSource.clip = bgMusic;
        MusicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
