using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoRender : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		Rigidbody2D ri = GetComponent<Rigidbody2D>();
		ri.AddForce(Vector2.left, ForceMode2D.Impulse);
	}
	public bool startBilt = false;
	public Material material;
	public Texture rt;
	void OnRenderImage(RenderTexture src, RenderTexture dest){
		if (startBilt){
			Graphics.Blit(rt, dest, material);
		}
		else{
			Graphics.Blit(src, dest);
		}
	}
}
