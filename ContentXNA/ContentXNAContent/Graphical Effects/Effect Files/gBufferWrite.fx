Texture2D colorTex;					//Texture used to draw sprite
float gameHeight = 600;

texture depthTex : register (t2);
sampler  TextureSampler  : register(s0);

//VS values
float2 viewport;

sampler depthSampler = sampler_state
{
	Texture = depthTex;
};

sampler colorSampler = sampler_state
{
	Texture = colorTex;
};

struct pixelShaderOut
{
	float4 color : COLOR0;
	float4 depth : COLOR1;	
};

struct vertexShaderOut
{
	float2 texCoord : TEXCOORD0;
	float4 position : SV_Position;
	float4 color : COLOR0;
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

	vertexShaderOut output;
	output.texCoord = texCoord;
	output.color = color;
	output.position = position;
}


pixelShaderOut PixelShaderFunction(float4 color : COLOR0, float2 texCoord : TEXCOORD0)
{
	pixelShaderOut output;
	float4 sample = tex2D(TextureSampler, texCoord);
	
	//Get colour from sample
	output.color = sample;

	//Calculate depth value
	output.depth.xyz = color.x;

	if (sample.a == 1)
	    output.depth.a = 1;
	else
		discard;

	return output;
}

technique SpriteBatch
{
	pass
	{
		//If vs provided, spritebatch is overrided!
		VertexShader = compile vs_3_0 SpriteVertexShader();
		PixelShader  = compile ps_3_0 PixelShaderFunction();
	}
}


