using System;
using System.Collections.Generic;
using UnityEngine;

namespace EnvDev
{
    public abstract class AbstractViewFactory : ScriptableObject
    {
        public abstract Type ModelType { get; }
    }

    public abstract class AbstractViewFactory<TModel, TView> : AbstractViewFactory
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
                try
                {
                    m_TypeToViewPrefabMap.Add(type, data);
                }
                catch (ArgumentException _)
                {
                    Debug.LogError($"Duplicate type: {type}");
                }
            }
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