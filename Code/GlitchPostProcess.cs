using Sandbox;
using Sandbox.Diagnostics;
using Sandbox.Rendering;
using System;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using static Sandbox.Citizen.CitizenAnimationHelper;

public sealed class GlitchPostProcess : PostProcess, Component.ExecuteInEditor
{
	// --------------------- EDITOR VALUES -------------------------------------
	[Property][Range( 1.0f, 100.0f )] public float EffectRate { get; set; }

	private ChromaticAberration ChromAbb;

	// --------------------- UPDATE LOOP ---------------------------------------

	protected override void UpdateCommandList()
	{
		ChromAbb = GameObject.GetComponentInChildren<ChromaticAberration>();
		
		if(Input.Pressed("TestGlitch"))
		{
			GlitchTimer = GlitchDuration;
			PopulateAttributeArrays();
		}

		if ( GlitchTimer > 0.0f )
		{
			float EffectStrength = GlitchTimer / GlitchDuration;

			if ( ChromAbb != null )
			{
				ChromAbb.Scale = EffectStrength;
			}

			CommandList.Attributes.Set( "EffectStrength", EffectStrength );

			CommandList.Attributes.Set( "RandomHeights", Heights );

			CommandList.Attributes.Set( "RandomThicknesses", Thicknesses );

			CommandList.Attributes.Set( "RandomDeltas", Deltas * EffectStrength );

			CommandList.Attributes.GrabFrameTexture( "ColorBuffer" );

			CommandList.Blit( Material.FromShader( "shaders/custom_post_process.shader" ) );

			GlitchTimer -= Time.Delta * EffectRate;
			if ( GlitchTimer < 0.0f )
			{
				GlitchTimer = 0.0f;
			}
		}
	}

	// --------------------- PRIVATE DATA -------------------------------------

	private float GlitchTimer = 0.0f;
	private const float GlitchDuration = 1.0f;

	private Vector4 Heights     = new Vector4();
	private Vector4 Thicknesses = new Vector4();
	private Vector4 Deltas      = new Vector4();

	// --------------------- PRIVATE METHODS -------------------------------------

	private void PopulateAttributeArrays()
	{
		Random rand = new Random();

		for (int i = 0; i < 4; i++ )
		{
			Heights[i]     = rand.Float( 0.00f, 1.0f );
			Thicknesses[i] = rand.Float( 0.01f, 0.1f );
			Deltas[i]      = rand.Float( -0.2f, 0.2f );
		}
	}
}
