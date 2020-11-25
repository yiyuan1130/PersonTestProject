namespace UnityEngine {
    
    [XLua.LuaCallCSharp]
    // [CreateAssetMenu(menuName = "Animation Curve", order = 400)]
    public class AnimationCurveAsset : ScriptableObject {
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

        public static implicit operator AnimationCurve(AnimationCurveAsset me)
        {
            return me.curve;
        }

        public static implicit operator AnimationCurveAsset(AnimationCurve curve)
        {
            AnimationCurveAsset asset = CreateInstance<AnimationCurveAsset>();
            asset.curve = curve;
            return asset;
        }
    }
}