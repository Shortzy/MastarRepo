using UnityEngine;
using System.Collections;

public class PlayModeCameraControl : MonoBehaviour
{
	public Camera cam;
	public float x = -2;
	public float y = 2;
	public float y2 = 1;

	public bool lookAtCar = false;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (lookAtCar) {
			GameObject car = GameObject.Find ("Car") as GameObject;
			cam.transform.position = car.transform.position + new Vector3 (x, y, 0); 
			cam.transform.LookAt (car.transform.position + new Vector3 (0, y2, 0));
			cam.orthographic = false;
		} else {
			cam.orthographic = true;
			cam.transform.position = Vector3.zero;
			cam.transform.rotation = Quaternion.Euler (Vector3.zero);
		}
	}
}
