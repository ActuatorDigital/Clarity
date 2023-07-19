// Made with Amplify Shader Editor v1.9.1.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Clarity/Debug_MipMap"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		[NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform half4 _Color;
		uniform sampler2D _MainTex;
		float4 _MainTex_TexelSize;


		half MyCustomExpression20( half2 texture_coordinate )
		{
			   float2 dx_vtc = ddx(texture_coordinate);
			   float2 dy_vtc = ddy(texture_coordinate);
			   float md = max(dot(dx_vtc, dx_vtc), dot(dy_vtc, dy_vtc));
			return 0.5 * log2(md);
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_MainTex3 = i.uv_texcoord;
			half2 appendResult23 = (half2(_MainTex_TexelSize.z , _MainTex_TexelSize.w));
			half2 texture_coordinate20 = ( i.uv_texcoord * appendResult23 );
			half localMyCustomExpression20 = MyCustomExpression20( texture_coordinate20 );
			half4 lerpResult26 = lerp( float4( 0,0.1282372,1,0 ) , saturate( ( _Color * tex2D( _MainTex, uv_MainTex3 ) ) ) , saturate( ( localMyCustomExpression20 - 0.75 ) ));
			half4 lerpResult32 = lerp( float4( 1,0,0,0 ) , lerpResult26 , saturate( ( 1.0 - ( localMyCustomExpression20 - 3.5 ) ) ));
			o.Emission = lerpResult32.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19108
Node;AmplifyShaderEditor.CommentaryNode;40;-354.5082,682.5949;Inherit;False;706.0144;230.4425;Comment;4;31;32;33;30;Too Far;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;38;-365.2257,387.0562;Inherit;False;528.3665;204.0154;Comment;2;34;29;Too Close;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;2;-1315.744,211.6632;Inherit;True;Property;_MainTex;MainTex;1;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;LockedToTexture2D;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexCoordVertexDataNode;21;-1026.297,537.1512;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexelSizeNode;22;-1294.867,633.1686;Inherit;False;-1;1;0;SAMPLER2D;;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;23;-1068.044,726.4026;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-821.7393,670.7407;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;3;-998.3792,183.7438;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-632.3792,70.74377;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;1;-880.3792,-152.2562;Inherit;False;Property;_Color;Color;0;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;5;-442.9579,77.12742;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CustomExpressionNode;20;-660.3481,679.8192;Inherit;False;   float2 dx_vtc = ddx(texture_coordinate)@$   float2 dy_vtc = ddy(texture_coordinate)@$   float md = max(dot(dx_vtc, dx_vtc), dot(dy_vtc, dy_vtc))@$$return 0.5 * log2(md)@;1;Create;1;True;texture_coordinate;FLOAT2;0,0;In;;Inherit;False;My Custom Expression;True;False;0;;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;12;672.5291,471.6814;Half;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Clarity/Debug_MipMap;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SaturateNode;33;-22.14198,747.6359;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;31;-184.5171,750.2487;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;26;6.872337,456.1613;Inherit;False;3;0;COLOR;0,0.1282372,1,0;False;1;COLOR;1,0,0,1;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;32;166.753,723.9944;Inherit;False;3;0;COLOR;1,0,0,0;False;1;COLOR;1,0,0,1;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;29;-164.843,456.5841;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;34;-334.4976,437.8719;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.75;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;30;-336.7673,746.1337;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;3.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.StickyNoteNode;42;-1418.766,-150.2369;Inherit;False;318.1715;222.7198;New Note;;1,1,1,1;Unity's mipmap debug shader only shows in scene view, this shader allows a similar effect at runtime.$$Unity's existing implementation:$https://aras-p.info/blog/2011/05/03/a-way-to-visualize-mip-levels/;0;0
WireConnection;22;0;2;0
WireConnection;23;0;22;3
WireConnection;23;1;22;4
WireConnection;25;0;21;0
WireConnection;25;1;23;0
WireConnection;3;0;2;0
WireConnection;4;0;1;0
WireConnection;4;1;3;0
WireConnection;5;0;4;0
WireConnection;20;0;25;0
WireConnection;12;2;32;0
WireConnection;33;0;31;0
WireConnection;31;0;30;0
WireConnection;26;1;5;0
WireConnection;26;2;29;0
WireConnection;32;1;26;0
WireConnection;32;2;33;0
WireConnection;29;0;34;0
WireConnection;34;0;20;0
WireConnection;30;0;20;0
ASEEND*/
//CHKSM=923C926AC4615AC2E8D399636DCFEDD10C8B716D