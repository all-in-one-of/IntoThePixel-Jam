using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> {

    AudioSource[] audiosources;

	// Use this for initialization
	void Start () {
        audiosources = GetComponents<AudioSource>();
        //Background Music
        audiosources[0].Play();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void shatterSound()
    {
        int soundIndex = Random.Range(1, 5);
        audiosources[soundIndex].Play();
    }
}
