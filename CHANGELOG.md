# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- GroupByShaderEditorWindow, find and show `Renderers` grouped by the shader they are using.
- Clarity/Debug_MipMap shader, show a similar effect to the Scene's `Miscellaneous/Mipmaps`, tinting textures blue for magnification, and red for lower than mipmap level 1.
- GroupByShadowsEditorWindow, find and show all 'Renderers' grouped by the shadow settings they are using.
- Find All Lights, Edit/Clarity menu item that inserts the t:light search for you.
- ProfileRecorderAlerts, Editor only debug warnings/errors if certain profile recorder value thresholds are exceeded, such as DrawCalls per frame.
- GroupByTexturesEditorWindow, find and show all 'Textures' and 'Materials' in use in the scene, grouped by the Texture format and size.
- LocationsEditorWindow, configure and open commonly used paths. These are saved per project so they can be shared among the team easily.
- Add extra info option, to log out all tracked performance recorders and light count, when a threshold is exceeded.
