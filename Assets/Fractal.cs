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

    private static readonly Vector3[] childDirections = {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };

    private static readonly Quaternion[] childOrientations = {
        Quaternion.identity,
        Quaternion.Euler (  0, 0, -90),
        Quaternion.Euler (  0, 0,  90),
        Quaternion.Euler ( 90, 0,   0),
        Quaternion.Euler (-90, 0,   0)
    };

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
        for (int i = 0; i < childDirections.Length; i++) {
            yield return new WaitForSeconds (delay);
            new GameObject ("Fractal Child").AddComponent<Fractal> ()
                .Initialize(this, i);
        }
    }


    private void Initialize( Fractal parent, int childIndex) {
        mesh        = parent.mesh;
        material    = parent.material;
        maxDepth    = parent.maxDepth;
        depth       = parent.depth + 1;
        childScale  = parent.childScale;

        transform.parent        = parent.transform;
        transform.localScale    = Vector3.one * childScale;
        // offset by half the size of the parent + half the size of the child
        transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
        transform.localRotation = childOrientations[childIndex];
    }

}
