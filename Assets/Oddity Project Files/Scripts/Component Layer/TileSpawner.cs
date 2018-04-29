using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour {

	public GameObject[] Tiles;

	private Queue<GameObject> availableTile = new Queue<GameObject>();

	public static TileSpawner intance {get; private set;}

	public float delay = .76f;
	private float lastTime;

	void Awake() 
	{
		intance = this;
		GrowPool();
	}

	void Update()
	{
		if(Time.time - lastTime > delay)
		{
			SpawnTile();
		}
		
	}

	private void SpawnTile()
	{
		lastTime = Time.time;
		delay = UnityEngine.Random.Range(0.35f,1f);
		var tile = GetFromPool();
		tile.transform.position = this.transform.position;
		tile.transform.position = new Vector3(transform.position.x + UnityEngine.Random.Range(-2f, 2f), transform.position.y, transform.position.z);
		tile.GetComponent<Rigidbody2D>().gravityScale = .2f;
		tile.GetComponent<NoGravity>().maxRotationAngleY = 3;
		tile.GetComponent<NoGravity>().maxRotationAngleZ = 3;
		tile.GetComponent<NoGravity>().YForce = 3;
		tile.GetComponent<NoGravity>().ZForce =2;
		tile.GetComponent<BoxCollider2D>().enabled = false;
		StartCoroutine(AddToPoolDelay(tile,3.5f));
		
	}

	IEnumerator AddToPoolDelay(GameObject tile, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		// Now do your thing here
		AddToPool(tile);
	}

	private GameObject GetFromPool()
	{

		if(availableTile.Count == 0)
			GrowPool();

		var instance = availableTile.Dequeue();
		instance.SetActive(true);
		return instance;

	}

	private void GrowPool(){
		for(int i=0; i<10; i++)
		{
			GameObject instanceToAdd = Instantiate(Tiles[UnityEngine.Random.Range(0,Tiles.Length)]);
			AddToPool(instanceToAdd);
			instanceToAdd.transform.position = this.transform.position;
		
		}
	}
	

	public void AddToPool(GameObject instance)
	{
		instance.SetActive(false);
		availableTile.Enqueue(instance);

	} 
}
