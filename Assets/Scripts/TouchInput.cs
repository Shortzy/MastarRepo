using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection.Emit;
using System.IO;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;


public class TouchInput : MonoBehaviour
{
	public  float zWidth = 7f;
	public  float yWidth = .5f;
	public  Transform emptyTrackPrefab;
	public PlayModeCameraControl camControl;
	public GameObject car;
	public GameObject startPositionPad;
	public Vector3 carStartingPosition;
	public Button brakeBut;
	private GameObject mouseTrail;
	private bool stopTakingInput = false;
	public List<List<Vector3>> listOfDots;

	//	private GameObject[] trail = new GameObject[20];	//used for multitouch

	void Start ()
	{
		listOfDots = new List<List<Vector3>> ();
		listOfDots.Add (new List<Vector3> ());
		startSceneSettings ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	//take input either in desktop or mobile and store it in ListOfDots list
	void getInputIntoListOfDots ()
	{
		//Don't take input while in play mode
		if (stopTakingInput) {
			return;
		}

		// temporary var for holding the list item from ListOfDots list
		List<Vector3> vTemp = null;

		//mobile input using touch
		#if MOBILE_INPUT
		// Look for all fingers
		for (int i = 0; i < Input.touchCount; i++) {
			Touch touch = Input.GetTouch (i);
	
			// Store this new value
			vTemp = listOfDots [listOfDots.Count - 1];
			Vector3 position = Camera.main.ScreenToWorldPoint (touch.position) + new Vector3 (0, 0, 1);
			vTemp.Add (position);
			listOfDots [listOfDots.Count - 1] = vTemp;

			switch (touch.phase) {
			//the finger has just started touching. start drawing the trail
			case TouchPhase.Began:
				// Initiate the trail
				mouseTrail = SpecialEffectsScript.MakeTrail (position);
				break;
			
		    //the finger has finished touching. release the trail and draw the input and prepare for new input
			case  TouchPhase.Ended:
				// release the trail
				mouseTrail = null;

				// Implement (draw) the last trail
				drawTrackFromPointsList (listOfDots [listOfDots.Count - 1]);

				// add new space for next inputs
				listOfDots.Add (new List<Vector3> ());
				break;
			
			//any other state. move the trail with the finger
			default :
				// update the trail with movement
				mouseTrail.transform.position = position; 
			}
		}
		#endif


		//Desktop input using mouse
		#if !MOBILE_INPUT
		if (Input.GetMouseButton (0) == true) {
			// Get input
			vTemp = listOfDots [listOfDots.Count - 1];
			Vector3 position = Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward;
			vTemp.Add (position);
			listOfDots [listOfDots.Count - 1] = vTemp;

			// Draw the trail
			if (mouseTrail == null) { // initial place
				mouseTrail = SpecialEffectsScript.MakeTrail (position);
			}
			mouseTrail.transform.position = position; // update with movement
		}

		// after mouse up start a new trail and a new list for a new line
		if (Input.GetMouseButtonUp (0) == true) {
			mouseTrail = null;
			drawTrackFromPointsList (listOfDots [listOfDots.Count - 1]);
			listOfDots.Add (new List<Vector3> ());
		}
		#endif

	}

	public void startSceneSettings ()
	{
		Vector3 leftSide = Camera.main.ScreenToWorldPoint (new Vector2 (0, Screen.height / 2)) + new Vector3 (0, 0, 4.5f);
		startPositionPad.transform.position = leftSide;
		carStartingPosition = leftSide + new Vector3 (2f, 1f, 0);
		car.transform.position = carStartingPosition;
	}

	public void restartScene ()
	{
		SceneManager.LoadScene ("Main");
	}

	public void refreshScene ()
	{
		car.GetComponent <Rigidbody> ().velocity = Vector3.zero;
		car.transform.rotation = Quaternion.Euler (new Vector3 (0f, 90f, 0f));
		car.transform.position = carStartingPosition;
	}

	public void togglePlayMode ()
	{
		if (camControl.lookAtCar == false) {
			camControl.lookAtCar = true;
			stopTakingInput = true;
			brakeBut.gameObject.SetActive (true);
		} else {
			camControl.lookAtCar = false;
			stopTakingInput = false;
			brakeBut.gameObject.SetActive (false);
			car.GetComponent <CarUserControl> ().stopSpeed ();
		}
	}


	public  void drawTrackFromPointsList (List<Vector3> inVer)
	{
		if (inVer == null || inVer.Count < 1) {
			return;
		}
		List<Vector3> meshVer = new List<Vector3> ();
		List<int> meshTri = new List<int> ();

		// relative positions of the twin points
		Vector3[] twinVer = new Vector3[4];
		twinVer [0] = Vector3.zero;
		twinVer [1] = new Vector3 (0, 0, zWidth); 
		twinVer [2] = new Vector3 (0, yWidth, zWidth); 
		twinVer [3] = new Vector3 (0, yWidth, 0); 

		// First Point Special case
		for (int j = 0; j < 4; j++) {
			meshVer.Add (inVer [0] + twinVer [j % 4]);
		}

		meshTri.Add (0);
		meshTri.Add (2);
		meshTri.Add (1);

		meshTri.Add (0);
		meshTri.Add (3);
		meshTri.Add (2);

		// loop for the rest of the points
		for (int i = 1; i < inVer.Count; i++) {
				
			for (int j = 0; j < 4; j++) {
				meshVer.Add (inVer [i] + twinVer [j]);
			}

			int x = i * 4;

			// Right side
			meshTri.Add (x);
			meshTri.Add (x - 1);
			meshTri.Add (x + 3);


			meshTri.Add (x);
			meshTri.Add (x - 4);
			meshTri.Add (x - 1);


			// Top side
			meshTri.Add (x);
			meshTri.Add (x - 3);
			meshTri.Add (x - 4);

			meshTri.Add (x);
			meshTri.Add (x + 1);
			meshTri.Add (x - 3);


			//Left side);
			meshTri.Add (x + 1);
			meshTri.Add (x - 2);
			meshTri.Add (x - 3);


			meshTri.Add (x + 1);
			meshTri.Add (x + 2);
			meshTri.Add (x - 2);


			//Bot side);
			meshTri.Add (x + 3);
			meshTri.Add (x - 2);
			meshTri.Add (x + 2);


			meshTri.Add (x + 3);
			meshTri.Add (x - 1);
			meshTri.Add (x - 2);


			//Back side);
			meshTri.Add (x);
			meshTri.Add (x + 2);
			meshTri.Add (x + 1);


			meshTri.Add (x);
			meshTri.Add (x + 3);
			meshTri.Add (x + 2);

		}



		Mesh mesh = new Mesh ();
		mesh.vertices = meshVer.ToArray ();
		mesh.triangles = meshTri.ToArray ();

		Transform trackClone = Instantiate (emptyTrackPrefab, Vector3.zero, Quaternion.Euler (Vector3.zero)) as Transform;
		MeshFilter meshFilter =	trackClone.gameObject.AddComponent <MeshFilter> ();


		MeshCollider mc = trackClone.gameObject.GetComponent <MeshCollider> ();
		mc.sharedMesh = mesh;


		//Texture problem here
		Vector2[] uvs = new Vector2[meshVer.Count];
		for (int i = 0; i < uvs.Length; i++) {
			
		}
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
		meshFilter.sharedMesh = mesh;
		meshFilter.mesh = mesh;
	}
}
	

	

