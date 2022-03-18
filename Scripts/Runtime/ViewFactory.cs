using System;
using System.Collections.Generic;
using UnityEngine;

namespace EnvDev
{
    public abstract class ViewFactory : ScriptableObject
    {
        public abstract Type ModelType { get; }

        public abstract GameObject CreateView<T>();
        public abstract GameObject CreateView<T>(Transform parent);
        public abstract GameObject CreateView<T>(Vector3 position, Quaternion rotation);
        public abstract GameObject CreateView<T>(Vector3 position, Quaternion rotation, Transform parent);
    }

    public abstract class ViewFactory<TModel, TView> : ViewFactory
        where TView : MonoBehaviour, IView<TModel>
    {
        public override Type ModelType => typeof(TModel);

        [SerializeField] List<string> m_Types = new List<string>();
        [SerializeField] List<TView> m_Prefabs = new List<TView>();

        readonly Dictionary<Type, TView> m_TypeToViewPrefabMap = new Dictionary<Type, TView>();

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

        public override GameObject CreateView<T>()
        {
            var prefab = GetViewPrefab(typeof(T));
            return Instantiate(prefab).gameObject;
        }

        public override GameObject CreateView<T>(Transform parent)
        {
            var prefab = GetViewPrefab(typeof(T));
            return Instantiate(prefab, parent).gameObject;
        }

        public override GameObject CreateView<T>(Vector3 position, Quaternion rotation)
        {
            var prefab = GetViewPrefab(typeof(T));
            return Instantiate(prefab, position, rotation).gameObject;
        }

        public override GameObject CreateView<T>(Vector3 position, Quaternion rotation, Transform parent)
        {
            var prefab = GetViewPrefab(typeof(T));
            return Instantiate(prefab, position, rotation, parent).gameObject;
        }

        public TView CreateView(TModel item)
        {
            var prefab = GetViewPrefab(item);
            var view = Instantiate(prefab);
            view.Init(item);
            return view;
        }

        public TView CreateView(TModel item, Transform parent)
        {
            var prefab = GetViewPrefab(item);
            var view = Instantiate(prefab, parent);
            view.Init(item);
            return view;
        }

        public TView CreateView(TModel item, Vector3 position, Quaternion rotation)
        {
            var prefab = GetViewPrefab(item);
            var view = Instantiate(prefab, position, rotation);
            view.Init(item);
            return view;
        }

        public TView CreateView(TModel item, Vector3 position, Quaternion rotation, Transform parent)
        {
            var prefab = GetViewPrefab(item);
            var view = Instantiate(prefab, position, rotation, parent);
            view.Init(item);
            return view;
        }

        TView GetViewPrefab(TModel key)
        {
            var type = key.GetType();
            return GetViewPrefab(type);
        }

        TView GetViewPrefab(Type type)
        {
            return m_TypeToViewPrefabMap[type];
        }
    }
}