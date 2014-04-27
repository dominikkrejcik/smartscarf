using UnityEngine;
using System.Collections;

public class BackgroundMangagerBehaviour : MonoBehaviour {

	public Texture2D[] BGs;
	public GameObject BG1, BG2;
	public float switchTime = 15f;

	private BackgroundBehaviour BGB1, BGB2;
	private bool fadOut, fadIn;
	private float alphaIn,alphaOut;

	// Use this for initialization
	void Start () {
		
		BGB1 = BG1.GetComponent<BackgroundBehaviour>();
		BGB2 = BG2.GetComponent<BackgroundBehaviour>();

		fadOut = false;
		fadIn = false;
				
		alphaOut = 1f;
		alphaIn = 0f;

		setBGTex(BGB1);
		setBGTex(BGB2);
		
		InvokeRepeating("setFadeOut", 0f, switchTime);
		InvokeRepeating("setFadeIn", switchTime/2f, switchTime);
	}

	void setBGTex(BackgroundBehaviour BG)
	{
		BG.setTexture(BGs[Random.Range(0, BGs.Length)]);
	}
	
	void setFadeOut()
	{
		fadOut = true;
	}

	void setFadeIn()
	{
		fadIn = true;
	}
	
	void fadeOut()
	{		
		if (fadOut)
		{
			alphaOut = Mathf.Lerp(alphaOut, 0f, 0.5f*Time.deltaTime);
			Color color = BG1.renderer.material.color;
			color.a = alphaOut;

			BG1.renderer.material.color = color;

			if (alphaOut < 0.01f)
			{
				color.a = 0f;
				BG1.renderer.material.color = color;
				fadOut = false;
				alphaOut = 1f;
				setBGTex(BGB1);
			}
		}
	}
	
	void fadeIn()
	{
		if (fadIn)
		{
			alphaIn = Mathf.Lerp(alphaIn, 1f, 0.5f*Time.deltaTime);
			Color color = BG1.renderer.material.color;
			color.a = alphaIn;
			
			BG1.renderer.material.color = color;
			
			if (alphaIn > 0.99f)
			{
				color.a = 1f;
				BG1.renderer.material.color = color;
				fadIn = false;
				alphaIn = 0f;
				setBGTex(BGB2);
			}
		}
	}

	// Update is called once per frame
	void Update () {

		fadeIn();

		fadeOut();

	}
}
