
#ifndef TREEPASS_INCLUDED
#define TREEPASS_INCLUDED

struct Attributes
{
	float4 positionOS   : POSITION;
	float3 normalOS     : NORMAL;
	float4 tangentOS    : TANGENT;
	float2 uv           : TEXCOORD0;
	float2 uvLM         : TEXCOORD1;
	float4 color : COLOR;

	UNITY_VERTEX_INPUT_INSTANCE_ID
};


struct Varyings
{

	float3 normalOS : NORMAL;
	float2 uv                       : TEXCOORD0;
	float2 uvLM                     : TEXCOORD1;
	float4 wsPosition   			: TEXCOORD2; // xyz: positionWS, w: vertex fog factor
	half3 normalWS                 : TEXCOORD3;
	half3 tangentWS                 : TEXCOORD4;
	float4 positionOS : TEXCOORD5;

	float4 color : COLOR;

#if _NORMALMAP
	half3 bitangentWS               : TEXCOORD5;
#endif

#ifdef _MAIN_LIGHT_SHADOWS
	float4 shadowCoord              : TEXCOORD6; // compute shadow coord per-vertex for the main light
#endif
	float4 positionCS               : SV_POSITION;
};



float Height;
float Base;
float _Offset;

float _AlphaCutoff;
float4 _LightColor;
float4 _DarkColor;
int _AmountOfColors;

float4 _ShadowColor;
float _ShadowStep;


float rand(float3 co)
{
	return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 53.539))) * 43758.5453);
}



//* The following has been kept to the bare minimum to transfer shadows and lighting *// 
Varyings Vertex(Attributes input)
{
	Varyings output;

	output.color = input.color;

	float3 posObj = input.positionOS.xyz;

	VertexPositionInputs vertexInput = GetVertexPositionInputs(posObj);

	VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

	// Computes fog factor per-vertex.
	float fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

	output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
	output.uvLM = input.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw;

	output.wsPosition = float4(vertexInput.positionWS, fogFactor);
	output.normalWS = vertexNormalInput.normalWS;
	output.tangentWS = vertexNormalInput.tangentWS;

#ifdef _NORMALMAP

	output.bitangentWS = vertexNormalInput.bitangentWS;
#endif
 
#ifdef _MAIN_LIGHT_SHADOWS

	output.shadowCoord = GetShadowCoord(vertexInput);
#endif

	// We just use the homogeneous clip position from the vertex input
	output.positionCS = vertexInput.positionCS;
	output.positionOS = input.positionOS;
	output.normalOS = input.normalOS;

	return output;
}



float3x3 RotY(float ang)
{
	return float3x3
	(
		cos(ang), 0, sin(ang),
		0, 1, 0,
		-sin(ang), 0, cos(ang)
	);
}

float3x3 RotX(float ang)
{
	return float3x3
	(
		1, 0, 0,
		0, cos(ang), -sin(ang),
		0, sin(ang), cos(ang)
	);

}

float3x3 RotZ(float ang)
{
	return float3x3
	(
		cos(ang), -sin(ang), 0,
		sin(ang), cos(ang), 0,
		0, 0, 1
	);
}

float3 RotV(float3 In, float3 Axis, float Rotation)
{
    float s = sin(Rotation);
    float c = cos(Rotation);
    float one_minus_c = 1.0 - c;

    Axis = normalize(Axis);
    float3x3 rot_mat = 
    {   one_minus_c * Axis.x * Axis.x + c, one_minus_c * Axis.x * Axis.y - Axis.z * s, one_minus_c * Axis.z * Axis.x + Axis.y * s,
        one_minus_c * Axis.x * Axis.y + Axis.z * s, one_minus_c * Axis.y * Axis.y + c, one_minus_c * Axis.y * Axis.z - Axis.x * s,
        one_minus_c * Axis.z * Axis.x - Axis.y * s, one_minus_c * Axis.y * Axis.z + Axis.x * s, one_minus_c * Axis.z * Axis.z + c
    };
    return mul(rot_mat, In);
}




