Shader "Custom/CorruptionShader" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
        _OutlineSize ("Outline Size", Float) = 10 
        _YCutOut ("Y CutOut", Float) = 0
        [MaterialToggle(_DISS_ON)] _Dissolution ("Enable Dissolution", Float) = 1 	//17
        _DissolveNoise ("Dissolve Noise", 2D) = "white" {}							//18
		_BurningColor ("Burning Color", Color) = (1,1,1,1)							//20
		_BurningWidth ("Burning Width", Range(0,0.2)) = 0.05 						//21
		_TileSize ("NoiseTilesRatio", Range(0,1.0)) = 0
    }
    SubShader {
    	Tags {"Queue"="Transparent"}
    	Cull Off
    	ZWrite Off
     	Blend One OneMinusSrcAlpha
        Pass {
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
			#pragma glsl_no_auto_normalization
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _DISS_OFF _DISS_ON

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _TintColor;
			float _OutlineSize;
			float _YCutOut;
			float _Alpha;
			
			
			sampler2D _DissolveNoise;
			float _BurningWidth;
			float4 _BurningColor;
			float _TileSize;

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
            	float yPosition = i.world.y;
            	
            	if(yPosition<(_YCutOut+(_OutlineSize/2.0)) && yPosition>(_YCutOut-(_OutlineSize/2.0))){
            		float magnitude = 1.0 - ((yPosition - (_YCutOut-(_OutlineSize/2.0)))/_OutlineSize);
            		
            		//#if _DISS_ON
						fixed4 dissolveColor = tex2D( _DissolveNoise, (i.world.xz*_TileSize));
						fixed highest = 0;
						if(dissolveColor.r>highest){highest = dissolveColor.r;}
						if(dissolveColor.g>highest){highest = dissolveColor.g;}
						if(dissolveColor.b>highest){highest = dissolveColor.b;}
						
						if(highest>magnitude || highest == 0){
							discard;
						}else if(highest>(magnitude -_BurningWidth) && magnitude <= 1-_BurningWidth){
							return _BurningColor;
						}
					//#endif
            	}else if(yPosition>_YCutOut){
            		discard;
            	}
                return _TintColor * textureColor;
            }
            ENDCG
        }
    }
}
