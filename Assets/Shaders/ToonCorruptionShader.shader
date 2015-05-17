Shader "Custom/ToonCorruptionShader" {
	Properties 
    {
		[MaterialToggle(_OUTL_ON)] _Outl ("Outline", Float) = 0 						//0
		[MaterialToggle(_TEX_ON)] _DetailTex ("Enable Detail texture", Float) = 0 	//1
		_MainTex ("Detail", 2D) = "white" {}        								//2
		_ToonShade ("Shade", 2D) = "white" {}  										//3
		[MaterialToggle(_COLOR_ON)] _TintColor ("Enable Color Tint", Float) = 0 	//4
		_Color ("Base Color", Color) = (1,1,1,1)									//5	
		[MaterialToggle(_VCOLOR_ON)] _VertexColor ("Enable Vertex Color", Float) = 0//6        
		_Brightness ("Brightness 1 = neutral", Float) = 1.0							//7	
		[MaterialToggle(_DS_ON)] _DS ("Enable DoubleSided", Float) = 0				//8	
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull mode", Float) = 2		//9	
		_OutlineColor ("Outline Color", Color) = (0.5,0.5,0.5,1.0)					//10
		_Outline ("Outline width", Float) = 0.01									//11
		[MaterialToggle(_ASYM_ON)] _Asym ("Enable Asymmetry", Float) = 0        	//12
		_Asymmetry ("OutlineAsymmetry", Vector) = (0.0,0.25,0.5,0.0)     			//13
		[MaterialToggle(_TRANSP_ON)] _Trans ("Enable Transparency", Float) = 0   	//14
		[Enum(TRANS_OPTIONS)] _TrOp ("Transparency mode", Float) = 0                //15
		
		
		[MaterialToggle(_CORR_ON)] _Corruption ("Enable Corruption", Float) = 1 
		_CorruptionTexture ("Corruption Texture", 2D) = "white" {}  
		_CorruptionColor ("Corruption Color", Color) = (0.5,0.5,0.5,0.3)
        _OutlineSize ("Corruption Wave Size", Float) = 10 
        _YCutOut ("Y CutOut", Float) = 1000
        _DissolveNoise ("Dissolve Noise", 2D) = "white" {}							//18
		_BurningColor ("Burning Color", Color) = (1,1,1,1)							//20
		_BurningWidth ("Burning Width", Range(0,0.2)) = 0 							//21
		_TileSize ("NoiseTilesRatio", Range(0,1.0)) = 0.15
		
    }
   
    Subshader 
    {
    	Tags { "RenderType"="Opaque" }
		LOD 250
    	ZWrite On
	   	Cull [_Cull]
		Lighting Off
		Fog { Mode Off }
		
        Pass 
        {
            Name "BASE"
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #include "UnityCG.cginc"
                #pragma glsl_no_auto_normalization
                #pragma multi_compile _TEX_OFF _TEX_ON
                #pragma multi_compile _COLOR_OFF _COLOR_ON
                #pragma multi_compile _CORR_OFF _CORR_ON

                
                #if _TEX_ON
                sampler2D _MainTex;
				half4 _MainTex_ST;
				#endif
				
				#if _CORR_ON
			    sampler2D _CorruptionTexture;
				half4 _CorruptionTexture_ST;
				fixed4 _CorruptionColor;
       		    float _OutlineSize;
        		float _YCutOut;
                sampler2D _DissolveNoise;
                half4 _DissolveNoise_ST;
                fixed4 _BurningColor;
                fixed _BurningWidth;
				float _TileSize;
				float _CorruptionAlpha = 0.3;
				#endif
				
				sampler2D _ToonShade;
                fixed _Brightness;
                
                #if _COLOR_ON
                fixed4 _Color;
                #endif
				
                struct appdata_base0 
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 texcoord : TEXCOORD0;
				};
				
                 struct v2f 
                 {
                    float4 pos : SV_POSITION;
                    half2 uvn : TEXCOORD1;
                    #if _TEX_ON
                    	half2 uv : TEXCOORD0;
                    #endif
                    #if _DISS_ON
                    	half2 disuv : TEXCOORD2;
                    #endif
                    #if _CORR_ON
	                   float4 world : TEXCOORD3;
	                   half2 uvC : TEXCOORD4;
					#endif
                    
                 };
               
                v2f vert (appdata_base0 v)
                {
                    v2f o;
                    o.pos = mul ( UNITY_MATRIX_MVP, v.vertex );
                    float3 n = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                    n = n * float3(0.5,0.5,0.5) + float3(0.5,0.5,0.5);
                    o.uvn = n.xy;
                    #if _TEX_ON
                    o.uv = TRANSFORM_TEX ( v.texcoord, _MainTex );
                    #endif
                    
                    #if _CORR_ON
	                   o.world =  mul( _Object2World, v.vertex );
	                   o.uvC = TRANSFORM_TEX ( v.texcoord, _CorruptionTexture );
					#endif
					
                    return o;
                }
                
                fixed4 frag (v2f i) : COLOR
                {
					
                	#if _CORR_ON
                		float4 mainTexture = float4(0.0,0.0,0.0,0.0);
                	   #if _TEX_ON
	                   		mainTexture = tex2D(_MainTex, i.uv);
	                   #endif
	                   #if _COLOR_ON
					   		mainTexture = mainTexture * _Color;
					   #endif
	                   float4 corruptionTexture = tex2D(_CorruptionTexture, i.uvC);
	                   corruptionTexture = corruptionTexture * _CorruptionColor;
	                   
	                   //Manual alphablending
	                   float alphaCorruption = corruptionTexture.a;
	                   float4 mixedTexture;
	                   mainTexture.r = mainTexture.r * (1.0 - alphaCorruption);
	                   mainTexture.g = mainTexture.g * (1.0 - alphaCorruption);
	                   mainTexture.b = mainTexture.b * (1.0 - alphaCorruption);
	                   
	                   corruptionTexture.r = corruptionTexture.r * (alphaCorruption);
	                   corruptionTexture.g = corruptionTexture.g * (alphaCorruption);
	                   corruptionTexture.b = corruptionTexture.b * (alphaCorruption);
	                   
	                   mixedTexture = mainTexture+corruptionTexture;
	                   mixedTexture.a = 1.0;
	                   //End of manual alphablending
	                   
            		   float yPosition = i.world.y;
            	
            			if(yPosition<(_YCutOut+(_OutlineSize/2.0)) && yPosition>(_YCutOut-(_OutlineSize/2.0))){
	            			float magnitude = 1.0 - ((yPosition - (_YCutOut-(_OutlineSize/2.0)))/_OutlineSize);
	            		
							fixed4 dissolveColor = tex2D( _DissolveNoise, (i.world.xz*_TileSize));
							fixed highest = 0;
							if(dissolveColor.r>highest){highest = dissolveColor.r;}
							if(dissolveColor.g>highest){highest = dissolveColor.g;}
							if(dissolveColor.b>highest){highest = dissolveColor.b;}
							
							if(highest>magnitude || highest == 0){
								//discard;
							}else if(highest>(magnitude -_BurningWidth) && magnitude <= 1-_BurningWidth){
								return _BurningColor;
							}else{
								return mixedTexture;
							}
		            	}else if(yPosition<_YCutOut){
		            		return mixedTexture;
		            	}
					#endif

					#if _COLOR_ON
						fixed4 toonShade = tex2D( _ToonShade, i.uvn )*_Color;
					#else
						fixed4 toonShade = tex2D( _ToonShade, i.uvn );
					#endif
					
					#if _TEX_ON
						fixed4 detail = tex2D ( _MainTex, i.uv );
						return  toonShade * detail*_Brightness;
					#else
						return  toonShade * _Brightness;
					#endif
                }
            ENDCG
        }
        
         	
        Pass
        {
            Cull Front
            ZWrite On
            CGPROGRAM
			#include "UnityCG.cginc"
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma glsl_no_auto_normalization
            #pragma vertex vert
 			#pragma fragment frag
			
            struct appdata_t 
            {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f 
			{
				float4 pos : SV_POSITION;
			};

            fixed _Outline;
            fixed _DissolveThreshold;

            
            v2f vert (appdata_t v) 
            {
                v2f o;
			    o.pos = v.vertex;
			    o.pos.xyz += v.normal.xyz *_Outline*0.01;
			    o.pos = mul(UNITY_MATRIX_MVP, o.pos);
			    return o;
            }
            
            fixed4 _OutlineColor;
            
            fixed4 frag(v2f i) :COLOR 
			{
				fixed4 outline = _OutlineColor;
		    	return outline;
			}
            
            ENDCG
        }
    }
    Fallback "Diffuse"
}
