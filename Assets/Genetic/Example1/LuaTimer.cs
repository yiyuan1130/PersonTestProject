using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class LuaTimer : MonoBehaviour {
    public void Excute(float n, LuaFunction luaFunction){
        StartCoroutine(delay(n, luaFunction));
    }
    IEnumerator delay(float n, LuaFunction luaFunction){
        yield return new WaitForSeconds(n);
        if (luaFunction != null){
            luaFunction.Call();
        }
    }
}
