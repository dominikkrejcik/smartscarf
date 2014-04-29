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
		GameObject source = Instantiate(soundSource, posistionVector, transform.rotation) as GameObject;
		audioSourceList.Add(source);
		
		SoundSourceBehaviour sourceBehaviour = source.GetComponent<SoundSourceBehaviour>();
		sourceBehaviour.setID(ID);

		updateSoundSource(sound, (audioSourceList.Count-1));
	}

	void updateSoundSource(AudioClip sound, int index)
	{
		AudioSource sourceComponent = audioSourceList[index].GetComponent<AudioSource>();
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

		for (int i=0; i<audioSourceList.Count; i++)
		{			
			SoundSourceBehaviour sourceBehaviour = audioSourceList[i].GetComponent<SoundSourceBehaviour>();
			if (sourceBehaviour.getID() == ID)
			{
				//update current source
				updateSoundSource(soundClip, i);
				return;
			}
		}

		//add new source if loop completes
		addSoundSource(soundClip, ID);
		return;
	}

	// Update is called once per frame
	void Update () {
	


	}


}