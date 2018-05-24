using UnityEngine;

public static class MaterialExtension
{
	public static void EnableEmission(this Material material)
	{
		material.EnableKeyword("_EMISSION"); 
	}

	public static void DisableEmission(this Material material)
	{
		material.DisableKeyword("_EMISSION"); 
	}

	public static Color GetEmissionRate(this Material material)
	{
		return material.GetColor("_EmissionColor");
	}

	public static void SetEmissionRate(this Material material, float emissionRate)
	{
		material.SetColor("_EmissionColor", material.color*emissionRate);
	}
}