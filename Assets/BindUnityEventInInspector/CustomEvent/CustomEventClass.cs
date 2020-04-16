using UnityEngine.Events;
using UnityEngine;

public class CustomEventClass : MonoBehaviour{
    public StartMoveEvent startMoveEvent;
    public OnMoveEvent onMoveEvent;
    public EndMoveEvent endMoveEvent;
}

// 开始移动事件
public class StartMoveEvent : UnityEvent<GameObject, string> {}

// 移动中事件
public class OnMoveEvent : UnityEvent<GameObject, string> {}

// 移动结束事件
public class EndMoveEvent : UnityEvent<GameObject, string> {}



