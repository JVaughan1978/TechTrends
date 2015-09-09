/************************************************************************
	ALPSController is the main class which manages custom rendering

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
using System.Collections;

[System.Serializable]
public class ALPSController : MonoBehaviour {
	
	//=====================================================================================================
	// Attributes
	//=====================================================================================================
	/**Constants**/
	public const float INCH_TO_MM = 25.4f;
	public static float DEFAULT_DPI = 96f;
	
	/**Public**/
	//The current device configuration
	public ALPSConfig deviceConfig = ALPSDevice.GetConfig(Device.DEFAULT);
	
	//One camera for each eye
	public GameObject cameraLeft;
	public GameObject cameraRight;
	public GameObject crosshairs;
	public ALPSGUI gui;

	//Head represents user's head
	public GameObject head;
	
	//Screen size
	public static int screenWidthPix;
	public static int screenHeightPix;
	public static int screenWidthMm;
	public static int screenHeightMm;
	
	//Crosshairs
	public bool crosshairsEnabled;
	
	/**Private**/
	public static float DPI;
	
	//=====================================================================================================
	// Functions
	//=====================================================================================================
	
	/// <summary>
	/// Initializes side-by-side rendering and head tracking. 
	/// </summary>
	public void Awake(){
		Application.targetFrameRate = 60;

		ALPSGUI.controller = this;
		PropagateConfig ();

		head = GameObject.Find("ALPSHead");
		if(head == null) head = new GameObject("ALPSHead");
		head.transform.parent = transform;
		head.transform.position = transform.position;

		crosshairs = GameObject.Find("ALPSCrosshairs");
		gui = GameObject.FindObjectOfType(typeof(ALPSGUI)) as ALPSGUI;

		DPI = Screen.dpi;

		#if UNITY_EDITOR
		head.AddComponent(typeof(MouseLook));
		screenWidthPix = Screen.width;
		screenHeightPix = Screen.height;
		#elif UNITY_ANDROID || UNITY_IPHONE
			if(head.GetComponent<ALPSGyro>() == null)
				head.AddComponent<ALPSGyro>();
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			#if UNITY_ANDROID 
				ALPSAndroid.Init (); 
				screenWidthPix = ALPSAndroid.WidthPixels ();
				screenHeightPix = ALPSAndroid.HeightPixels ();
			#elif UNITY_IPHONE
				screenWidthPix = Screen.width;
				screenHeightPix = Screen.height;
			#endif
		#endif
		
		//Make sure the longer dimension is width as the phone is always in landscape mode
		if(screenWidthPix<screenHeightPix){
			int tmp = screenHeightPix;
			screenHeightPix = screenWidthPix;
			screenWidthPix = tmp;
		}
		screenWidthMm = (int)PixelToMm (screenWidthPix);
		screenHeightMm = (int)PixelToMm (screenHeightPix);

		for (var i=0; i<2; i++) {
			bool left = (i==0);
			
			GameObject OneCamera = GameObject.Find(left?"CameraLeft":"CameraRight");
			if(OneCamera == null) OneCamera = new GameObject(left?"CameraLeft":"CameraRight");
			if(OneCamera.GetComponent<Camera>() == null) OneCamera.AddComponent<Camera>();
			if(OneCamera.GetComponent<ALPSCamera>() == null) OneCamera.AddComponent<ALPSCamera>();
			OneCamera.GetComponent<ALPSCamera>().leftEye = left;
			OneCamera.transform.parent = head.transform;
			OneCamera.transform.position = head.transform.position;
			if(left)cameraLeft = OneCamera;
			else cameraRight = OneCamera;
		}

		// Setting the cameras' depth
		cameraLeft.GetComponent<Camera>().depth = 0;
		cameraRight.GetComponent<Camera>().depth = 1;

		ALPSCamera[] ALPSCameras = FindObjectsOfType(typeof(ALPSCamera)) as ALPSCamera[];
		foreach (ALPSCamera cam in ALPSCameras) {
			cam.Init();
		}

		if(gui != null) gui.Init ();

		ClearDirty();
	}

	/// <summary>
	/// Propagates the current configuation to the other classes
	/// </summary>
	public void PropagateConfig(){
		ALPSCamera.deviceConfig = deviceConfig;
		ALPSGUI.deviceConfig = deviceConfig;
	}
	
	/// <summary>
	/// Resets all the settings and applies the current DeviceConfig
	/// </summary>
	public void ClearDirty(){
		if (DPI <= 0) {
			DPI = DEFAULT_DPI;
		}
		
		updateCameras ();
		
		if(crosshairs!=null) crosshairs.SetActive (crosshairsEnabled);

		PropagateConfig ();

		if (gui != null) {
			gui.updateConfigText ();
		}
	}

	/// <summary>
	/// Updates the cameras. (Barrel distortion, Chromatic correction, position, etc.)
	/// </summary>
	public void updateCameras(){
		if(cameraLeft!=null && cameraRight!=null){
			
			Vector3 camLeftPos = cameraLeft.transform.localPosition; 
			camLeftPos.x = -deviceConfig.stereoDistance * 0.0005f;
			cameraLeft.transform.localPosition = camLeftPos;
			
			Vector3 camRightPos = cameraRight.transform.localPosition;
			camRightPos.x = deviceConfig.stereoDistance * 0.0005f;
			cameraRight.transform.localPosition = camRightPos;
			
			cameraLeft.GetComponent<Camera>().fieldOfView = deviceConfig.fieldOfView;
			cameraRight.GetComponent<Camera>().fieldOfView = deviceConfig.fieldOfView;
			
			cameraLeft.GetComponent<ALPSCamera>().UpdateDistortion();
			cameraRight.GetComponent<ALPSCamera>().UpdateDistortion();
		}
	}
	
	/// <summary>
	/// Sets a new device configuration.
	/// </summary>
	// <param name="_device">Name of the device.</param>
	public void SetDevice(Device _device){
		deviceConfig = ALPSDevice.GetConfig (_device);
		ClearDirty ();
	}

	/// <summary>
	/// Set K1 to x.
	/// </summary>
	// <param name="_x">K1.</param>
	public void SetDeviceK1(float _x){
		deviceConfig.K1 = _x;
		ClearDirty ();
	}
	
	/// <summary>
	/// Set K2 to x.
	/// </summary>
	// <param name="_x">K2.</param>
	public void SetDeviceK2(float _x){
		deviceConfig.K2 = _x;
		ClearDirty ();
	}
	
	/// <summary>
	/// Set Chromatic Correction to x.
	/// </summary>
	// <param name="_x">Chromatic Correction.</param>
	public void SetDeviceCC(float _x){
		deviceConfig.CC = _x;
		ClearDirty ();
	}

	/// <summary>
	/// Set IPD to x.
	/// </summary>
	// <param name="_x">IPD in mm.</param>
	public void SetDeviceIPD(float _x){
		deviceConfig.IPD = _x;
		ClearDirty ();
	}

	/// <summary>
	/// Set ILD to x.
	/// </summary>
	// <param name="_x">ILD in mm.</param>
	public void SetDeviceILD(float _x){
		deviceConfig.ILD = _x;
		ClearDirty ();
	}

	/// <summary>
	/// Is Barrel Distortion active.
	/// </summary>
	// <param name="_isActive">Is Barrel Distortion active or not.</param>
	public void SetBarrelDistortionActive(bool _isActive){
		deviceConfig.barrelDistortionActive = _isActive;
		ClearDirty ();
	}

	/// <summary>
	/// Is Chromatic Correction active.
	/// </summary>
	// <param name="_isActive">Is Chromatic Correction active or not.</param>
	public void SetChromaticCorrectionActive(bool _isActive){
		deviceConfig.chromaticCorrectionActive = _isActive;
		ClearDirty ();
	}

	/// <summary>
	/// Are crosshairs active.
	/// </summary>
	// <param name="_isActive">Are crosshairs active or not.</param>
	public void SetCrosshairsActive(bool _isActive){
		crosshairsEnabled = _isActive;
		ClearDirty ();
	}

	/// <summary>
	/// Set Device Width to x in mm.
	/// </summary>
	// <param name="_x">Device Width in mm.</param>
	public void SetDeviceWidth(int _x){
		deviceConfig.width = _x;
		ClearDirty ();
	}
	
	/// <summary>
	/// Set Device Height to x in mm.
	/// </summary>
	// <param name="_x">Device Height in mm.</param>
	public void SetDeviceHeight(int _x){
		deviceConfig.height = _x;
		ClearDirty ();
	}
	
	/// <summary>
	/// Add x to K1.
	/// </summary>
	// <param name="_x">Float to add to K1.</param>
	public void AddToK1(float _x){
		deviceConfig.K1 += _x;
		ClearDirty ();
	}
	
	/// <summary>
	/// Add x to K2.
	/// </summary>
	// <param name="_x">Float to add to K2.</param>
	public void AddToK2(float _x){
		deviceConfig.K2 += _x;
		ClearDirty ();
	}
	
	/// <summary>
	/// Add x to Chromatic Correction.
	/// </summary>
	// <param name="_x">Float to add to Chromatic Correction.</param>
	public void AddToChromaticCorrection(float _x){
		deviceConfig.CC += _x;
		ClearDirty ();
	}

	/// <summary>
	/// Add x to IPD.
	/// </summary>
	// <param name="_x">Float to add to IPD in mm.</param>
	public void AddToIPD(float _x){
		deviceConfig.IPD += _x;
		ClearDirty ();
	}
	
	/// <summary>
	/// Add x to ILD.
	/// </summary>
	// <param name="_x">Float to add to ILD in mm.</param>
	public void AddToILD(float _x){
		deviceConfig.ILD += _x;
		ClearDirty ();
	}

	/// <summary>
	/// Add x to Device Width in mm.
	/// </summary>
	// <param name="_x">int to add to Device Width in mm.</param>
	public void AddToDeviceWidth(int _x){
		deviceConfig.width += _x;
		ClearDirty ();
	}

	/// <summary>
	/// Add x to Device Height in mm.
	/// </summary>
	// <param name="_x">int to add to Device Height in mm.</param>
	public void AddToDeviceHeight(int _x){
		deviceConfig.height += _x;
		ClearDirty ();
	}


	/// <summary>
	/// Copy camera settings to left and right cameras. Will overwrite culling masks.
	/// </summary>
	/// <param name="_cam">The camera from which you want to copy the settings.</param>
	public void SetCameraSettings(Camera _cam){
		cameraLeft.GetComponent<Camera>().CopyFrom (_cam);
		cameraRight.GetComponent<Camera>().CopyFrom (_cam);
		cameraLeft.GetComponent<Camera>().rect = new Rect (0,0,0.5f,1);
		cameraRight.GetComponent<Camera>().rect = new Rect (0.5f,0,0.5f,1);
	}
	
	/// <summary>
	/// Adds left and right layers to the existing culling masks for left and right cameras.
	/// </summary>
	/// <param name="_leftLayer">Name of the layer rendered by the left camera.</param>
	/// <param name="_rightLayer">Name of the layer rendered by the right camera.</param>
	public int SetStereoLayers(string _leftLayer, string _rightLayer){
		int leftLayer = LayerMask.NameToLayer (_leftLayer);
		int rightLayer = LayerMask.NameToLayer (_rightLayer);
		if (leftLayer < 0 && rightLayer < 0) return -1;
		
		cameraLeft.GetComponent<Camera>().cullingMask |= 1 << LayerMask.NameToLayer(_leftLayer);
		cameraLeft.GetComponent<Camera>().cullingMask &=  ~(1 << LayerMask.NameToLayer(_rightLayer));
		
		cameraRight.GetComponent<Camera>().cullingMask |= 1 << LayerMask.NameToLayer(_rightLayer);
		cameraRight.GetComponent<Camera>().cullingMask &=  ~(1 << LayerMask.NameToLayer(_leftLayer));
		
		return 0;
	}
	
	/// <summary>
	/// Returns point of view position. This can be useful for setting up a Raycast.
	/// </summary>
	public Vector3 PointOfView(){
		//returns current position plus NeckToEye vector
		return new Vector3(transform.position.x,transform.position.y + ALPSConfig.neckPivotToEye.y*0.001f,transform.position.z + ALPSConfig.neckPivotToEye.x*0.001f);
	}
	
	/// <summary>
	/// Returns right camera forward direction vector. This can be useful for setting up a Raycast.
	/// </summary>
	public Vector3 RaycastForwardDirection(){
		return cameraRight.GetComponent<Camera>().transform.forward;
	}
	
	/// <summary>
	/// Returns the position of either the left camera, the right camera or the center point. This may be useful for setting up a Raycast.
	/// </summary>
	/// <param name="_origin">Either "left", "right" or "center".</param>
	public Vector3 RaycastOrigin(string _origin){
		Vector3 origin = new Vector3();
		switch(_origin.ToUpper ()){
		case "LEFT":
			origin = cameraLeft.transform.position;
			break;
		case "RIGHT":
			origin = cameraRight.transform.position;
			break;
		case "CENTER":
			origin = PointOfView();
			break;
		default:
			Debug.LogError("RaycastOrigin(string _origin): You can only choose 'left', 'right' or 'center' as a Raycast origin. "+_origin+" is not an expected value");
			break;
		}
		return origin;
	}
	
	/// <summary>
	/// Sets the vertical field of view for both cameras.
	/// </summary>
	/// <param name="_fov">The vertical fiel of view to be set (between 1 and 180).</param>
	public void setFieldOfView(float _fov){
		if (_fov < 1 || _fov > 180) {
			Debug.LogWarning("setFieldOfView(float _fov): Field of view must range between 1 and 180");
			_fov = (_fov<1)?1:180;
		}
		deviceConfig.fieldOfView = _fov;
		ClearDirty ();
	}
	
	/// <summary>
	/// Returns left and right cameras.
	/// </summary>
	public Camera[] GetCameras(){
		Camera[] cams = {cameraLeft.GetComponent<Camera>(), cameraRight.GetComponent<Camera>()};
		return cams;
	}

	public static float PixelToMm(float pixel){
		return (float)((INCH_TO_MM * pixel) / DPI);
	} 
	public static float MmToPixel(float mm){
		return (float)(DPI * (mm / INCH_TO_MM));
	}
} 