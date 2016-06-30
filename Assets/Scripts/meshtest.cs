using UnityEngine;
using System.Collections;

public class meshtest : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		Mesh mesh = new Mesh ();

//		mf = GetComponent <MeshFilter> ().mesh;


		Vector3[] newVer = new Vector3[10];
//		for (int i = 0; i < 3; i++) {
		//			newVer [i] = mesh.vertices [i];
//		}
		newVer [0] = new Vector3 (0, 0, 0);
		newVer [1] = new Vector3 (1, 0, 0);
		newVer [2] = new Vector3 (1, 1, 0);
		newVer [3] = new Vector3 (3, 2, 0);
		newVer [4] = new Vector3 (2, 1, 0);
		newVer [5] = new Vector3 (2, 2, 0);
		newVer [6] = new Vector3 (3, 1, 0);
		newVer [7] = new Vector3 (3, 3, 0);
		newVer [8] = new Vector3 (4, 2, 0);


		int[] tris = { 0, 2, 1, 1, 2, 4, 	 4, 2, 5, 4, 5, 3, 5, 7, 3, 3, 7, 8 };
		mesh.vertices = newVer;
		mesh.triangles = tris;

		MeshFilter meshFilter =	gameObject.AddComponent <MeshFilter> ();
		meshFilter.mesh = mesh;

//		foreach (int v in mesh.triangles) {
//			print (v);
//		}
//		foreach (Vector3 v in mesh.vertices) {
//			print (v.x + " " + v.y + " " + v.z);
//		}

	}

	// Update is called once per frame
	void Update ()
	{
//		float x = 5.3;
//		float y = 3.7;


	}
}
