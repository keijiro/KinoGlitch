//
// KinoGlitch - Video glitch effect
//
// Copyright (C) 2015 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
Shader "Hidden/Kino/Glitch/Digital"
{
    Properties
    {
        _MainTex  ("-", 2D) = "" {}
        _NoiseTex ("-", 2D) = "" {}
        _TrashTex ("-", 2D) = "" {}
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    sampler2D _NoiseTex;
    sampler2D _TrashTex;
    float _Intensity;

    float4 frag(v2f_img i) : SV_Target 
    {
        float4 glitch = tex2D(_NoiseTex, i.uv);

        float thresh = 1.001 - _Intensity * 1.001;
        float w_d = step(thresh, pow(glitch.z, 2.5)); // displacement glitch
        float w_f = step(thresh, pow(glitch.w, 2.5)); // frame glitch
        float w_c = step(thresh, pow(glitch.z, 3.5)); // color glitch

        // Displacement.
        float2 uv = frac(i.uv + glitch.xy * w_d);
        float4 source = tex2D(_MainTex, uv);

        // Mix with trash frame.
        float3 color = lerp(source, tex2D(_TrashTex, uv), w_f).rgb;

        // Shuffle color components.
        float3 neg = saturate(color.grb + (1 - dot(color, 1)) * 0.5);
        color = lerp(color, neg, w_c);

        return float4(color, source.a);
    }

    ENDCG

    SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0
            ENDCG
        }
    }
}
