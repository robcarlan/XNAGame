Texture2D lightTex;
Texture2D depthTex;
float2 viewport;
float4 ambientColor;

sampler  TextureSampler  : register(s0);
sampler depthSampler =
sampler_state
{
	Texture = <depthTex>;
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

float4 PSPointLight(float4 col : COLOR0, float2 screenSpace : SV_Position, float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 outCol = tex2D(TextureSampler, texCoord);

	if (tex2D(depthSampler, texCoord).x <screenSpace.y)
	{
		outCol.a = outCol.x;
		outCol.xyz = outCol.x;
		outCol *= col;
	};
	return outCol;
}

float4 PSAmbientLight(float4 col : COLOR0, float2 screenSpace : SV_Position, float2 texCoord : TEXCOORD0) : COLOR0
{
	return ambientColor;
}

technique PointLightPass
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 SpriteVertexShader();
        PixelShader = compile ps_3_0 PSPointLight();
    }
}

technique AmbientPass
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 SpriteVertexShader();
		PixelShader = compile ps_3_0 PSAmbientLight();
	}
}
