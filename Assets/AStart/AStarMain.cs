using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using XLua;

namespace AStart{
    public class AStarMain : MonoBehaviour{
        void Awake(){
            LuaEnv env = new LuaEnv();
            string luaMainPath = Application.dataPath + "/" + "Lua/astar/astar.lua";
            env.DoString(string.Format("dofile '{0}'", luaMainPath));
            // env.DoString(string.Format("require 'astar/astar' "));
        }
    }
}