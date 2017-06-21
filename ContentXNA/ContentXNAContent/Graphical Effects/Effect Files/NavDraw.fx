uniform float4x4 xWorld;
uniform float4x4 xViewProjection;
float4 meshColour = float4(0.5,0.5,0.5,0.25);
float4 lineColour = float4(1,1,1,1);

void VS_Fill(in float4 inPos : POSITION, out float4  outPos: POSITION, inout float4 outColor:COLOR0 )
{
    float4 tmp = mul (inPos, xWorld);
    outPos = mul (tmp, xViewProjection);
    outColor = outColor; 
}

void VS_Outline(in float4 inPos : POSITION, out float4  outPos: POSITION, out float4 outColor:COLOR0 )
{
    float4 tmp = mul (inPos, xWorld);
    outPos = mul (tmp, xViewProjection);
    outColor = lineColour; 
}

float4 PS(in float4 outPos : POSITION, in float4 outColor : COLOR0) : COLOR0
{
	float4 val = outColor;
	return val;
}

technique Lines
{
    pass fillPass
    {   
        VertexShader = compile vs_2_0 VS_Fill();
		PixelShader = compile ps_2_0 PS();
        FILLMODE = SOLID;
        CULLMODE = NONE;        
    }  

	pass outlinePass
	{
		VertexShader = compile vs_2_0 VS_Outline();
		PixelShader = compile ps_2_0 PS();
		FILLMODE = WIREFRAME;
        CULLMODE = NONE;   
	}
}