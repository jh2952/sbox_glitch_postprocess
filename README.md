# Glitch Post Effect – s&box Sandbox Test

This project is a simple exploration of the s&box engine by Facepunch, focusing on implementing a custom glitch post-processing effect. The goal was to get hands-on experience with the engine's rendering pipeline and post-processing system, rather than to create a production-ready effect.

## Example:

https://github.com/user-attachments/assets/2cd4d660-0083-4596-aaa6-2a00d9474289

## Features

- **Custom HLSL Shader:**  
    Implemented in `custom_post_process.shader` to create the glitch visual.

- **GlitchPostProcess Component:**  
    A new C# component inheriting from the engine's post-process base, integrating seamlessly with the existing architecture.

- **Chromatic Aberration Integration:**  
    The glitch effect is visually enhanced by tying it into the chromatic aberration component that already exists in the engine.

## Usage

1. **Attach the Effect:**  
     Add the `GlitchPostEffect` (and optionally the `ChromaticAbberation`) component(s) to your camera.

2. **Trigger the Glitch:**  
     Press `KP_0` (Numpad 0) to activate the effect.
