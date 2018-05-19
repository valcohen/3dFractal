using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {

    public Mesh mesh;
    public Material material;

	void Start () {
        gameObject.AddComponent<MeshFilter> ().mesh = mesh;
        gameObject.AddComponent<MeshRenderer> ().material = material;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
