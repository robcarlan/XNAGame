Texture2D colorTex;					//Texture used to draw sprite
float2 depth;						//Changed by each new sprite
float gameHeight = 600;

float specularityPower = 0.8f;		//Unused
float specularityIntensity = 0.5f;	//Unused


texture depthTex : register (t2);

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
	half4 color : COLOR0;	
	half4 normal : COLOR1;
	half4 depth : COLOR2;
};

void SpriteVertexShader(inout float4 color    : COLOR0,
                            inout float2 texCoord : TEXCOORD0,
                            inout float4 position : POSITION0)
{
}

pixelShaderOut PixelShaderFunction(
							float4 color    : COLOR0,
                            float2 xy : TEXCOORD0,
                            float4 position : POSITION0)
{
    pixelShaderOut output;
	float4 sample = tex2D(colorSampler, xy);

	output.color = sample;

	output.normal.rgb = 1 - output.color; //Unused
	output.normal.a = output.color.a;	  //Unused
	output.depth = depth.y / gameHeight;

	if ( sample.a == 1 )
	{	
		//Pixel is opaque, test depth
		float prevDepth = 0.0f;//tex2D(depthSampler, xy);	//old z ---Wrong

		if (prevDepth.r > output.depth.r) 
		{
			//Use previous values
			//output.depth = prevDepth;
			discard;
		}
	}
	else	//Pixel is transparent, so use previous values
		discard;

    return output;
}

technique Technique1
{
    pass Pass1
    {
		//VertexShader = compile vs_2_0 SpriteVertexShader();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
