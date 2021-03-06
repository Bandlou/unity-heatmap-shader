# Unity Heatmap Shader
## Contributors
- Louis Bandelier

## Renders a heatmap grid on a quad mesh using a custom shader

![Unity heatmap shader](ReadmeData/unity_heatmap_shader_screenshot.png "Unity heatmap shader")

## The same but with a 2D sprite for the heat types
- This model could be easily used to render a tilemap.
- Note that the tiles are cropped in order to prevent glitchy lines or gaps between them during rendering (see [this video](https://www.youtube.com/watch?v=kbx528-lnoU)).

![Unity heatmap sprite shader](ReadmeData/unity_heatmap_sprite_shader_screenshot.png "Unity heatmap sprite shader")

## References
- [How to pass a structured buffer in to fragment shader](https://forum.unity.com/threads/how-to-pass-a-structured-buffer-in-to-fragment-shader.862216/)
- [Buffer Type - Microsoft Docs](https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-buffer)
- [Code Monkey: Make Awesome Effects with Meshes in Unity | How to make a Mesh](https://www.youtube.com/watch?v=11c9rWRotJ8)
- [Unlit shader transparency](https://forum.unity.com/threads/unlit-with-adjustable-alpha.115455/)
