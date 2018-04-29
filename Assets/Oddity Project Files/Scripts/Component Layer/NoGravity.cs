using UnityEngine;
using System.Collections;

public class NoGravity : MonoBehaviour {

	public float maxRotationAngleY;
	public float maxRotationAngleZ;

	public float YForce;
	public float ZForce;

	public bool switchFlagZ;
	public bool switchFlagY;


	void Update()
	{

		if(switchFlagZ)
		{
			transform.Rotate(Vector3.forward * Time.fixedDeltaTime * -(75*ZForce));

		} else 
		{
			transform.Rotate(Vector3.forward * Time.fixedDeltaTime * (75*ZForce));
		}


		if(transform.rotation.z >= maxRotationAngleZ || transform.rotation.z <= -maxRotationAngleZ)
		{
			switchFlagZ = !switchFlagZ;
		}


		if(switchFlagY)
		{
			transform.Rotate(Vector3.up * Time.fixedDeltaTime * -(75*YForce));

		} else 
		{
			transform.Rotate(Vector3.up * Time.fixedDeltaTime * (75*YForce));
		}


		if(transform.rotation.y >= maxRotationAngleY || transform.rotation.y <= -maxRotationAngleY)
		{
			switchFlagY = !switchFlagY;
		}
		
	}


	public void PlacedTile()
	{
		this.maxRotationAngleY = 0.02f;
		this.maxRotationAngleZ = 0.02f;
		
	}
	
    
}


