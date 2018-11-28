// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Effects/EnergyShield"
{
	Properties
	{
		_MainTex("Pattern", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Smoothness("Edge Smoothness", Range(0.25,1)) = 0.5
		_Whiteness("Edge Whiteness", Range(0,2)) = 1
		_Strength("Strength", Range(0,5)) = 1
	}
		SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		//Blend SrcAlpha OneMinusSrcAlpha
		Blend One One
		ZWrite Off
		Pass
		{
			Cull Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				fixed3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed3 viewDir : TEXCOORD1;
				fixed3 worldPos : TEXCOORD2;
				float4 vertex : SV_POSITION;
				fixed3 normal : NORMAL;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fixed4 _Color;
			fixed _Smoothness;
			fixed _Whiteness;
			fixed _Strength;
			uniform float3 Collisions[20];
			uniform float SpreadDistances[20];
			
			v2f vert (appdata v)
			{
				v2f o;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = normalize(UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex)));
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 pattern = tex2D(_MainTex, i.uv);

				fixed dotProd = dot(i.normal, i.viewDir);
				if (dotProd < 0) {
					dotProd = 1;
				}
				fixed rim = max(0, 1 - dotProd / _Smoothness);
				fixed4 result = (_Color * rim) + (rim * _Whiteness);

				float amount = 0;
				for (int j = 0; j < 20; j++) {
					float dist = distance(i.worldPos, Collisions[j]);
					amount += max(0, 1 - (3*abs(dist - SpreadDistances[j])));
				}
				float4 additive = lerp(0, pattern.r, amount) * _Color;
				return (result + additive) * _Strength;
			}
			ENDCG
		}
	}
}
