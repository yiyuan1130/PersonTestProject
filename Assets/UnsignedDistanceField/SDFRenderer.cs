using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Style{
	RIM = 0,
	PURE = 1,
	Edge = 2,
}
public enum Step{
	FIRST = 1,
	SECOND = 2,
	THIRD = 3,
	ALL = 4,
}
[ExecuteInEditMode]
[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshFilter))]
public class SDFRenderer : MonoBehaviour {
	public Sprite _sprite;
	public Style _style = Style.PURE;
	public Texture _ramp;
	public Step _step = Step.ALL;
	public Color _color = Color.white;

	[HideInInspector]
	public MeshRenderer meshRenderer;

	public Sprite sprite {
		get{
			return _sprite;
		}
		set{
			_sprite = value;
			meshRenderer.sharedMaterial.SetTexture("_MainTex", _sprite.texture);
		}
	}

	public Style style {
		get{
			return _style;
		}
		set{
			_style = value;
			meshRenderer.sharedMaterial.SetInt("_Style", (int)_style);
		}
	}

	public Texture ramp{
		get{
			return _ramp;
		}
		set{
			_ramp = value;
			meshRenderer.sharedMaterial.SetTexture("_RampTex", _ramp);
		}
	}

	public Step step {
		get{
			return _step;
		}
		set{
			_step = value;
			meshRenderer.sharedMaterial.SetInt("_Step", (int)_step);
		}
	}

	public Color color{
		get{
			return _color;
		}
		set{
			_color = value;
			meshRenderer.sharedMaterial.SetVector("_Color", color);
		}
	}

	void Awake(){
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
		meshRenderer = gameObject.GetComponent<MeshRenderer>();
		meshFilter.mesh = GenMesh();

#if UNITY_EDITOR
		Material mat = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/UnsignedDistanceField/sdf_mat.mat", typeof(Material)) as Material;
		Material material = Object.Instantiate(mat) as Material;
		meshRenderer.sharedMaterial = material;
		sprite = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/UnsignedDistanceField/udf/a_lower_sdf.png", typeof(Sprite)) as Sprite;
		ramp = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/UnsignedDistanceField/ramp.png", typeof(Texture)) as Texture;;
		style = Style.RIM;
		step = Step.ALL;
		color = Color.red;
#endif
	}

	Mesh GenMesh(){
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[]{
			new Vector3(-0.5f, -0.5f, 0),
			new Vector3(-0.5f, 0.5f, 0),
			new Vector3(0.5f, 0.5f, 0),
			new Vector3(0.5f, -0.5f, 0),
		};
		mesh.triangles = new int[]{
			0, 1, 2,
			0, 2, 3
		};
		mesh.normals = new Vector3[]{
			new Vector3(0, 0, -1),
			new Vector3(0, 0, -1),
			new Vector3(0, 0, -1),
			new Vector3(0, 0, -1),
		};
		mesh.uv = new Vector2[]{
			new Vector2(0, 0),
			new Vector2(0, 1),
			new Vector2(1, 1),
			new Vector2(1, 0),
		};
		return mesh;
	}
}
