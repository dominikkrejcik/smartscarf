using UnityEngine;
using System.Collections;

public class SoundSourceBehaviour : MonoBehaviour {

	public GameObject talkIndicator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(audio.isPlaying)
		{
			talkIndicator.renderer.enabled = true;
		}
		else
		{
			talkIndicator.renderer.enabled = false;
		}
	}
}
