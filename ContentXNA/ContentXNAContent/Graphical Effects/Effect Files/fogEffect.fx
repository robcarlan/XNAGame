Texture2D perlinTex;

float time;
float fogSpeed;
float fogSharpness;
float fogTransparency;

sampler fogSample :register (s1) = 

sampler_state
{
    Texture = <perlinTex>;
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
	float4 Output = (float4)0;   

	float4 perlin;
	perlin =  tex2D(fogSample, (TexCoords) + time)/2;
    perlin += tex2D(fogSample, (TexCoords)*2 + time)/4;
    perlin += tex2D(fogSample, (TexCoords)*4 + time)/8;
    perlin += tex2D(fogSample, (TexCoords)*8 + time)/16;
    perlin += tex2D(fogSample, (TexCoords)*16 + time)/32;
    perlin += tex2D(fogSample, (TexCoords)*32 + time)/32;   

	Output = perlin;
	Output.a = 1 * fogTransparency;
	return Output;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
