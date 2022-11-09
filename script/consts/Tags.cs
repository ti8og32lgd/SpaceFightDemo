using System;
using System.Collections.Generic;

namespace script.consts
{
    public static class Tags
    {
        public const string PlayerRocket="PlayerRocket";
        public const string Player = "Player";
        public const string Bound = "bound";
        public const string Enemy = "Enemy";
        public const string Bullet = "Bullet";
        public const string Wingman = "Wingman";
        public const string Shield = "Shield";

        public const string Prop = "Prop";
    }

    
    [Serializable]
    public enum EntityType
    {
        Player,
        Enemy,
        Bullet,
        Wingman,
        Prop,
    }

    public static class EntityManager
    {

        private static Dictionary<EntityType, string> _map = new()
        {
            { EntityType.Player,Tags.Player},
            { EntityType.Enemy,Tags.Enemy},
            { EntityType.Bullet,Tags.Bullet},
            { EntityType.Wingman,Tags.Wingman},
            {EntityType.Prop,Tags.Prop},
        };
        public static  string GetEntityTag(this EntityType type)
        {
            if (_map.ContainsKey(type))
            {
                return _map[type];
            }
            throw new  MissingMemberException($"{type} has no corresponding tag");
        }

        public static bool IsOppositeEntity(this EntityType t1, EntityType t2)
        {
            if (t1 is EntityType.Player or EntityType.Wingman)
            {
                return t2 == EntityType.Enemy;
            }

            if (t1 == EntityType.Enemy)
            {
                return t2 is EntityType.Player or EntityType.Wingman;
            }

            return false;
        }
     
    }
}