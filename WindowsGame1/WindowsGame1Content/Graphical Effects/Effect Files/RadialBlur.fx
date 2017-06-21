float hardLengthSqrd;
float softLengthSqrd;
float texLength;
float texHeight;
float fPerPixX;
float fPerPixY;
static const float PI = 3.14159265f;

float magnitudeBase;	// Any scalar quantity
float magnitudeMultiplier; // [0...1], used when blur effect is beginning / ending

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

float getBlurAmount( float2 TexCoords )
{
	float quantity;
	float pixelDistanceX = ((texLength * 0.5) - TexCoords.x * texLength);
	float pixelDistanceY = ((texHeight * 0.5) - TexCoords.y * texHeight);
	float pixelDistanceSqrd = ( pixelDistanceX * pixelDistanceX + pixelDistanceY * pixelDistanceY );

	if (pixelDistanceSqrd > hardLengthSqrd)
	{
		quantity = 1;
	}
	else
	{

		if ( pixelDistanceSqrd > softLengthSqrd )
		{	
			//Apply wave function to create gradual dropoff 
			quantity = 1 - cos ( radians( (pixelDistanceSqrd - softLengthSqrd) / (hardLengthSqrd - softLengthSqrd) * 90));
		}
		//else if ( Length < radialGradient )
		//{
		//	quantity = 1 - cos ( radians (pixelDistanceSqrd / (radialGradient * radialGradient) * 90)) ;
		//}
		else
			quantity = 0;
	}

	return quantity;
}

float4 blurHorizontal( float2 TexCoords : TEXCOORD0 ) : COLOR0
{
	float blurMagnitude = getBlurAmount(TexCoords) * magnitudeBase; 

	float4 sum;
	sum = tex2D( texSample, TexCoords ) * 0.16;
	sum += tex2D( texSample, float2( TexCoords.x + blurMagnitude  * 1 * magnitudeMultiplier, TexCoords.y ) ) * 0.15; 
	sum += tex2D( texSample, float2( TexCoords.x + blurMagnitude  * 2 * magnitudeMultiplier, TexCoords.y ) ) * 0.12; 
	sum += tex2D( texSample, float2( TexCoords.x + blurMagnitude  * 3 * magnitudeMultiplier, TexCoords.y ) ) * 0.09; 
	sum += tex2D( texSample, float2( TexCoords.x + blurMagnitude  * 4 * magnitudeMultiplier, TexCoords.y ) ) * 0.05; 
	sum += tex2D( texSample, float2( TexCoords.x - blurMagnitude  * 1 * magnitudeMultiplier, TexCoords.y ) ) * 0.15; 
	sum += tex2D( texSample, float2( TexCoords.x - blurMagnitude  * 2 * magnitudeMultiplier, TexCoords.y ) ) * 0.12; 
	sum += tex2D( texSample, float2( TexCoords.x - blurMagnitude  * 3 * magnitudeMultiplier, TexCoords.y ) ) * 0.09; 
	sum += tex2D( texSample, float2( TexCoords.x - blurMagnitude  * 4 * magnitudeMultiplier, TexCoords.y ) ) * 0.05; 

	sum.a = 1.0;

	return sum;
}

float4 blurVertical( float2 TexCoords : TEXCOORD0 ) : COLOR0
{
	float blurMagnitude = getBlurAmount(TexCoords) * magnitudeBase; 

	float4 sum = 0;

	sum += tex2D( texSample, TexCoords ) * 0.16;
	sum += tex2D( texSample, float2( TexCoords.x, TexCoords.y + blurMagnitude  * 1 * magnitudeMultiplier ) ) * 0.15 ; 
	sum += tex2D( texSample, float2( TexCoords.x, TexCoords.y + blurMagnitude  * 2 * magnitudeMultiplier ) ) * 0.12 ; 
	sum += tex2D( texSample, float2( TexCoords.x, TexCoords.y + blurMagnitude  * 3 * magnitudeMultiplier ) ) * 0.09 ; 
	sum += tex2D( texSample, float2( TexCoords.x, TexCoords.y + blurMagnitude  * 4 * magnitudeMultiplier ) ) * 0.05 ; 
	sum += tex2D( texSample, float2( TexCoords.x, TexCoords.y - blurMagnitude  * 1 * magnitudeMultiplier ) ) * 0.15 ; 
	sum += tex2D( texSample, float2( TexCoords.x, TexCoords.y - blurMagnitude  * 2 * magnitudeMultiplier ) ) * 0.12 ; 
	sum += tex2D( texSample, float2( TexCoords.x, TexCoords.y - blurMagnitude  * 3 * magnitudeMultiplier ) ) * 0.09 ; 
	sum += tex2D( texSample, float2( TexCoords.x, TexCoords.y - blurMagnitude  * 4 * magnitudeMultiplier ) ) * 0.05 ;

	sum.a = 1.0;

	return sum;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 blurHorizontal();
    }
	pass Pass2
	{
		PixelShader = compile ps_2_0 blurVertical();
	} 
}
