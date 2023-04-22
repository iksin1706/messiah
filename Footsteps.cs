using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Footsteps : MonoBehaviour
{
    private NavMeshAgent agent;
    private AudioSource audioSource;
    public float timeDelay;

    float time = 0;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timeDelay = 1.5f /  agent.velocity.magnitude;
        time += Time.deltaTime;
        if (agent.desiredVelocity!=Vector3.zero && !audioSource.isPlaying&&time>=timeDelay)
        {
            
            audioSource.volume = Random.Range(0.04f, 0.1f);
            audioSource.pitch = Random.Range(0.5f, 1.2f);
            audioSource.Play();
            time = 0;
        }

    }


}
