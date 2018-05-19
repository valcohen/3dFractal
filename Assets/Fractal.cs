using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {

    public Mesh mesh;
    public Material material;
    public int maxDepth     = 4;
    public float childScale = 0.5f;
    public float delay      = 0.5f;
    public Color startColor = Color.yellow;
    public Color endColor   = Color.red;

    private int depth;
    private Material[] materials;

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
        if (materials == null) {
            InitializeMaterials ();
        }
        gameObject.AddComponent<MeshFilter> ().mesh = mesh;
        gameObject.AddComponent<MeshRenderer> ().material = materials[depth];
        if (depth < maxDepth) {
            StartCoroutine (CreateChildren ());
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator CreateChildren () {
        for (int i = 0; i < childDirections.Length; i++) {
            yield return new WaitForSeconds (Random.Range(0.1f, 0.5f)); // delay
            new GameObject ("Fractal Child").AddComponent<Fractal> ()
                .Initialize(this, i);
        }
    }

    private void Initialize( Fractal parent, int childIndex) {
        mesh        = parent.mesh;
        materials   = parent.materials;
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

    private void InitializeMaterials() {
        materials = new Material[maxDepth + 1];
        for (int i = 0; i <= maxDepth; i++) {
            materials [i] = new Material (material);
            materials[i].color = Color.Lerp (startColor, endColor, (float)i / maxDepth);
        }
    }

}
