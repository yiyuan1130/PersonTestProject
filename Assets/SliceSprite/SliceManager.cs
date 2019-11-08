using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Serialization;



namespace MiaoKids{
	[RequireComponent (typeof(SliceSprite))]
	public class SliceManager : MonoBehaviour {
		[Header("指定的材质球，名字：SliceSprite_Mesh")]
		public Material meshMaterial;

		[Header("偏移量：越大小孩子切割时候容错率越高，自动吸附到顶点")]
		[Range(0, 1)]
		public float offset = 0.05f;

		[Header("画的线宽度")]
		[Range(0, 1)]
		public float lineWidth = 0.01f;

		[Header("线的层级，如果不设置默认比Sprite大1")]
		public int orderInLayer = -1;

		[Header("指定的画线材质球，名字：SliceSprite_Line")]
		public Material lineMaterial;

		SliceSprite sliceSprite;

		void Awake(){
			sliceSprite = gameObject.GetComponent<SliceSprite>();
			sliceSprite.material = meshMaterial;
			sliceSprite.offset = offset;
			sliceSprite.lineWidth = lineWidth;
			sliceSprite.orderInLayer = orderInLayer;
			sliceSprite.lineMaterial = lineMaterial;
		}

#region 切割事件
		[SerializeField]
		public class StartSliceEvent : UnityEvent {}
		private StartSliceEvent m_StartSlice = new StartSliceEvent();
		public StartSliceEvent onStartSlice{
			get {return m_StartSlice;}
			set {m_StartSlice = value;}
		}

		[SerializeField]
		public class EndSliceEvent : UnityEvent<GameObject, GameObject, GameObject>{}
		private EndSliceEvent m_EndSlice = new EndSliceEvent();
		public EndSliceEvent onEndSlice{
			get {return m_EndSlice;}
			set {m_EndSlice = value;}
		}
#endregion
	}
}
