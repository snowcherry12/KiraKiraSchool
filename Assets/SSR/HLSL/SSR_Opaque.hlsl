#ifndef POST_SSR
#define POST_SSR

float random01(float2 uv)
{
	float noise = frac(sin(dot(uv, float2(12.9898f, 78.233f))) * 43758.5453f);
	return noise;
}

float2 randomVector2D(float2 pos)
{
	float x = random01(pos.xx);
	float y = random01(pos.yy);

	return (float2(x, y) * 2) - 1;
}

float3 randomVector(float3 pos)
{
	float x = random01(pos.xx);
	float y = random01(pos.yy);
	float z = random01(pos.zz);

	return (float3(x, y, z) * 2) - 1;
}

float SceneDepth(float2 UV)
{
	return LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV), _ZBufferParams);
}

TEXTURE2D_X(_BlitTexture);
float3 SceneColor(float2 uv)
{
	uint2 pixelCoords = uint2(uv * _ScreenSize.xy);
	return  LOAD_TEXTURE2D_X_LOD(_BlitTexture, pixelCoords, 0).xyz;
}

//float3 SceneColor(float2 UV)
//{
//	return SHADERGRAPH_SAMPLE_SCENE_COLOR(UV);
//}

float3 ReconstructPosFromDepth(float depth, float2 screenPos)
{
	float3 viewVector = mul(unity_CameraInvProjection, float4(screenPos.xy * 2 - 1, 0, -1));
	float3 viewDirection = mul(unity_CameraToWorld, float4(viewVector, 0));
	float3 cameraDirection = (-1 * mul((float3x3)UNITY_MATRIX_M, transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V))[2].xyz));

	float ViewDotCam = dot(viewDirection, cameraDirection);
	float3 ViewDivCam = viewDirection / ViewDotCam;
	float3 ViewMulDepth = ViewDivCam * depth;
	float3 pos = ViewMulDepth + _WorldSpaceCameraPos;
	return pos;
}

float ReconstructDepthFromPos(float3 pos, float2 screenPos)
{
	float3 viewVector = mul(unity_CameraInvProjection, float4(screenPos.xy * 2 - 1, 0, -1));
	float3 viewDirection = mul(unity_CameraToWorld, float4(viewVector, 0));
	float3 cameraDirection = (-1 * mul((float3x3)UNITY_MATRIX_M, transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V))[2].xyz));

	float ViewDotCam = dot(viewDirection, cameraDirection);
	float3 ViewDivCam = viewDirection / ViewDotCam;

	float3 ViewMulDepth = pos - _WorldSpaceCameraPos;

	float depth = ViewMulDepth.z / ViewDivCam.z;

	return depth;
}

float2 WorldToScreenPos(float3 pos)
{
	pos = normalize(pos - _WorldSpaceCameraPos) * (100000000) + _WorldSpaceCameraPos;
	float3 toCam = mul(unity_WorldToCamera, pos);
	float camPosZ = toCam.z;
	float height = 2 * camPosZ / unity_CameraProjection._m11;
	float width = _ScreenParams.x / _ScreenParams.y * height;
	float2 uvCoords;
	uvCoords.x = (toCam.x + width / 2) / width;
	uvCoords.y = (toCam.y + height / 2) / height;
	return uvCoords;
}

float3 ScreenToWorldPos(float2 screenPos)
{
	float depth = SceneDepth(screenPos);
	return ReconstructPosFromDepth(depth, screenPos);
}

float3 hash33(float3 p3)
{
	p3 = frac(p3 * float3(0.1031, 1030, 0.0973));
	p3 += dot(p3, p3.xyz + 33.33);
	return frac((p3.xxy + p3.yxx) * p3.zyx);
}

