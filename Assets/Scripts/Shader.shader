Shader "HelloShaders"
{
	Properties
	{
		_Color ("Color", Color) = (1.0,1.0,1.0,1.0)
	}
	
	SubShader
	{
		Pass
		{
			Tags { "LightMode" = "ForwardBase"}
			CGPROGRAM
			
			// pragmas
			#pragma vertex vert
			#pragma fragment frag
			
			// vars
			uniform fixed4 _Color;
			
			// unity vars
			uniform fixed4 _LightColor0;
			//float4x4 _World2Object;
			// input
			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
			
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				fixed4 col : COLOR;
			};
			
			// vertex
			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				float3 normalDirection = normalize(mul(float4(v.normal, 0.0), _World2Object).xyz);
				float3 lightDirection;
				float atten = 1.0;
				
				lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				
				float3 diffuseReflection = atten * _LightColor0.xyz * _Color.rgb * max(dot(normalDirection, lightDirection), 0.0);
				
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);  
				o.col = float4( diffuseReflection, 1.0); 
				return o;
			};
			
			
			// fragment
			fixed4 frag(vertexOutput v) : COLOR
			{
				return v.col;
			};
			
			ENDCG
		}	
	}
	
	// why
	//Fallback "Diffuse"
}