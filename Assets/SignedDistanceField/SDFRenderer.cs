using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SDFStyle{
	RAMP = 0,
	PURE = 1,
	EDGE = 2,
}
public enum SDFStep{
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
	public Color _color = Color.white;
	public SDFStyle _style = SDFStyle.PURE;
	public Texture _ramp;
	public SDFStep _step = SDFStep.ALL;
	public Material _material;

	private Material _meshMaterial;
	MeshRenderer _meshRenderer;
	public MeshRenderer meshRenderer {
		get {
			if (_meshRenderer == null)
				_meshRenderer = gameObject.GetComponent<MeshRenderer>();
			return _meshRenderer;
		}
	}

	public Sprite sprite {
		get{
			return _sprite;
		}
		set{
			_sprite = value;
			if (_sprite)
				material.SetTexture("_MainTex", _sprite.texture);
			else
				material.SetTexture("_MainTex", null);
		}
	}

	public SDFStyle style {
		get{
			return _style;
		}
		set{
			_style = value;
			material.SetInt("_Style", (int)_style);
		}
	}

	public Texture ramp{
		get{
			return _ramp;
		}
		set{
			_ramp = value;
			if (ramp)
				material.SetTexture("_RampTex", _ramp);
			else
				material.SetTexture("_RampTex", null);
		}
	}

	public SDFStep step {
		get{
			return _step;
		}
		set{
			_step = value;
			material.SetInt("_Step", (int)_step);
		}
	}

	public Color color{
		get{
			return _color;
		}
		set{
			_color = value;
			material.SetVector("_Color", color);
		}
	}

	public Material material {
		get{
            return _meshMaterial;
		}

        set{
			_material = value;
			if (_material){
				_meshMaterial = GameObject.Instantiate(_material);
				meshRenderer.material = _meshMaterial;
			}
			else{
				_material = null;
				meshRenderer.material = null;
			}
		}
	}
	void Awake() {
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
		meshFilter.mesh = GenMesh();
	}

	void Update(){
#if UNITY_EDITOR
		material = _material;
		if (material){
			style = _style;
			ramp = _ramp;
			step = _step;
			sprite = _sprite;
			color = _color;
		}
#endif
	}

	Mesh GenMesh(){
		float halfWidth = 1100.0f / 100 / 2;
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[]{
			new Vector3(-halfWidth, -halfWidth, 0),
			new Vector3(-halfWidth, halfWidth, 0),
			new Vector3(halfWidth, halfWidth, 0),
			new Vector3(halfWidth, -halfWidth, 0),
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
