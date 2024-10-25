#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

void MainLight_half(half3 WorldPos, out half3 Direction, out half3 Color, out half ShadowAtten)
{
    #ifdef SHADERGRAPH_PREVIEW
    Direction = float3(0.5, 0.5, 0);
    Color = 1;
    ShadowAtten = 1;
    #else
    
	half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);

    Light mainLight = GetMainLight(shadowCoord);
    Direction = mainLight.direction;
    Color = mainLight.color;

    #if !defined(_MAIN_LIGHT_SHADOWS) || defined(_RECEIVE_SHADOWS_OFF)
		ShadowAtten = 1.0h;
    #else
	    ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
	    half shadowStrength = GetMainLightShadowStrength();
	    ShadowAtten = SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture,
	    sampler_MainLightShadowmapTexture),
	    shadowSamplingData, shadowStrength, false);
    #endif
#endif
}

#endif
