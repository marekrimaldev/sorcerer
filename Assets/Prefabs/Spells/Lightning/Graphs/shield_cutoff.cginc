void ComputeShieldCutoff_float (float3 shieldDir, float3 normal, float inner, float outer, out float Out) 
{
	float angle = clamp(dot(shieldDir, normal), 0, 1);
	if (angle > outer)
	{
		Out = 1;

		if (angle < inner)
		{
			float cuttOffRange = inner - outer;
			float intensity = clamp(pow((angle - outer), 2) / cuttOffRange, 0.0, 1.0);
			Out = intensity;
		}
	}
	else
	{
		Out = 0;
	}

}
