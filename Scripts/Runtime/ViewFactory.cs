using System;
using System.Collections.Generic;
using UnityEngine;

namespace EnvDev
{
    public abstract class ViewFactory : ScriptableObject
    {
        public abstract Type ModelType { get; }

        public abstract GameObject CreateView<T>(T model);
        public abstract GameObject CreateView<T>(T model, Transform parent);
        public abstract GameObject CreateView<T>(T model, Vector3 position, Quaternion rotation);
        public abstract GameObject CreateView<T>(T model, Vector3 position, Quaternion rotation, Transform parent);
    }

    public abstract class ViewFactory<TModel> : ViewFactory
    {
        public override Type ModelType => typeof(TModel);

        [SerializeField] List<string> m_Types = new List<string>();
        [SerializeField] List<GameObject> m_Prefabs = new List<GameObject>();

        readonly Dictionary<Type, GameObject> m_TypeToViewPrefabMap = new Dictionary<Type, GameObject>();

        void OnEnable()
        {
            m_TypeToViewPrefabMap.Clear();
            var count = m_Types.Count;
            for (var i = 0; i < count; i++)
            {
                var typeName = m_Types[i];
                var type = Type.GetType(typeName);
                if (type == null)
                {
                    Debug.LogError($"Failed to load type: {typeName}");
                    continue;
                }

                var data = m_Prefabs[i];
                if (!m_TypeToViewPrefabMap.ContainsKey(type))
                {
                    m_TypeToViewPrefabMap.Add(type, data);
                }
                else
                {
                    Debug.LogError($"Duplicate type: {type}");
                }
            }
        }

        public override GameObject CreateView<T>(T model)
        {
            var prefab = GetViewPrefab(model.GetType());
            return Instantiate(prefab).gameObject;
        }

        public override GameObject CreateView<T>(T model, Transform parent)
        {
            var prefab = GetViewPrefab(model.GetType());
            return Instantiate(prefab, parent);
        }

        public override GameObject CreateView<T>(T model, Vector3 position, Quaternion rotation)
        {
            var prefab = GetViewPrefab(model.GetType());
            return Instantiate(prefab, position, rotation);
        }

        public override GameObject CreateView<T>(T model, Vector3 position, Quaternion rotation, Transform parent)
        {
            var prefab = GetViewPrefab(model.GetType());
            return Instantiate(prefab, position, rotation, parent).gameObject;
        }

        GameObject GetViewPrefab(TModel key)
        {
            var type = key.GetType();
            return GetViewPrefab(type);
        }

        GameObject GetViewPrefab(Type type)
        {
            return m_TypeToViewPrefabMap[type];
        }
    }
}