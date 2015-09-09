/************************************************************************
	ALPSControllerEditor is a custom editor for ALPSController class
	
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
using UnityEditor;

[System.Serializable]
[CustomEditor(typeof(ALPSController))]
public class ALPSControllerEditor : Editor {

	//=====================================================================================================
	// Attributes
	//=====================================================================================================

	/**Public**/
	public ALPSConfig deviceConfig;
	public ALPSController controller;

	public Device Device{
		get{
			return deviceConfig.deviceName;
		}
		set{
			if(deviceConfig.deviceName != value){
				controller.SetDevice(value);
				OnEnable();
			}
		}
	}

	//=====================================================================================================
	// Functions
	//=====================================================================================================

	public void OnEnable()
	{
		controller = (ALPSController)target;
		deviceConfig = (controller.deviceConfig == null)? ALPSDevice.GetConfig(Device.DEFAULT):controller.deviceConfig;
		controller.deviceConfig = deviceConfig;
		ALPSCamera.deviceConfig = deviceConfig;
	}

	public override void OnInspectorGUI(){

		deviceConfig = controller.deviceConfig;

		//Device
		Device = (Device)EditorGUILayout.EnumPopup("Device:",Device);

		//IPD
		deviceConfig.IPD = EditorGUILayout.FloatField (new GUIContent("IPD", "Inter Pupilary Distance in millimeter. This must match the distance between the user's eyes"),deviceConfig.IPD);

		//ILD
		deviceConfig.ILD = EditorGUILayout.FloatField (new GUIContent("ILD","Inter Lens Distance in millimeter. This is the distance between the lenses of the headset."),deviceConfig.ILD);
	
		//Stereo distance
		deviceConfig.stereoDistance = EditorGUILayout.FloatField (new GUIContent("Stereo distance", "Distance between both cameras in millimeter (default is IPD). This can be tweaked to change the stereo strength."),deviceConfig.stereoDistance);

		//Field Of View
		deviceConfig.fieldOfView = EditorGUILayout.Slider ("Vertical FOV",deviceConfig.fieldOfView, 1, 180);

		//Screen size
		deviceConfig.width = EditorGUILayout.IntField (new GUIContent("Headset width", "Width of the viewport in millimeter"), deviceConfig.width);
		deviceConfig.height = EditorGUILayout.IntField (new GUIContent("Headset height", "Height of the viewport in millimeter"), deviceConfig.height);

		//Barrel distortion
		deviceConfig.barrelDistortionActive = EditorGUILayout.Toggle ("Barrel distortion", deviceConfig.barrelDistortionActive); 
		if (deviceConfig.barrelDistortionActive) {
			deviceConfig.K1 = EditorGUILayout.FloatField ("\tk1", deviceConfig.K1);
			deviceConfig.K2 = EditorGUILayout.FloatField ("\tk2", deviceConfig.K2);
		}

		//Chromatic correction
		deviceConfig.chromaticCorrectionActive = EditorGUILayout.Toggle ("Chromatic correction",deviceConfig.chromaticCorrectionActive);
		if (deviceConfig.chromaticCorrectionActive) {
			deviceConfig.CC = EditorGUILayout.FloatField ("\tCorrection intensity", deviceConfig.CC);
		}

		//Crosshairs
		controller.crosshairsEnabled = EditorGUILayout.Toggle("Crosshair", controller.crosshairsEnabled); 

		if (GUI.changed) {
			controller.ClearDirty();
			EditorUtility.SetDirty(target);
		}
	}
}
