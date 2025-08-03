MODES
{
    Forward();
}

FEATURES
{
    #include "common/features.hlsl"
}

COMMON
{
	#include "common/shared.hlsl"
}

struct VertexInput
{
    float3 vPositionOs : POSITION;
    float2 vTexCoord : TEXCOORD0;
};

struct PixelInput
{
    float2 vTexCoord : TEXCOORD0;

	#if ( PROGRAM == VFX_PROGRAM_VS )
		float4 vPositionPs : SV_Position;
	#endif

	#if ( ( PROGRAM == VFX_PROGRAM_PS ) )
		float4 vPositionSs : SV_Position;
	#endif
};

VS
{
    PixelInput MainVs( VertexInput i )
    {
        PixelInput o;
        
        o.vPositionPs = float4( i.vPositionOs.xy, 0.0f, 1.0f );
        o.vTexCoord = i.vTexCoord;
        return o;
    }
}

PS
{
    RenderState( DepthWriteEnable, false );
    RenderState( DepthEnable, false );

    SamplerState s < Filter( Linear ); >;
	
    Texture2D g_tColorBuffer < Attribute( "ColorBuffer" ); SrgbRead( true ); >;

	float4 g_RandomHeights < Attribute( "RandomHeights" ); >;

	float4 g_RandomThicknesses < Attribute("RandomThicknesses"); >;

	float4 g_RandomDeltas < Attribute( "RandomDeltas" ); >;

    float4 MainPs( PixelInput i ) : SV_Target0
    {
        float2 UV = i.vTexCoord;

		int GlitchCount = 4;

		for(int index = 0; index < GlitchCount; index++)
		{
			float randomHeight = g_RandomHeights[index];
			float randomThickness = g_RandomThicknesses[index];
			float randomDelta = g_RandomDeltas[index];

			bool isAboveBottomOfGlitch = UV.y > randomHeight - randomThickness;
			bool isBelowTopOfGlitch = UV.y < randomHeight + randomThickness;

			if(isAboveBottomOfGlitch && isBelowTopOfGlitch)
			{
				UV.x += randomDelta;
				break;
			}
		}

		float4 C = g_tColorBuffer.Sample( s, UV );

		return C;
    }
}