Shader "Roystan/Toon/Water Tut Ortho"
{
    Properties
    {
		_DepthGradientShallow("Depth Gradient Shallow", Color) = (0.325, 0.807, 0.971, 0.725)
		_DepthGradientDeep("Depth Gradient Deep", Color) = (0.086, 0.407, 1, 0.749)
		_DepthMaxDistance("Depth Maximum Distance", Float) = 1
		_FoamColor("Foam Color", Color) = (1,1,1,1)
		_SurfaceNoise("Surface Noise", 2D) = "white" {}
		_SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)
		_SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777
		_SurfaceDistortion("Surface Distortion", 2D) = "white" {}	
		_SurfaceDistortionAmount("Surface Distortion Amount", Range(0, 1)) = 0.27
		_FoamMaxDistance("Foam Maximum Distance", Float) = 0.4
		_FoamMinDistance("Foam Minimum Distance", Float) = 0.04		
    }
    SubShader
    {
		Tags
		{
			"Queue" = "Transparent"
		}

        Pass
        {
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

            CGPROGRAM
			#define SMOOTHSTEP_AA 0.01

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			float4 alphaBlend(float4 top, float4 bottom)
			{
				float3 color = (top.rgb * top.a) + (bottom.rgb * (1 - top.a));
				float alpha = top.a + bottom.a * (1 - top.a);

				return float4(color, alpha);
			}

            struct appdata
            {
                float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;	
				float2 noiseUV : TEXCOORD0;
				float2 distortUV : TEXCOORD1;
				float4 screenPosition : TEXCOORD2;
				float3 viewNormal : NORMAL;
            };

			sampler2D _SurfaceNoise;
			float4 _SurfaceNoise_ST;

			sampler2D _SurfaceDistortion;
			float4 _SurfaceDistortion_ST;

            v2f vert (appdata v)
            {
                v2f o;

				// Obtén la posición del vértice en espacio de clip (proyección)
                o.vertex = UnityObjectToClipPos(v.vertex);
				
				// En cámaras ortográficas, ComputeScreenPos sigue siendo útil
				o.screenPosition = ComputeScreenPos(o.vertex);
				
				o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);
				o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);
				o.viewNormal = COMPUTE_VIEW_NORMAL;

                return o;
            }

			float4 _DepthGradientShallow;
			float4 _DepthGradientDeep;
			float4 _FoamColor;

			float _DepthMaxDistance;
			float _FoamMaxDistance;
			float _FoamMinDistance;
			float _SurfaceNoiseCutoff;
			float _SurfaceDistortionAmount;

			float2 _SurfaceNoiseScroll;

			sampler2D _CameraDepthTexture;
			sampler2D _CameraNormalsTexture;

            float4 frag (v2f i) : SV_Target
            {
				// Para cámaras ortográficas, la profundidad puede ser tomada directamente
				float existingDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;

				// Diferencia de profundidad, en unidades de Unity, entre la superficie del agua y el objeto detrás de ella
				float depthDifference = existingDepth01 - i.screenPosition.z;

				// Calcula el color del agua basado en la diferencia de profundidad usando los colores de gradiente
				float waterDepthDifference01 = saturate(depthDifference / _DepthMaxDistance);
				float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep, waterDepthDifference01);
				
				// Obtén la normal en espacio de vista de la superficie detrás del píxel actual
				float3 existingNormal = tex2Dproj(_CameraNormalsTexture, UNITY_PROJ_COORD(i.screenPosition));
				
				// Calcula la cantidad de espuma mostrada basado en la diferencia entre las normales de la superficie del agua y el objeto detrás
				float3 normalDot = saturate(dot(existingNormal, i.viewNormal));
				float foamDistance = lerp(_FoamMaxDistance, _FoamMinDistance, normalDot);
				//float foamDepthDifference01 = saturate(depthDifference / foamDistance);
				float foamDepthDifference01 = 0.5;

				float surfaceNoiseCutoff = foamDepthDifference01 * _SurfaceNoiseCutoff;

				float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV).xy * 2 - 1) * _SurfaceDistortionAmount;

				// Distorciona las UV del ruido basado en los canales RG de la textura de distorsión
				float2 noiseUV = float2((i.noiseUV.x + _Time.y * _SurfaceNoiseScroll.x) + distortSample.x, 
				(i.noiseUV.y + _Time.y * _SurfaceNoiseScroll.y) + distortSample.y);
				float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;

				// Utiliza smoothstep para obtener anti-aliasing en la transición de espuma a superficie
				float surfaceNoise = smoothstep(surfaceNoiseCutoff - SMOOTHSTEP_AA, surfaceNoiseCutoff + SMOOTHSTEP_AA, surfaceNoiseSample);

				float4 surfaceNoiseColor = _FoamColor;
				surfaceNoiseColor.a *= surfaceNoise;

				// Combina la espuma con la superficie usando mezcla alfa normal
				return alphaBlend(surfaceNoiseColor, waterColor);
            }
            ENDCG
        }
    }
}