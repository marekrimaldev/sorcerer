#ifndef PROCEDURAL_PLANETS
#define PROCEDURAL_PLANETS

void Add_float(float3 A, float B, float3 Out)
{
	Out = A + B;
}

void ExtrudeAlongNormal_float(float3 position, float3 normal, float distance, float3 Out) 
{
	//Out = position + normal * distance;
	Out = float3(1, 1, 1); 
}

#endif