[maxvertexcount(18)]
void Geometry(triangle Varyings input[3], inout TriangleStream<Varyings> outStream)
{
	int i = 0;

	Varyings o = input[i];
	Varyings o2 = input[i];
	Varyings o3 = input[i];
	Varyings o4 = input[i];

	float3 normal = o.normalWS;
	float3 tangent = o.tangentWS;

	float2 uv = (input[i].positionOS.xy);

	float3 randomRotationAxis = float3(rand(o.wsPosition.xyz), rand(o.wsPosition.xyz + 1), rand(o.wsPosition.xyz + 2));
	float randomRotation = rand(o.wsPosition.xyz) * PI * 2;

	float3 rotatedTangent = normalize(RotV(tangent, randomRotationAxis, randomRotation));
	float3 rotatedNormal = normalize(RotV(normal, randomRotationAxis, randomRotation));

	float3 wsPosition = o.wsPosition.xyz;
	float3 vBase = rotatedTangent * Base;
	float3 vHeight = rotatedNormal * Height;

	float3 offset = (vBase * 0.5) + (vHeight * 0.5) - (vHeight * _Offset);
	float3 position = wsPosition - offset;

	float3 s1_1_pos = position;
	float3 s1_2_pos = position + vBase;
	float3 s1_3_pos = position + vHeight + vBase;
	float3 s1_4_pos = position + vHeight;

	o.positionCS = TransformWorldToHClip(s1_1_pos);
	o2.positionCS = TransformWorldToHClip(s1_2_pos);
	o3.positionCS = TransformWorldToHClip(s1_3_pos);
	o4.positionCS = TransformWorldToHClip(s1_4_pos);

	float3 norm = mul(rotatedTangent, RotY(PI / 2));

	o.normalWS = normal;
	o2.normalWS = normal;
	o3.normalWS = normal;
	o4.normalWS = normal;

	o4.uv = TRANSFORM_TEX(float2(0, 1), _BaseMap);
	o3.uv = TRANSFORM_TEX(float2(1, 1), _BaseMap);
	o2.uv = TRANSFORM_TEX(float2(1, 0), _BaseMap);
	o.uv = TRANSFORM_TEX(float2(0, 0), _BaseMap);

	outStream.Append(o4);
	outStream.Append(o3);
	outStream.Append(o);
	outStream.RestartStrip();

	outStream.Append(o3);
	outStream.Append(o2);
	outStream.Append(o);
	outStream.RestartStrip();


	// Perpendicular plane
	rotatedTangent = normalize(RotV(tangent, normal, PI / 2));
	rotatedTangent = normalize(RotV(rotatedTangent, randomRotationAxis, randomRotation));

	vBase = rotatedTangent * Base;
	vHeight = rotatedNormal * Height;

	offset = (vBase * 0.5) + (vHeight * 0.5) - (vHeight * _Offset);
	position = wsPosition - offset;

	Varyings s2_1 = input[i];
	Varyings s2_2 = input[i];
	Varyings s2_3 = input[i];
	Varyings s2_4 = input[i];

	float3 s2_1_pos = position;
	float3 s2_2_pos = position + vBase;
	float3 s2_3_pos = position + vHeight + vBase;
	float3 s2_4_pos = position + vHeight;

	s2_1.positionCS = TransformWorldToHClip(s2_1_pos);
	s2_2.positionCS = TransformWorldToHClip(s2_2_pos);
	s2_3.positionCS = TransformWorldToHClip(s2_3_pos);
	s2_4.positionCS = TransformWorldToHClip(s2_4_pos);

	s2_1.normalWS = normal;
	s2_2.normalWS = normal;
	s2_3.normalWS = normal;
	s2_4.normalWS = normal;

	s2_1.uv = TRANSFORM_TEX(float2(0, 0), _BaseMap);
	s2_2.uv = TRANSFORM_TEX(float2(1, 0), _BaseMap);
	s2_3.uv = TRANSFORM_TEX(float2(1, 1), _BaseMap);
	s2_4.uv = TRANSFORM_TEX(float2(0, 1), _BaseMap);

	outStream.Append(s2_4);
	outStream.Append(s2_3);
	outStream.Append(s2_1);
	outStream.RestartStrip();

	outStream.Append(s2_3);
	outStream.Append(s2_2);
	outStream.Append(s2_1);
	outStream.RestartStrip();


	// Cross plane
	Varyings s3_1 = input[i];
	Varyings s3_2 = input[i];
	Varyings s3_3 = input[i];
	Varyings s3_4 = input[i];

	float3 baseDirection = normalize(RotV(normal, tangent, PI/2));
	baseDirection = normalize(RotV(baseDirection, randomRotationAxis, randomRotation));

	float3 heightDirection = tangent;
	heightDirection = normalize(RotV(heightDirection, randomRotationAxis, randomRotation));

	vBase = baseDirection * Base;
	vHeight = heightDirection * Base;

	offset = (vBase * 0.5) + (vHeight * 0.5) - (vHeight * _Offset);
	position = wsPosition - offset;

	float3 s3_1_pos = position;
	float3 s3_2_pos = position + vBase;
	float3 s3_3_pos = position + vHeight + vBase;
	float3 s3_4_pos = position + vHeight;

	s3_1.positionCS = TransformWorldToHClip(s3_1_pos);
	s3_2.positionCS = TransformWorldToHClip(s3_2_pos);
	s3_3.positionCS = TransformWorldToHClip(s3_3_pos);
	s3_4.positionCS = TransformWorldToHClip(s3_4_pos);

	s3_1.normalWS = normal;
	s3_2.normalWS = normal;
	s3_3.normalWS = normal;
	s3_4.normalWS = normal;

	s3_1.uv = TRANSFORM_TEX(float2(0, 0), _BaseMap);
	s3_2.uv = TRANSFORM_TEX(float2(1, 0), _BaseMap);
	s3_3.uv = TRANSFORM_TEX(float2(1, 1), _BaseMap);
	s3_4.uv = TRANSFORM_TEX(float2(0, 1), _BaseMap);

	outStream.Append(s3_4);
	outStream.Append(s3_3);
	outStream.Append(s3_1);
	outStream.RestartStrip();

	outStream.Append(s3_3);
	outStream.Append(s3_2);
	outStream.Append(s3_1);
	outStream.RestartStrip();

}


