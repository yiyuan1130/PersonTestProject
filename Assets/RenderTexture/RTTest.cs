using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RTTest : MonoBehaviour {

	RenderTexture renderTexture;
	public Image image;

	public Camera camera;

	void Start() {
		CommandBuffer buffer = new CommandBuffer();
		buffer.name = "buffer";
		camera.AddCommandBuffer(CameraEvent.BeforeImageEffects, buffer);
	}

	void Update(){
		renderTexture = camera.targetTexture;
		Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
		texture2D.Apply();
		Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, renderTexture.width, renderTexture.height), Vector2.zero);
		image.sprite = sprite;
		gameObject.tag = "xxx";
		gameObject.AddComponent<BoxCollider>().size = Vector3.one;
		// gameObject.GetComponent<MeshRenderer>().sharedMaterial = new Material("XXX");

	}
}