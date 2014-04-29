using UnityEngine;
using System.Collections;

public class SoundSourceBehaviour : MonoBehaviour {

	public GameObject talkIndicator;
	public Texture2D[] profiles;

	private int ID;

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

	public int getID()
	{
		return ID;
	}

	public void setID(int id)
	{
		ID = id;
		renderer.material.mainTexture = profiles[id];
	}
}
