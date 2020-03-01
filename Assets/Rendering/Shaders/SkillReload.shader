// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/WeaponReload"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _CooldownProgress("CooldownLeft", Float) = 1
        _MainColor("Main Color", Color) = (1, 1, 1, 1)
        _SkillActive("Is Skill Active", Float) = 0
        _SkillActiveColor("Skill Active Color", Color) = (1, 0.7, 0.7, 1)
    }
    SubShader
    {
        Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature SKILL_ACTIVE

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _CooldownProgress;
            float4 _MainTex_TexelSize;
            float4 _MainColor;
            float _SkillActive;
            float4 _SkillActiveColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                col.a = i.uv[1] > _CooldownProgress ? 0 : col.a * 0.7;
                if (_SkillActive == 1) {
                    return half4(_SkillActiveColor.rgb, col.a);
                }
                else {
                    return half4(i.color.rgb, col.a);
                }
                    
            }
            ENDCG
        }
    }
}
