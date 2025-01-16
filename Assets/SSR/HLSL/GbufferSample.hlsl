#ifndef Gb
#define Gb

//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/BRDF.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Random.hlsl"
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonLighting.hlsl"
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/ImageBasedLighting.hlsl"

TEXTURE2D_X_HALF(_GBuffer0); // color.rgb + materialFlags.a
TEXTURE2D_X_HALF(_GBuffer1); // specular.rgb + oclusion.a
TEXTURE2D_X_HALF(_GBuffer2); // normalWS.rgb + smoothness.a

#if defined(_BACKFACE_ENABLED)
TEXTURE2D_X(_CameraBackDepthTexture);
#endif

SAMPLER(sampler_BlitTexture);
SAMPLER(my_point_clamp_sampler);
float4 _BlitTexture_TexelSize;

#ifndef kMaterialFlagSpecularSetup
#define kMaterialFlagSpecularSetup 8 // Lit material use specular setup instead of metallic setup
#endif

#ifndef kDieletricSpec
#define kDieletricSpec half4(0.04, 0.04, 0.04, 1.0 - 0.04) // standard dielectric reflectivity coef at incident angle (= 4%)
#endif

uint UnpackMaterialFlags(float packedMaterialFlags)
{
    return uint((packedMaterialFlags * 255.0h) + 0.5h);
}

#if defined(_GBUFFER_NORMALS_OCT)
half3 UnpackNormal(half3 pn)
{
    half2 remappedOctNormalWS = half2(Unpack888ToFloat2(pn));           // values between [ 0, +1]
    half2 octNormalWS = remappedOctNormalWS.xy * half(2.0) - half(1.0); // values between [-1, +1]
    return half3(UnpackNormalOctQuadEncode(octNormalWS));               // values between [-1, +1]
}
#else
half3 UnpackNormal(half3 pn) { return pn; }                             // values between [-1, +1]
#endif

void GetGbuffer_float(float2 uv, sampler sample_GB, out float3 Diffuse, out float AO, out float3 Specular, out float Smoothness, out float Metallic, out float3 Normal)
{
    half4 gBuffer0 = SAMPLE_TEXTURE2D_X_LOD(_GBuffer0, sample_GB, uv, 0);
    half4 gBuffer1 = SAMPLE_TEXTURE2D_X_LOD(_GBuffer1, sample_GB, uv, 0);
    half4 gBuffer2 = SAMPLE_TEXTURE2D_X_LOD(_GBuffer2, sample_GB, uv, 0);

    Diffuse = gBuffer0.rgb;
    Specular = (UnpackMaterialFlags(gBuffer0.a) == kMaterialFlagSpecularSetup) ? gBuffer1.rgb : lerp(kDieletricSpec.rgb, max(Diffuse.rgb, kDieletricSpec.rgb), gBuffer1.r); // Specular & Metallic setup conversion 
    Metallic = gBuffer1.r;
    AO = gBuffer1.a;
    Normal = UnpackNormal(gBuffer2.rgb);
    Smoothness = gBuffer2.a;
}

#endif