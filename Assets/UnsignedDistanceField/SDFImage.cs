using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [SerializeField]
// [ExecuteInEditMode]
public class SDFImage : Image {
	public Sprite _sprite;
	public Style _style = Style.PURE;
	public Texture _ramp;
	public Step _step = Step.ALL;
	public Color _color = Color.white;

	public new Sprite sprite {
		get{
			return _sprite;
		}
		set{
			_sprite = value;
			this.SetTexture("_MainTex", _sprite.texture);
		}
	}

	public Style style {
		get{
			return _style;
		}
		set{
			_style = value;
			this.material.SetInt("_Style", (int)_style);
		}
	}

	public Texture ramp{
		get{
			return _ramp;
		}
		set{
			_ramp = value;
			this.SetTexture("_RampTex", _ramp);
		}
	}

	public Step step {
		get{
			return _step;
		}
		set{
			_step = value;
			this.material.SetInt("_Step", (int)_step);
		}
	}

	public Color color{
		get{
			return _color;
		}
		set{
			_color = value;
			this.material.SetVector("_Color", color);
		}
	}

	public void SetTexture(string name, Texture texture){
		this.material.SetTexture(name, texture);
		base.UpdateMaterial();
	}

	override protected void Reset(){
        base.Reset();
#if UNITY_EDITOR
		if (Application.isPlaying)
			return;
		// Material mat = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/UnSignedDistanceField/sdf_mat_ui.mat", typeof(Material)) as Material;
		// Material material = Object.Instantiate(mat) as Material;
		// this.material = material;
		// sprite = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/unSignedDistanceField/udf/a_lower_sdf.png", typeof(Sprite)) as Sprite;
		// ramp = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/unSignedDistanceField/ramp.png", typeof(Texture)) as Texture;;
		// style = Style.RAMP;
		// step = Step.ALL;
		// color = Color.red;
        Shader shader = Shader.Find("iHuman/SDF/ui");
        Material mat = new Material(shader);
        this.material = mat;
        sprite = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/unSignedDistanceField/udf/a_lower_sdf.png", typeof(Sprite)) as Sprite;
		ramp = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/unSignedDistanceField/ramp.png", typeof(Texture)) as Texture;;
		style = Style.RAMP;
		step = Step.ALL;
		color = Color.red;
#endif
	}
}
