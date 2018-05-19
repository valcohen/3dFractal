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
    public Color tipColor   = Color.green;
    public bool colorTips   = false;
    public ColorRangeName colorRange;

    public float maxRotationSpeed = 30f;
    public bool randomizeSpeed = false;

    private int depth;
    private Material[,] materials;
    private float rotationSpeed;

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
        rotationSpeed = (randomizeSpeed) 
                      ? Random.Range (-maxRotationSpeed, maxRotationSpeed)
                      : maxRotationSpeed;
        gameObject.AddComponent<MeshFilter> ().mesh = mesh;
        gameObject.AddComponent<MeshRenderer> ().material = materials[depth, (int)colorRange];
        if (depth < maxDepth) {
            StartCoroutine (CreateChildren ());
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate (0, rotationSpeed * Time.deltaTime, 0);
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
        maxRotationSpeed = parent.maxRotationSpeed;

        transform.parent        = parent.transform;
        transform.localScale    = Vector3.one * childScale;
        // offset by half the size of the parent + half the size of the child
        transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
        transform.localRotation = childOrientations[childIndex];
    }

    private void InitializeMaterials() {
        materials = new Material[maxDepth + 1, 2];
        for (int i = 0; i <= maxDepth; i++) {
            float t = i / (maxDepth - 1f);
            t *= t;

            materials [i, 0] = new Material (material);
            materials [i, 0].color = Color.Lerp (startColor, endColor, (float)i / maxDepth);

            materials [i, 1] = new Material (material);
            materials [i, 1].color = Color.Lerp (startColor, endColor, t);
        }
        if (colorTips) {
            materials [maxDepth, 0].color = tipColor;
            materials [maxDepth, 1].color = tipColor;
        }
    }

    public enum ColorRangeName {
        Linear,
        Squared
    }

}
