Shader "Custom/SmoothAlphaGradient"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _AlphaStart ("Alpha Start", Range(0,1)) = 0
        _AlphaEnd ("Alpha End", Range(0,1)) = 1
        _Smoothness ("Smoothness", Range(0.1, 5)) = 1
        _Offset ("Offset", Range(-1, 1)) = 0
        
        // Stencil properties for UI
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 8
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _AlphaStart;
            float _AlphaEnd;
            float _Smoothness;
            float _Offset;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.color = v.color * _Color;
                return OUT;
            }

            float smoothTransition(float t, float smoothness)
            {
                // Несколько вариантов на выбор (раскомментируйте нужный):
                
                // Вариант 1: Степенная функция (простая)
                // return pow(t, smoothness);
                
                // Вариант 2: Sigmoid-подобная функция (более плавная)
                // float p = smoothness * 5;
                // return 1.0 / (1.0 + exp(-p * (t - 0.5)));
                
                // Вариант 3: Smoothstep с контролем крутизны (рекомендую)
                float low = 0.5 - 0.5 / smoothness;
                float high = 0.5 + 0.5 / smoothness;
                return smoothstep(low, high, t);
                
                // Вариант 4: Кубическая (очень плавная)
                // return t * t * (3.0 - 2.0 * t);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.texcoord) * i.color;
                
                // Базовый параметр t от 0 до 1 по X координате с учетом смещения
                float t = i.texcoord.x + _Offset;
                t = saturate(t); // Ограничиваем от 0 до 1
                
                // Применяем smoothness для контроля плавности
                float alpha = lerp(_AlphaStart, _AlphaEnd, smoothTransition(t, _Smoothness));
                
                color.a *= alpha;
                
                return color;
            }
            ENDCG
        }
    }
}