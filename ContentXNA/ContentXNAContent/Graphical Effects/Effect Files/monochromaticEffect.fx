float magnitudeBase;	// Any scalar quantity
float magnitudeMultiplier; // [0...1], used when blur effect is beginning / ending
float rand;

Texture2D renderedTexture;

#define max(a,b) a > b ? a : b;
#define min(a,b) a < b ? a : b;

sampler texSample : register (s0) = 
sampler_state
{
	Texture = <renderedTexture>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

struct PixelToFrame
{
    float4 Color        : COLOR0;
};

float4 getMonochromatic( float2 TexCoords : TEXCOORD0 ) : COLOR0
{
	float4 input = tex2D ( texSample, TexCoords );
	input.a = 1;
	float colAvg = (input.r + input.g + input.b) / 3;
	input.rgb = pow( colAvg, magnitudeBase );
	input.rgb += noise(rand) * 1 / magnitudeBase;

	return input;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 getMonochromatic();
    }
}
