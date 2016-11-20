#define MAXLIGHTS 1

#define DIRECTION 0
#define POINT 1
#define SPOT 2

int LightsCount = 1;

float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;


//Light source properties
float3 LightColor[MAXLIGHTS];
//


float3 LightDirection[MAXLIGHTS];

float3 LightPosition[MAXLIGHTS];
float LightAttenuation[MAXLIGHTS];
float LightFalloff[MAXLIGHTS];

float LightConeAngle[MAXLIGHTS];

int LightType[MAXLIGHTS];

//Material properties
float3 DiffuseColor = float3(1, 1, 1);
float Shininess = 32;
float3 SpecularColor = float3(1, 1, 1);
float3 AmbientColor = float3(0.1, 0.1, 0.1);

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float3 Normal : TEXCOORD0;
	float3 ViewDirection : TEXCOORD1;
	float3 WorldPosition : TEXCOORD2;
};

//Clipping plane
float Side;

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	float4 worldPosition = mul(input.Position, World);
	float4x4 viewProjection = mul(View, Projection);

	output.Position = mul(worldPosition, viewProjection);
	output.Normal = mul(input.Normal, World);
	output.ViewDirection = worldPosition - CameraPosition;
	output.WorldPosition = worldPosition;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	
	// Start with ambient lighting
	float3 lighting = AmbientColor;

	float3 normal = normalize(input.Normal);
	float3 view = normalize(input.ViewDirection);

	for (int i = 0; i < LightsCount; i++)
	{
		if (LightType[i] == DIRECTION)
		{
			//Direction light diffuse
			float3 lightDir = normalize(LightDirection[i]);
			float diffuse = saturate(dot(normal, lightDir));
			lighting += LightColor[i] * DiffuseColor * diffuse;

			//Direction light specular
			float3 refl = reflect(lightDir, normal);
			float specular = saturate(pow(dot(refl, view), Shininess));
			lighting += LightColor[i] * SpecularColor * specular;
		}
		else if (LightType[i] == POINT)
		{
			float att = 1 - pow(clamp(distance(LightPosition[i], input.WorldPosition) / LightAttenuation[i], 0, 1), LightFalloff[i]);

			//Point light diffuse
			float3 lightDir = normalize(LightPosition[i] - input.WorldPosition);
			float diffuse = saturate(dot(normal, lightDir));
			lighting += LightColor[i] * DiffuseColor * diffuse * att;

			//Point light specular
			float3 refl = reflect(lightDir, normal);
			float specular = saturate(pow(dot(refl, view), Shininess));
			lighting += LightColor[i] * SpecularColor * specular * att;
		}
		else if (LightType[i] == SPOT)
		{
			//Spot light diffuse
			float3 lightDir = normalize(LightPosition[i] - input.WorldPosition);
			float diffuse = saturate(dot(normal, lightDir));

			float d = dot(-lightDir, normalize(LightDirection[i]));
			float a = cos(LightConeAngle[i]);
			float att = a < d ? 1 - pow(a / d, LightFalloff[i]) : 0;

			lighting += DiffuseColor * diffuse * att;
			
			//Spot light specular
			float3 refl = reflect(lightDir, normal);
			float specular = saturate(pow(dot(refl, view), Shininess));
			lighting += SpecularColor * specular * att;
		}
	}

	return float4(saturate(lighting), 1);
}

technique Technique1
{
	pass Pass1
	{
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;

		VertexShader = compile vs_4_0 VertexShaderFunction();
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}