using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {
	
	public GameObject soundSource;
	public AudioClip clip;

	
	private List<GameObject> audioSourceList = new List<GameObject>();

	// Use this for initialization
	void Start () {
		float[] samples = new float[clip.samples * clip.channels];

		clip.GetData(samples, 0);

		receiveFloats(samples, 0);
	}
	
	Vector3 getNewPosistionVecotr()
	{
		//place sources around user
		
		return (Quaternion.Euler(0, 0, ((360f)/(audioSourceList.Count+1)	)) * Vector3.right * 4);
		
	}

	void addSoundSource(AudioClip sound)
	{
		Vector3 posistionVector = getNewPosistionVecotr();
		
		GameObject source = Instantiate(soundSource, posistionVector, Quaternion.identity) as GameObject;
		
		audioSourceList.Add(source);
		
		AudioSource sourceComponent = source.GetComponent<AudioSource>();
		sourceComponent.PlayOneShot(sound);
	}

	void updateSoundSource(AudioClip sound, int ID)
	{
		AudioSource sourceComponent = audioSourceList[ID].GetComponent<AudioSource>();
		sourceComponent.PlayOneShot(sound);
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

	void receiveFloats(float[] dataFloats, int ID)
	{
		AudioClip soundClip = clip;

		soundClip.SetData(dataFloats, 0);

		if (ID >= audioSourceList.Count)
		{
			//add new source
			addSoundSource(soundClip);

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
