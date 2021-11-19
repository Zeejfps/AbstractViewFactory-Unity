using System;
using System.Collections.Generic;
using System.Linq;

namespace _Project.InventorySystem.Scripts.Editor
{
    public static class ReflectionUtils
    {
        public static List<Type> FindAllTypesThatExtend(Type targetType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return (from assembly in assemblies 
                from type in assembly.GetTypes() 
                where type != targetType &&
                      !type.IsAbstract &&
                      !type.IsGenericType &&
                      targetType.IsAssignableFrom(type)
                select type).ToList();
        }
    }
}