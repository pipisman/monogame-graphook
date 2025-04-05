sampler2D FireTexture;   // Fire intensity buffer
sampler1D Palette;       // Fire color gradient

float2 TexelSize;        // Size of one pixel in texture

float4 FirePixelShader(float2 texCoord : TEXCOORD0) : COLOR
{
    // Read current fire intensity (grayscale value)
    float intensity = tex2D(FireTexture, texCoord).r;

    // Fire spreads upwards, average with pixels below
    float below1 = tex2D(FireTexture, texCoord + float2(0, TexelSize.y)).r;
    float below2 = tex2D(FireTexture, texCoord + float2(-TexelSize.x, TexelSize.y)).r;
    float below3 = tex2D(FireTexture, texCoord + float2(TexelSize.x, TexelSize.y)).r;

    // Compute new intensity based on neighboring pixels
    intensity = (below1 + below2 + below3) / 3.0;

    // Reduce intensity slightly (simulates fire cooling)
    intensity *= 0.98;

    // Fetch fire color from palette
    float4 fireColor = tex1D(Palette, intensity);

    return fireColor;
}

technique FireEffect
{
    pass P0
    {
        PixelShader = compile ps_2_0 FirePixelShader();
    }
}
