using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {

    public Mesh mesh;
    public Material material;
    public int maxDepth     = 4;
    public float childScale = 0.5f;
    public float delay      = 0.5f;

    private int depth;

	void Start () {
        gameObject.AddComponent<MeshFilter> ().mesh = mesh;
        gameObject.AddComponent<MeshRenderer> ().material = material;
        if (depth < maxDepth) {
            StartCoroutine (CreateChildren ());
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator CreateChildren () {
        yield return new WaitForSeconds (delay);
        new GameObject ("Fractal Child").AddComponent<Fractal> ()
            .Initialize(this, Vector3.up, Quaternion.identity);
        yield return new WaitForSeconds (delay);
        new GameObject ("Fractal Child").AddComponent<Fractal> ()
            .Initialize(this, Vector3.right, Quaternion.Euler(0, 0, -90));
        yield return new WaitForSeconds (delay);
        new GameObject ("Fractal Child").AddComponent<Fractal> ()
            .Initialize(this, Vector3.left, Quaternion.Euler(0, 0, 90));
    }


    private void Initialize( Fractal parent, Vector3 direction, Quaternion orientation) {
        mesh        = parent.mesh;
        material    = parent.material;
        maxDepth    = parent.maxDepth;
        depth       = parent.depth + 1;
        childScale  = parent.childScale;

        transform.parent        = parent.transform;
        transform.localScale    = Vector3.one * childScale;
        transform.localPosition = direction * (0.5f + 0.5f * childScale);
        transform.localRotation = orientation;
    }

}