void SSR_float(float3 viewDir, float3 cameraDir, float3 _normal, float2 screenPosition, float _StepSize, int _MaxSteps, int _BinarySearchSteps, float _Thickness, float _Smoothness, float _MinSmoothness, out float3 col, out float3 blitSource)
{
	//float3 blendCol = float3(0.1, 0.5, 1.2);
	float3 blendCol = 0;

	blitSource = SceneColor(screenPosition);

	if (_Smoothness < _MinSmoothness)
	{
		col = blendCol;
		return;
	}

	float3 _position = ScreenToWorldPos(screenPosition);

	float d = distance(_position, _WorldSpaceCameraPos);
	if (d > 70)
	{
		_StepSize = 1;
		_Thickness = 1;
	}
	if (d > 1000)
	{
		_StepSize = 10;
		_Thickness = 10;
	}

	float3 camRay = normalize(_position - _WorldSpaceCameraPos);
	float3 rayDir = normalize(reflect(camRay, normalize(_normal)));
	//rayDir = normalize(_normal);

	//float3 jitter = randomVector(rayDir) * ((1 - pow(_Smoothness, 0.1)) + 0.0001);
	//rayDir += jitter;

	//rayDir += (hash33(_position.xyz * 10) - float3(0.5, 0.5, 0.5) * (1 - _Smoothness));

	float distTravelled = 0;
	float prevDistance = 0;

	float3 color = 0;
	float2 uv = 0;
	float depth = _Thickness;

	UNITY_LOOP
	for (int j = 0; j < _MaxSteps; j++)
	{
		if (j == _MaxSteps - 1)
		{
			j = 10000000;
		}

		prevDistance = distTravelled;

		distTravelled += _StepSize * (j + 1);

		float3 rayPos = _position + (rayDir * distTravelled);
		float3 projectedPos = ScreenToWorldPos(WorldToScreenPos(rayPos));

		//traceDepth = min((rayPos - _position).y / 100, 1);

		float projectedPosDist = distance(projectedPos, _WorldSpaceCameraPos);
		float rayPosDist = distance(rayPos, _WorldSpaceCameraPos);

		depth = rayPosDist - projectedPosDist;
		if (depth > 0 && depth < _Thickness * (j + 1) && j > 0)
		{
			UNITY_LOOP
			for (int k = 0; k < _BinarySearchSteps; k++)
			{
				float midPointDist = (distTravelled + prevDistance) * 0.5;
				rayPos = _position + rayDir * midPointDist;
				//projectedPos = ScreenToWorldPos(WorldToScreenPos(rayPos));
				if (distance(projectedPos, _WorldSpaceCameraPos) <= distance(rayPos, _WorldSpaceCameraPos))
				{
					distTravelled = midPointDist;
					uv = WorldToScreenPos(rayPos);
				}
				else 
				{
					prevDistance = midPointDist;
				}
			}
			break;
		}
	}

	col = min(SceneColor(uv), 250 * _Smoothness);

	float3 c1 = SceneColor(uv);
	float3 c2 = SceneColor(screenPosition);
	if (c1.x == c2.x && c1.y == c2.y && c1.z == c2.z)
	{
		col = blendCol;
	}

	if (uv.y < 0.1)
	{
		float blend = (clamp(uv.y, 0, 1)) * 10;
		col = lerp(col, blendCol, 1 - blend);
	}

	if (uv.y > 0.9)
	{
		float blend = (1 - clamp(uv.y, 0, 1)) * 10;
		col = lerp(col, blendCol, 1 - blend);
	}

	if (uv.x < 0.1)
	{
		float blend = (clamp(uv.x, 0, 1)) * 10;
		col = lerp(col, blendCol, 1 - blend);
	}

	if (uv.x > 0.9)
	{
		float blend = (1 - clamp(uv.x, 0, 1)) * 10;
		col = lerp(col, blendCol, 1 - blend);
	}

	float view = dot(rayDir, cameraDir);
	if (view < -0.1)
	{
		float blend = (-view - 0.5) * 2;
		blend = smoothstep(0.8, 1, 1 - blend);

		col = lerp(blendCol, col, blend);
		col = blendCol;
	}
}

#endif