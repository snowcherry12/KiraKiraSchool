#ifndef _SSR
#define _SSR

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

float3 SceneColor(float2 UV)
{
	return SHADERGRAPH_SAMPLE_SCENE_COLOR(UV);
}

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

float3 BlurSceneColor(float strength, float radius, float iterations, float2 uv, float2 screenSize)
{
	// Sample the surrounding pixels using a 5x5 kernel
	float3 color = 0.0;
	float weightSum = 0.0;

	float widthCorrection = 16 / 9;

	UNITY_LOOP
	for (int y = -iterations; y <= iterations; y++)
	{
		for (int x = -iterations; x <= iterations; x++)
		{
			// Calculate the weight for the current pixel using the Gaussian function
			float weight = exp(-((x * x) + (y * y)) / (2 * (strength * strength))) / (2 * 3.141592 * (strength * strength));

			// Sample the pixel at the current offset
			float3 texSample = SceneColor(uv + (float2(x, y) * float2(radius / widthCorrection, radius) ) ).xyz;

			// Accumulate the weighted pixel value
			color += texSample * weight;
			weightSum += weight;
		}
	}

	// Normalize the color by the sum of the weights
	color /= weightSum;

	return color;
}

void SSR_float(/*Texture2D _GrabPassTex, sampler sampler_GrabPassTex,*/ float3 viewDir, float3 cameraDir, float3 _normal, float3 _position, float _StepSize, int _MaxSteps, int _BinarySearchSteps, float _Thickness, float _Smoothness, float2 screenPosition, out float3 col)
{
	//float3 blendCol = float3(0.1, 0.5, 1.2);
	float3 blendCol = float3(0, 0, 0);

	float3 camRay = normalize(_position - _WorldSpaceCameraPos);
	float3 rayDir = normalize(reflect(camRay, normalize(_normal)));

	//float3 jitter = randomVector(rayDir) * ((1 - _Smoothness) + 0.0001);
	//rayDir += jitter;

	float distTravelled = 0;
	float prevDistance = 0;

	float3 color = 0;
	float2 uv = 0;
	float depth = _Thickness;

	float d = distance(_position, _WorldSpaceCameraPos);
	if (d > 500)
	{
		//_StepSize = 1;
		_Thickness = 10;
	}

	UNITY_LOOP
	for (int j = 0; j < _MaxSteps; j++)
	{
		if (j == _MaxSteps - 1)
		{
			j = 1000000000;
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

	//float3 SSR_Color = SAMPLE_TEXTURE2D(_GrabPassTex, sampler_GrabPassTex, uv).xyz;
	//col = min(SSR_Color, 50);
	col = min(SceneColor(uv), 5000);

	//col = float3(uv, 0);

	//col = frac(ScreenToWorldPos(WorldToScreenPos(_position)) * 0.1);
	//col = min(SceneDepth(WorldToScreenPos(_position)) * 0.000001, 1);
	//col = frac(SceneDepth(WorldToScreenPos(_position)) * 0.1);
	//uv = 0.5;
	//rayDir = cameraDir;


	//uv = WorldToScreenPos(_position);
	//col = min(BlurSceneColor((_Smoothness * 10 * traceDepth) + 0.001, 0.015, 5, uv, 0), 50);

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
	if (view < -0.5)
	{
		float blend = (-view - 0.5) * 2;
		blend = smoothstep(0.8, 1, 1 - blend);

		col = lerp(blendCol, col, blend);
	}
}

#endif