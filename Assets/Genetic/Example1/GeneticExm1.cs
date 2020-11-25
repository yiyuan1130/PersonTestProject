using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class GeneticExm1 : MonoBehaviour {
	void Awake(){
		LuaEnv env = new LuaEnv();
		string luaMainPath = Application.dataPath + "/" + "Genetic/Example1/Lua/main.lua";
		env.DoString(string.Format("dofile '{0}'", luaMainPath));
	}
}
