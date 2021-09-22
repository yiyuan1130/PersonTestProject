using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStarCSSharp {
    public static class ActorManager
    {
        static List<ActorMove> actorMoveList;
        public static void Init()
        {
            actorMoveList = new List<ActorMove>();
        }
        public static void CreateActorMove(ActorMove actorMove)
        {
            actorMoveList.Add(actorMove);
        }

        public static void RemoveActorMove(ActorMove actorMove)
        {
            actorMoveList.Remove(actorMove);
        }

        public static List<ActorMove> GetActorMoveList()
        {
            return actorMoveList;
        }
    }
}