float4 TransformWorldToShadowCoords(float3 positionWS)
{
//Explain a bit about Cascading
#ifdef _MAIN_LIGHT_SHADOWS_CASCADE
	half cascadeIndex = ComputeCascadeIndex(positionWS);
#else
	half cascadeIndex = 0;
#endif

	//Converting the world to shadow coordinates and passing in cascade index (matrix multiply to transform)
	return mul(_MainLightWorldToShadow[cascadeIndex], float4(positionWS, 1.0));
}





half4 Fragment(Varyings input, bool vf : SV_IsFrontFace) : SV_Target
{
	// --- Variables ---
	half3 normal = normalize(input.normalWS);
	
	int amountOfColors = _AmountOfColors - 1;

	Light mainLight;
	float3 wsPosition = input.wsPosition.xyz;
	half3 viewDirectionWS = SafeNormalize(GetCameraPositionWS() - wsPosition);

	// --- Shadows ---
	float _ReceiveShadows = 1;

 	if (_ReceiveShadows == 1) {

		#if SHADOWS_SCREEN
			//If shadows are in screen space then you can simply calculate the screen space or HClip of the current vertex and then you can get the shadow coordinate
			float4 clipPos = TransformWorldToHClip(input.positionWS);
			float4 shadowCoord = ComputeScreenPos(clipPos);
		#else
			 //If it isn't we have to take a different approach
			float4 shadowCoord = TransformWorldToShadowCoords(wsPosition);
		#endif
		
		//We can get the mainlight with the given shadowCoordinate and then it will give us properties such as the attenuation
		mainLight = GetMainLight(shadowCoord);

	 } else {
		 
		#ifdef _MAIN_LIGHT_SHADOWS
			mainLight = GetMainLight(input.shadowCoord);
		#else
			mainLight = GetMainLight();
		#endif

	}


	// --- Lighting ---

	float3 mainLightDirection = mainLight.direction;

	float lightingAllignment = dot(mainLightDirection, normal);
	float lightIntensity = ceil(lightingAllignment * amountOfColors) / amountOfColors;

	float fogFactor = input.wsPosition.w;

	float3 color = lerp(_DarkColor, _LightColor, lightIntensity);

	color = step(mainLight.shadowAttenuation, _ShadowStep)
		? _ShadowColor
		: color;

	//color = lerp(_ShadowColor, color, clamp(mainLight.shadowAttenuation + _ShadowStep, 0, 1));

	color = MixFog(color, fogFactor);


	// --- Alpha ---
	float alpha = _BaseMap.Sample(sampler_BaseMap, input.uv).a;
	clip(alpha - _AlphaCutoff);


	return half4(color, alpha);
}

#endif
