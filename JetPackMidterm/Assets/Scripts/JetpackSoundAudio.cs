using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackSoundAudio : MonoBehaviour {
	public GameObject _MMOCharacterController;
	public AudioClip MusicClip;
	public AudioSource MusicSource;
	// Use this for initialization
	void Start () {
		MusicSource.clip = MusicClip;
		MusicSource.Stop();

	}
	
	// Update is called once per frame
	void Update () {
		if(_MMOCharacterController.GetComponent<MMOCharacterController>().CheckFuel())
		{
			if(Input.GetKeyDown(KeyCode.Space)) MusicSource.Play();
			if(Input.GetKeyUp(KeyCode.Space)) MusicSource.Stop();
		}
		else MusicSource.Stop();
	}
}
