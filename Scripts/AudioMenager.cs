using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioMenager : MonoBehaviour
{

    public List<Song> songs;
    public static AudioMenager instance;

    // Start is called before the first frame update
    void Awake()
    {
        
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach (var s in songs)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        if (SceneManager.GetActiveScene().buildIndex == 0) Play("nordic");
    }

    public void Play(string name)
    {
        var s = songs.Find(song => song.name.ToLower() == name.ToLower());
        if (s == null)
            return;
        s.source.Play();
    }
    public void PlayOnly(string name)
    {
        foreach (var song in songs) song.source.Stop();

        var s = songs.Find(song => song.name.ToLower() == name.ToLower());
        if (s == null)
            return;
        s.source.Play();
    }

}
