#ifndef blur
#define blur

void Blur_float(Texture2D tex, sampler samplertex, float strength, float radius, float iterations, float2 uv, float2 screenSize, out float4 color)
{
    // Sample the surrounding pixels using a 5x5 kernel
    color = 0.0;
    float weightSum = 0.0;

    float widthCorrection = screenSize.x / screenSize.y;

    UNITY_LOOP
    //[unroll]
    for (int y = -iterations; y <= iterations; y++)
    {
        UNITY_LOOP
        for (int x = -iterations; x <= iterations; x++)
        {
            // Calculate the weight for the current pixel using the Gaussian function
            float weight = exp(-((x * x) + (y * y)) / (2 * (strength * strength))) / (2 * 3.141592 * (strength * strength));

            // Sample the pixel at the current offset
            //float4 texSample = tex2D(tex, uv + float2(x, y));
            float4 texSample = SAMPLE_TEXTURE2D(tex, samplertex, uv + (float2(x, y) * float2(radius / widthCorrection, radius)));

            // Accumulate the weighted pixel value
            color += texSample * weight;
            weightSum += weight;
        }
    }

    // Normalize the color by the sum of the weights
    color /= weightSum;

    color = clamp(color, 0, 50000);

    return;
}

//void Blur_float(Texture2D tex, sampler samplertex, float strength, float radius, float iterations, float2 uv, float2 screenSize, out float4 color)
//{
//    // Sample the surrounding pixels using a 5x5 kernel
//    color = 0.0;
//    float weightSum = 0.0;
//
//    float widthCorrection = screenSize.x / screenSize.y;
//
//    for (int y = -iterations; y <= iterations; y++)
//    {
//        for (int x = -iterations; x <= iterations; x++)
//        {
//            // Sample the pixel at the current offset
//            float4 s = SAMPLE_TEXTURE2D(tex, samplertex, uv + (float2(x, y) * float2(radius / widthCorrection, radius)));
//
//            // Calculate the intensity difference between the center pixel and the current pixel
//            float intensityDiff = abs(SAMPLE_TEXTURE2D(tex, samplertex, uv).r - s.r);
//
//            // Calculate the spatial distance between the center pixel and the current pixel
//            float spatialDiff = sqrt((x * x) + (y * y));
//
//            // Calculate the weight for the current pixel using the bilateral filter function
//            float weight = exp(-intensityDiff / 2) * exp(-spatialDiff / 2);
//
//            // Accumulate the weighted pixel value
//            color += s * weight;
//            weightSum += weight;
//        }
//    }
//
//    // Normalize the color by the sum of the weights
//    color /= weightSum;
//
//    return;
//}
#endif