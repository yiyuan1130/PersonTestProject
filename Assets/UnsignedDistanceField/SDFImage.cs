using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent (typeof(Image))]
public class SDFImage : MonoBehaviour {
	public Sprite _sprite;
	public Color _color = Color.white;
	public SDFStyle _style = SDFStyle.PURE;
	public Texture _ramp;
	public SDFStep _step = SDFStep.ALL;
	public Material _material;

	private Material _imageMaterial;
	Image _image;
	public Image image {
		get {
			if (_image == null)
				_image = gameObject.GetComponent<Image>();
			return _image;
		}
	}

	public Sprite sprite {
		get{
			return _sprite;
		}
		set{
			_sprite = value;
			if (_sprite){
                _image.sprite = _sprite;
				material.SetTexture("_MainTex", _sprite.texture);
            }
            else{
                _image.sprite = null;
				material.SetTexture("_MainTex", null);
            }
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
            _image.color = _color;
		}
	}

	public Material material {
		get{
            return _imageMaterial;
		}

        set{
			_material = value;
			if (_material){
				_imageMaterial = GameObject.Instantiate(_material);
				image.material = _imageMaterial;
			}else{
                image.material = null;
            }
		}
	}
	void Awake() {
		_image = GetComponent<Image>();
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
}
