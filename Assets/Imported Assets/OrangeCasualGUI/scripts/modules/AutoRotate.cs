using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour {

    public float speed = 0.1f;

    private float angle = 0;
    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(0f, 0f, Time.deltaTime * speed);
	}
}
