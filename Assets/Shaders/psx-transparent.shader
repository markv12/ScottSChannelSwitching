Shader "psx/transparent" {
	Properties{
		[HDR] _Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
		SubShader{
			Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
			LOD 100
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			Pass {
				CGPROGRAM

					#pragma vertex vert
					#pragma fragment frag
					#include "UnityCG.cginc"
					#include "noiseSimplex.cginc"

					struct v2f
					{
						fixed4 pos : SV_POSITION;
						half4 color : COLOR0;
						half3 normal : TEXCOORD1;
						float3 worldPos : TEXCOORD2;
					};

					float4 _Color;

					v2f vert(appdata_full v)
					{
						v2f o;

						//Vertex snapping
						float4 snapToPixel = UnityObjectToClipPos(v.vertex);
						float4 vertex = snapToPixel;
						vertex.xyz = snapToPixel.xyz / snapToPixel.w;
						vertex.x = floor(160 * vertex.x) / 160;
						vertex.y = floor(120 * vertex.y) / 120;
						vertex.xyz *= snapToPixel.w;
						o.pos = vertex;

						//Vertex lighting 
					//	o.color =  float4(ShadeVertexLights(v.vertex, v.normal), 1.0);
						o.color = float4(ShadeVertexLightsFull(v.vertex, v.normal, 4, true), 1.0);
						o.color *= v.color;

						float distance = length(mul(UNITY_MATRIX_MV,v.vertex));

						//Affine Texture Mapping
						float4 affinePos = vertex; //vertex;				
						o.normal = distance + (vertex.w*(UNITY_LIGHTMODEL_AMBIENT.a * 8)) / distance / 2;

						o.worldPos = mul(unity_ObjectToWorld, v.vertex);
						return o;
					}

					uniform float4 VisionPoints[25];

					float4 frag(v2f IN) : COLOR
					{
						half4 c = IN.color * _Color;

						float closeness = 0;
						float coloredness = 0;
						float lum = -0.5+(0.2126*c.r + 0.7152*c.g + 0.0722*c.b);
						float theNoise = snoise(IN.worldPos * 3);
						for (int j = 0; j < 25; j++) {
							float4 thePoint = VisionPoints[j];
							float dist = distance(IN.worldPos, thePoint);
							dist += (theNoise + lum);

							if (dist < thePoint.a) {
								closeness += 1;
								coloredness = 0;
							}
							else if (dist < (thePoint.a + 0.8) && closeness == 0) {
								coloredness += 1;
							}
							
						}
						clip(-0.5 + closeness + coloredness);
						if (coloredness > 0) {
							c.b = 2.5;
						}
	/*					if (coloredness > 0) {
							return float4(1, 0, 0, 1);
						}*/

						return c;
					}
				ENDCG
			}
	}
	FallBack "VertexLit"
}