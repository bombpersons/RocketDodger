using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmokeTrail : MonoBehaviour {

	// Public classes
	public class TrailPoint {
		// Vars
		public float Speed = 0;
		public Vector2 Pos;
		public Vector2 Normal;
		public Vector2 Forwards;
		public Color Color;
		public float BackSpeed;
	}

	// Public vars
	public float DispersionRate;
	public float AlphaDecay;
	public float DropRate;
	public int MaxVerts;
	public Transform Follow;
	public float BackSpeed;
	public Vector2 OutDir;
	public Vector2 BackwardsDir;
	public Color BoostColor;
	public Color Color;
	public float StartWidth;
	public bool Enabled {
		get {
			return enabled;
		}
		set {
			enabled = value;
		}
	}
	public bool BoostEnabled = false;

	// Private vars
	MeshFilter filter;
	LinkedList<TrailPoint> points = new LinkedList<TrailPoint>();

	float cooldown;
	bool enabled = false;

	Vector3[] vertices;
	int[] indices;
	Vector2[] uvs;
	Color[] colors;

	// Use this for initialization
	void Start () {
		filter = GetComponent<MeshFilter>();
		filter.mesh = new Mesh();

		// Create the vertices array
		vertices = new Vector3[MaxVerts];
		indices = new int[MaxVerts];
		for (int i = 0; i < indices.Length; ++i) {
			indices[i] = i;
		}
		uvs = new Vector2[MaxVerts];
		for (int i = 0; i < uvs.Length; ++i) {
			uvs[i] = new Vector2(0, 0);
		}
		colors = new Color[MaxVerts];
		for (int i = 0; i < colors.Length; ++i) {
			colors[i] = Color.white;
		}
	}

	// Update is called once per frame
	void Update () {
		// If the follow died, then die.
		if (Follow == null) {
			// Disable it
			Enabled = false;
		} else {
			// Cooldown
			cooldown -= Time.deltaTime;
			if (cooldown < 0) {
				// Reset it.
				cooldown = DropRate;

				// Check if we have a rigid body.
				Vector2 normal = Vector2.zero;
				Vector2 outdir = OutDir;

				Rigidbody rigidBody = Follow.GetComponent<Rigidbody>();
				if (rigidBody) {
					normal = new Vector2(-rigidBody.velocity.z, rigidBody.velocity.x).normalized;
				} else {
					normal = new Vector2(-BackwardsDir.y, BackwardsDir.x).normalized;
					outdir = BackwardsDir;
				}

				// Drop down some points.
				TrailPoint left = new TrailPoint();
				left.Normal = normal;
				left.Forwards = outdir;
				left.Pos = new Vector2(Follow.position.x, Follow.position.z) + normal * StartWidth;
				left.Speed = DispersionRate + Random.Range(-0.5f, 0.5f) * DispersionRate;
				left.BackSpeed = BackSpeed;// + Follow.rigidbody.velocity.magnitude;
				TrailPoint right = new TrailPoint();
				right.Normal = normal;
				right.Forwards = outdir;
				right.Pos = new Vector2(Follow.position.x, Follow.position.z) - normal * StartWidth;
				right.Speed = -DispersionRate + Random.Range(-0.5f, 0.5f) * DispersionRate;
				right.BackSpeed = BackSpeed;// + Follow.rigidbody.velocity.magnitude;

				// Set the color
				if (BoostEnabled) {
					left.Color = BoostColor;
					right.Color = BoostColor;
				} else {
					left.Color = Color;
					right.Color = Color;
				}

				// Make them invisible if it is disabled.
				if (!Enabled) {
					left.Color *= 0;
					right.Color *= 0;
				}

				points.AddLast(right);
				points.AddLast(left);
			}
		}

		// If the point list is over the maximimum.
		while (points.Count >= MaxVerts) {
			points.RemoveFirst();
		}

		// Update the points.
		int count = 0;
		bool allAlpha = true;
		foreach (TrailPoint p in points) {
			p.Pos += p.Normal * p.Speed * Time.deltaTime;
			p.Pos -= p.Forwards * p.BackSpeed * Time.deltaTime;
			p.Color.a -= p.Color.a * AlphaDecay * Time.deltaTime;
			vertices[count] = new Vector3(p.Pos.x, 0, p.Pos.y);
			colors[count] = p.Color;
			++count;

			if (p.Color.a > 0.01f)
				allAlpha = false;
		}
		if (allAlpha && Follow == null)
			Destroy(gameObject);

		// Create the mesh.
		filter.mesh.vertices = vertices;
		filter.mesh.uv = uvs;
		filter.mesh.colors = colors;

		// Generate indices
		indices = new int[(points.Count - 2) * 3];
		for (int i = 0; i < points.Count - 2; ++i) {
			if (i % 2 == 0) {
				indices[i*3 + 0] = i;
				indices[i*3 + 1] = i + 1;
				indices[i*3 + 2] = i + 2;
			} else {
				indices[i*3 + 0] = i + 2;
				indices[i*3 + 1] = i + 1;
				indices[i*3 + 2] = i;
			}
		}
		//filter.mesh.SetTriangleStrip(indices, 0);
		// Temp
		filter.mesh.SetTriangles(indices, 0);
		//filter.mesh.RecalculateNormals();

		// Set the mesh bounds
		filter.mesh.RecalculateBounds();
	}
}
