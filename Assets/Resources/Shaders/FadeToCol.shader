Shader "Custom/FadeToCol" {
    Properties {
       _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {   
       Tags { "RenderType"="Opaque" }
        
       CGPROGRAM
       #pragma surface surf Lambert 
 
       struct Input {
         float2 uv_MainTex;
         float3 worldPos;
       };
       sampler2D _MainTex;
 
       void surf (Input IN, inout SurfaceOutput o) {
         half4 tex = tex2D (_MainTex, IN.uv_MainTex);
         half4 output = lerp (tex, 0, max(abs(IN.worldPos.x), abs(IN.worldPos.z))*0.01 - 0.1);
         o.Albedo = output.rgb;
       }
       ENDCG
    } 
    FallBack "Diffuse"
}
