Shader "Skybox/Daynight"
{
    Properties
    {
        _Texture1("Texture1", 2D) = "white" {}
        _Texture2("Texture2", 2D) = "white" {}
        _Blend("Blend", Range(0, 1)) = 0.5
        _Rotation("Rotation", Range(0.000000, 360.000000)) = 0.000000
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata
            {
                float4 vertex : POSITION;
            };
 
            struct v2f
            {
                float3 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _Texture1;
            sampler2D _Texture2;
            float _Blend;
            float _Rotation;

            v2f vert(appdata v)
            {
                v2f o;
                o.texcoord = v.vertex.xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
 
            float2 ToRadialCoords(float3 coords)
            {
                float3 normalizedCoords = normalize(coords);
                float latitude = acos(normalizedCoords.y);
                float longitude = atan2(normalizedCoords.z, normalizedCoords.x);
                float2 sphereCoords = float2(longitude, latitude) * float2(0.5 / UNITY_PI, 1.0 / UNITY_PI);
                return float2(0.5, 1.0) - sphereCoords;
            }
 
            fixed4 frag(v2f i) : SV_Target
            {
                float2 tc = ToRadialCoords(i.texcoord);
                tc.x += _Rotation / 360.0; // 텍스처 좌표를 이동시켜 하늘이 회전하는 효과 구현
                tc.x = frac(tc.x); // 좌표를 0-1 범위로 유지
                fixed4 tex1 = tex2D(_Texture1, tc);
                fixed4 tex2 = tex2D(_Texture2, tc);
                return lerp(tex1, tex2, _Blend);
            }
            ENDCG
        }
    }
}
