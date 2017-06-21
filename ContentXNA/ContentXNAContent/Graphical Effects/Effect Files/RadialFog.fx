float Length;
float texLength;
float texHeight;
const float radialGradient = 100.0f;
static const float PI = 3.14159265f;
bool isFadingOut;
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

float4 PixelShaderFunction( float2 TexCoords : TEXCOORD0 ) : COLOR0
{
	PixelToFrame col;
	float pixelDistanceX = ((texLength * 0.5) - TexCoords.x * texLength);
	float pixelDistanceY = ((texHeight * 0.5) - TexCoords.y * texHeight);
	float pixelDistanceSqrd = ( pixelDistanceX * pixelDistanceX + pixelDistanceY * pixelDistanceY );
	col.Color.a = 1;

	float lengthSqrd = (Length)*(Length);

	if (pixelDistanceSqrd > lengthSqrd || Length <= 0)
	{
		col.Color.rgb = 0;
	}
	else
	{
		float dropOffRange = (Length- radialGradient) * (Length - radialGradient);

		if ( pixelDistanceSqrd > dropOffRange )
		{	
			//Apply wave function to create gradual dropoff 
			col.Color.rgb = cos ( radians( (pixelDistanceSqrd - dropOffRange) / (lengthSqrd - dropOffRange) * 90));
		}
		else if ( Length < radialGradient )
		{
			col.Color.rgb = cos ( radians (pixelDistanceSqrd / (radialGradient * radialGradient) * 90)) ;
		}
		else
			col.Color.rgb = 1;
	}

	col.Color.a = col.Color.a;
	col.Color.rgb *= tex2D( texSample, TexCoords );

	return float4(col.Color);

}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
