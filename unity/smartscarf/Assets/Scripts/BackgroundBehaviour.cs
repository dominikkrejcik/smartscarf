using UnityEngine;
using System.Collections;

public class BackgroundBehaviour : MonoBehaviour {

	private Texture2D background;
	private Mesh mesh1, mesh2;

	// Use this for initialization
	void Start () {

		createPlane();
	}

	void createPlane()
	{
		Vector3[] vecs = vecArrMake();
		Mesh mesh = meshMaker(vecs, uvArrMake(vecs), triArrMake());
		Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);

	}
	
	Vector3[] vecArrMake()
	{
		Vector3[] vecArr = new Vector3[4];
		
		vecArr[0] = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane));
		vecArr[1] = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
		vecArr[2] = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane));
		vecArr[3] = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

		return vecArr;
	}
	
	Vector2[] uvArrMake(Vector3[] vecArr)
	{
		Vector2[] uv = new Vector2[vecArr.Length];
		float scaleAndTransform = 0.5f;
		
		for (int i=0; i<vecArr.Length; i++)
		{
			uv[i].x = -((vecArr[i].x/4)*scaleAndTransform)+scaleAndTransform;
			uv[i].y = ((vecArr[i].y/4)*scaleAndTransform)+scaleAndTransform;
		}
		return uv;
	}
	
	int[] triArrMake()
	{
		//Triangles for the mesh; the actual polygons, simply an array of groups of 3 ints. This is used to tell what order the vectors should be joined up to make the polygons
		int[] tri = new int[2*3];
		int arrayOffset = 0;
		int triOffset = 1;
		
		for (int i=0; i<tri.Length; i+=3)
		{	
			tri[i+arrayOffset] = 0;
			arrayOffset++;
			tri[i+arrayOffset] = triOffset;
			arrayOffset++;
			triOffset++;
			tri[i+arrayOffset] = triOffset;
			arrayOffset = 0;
		}
		return tri;
	}
	
	Mesh meshMaker(Vector3[] newVertices, Vector2[] newUV, int[] newTriangles)
	{
		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		mesh.vertices = newVertices;
		mesh.uv = newUV;
		mesh.triangles = newTriangles;
		mesh.RecalculateNormals();
		return mesh;
	}

	public void setTexture(Texture2D tex)
	{		
		renderer.material.mainTexture = tex;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
