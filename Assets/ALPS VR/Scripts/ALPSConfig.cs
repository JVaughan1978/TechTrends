/************************************************************************
	ALPSConfig describes a configuration for one particular device
		
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
public class ALPSConfig{

	//=====================================================================================================
	// Attributes
	//=====================================================================================================

	/**Public**/
	//Vector between eyes and the pivot point (neck) in mm.
	public static Vector2 neckPivotToEye = new Vector2 (80f,120f);
	
	//Configuration name.
	public Device deviceName;
	
	//Is barrel distortion enabled or not.
	public bool barrelDistortionActive;
	
	//Is chromatic correction enabled or not.
	public bool chromaticCorrectionActive;
	
	//Inter pupillary distance in millimeters. Must match the distance between
	//users' eyes.
	public float IPD;

	//Inter lens distance in millimeters. 
	public float ILD;

	//Stereo distance in millimeters. Should match the IPD but can be tweaked to increase 
	//or decrease the stereo effect. Basically, with an ILD set to 0, there is no 3D effect.
	public float stereoDistance;

	//Vertical field of view of both cameras.
	[Range(1,179)]
	public float fieldOfView;
	
	//Chromatic correction coefficient.
	public float CC;
	
	//Barrel distortion parameters.
	public float K1,K2;

	//Width of the viewport in mm.
	private int _width;
	public int width{
		get{return _width;}
		set{
			if(value<0)_width=0;
			else _width=value;
		}
	}

	//Height of the viewport in mm.
	private int _height;
	public int height{
		get{return _height;}
		set{
			if(value<0)_height=0;
			else _height=value;
		}
	}


	//=====================================================================================================
	// Functions
	//=====================================================================================================


	/// <summary>
	/// Creates a new device configuration.
	/// </summary>
	/// <param name="_DeviceName">Device name.</param>
	/// <param name="_EnableBarrelDistortion">True if barrel distortion must be enabled, false otherwise.</param>
	/// <param name="_EnableChromaticCorrection">True if chromatic correction must be enabled, false otherwise.</param>
	/// <param name="_FixedSize">True is viewport must be fixed insize, false if viewport must be fullscreen.</param>
	/// <param name="_IPD">Inter pupillary distance in millimeters. Must match the distance between users' eyes</param>
	/// <param name="_ILD">IInter lens distance in millimeters. Should match the IPD but can be tweaked to increase or decrease the stereo effect.</param>
	/// <param name="_FieldOfView">Cameras field of view.</param>
	/// <param name="_ChromaticCorrection">Chromatic Correction factor.</param>
	/// <param name="_k1">Barrel distortion first order factor.</param>
	/// <param name="_k2">Barrel distortion second order factor.</param>
	/// <param name="_Width">Viewport width in millimeters if fixed in size, ignored otherwise.</param>
	/// <param name="_Height">Viewport height in millimeters if fixed in size, ignored otherwise.</param>
	public ALPSConfig(Device _DeviceName, bool _EnableBarrelDistortion, bool _EnableChromaticCorrection, bool _FixedSize, float _IPD, float _ILD, float _FieldOfView, float _ChromaticCorrection, float _k1, float _k2, int _Width, int _Height){

		deviceName = _DeviceName;
		IPD = _IPD;
		ILD = _ILD;
		fieldOfView = _FieldOfView;
		CC = _ChromaticCorrection;
		K1 = _k1;
		K2 = _k2;
		width = _Width;
		height = _Height;
		barrelDistortionActive = _EnableBarrelDistortion;
		chromaticCorrectionActive = _EnableChromaticCorrection;
		stereoDistance = _IPD;
	}
}
