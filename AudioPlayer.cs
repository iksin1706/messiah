using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public string startSongName;
    private AudioMenager menager;

    void Start()
    {
        menager = GameObject.Find("AudioMeneger").GetComponent<AudioMenager>();
        PlayOnly(startSongName);
    }

    public void PlayOnly(string name)
    {
        menager.PlayOnly(name);
    }

    public void Play(string name)
    {
        menager.Play(name);
    }







}
