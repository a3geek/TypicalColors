Shader "Hidden/TypicalColorsChecker"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ThresholdX("Threshold X", float) = 0.1
	}
	SubShader
	{
		Cull Off
		ZWrite Off
		ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct TypicalColor
			{
				float4 color;
			};
			
			uniform sampler2D _MainTex;
			uniform float _ThresholdX, _ThresholdBolid;
			uniform int _Length;
			uniform StructuredBuffer<TypicalColor> _TypicalColors;

			float4 frag (v2f_img i) : SV_Target
			{
				if (i.uv.x > _ThresholdX + _ThresholdBolid || _Length <= 0)
				{
					return tex2D(_MainTex, i.uv);
				}
				else if (i.uv.x >= _ThresholdX - _ThresholdBolid && i.uv.x <= _ThresholdX + _ThresholdBolid)
				{
					return 1;
				}

				float rate = 1.0 / _Length;
				int idx = min(_Length - 1, floor(i.uv.y / rate));
				return _TypicalColors[idx].color;
			}
			ENDCG
		}
	}
}
