/************************************************************************
	ALPSGyro is an interface for head tracking using Android native sensors

    Copyright (C) 2015  ALPS VR.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

************************************************************************/

using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class ALPSGyro : MonoBehaviour {
#if !UNITY_EDITOR	
	//=====================================================================================================
	// Attributes
	//=====================================================================================================

	/**Private**/
	private Quaternion landscapeLeft = Quaternion.Euler(90, 0, 0);
	private Quaternion orientation = Quaternion.identity;
	private float q0,q1,q2,q3;
	private Gyroscope gyro;

	//=====================================================================================================
	// Functions
	//=====================================================================================================
	#if UNITY_ANDROID
	[DllImport ("alps_native_sensor")] private static extern void get_q(ref float q0,ref float q1,ref float q2,ref float q3);
	[DllImport ("alps_native_sensor")] private static extern void init();
	#endif
	/// <summary>
	/// Initializes ALPS native plugin.
	/// </summary>
	public void Awake(){
		#if UNITY_ANDROID
			init();
		#elif UNITY_IPHONE
			gyro = Input.gyro;
			gyro.enabled=true;
		#endif
	}

	/// <summary>
	/// Updates head orientation after all Update functions have been called.
	/// </summary>
	public void LateUpdate () {
		getOrientation();
		transform.localRotation = landscapeLeft * orientation;
	}

	/// <summary>
	/// Gets orientation from ALPS native plugin.
	/// </summary>
	public void getOrientation(){
		#if UNITY_ANDROID
			get_q (ref q0,ref q1,ref q2,ref q3);
			orientation.x = q1;
			orientation.y = -q0;
			orientation.z = q2;
			orientation.w = q3;
		#elif UNITY_IPHONE
			orientation.x = -gyro.attitude.x;
			orientation.y = -gyro.attitude.y;
			orientation.z = gyro.attitude.z;
			orientation.w = gyro.attitude.w;
		#endif
	}
#endif
}
