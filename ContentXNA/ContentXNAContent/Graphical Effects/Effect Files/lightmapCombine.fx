Texture2D colourTex;
Texture1D sunTex;
float2 viewport;
float time;

sampler lightSampler  : register(s0);
sampler colorSampler =
sampler_state
{
	Texture = <colourTex>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

sampler sunSampler =
sampler_state
{
	Texture = <sunTex>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

void SpriteVertexShader(inout float4 color : COLOR0,
                            inout float2 texCoord : TEXCOORD0,
                            inout float4 position : SV_Position)
{
	// Half pixel offset for correct texel centering. 
    position.xy -= 0.5; 
 
    // Viewport adjustment. 
    position.xy = position.xy / viewport; 
    position.xy *= float2(2, -2); 
    position.xy -= float2(1, -1); 
}

float4 PixelShaderFunction(float4 col : COLOR0, float2 screenSpace : SV_Position, float2 texCoord : TEXCOORD0) : COLOR0
{
	col.xyz = 0.2;//tex2D(sunSampler, time);
	col.a = 0.5;
	float4 gameCol = tex2D(colorSampler, texCoord);
	float4 lightCol = (tex2D(lightSampler, texCoord));
	float4 finalVal;
	finalVal.xyz = gameCol * (col.xyz * col.a + lightCol.xyz * lightCol.a);
	finalVal.a = 1;
	return finalVal;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 SpriteVertexShader();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
