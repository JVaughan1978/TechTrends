/************************************************************************
	ALPSDevice provides a specific configuration for each supported device

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

	//====================================================================================================
	// Attributes
	//====================================================================================================

	public enum Device{
		DEFAULT,
		ALTERGAZE,
		CARDBOARD,
		FREEFLY_VR,
		//VISR,
		VR_ONE
	};

	public enum ScreenOption{
		FixedSize, 
		FullScreen
	};

public static class ALPSDevice {

	//====================================================================================================
	// Functions
	//====================================================================================================

	/// <summary>
	/// Returns device configuration corresponding to a device name.
	/// </summary>
	/// <param name="_device">Device name.</param>
	public static ALPSConfig GetConfig(Device _device){
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
		ALPSConfig config;
		switch (_device) {
			case Device.ALTERGAZE:
			config = new ALPSConfig(Device.ALTERGAZE,true,true,false,62f,62f,85f,-1f,0.4f,0.2f,0,0);
				break;
			case Device.CARDBOARD:
			config = new ALPSConfig(Device.CARDBOARD,true,true,false,62f,56f,85f,0.4f,1f,0.2f,128,75);
				break;
			case Device.FREEFLY_VR:
			config = new ALPSConfig(Device.FREEFLY_VR,true,true,false,62f,62f,85f,-2f,0.7f,0.2f,140,75);
				break;
			case Device.VR_ONE:
			config = new ALPSConfig(Device.VR_ONE,true,true,false,62f,62f,85f,-2f,1.0f,10.0f,125,65);
				break;
			case Device.DEFAULT:
			default: 
				config = new ALPSConfig(Device.DEFAULT,false,false,false,62f,62f,85f,0f,0f,0f,0,0);
				break;
		}
		return config;
	}

	/// <summary>
	/// Returns the device name without underscore.
	/// </summary>
	/// <param name="_device">Device name.</param>
	public static string ToStringReadable(this Device _device){
		return _device.ToString ().Replace ('_',' ');
	}
}
