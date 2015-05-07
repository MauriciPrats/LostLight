Shader "Custom/CorruptionAthmosphereShader" {
	 Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _OriginalColor ("Original Color", Color) = (0.5,0.5,0.5,0.5)
        _CorruptedColor ("Corrupted Color", Color) = (0.5,0.5,0.5,0.5)
        _OutlineColor ("Outline Color", Color) = (1.0,1.0,1.0,1.0)
        _OutlineSize ("Outline Size", Float) = 0.1 
        _YCutOut ("Y CutOut", Float) = 0
    }
    SubShader {
    	Tags { "Queue" = "Transparent" }
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _OriginalColor;
			float4 _CorruptedColor;
			float4 _OutlineColor;
			float _OutlineSize;
			float _YCutOut;

            struct vertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
            };

            struct fragmentInput{
                float4 position : SV_POSITION;
                half2 uv : TEXCOORD0;
                float4 world : TEXCOORD1;
            };

            fragmentInput vert(vertexInput i){
                fragmentInput o;
                //o.texcoord0 = i.texcoord0;
                o.position = mul (UNITY_MATRIX_MVP, i.vertex);
                o.world =  mul( _Object2World, i.vertex );
                o.uv = TRANSFORM_TEX( i.texcoord0, _MainTex );
                return o;
            }

            float4 frag(fragmentInput i) : SV_Target {
            	float4 textureColor = tex2D(_MainTex, i.uv);
            	float4 finalColor = _CorruptedColor * textureColor;
            	
            	if(i.world.y<(_YCutOut+_OutlineSize) && i.world.y>(_YCutOut-_OutlineSize)){
            		finalColor = _OutlineColor;
            	}
            	if(i.world.y>_YCutOut){
            		finalColor = textureColor * _OriginalColor;
            	}
                return finalColor;
            }
            ENDCG
        }
    }
}
