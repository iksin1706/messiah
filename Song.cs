using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Song 
{
    public string name;

    public AudioClip clip;
    [Range(0,1)]
    public float volume=0.5f;
    [Range(-1, 2)]
    public float pitch=1;
    public bool loop;

    [HideInInspector]
    public AudioSource source;

}
