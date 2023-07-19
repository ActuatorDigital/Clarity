# Clarity

<img src="https://img.shields.io/badge/unity-2021.3-green.svg?style=flat-square" alt="unity 2021.3">

A collection of Unity Editor Tool(s) that assist in understand usage and performance.

## What is it and why should I use it?

These are being used to help identify and optimise use of shaders and texture size in performance constrained environments.

## Dependencies

- None.

## Installation / Minimum required Setup

Distributed as a git package ([How to install package from git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html))

### Git URL

```
https://github.com/ActuatorDigital/Clarity.git?path=Packages/Clarity
```

## Editor

Clarity provides the following editor window(s).

### Scene Shader Usage

Editor window found in `Window->Clarity/Group By Shaders`. Finds all renderers, and shows them grouped by the shader they use.

!["Scene renderers grouped by the shader their material is using."](img/groupbyshaderwindow.png?raw=true "Scene renderers grouped by the shader their material is using.")

The top text field will do a partial match on the shader name and collapse matches.

Example workflow:

1. Open a scene.
2. Open `Group By Shaders` window.
3. Scroll through to find shader(s) that are known expensive, e.g. 'Standard'
4. Click on Renderers underneath the shader.
5. View the material they are using.
6. If it could use a simpler shader, change it.
7. Return to `Group By Shaders` window, it will update it's content.
8. Goto 3.

## Runtime

Clarity provides the following runtime components.

### Mipmap Level Shader

The scene view can be set to render mode `miscellaneous->Mipmaps`. This tints texture magnification blue, and tints texture sample below mip level 1 red. This is useful in tweaking texture size to match texel size. Unfortunately it is editor only and scene view only.

!["Left: Mip levels in scene shading mode, and Right: Debug_Mipmap shader in use."](img/debugmiplevelshader.png?raw=true "Comparison between scene mip mode and runtime shader effect.")

The `Clarity/Debug_MipMap` produces a similar effect in a shader. This allows for checking mipmap usage in the game view, during play and also in builds where device specific settings might otherwise be hard to replicate.
