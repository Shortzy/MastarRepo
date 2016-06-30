using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{

	[RequireComponent (typeof(CarController))]
	public class CarUserControl : MonoBehaviour
	{

		public float v = 0;

		private CarController m_Car;
		// the car controller we want to use


		private void Awake ()
		{
			// get the car controller
			m_Car = GetComponent<CarController> ();
		}

		public void fullSpeed ()
		{
			v = 1;
		}

		public void brakeSpeed ()
		{
			v = -1;
		}

		public void stopSpeed(){
			v = 0;
		}
		private void FixedUpdate ()
		{
			// pass the input to the car!
#if !MOBILE_INPUT
			float h = CrossPlatformInputManager.GetAxis ("Horizontal");
			float v = CrossPlatformInputManager.GetAxis ("Vertical");
			float handbrake = CrossPlatformInputManager.GetAxis ("Jump");
			m_Car.Move (h, v, v, handbrake);
#else
			float h = Input.acceleration.x;
			m_Car.Move (h, v, v, 0f);
#endif
		}
	}
}
