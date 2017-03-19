using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

   // static SoundManager soundmanager;
    AudioSource[] audiosources;

    public static SoundManager Instance;

	// Use this for initialization
	void Start () {
        Instance = this;
        audiosources = GetComponents<AudioSource>();
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
