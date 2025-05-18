float saturation; // The saturation parameter

texture Texture0;
sampler2D TextureSampler = sampler_state
{
    texture = <Texture0>;
};

float4 PS_Saturation(float2 texCoord : TEXCOORD) : COLOR
{
    // Sample the color from the texture
    float4 color = tex2D(TextureSampler, texCoord);
    
    // Convert the color to grayscale using the luminance formula
    float gray = dot(color.rgb, float3(0.3, 0.59, 0.11));
    
    // Interpolate between grayscale and the original color based on saturation
    color.rgb = lerp(float3(gray, gray, gray), color.rgb, saturation);
    
    return color;
}

technique Saturation
{
    pass P0
    {
        PixelShader = compile ps_2_0 PS_Saturation();
    }
}
