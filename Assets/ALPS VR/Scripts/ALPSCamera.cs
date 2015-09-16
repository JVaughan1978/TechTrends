/************************************************************************
	ALPSCamera manages both cameras required for stereo vision
	
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

public class ALPSCamera : MonoBehaviour{

	//=====================================================================================================
	// Attributes
	//=====================================================================================================

	/**Public**/
	public static ALPSConfig deviceConfig;
	public bool leftEye;
	public Camera cameraComponent;
	public Material barrelMaterial;

	/**Private**/
	private Vector2 CenterILD;
	private Vector2 CenterIPD;
	private Vector2 MinusCenterIPDMask; 
	private Vector2 OneMinusCenterIPDMask;

	//=====================================================================================================
	// Functions
	//=====================================================================================================

	/// <summary>
	/// Initializes the camera.
	/// </summary>
	public void Init(){
		cameraComponent = GetComponent<Camera> ();
		Vector3 camPos = cameraComponent.transform.localPosition; 
		camPos.x = (leftEye?-1:1) * deviceConfig.stereoDistance * 0.0005f;
		//We divide by a thousand because neckPivotToEye is in mm while Unity is in meter
		camPos.z = ALPSConfig.neckPivotToEye.x * 0.001f;
		camPos.y = ALPSConfig.neckPivotToEye.y * 0.001f;
		cameraComponent.transform.localPosition = camPos;
	}

	/// <summary>
	/// Updates the shaders used for barrel distortion and chromatic correction.
	/// </summary>
	public void UpdateDistortion(){

		cameraComponent.rect = new Rect ((leftEye?0f:0.5f),0f,0.5f,1f);
		cameraComponent.aspect = ALPSController.screenWidthPix*0.5f / ALPSController.screenHeightPix;

		float normalizedILD = NormalizedDistance (deviceConfig.ILD);


		CenterILD = new Vector2 ((leftEye ? -1 : 1) * (0.5f - normalizedILD), -0.5f + 0.5f);;

		//Seems that CenterIPD is better to be set in the middle of the screen. Tests with IPD matching the user's
		//IPD give very weird results..
		CenterIPD = new Vector2 ((leftEye?0.5f:-0.5f+1), 0.5f);
		//CenterIPD = new Vector2 ((leftEye?normalizedIPD:-normalizedIPD+1)+CenterILD.x, 0.5f);

		MinusCenterIPDMask = -CenterIPD;
		OneMinusCenterIPDMask = MinusCenterIPDMask + new Vector2 (1.0f, 1.0f);
		barrelMaterial = Resources.Load ("Materials/BarrelDistortion"+(deviceConfig.chromaticCorrectionActive?"CC":"")+(leftEye?"Left":"Right")) as Material;

		barrelMaterial.SetVector ("_CenterIPD",CenterIPD);
		barrelMaterial.SetVector ("_CenterILD",CenterILD);
		barrelMaterial.SetVector ("_MinusCenterIPDMask",MinusCenterIPDMask);
		barrelMaterial.SetVector ("_OneMinusCenterIPDMask",OneMinusCenterIPDMask);

		if (deviceConfig.barrelDistortionActive) {
			barrelMaterial.SetVector ("_k", new Vector2 (deviceConfig.K1, deviceConfig.K2));
		} else {
			barrelMaterial.SetVector ("_k", new Vector2 (0, 0));
		}
	
		//If the screen of the smartphone is smaller, the displayed distortion is in fact just a portion
		//of the real distortion which is fixed relatively to the device
		Vector2 radiusCoef;
		if(deviceConfig.width !=0 || deviceConfig.height != 0){
			radiusCoef = new Vector2 ((float)ALPSController.screenWidthMm / (float)deviceConfig.width, (float)ALPSController.screenHeightMm / (float)deviceConfig.height);
		}else{
			radiusCoef = new Vector2 (1,1);
		}
		barrelMaterial.SetVector ("_radiusCoef",radiusCoef);
		barrelMaterial.SetFloat ("_cc",deviceConfig.CC*0.01f);
	}
	
	/// <summary>
	/// Normalized lens distance to the center of the screen. The origin is on the bottom left corner.
	/// The middle of the half screen is represented by the vector (0.5f,0.5f).
	/// </summary>
	/// <param name="_ILD">Inter-Lens Distance or Inter-Pupillary Distance in mm.</param>
	public static float NormalizedDistance(float _ID){
		return ALPSController.MmToPixel (_ID) / (ALPSController.screenWidthPix);
	}

	/// <summary>
	/// Draws render texture on mesh.
	/// </summary>
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit (source, destination, barrelMaterial);
	}
}