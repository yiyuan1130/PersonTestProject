using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SDFImage : Image {
	public Sprite _sprite;
	public Style _style = Style.PURE;
	public Texture _ramp;
	public Step _step = Step.ALL;
	public Color _color = Color.white;

	public Sprite sprite {
		get{
			return _sprite;
		}
		set{
			_sprite = value;
			this.material.SetTexture("_MainTex", _sprite.texture);
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
			this.material.SetTexture("_RampTex", _ramp);
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

	void Awake(){
#if UNITY_EDITOR
		Material mat = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/UnsignedDistanceField/sdf_mat_ui.mat", typeof(Material)) as Material;
		Material material = Object.Instantiate(mat) as Material;
		this.material = material;
		sprite = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/UnsignedDistanceField/udf/a_lower_sdf.png", typeof(Sprite)) as Sprite;
		ramp = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/UnsignedDistanceField/ramp.png", typeof(Texture)) as Texture;;
		style = Style.RIM;
		step = Step.ALL;
		color = Color.red;
#endif
	}
}
