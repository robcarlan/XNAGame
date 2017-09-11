Texture2D renderedTexture;
Texture1D dayTexture;
float time : register (c0);

sampler renderSample : register(s0) = 
sampler_state
{
    Texture = <renderedTexture>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

sampler daySample :register (s1) = 

sampler_state
{
    Texture = <dayTexture>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

struct PixelToFrame
{
    float4 Color        : COLOR0;
};

float4 PixelShaderFunction( float2 TexCoords : TEXCOORD0 ) : COLOR0
{
	PixelToFrame output = (PixelToFrame)0; 
	output.Color.rgb = tex2D( renderSample, TexCoords ) * tex2D( daySample, time);
	output.Color.a = 1;
	return float4(output.Color);
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
