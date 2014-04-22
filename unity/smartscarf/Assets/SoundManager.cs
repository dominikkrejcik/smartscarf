using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {
	
	public GameObject soundSource;

	private List<GameObject> audioSourceList = new List<GameObject>();

	// Use this for initialization
	void Start () {

	}

	Vector3 getNewPosistionVector()
	{
		//place sources around user
		
		return (Quaternion.Euler(0, 0, ((360f)/(audioSourceList.Count+1)	)) * Vector3.right * 4);
		
	}

	void addSoundSource(AudioClip sound, int ID)
	{
		Vector3 posistionVector = getNewPosistionVector();
		GameObject source = Instantiate(soundSource, posistionVector, Quaternion.identity) as GameObject;
		audioSourceList.Add(source);

		updateSoundSource(sound, ID);
	}

	void updateSoundSource(AudioClip sound, int ID)
	{
		AudioSource sourceComponent = audioSourceList[ID].GetComponent<AudioSource>();
		sourceComponent.audio.clip = sound;
		sourceComponent.Play();
	}

	void removeSoundSource(int ID)
	{
		Destroy(audioSourceList[ID]);
		audioSourceList.RemoveAt(ID);
	}

	public void removeSoundSources()
	{
		
		for (int i=0; i < audioSourceList.Count; i++)
		{
			Destroy(audioSourceList[i]);
		}
		
		audioSourceList.Clear();
	}

	public void receiveFloats(float[] dataFloats, int ID)
	{

		AudioClip soundClip = AudioClip.Create("clip", dataFloats.Length, 1, 8000, true, false);

		soundClip.SetData(dataFloats, 0);

		if (ID >= audioSourceList.Count)
		{
			//add new source
			addSoundSource(soundClip, ID);

		}
		else
		{
			//update current source
			updateSoundSource(soundClip, ID);

		}
	}

	// Update is called once per frame
	void Update () {
	


	}


}