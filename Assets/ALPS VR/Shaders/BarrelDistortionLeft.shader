Shader "Custom/BarrelDistortionLeft" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "" {}
	}
	
	// Shader code pasted into all further CGPROGRAM blocks
	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	struct v2f {
		half4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
	};
	
	sampler2D _MainTex;
	uniform fixed2 _CenterILD;
	uniform fixed2 _CenterIPD;
	uniform fixed2 _CenterScreen;
	uniform fixed2 _MinusCenterIPDMask;
	uniform fixed2 _OneMinusCenterIPDMask;
	uniform fixed2 _k;
	uniform fixed2 _radiusCoef;
	
	v2f vert( appdata_img v ) 
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	} 
	
	fixed4 frag(v2f i) : SV_Target 
	{
		//origin is in the bottom-left corner. Subtract _CenterILD allows to calculate 
		//the new coordinates relatively to center of the distortion.
		//fixed2 diff = i.uv+fixed2(0.25f,0.0f) - _CenterILD;
        //fixed2 diff = i.uv + _CenterScreen - _CenterILD;
        fixed2 diff = (i.uv + _CenterILD - _CenterIPD);
        fixed2 diffNormalized = diff * _radiusCoef;
        //dot product of the shifted coordinates to get the squared radius.
      	fixed radius = dot(diffNormalized,diffNormalized);
      	
      	//Radius is multiplied by the ratio between the half-screen width and the half-headset viewport width
      	//so that the distortion adapts to the size of screen but is the same in the real world.
      	//radius*=radiusCoef;
      	
      	//Distorted coordinates are calculted with k1 and k2
		half2 radialOffset = (diff) * (1 + radius*(_k.x + _k.y*radius));
		
		//Dependent texture lookup read to get the new pixel. We add  _CenterILD to reset
		//the origin of the coordinates system.
		fixed4 color = tex2D(_MainTex ,radialOffset+_CenterIPD);
		
		//Because clamp mode is activated, texture lookup reads may be outside the bounds of the texture and return
		//the pixel on the border. We set this pixel to black.
		fixed b = step(radialOffset.x,_OneMinusCenterIPDMask.x) * step(_MinusCenterIPDMask.x,radialOffset.x) * step(radialOffset.y,_OneMinusCenterIPDMask.y) * step(_MinusCenterIPDMask.y,radialOffset.y);
		color*=b ;
		
		//Return the pixel.
		return color;
	}
	ENDCG 
	
Subshader {
 Pass {
	  ZTest Always Cull Off ZWrite Off
	  Fog { Mode off }  
	  
      CGPROGRAM
      #pragma fragmentoption ARB_precision_hint_fastest 
      #pragma vertex vert
      #pragma fragment frag
      ENDCG
  }
  
}

Fallback off
	
} // shader