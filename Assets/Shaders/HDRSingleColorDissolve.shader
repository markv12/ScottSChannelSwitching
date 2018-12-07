Shader "Custom/HDRSingleColorDissolve"
{
	Properties
	{
		[HDR] _Color("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Name "FORWARD"
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "noiseSimplex.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD0;
			};

			float4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			uniform float4 VisionPoints[25];
			fixed4 frag (v2f i) : SV_Target
			{
				float closeness = 0;
				float coloredness = 0;
				float theNoise = snoise(i.worldPos * 3);
				for (int j = 0; j < 25; j++) {
					float4 thePoint = VisionPoints[j];
					float dist = distance(i.worldPos, thePoint);
					dist += (theNoise-0.97);

					if (dist < thePoint.a) {
						closeness += 1;
						coloredness = 0;
					}
					else if (dist < (thePoint.a + 0.8) && closeness == 0) {
						coloredness += 1;
					}
				}
				clip(-0.5 + closeness + coloredness);
				return _Color;
			}
			ENDCG
		}
	}
		FallBack "VertexLit"
}
