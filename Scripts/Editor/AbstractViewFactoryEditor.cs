using System;
using System.Collections.Generic;
using System.Linq;
using _Project.InventorySystem.Scripts.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractViewFactory), true)]
public class AbstractViewFactoryEditor : Editor
{
   const string k_TypesPropertyName = "m_Types";
   const string k_PrefabsPropertyName = "m_Prefabs";

   List<Type> m_Types;
   SerializedProperty m_TypesProperty;
   SerializedProperty m_PrefabsProperty;

   void OnEnable()
   {
      var dataTable = (AbstractViewFactory)target;
      m_TypesProperty = serializedObject.FindProperty(k_TypesPropertyName);
      m_PrefabsProperty = serializedObject.FindProperty(k_PrefabsPropertyName);
      m_Types = ReflectionUtils.FindAllTypesThatExtend(dataTable.ModelType);
      
      serializedObject.Update();

      var count = m_Types.Count;
      for (var i = 0; i < count; i++)
      {
         var type = m_Types[i];
         var typeName = type.AssemblyQualifiedName;
         
         var index = IndexOf(m_TypesProperty, typeName);
         if (index < 0)
         {
            m_TypesProperty.InsertArrayElementAtIndex(i);
            m_TypesProperty.GetArrayElementAtIndex(i).stringValue = typeName;
            m_PrefabsProperty.InsertArrayElementAtIndex(i);
         }
         else if (index != i)
         {
            m_TypesProperty.SwapArrayElements(i, index);
            m_PrefabsProperty.SwapArrayElements(i, index);
         }
      }

      // Trim the arrays to match
      m_TypesProperty.arraySize = count;
      m_PrefabsProperty.arraySize = count;
      
      serializedObject.ApplyModifiedPropertiesWithoutUndo();
   }

   public override void OnInspectorGUI()
   {
      using (new EditorGUI.DisabledScope(true))
         EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"), true);

      serializedObject.Update();
      
      var count = m_Types.Count;
      for (var i = 0; i < count; i++)
      {
         var type = m_Types[i];
         var label = type.Name;
         var viewDataProperty = m_PrefabsProperty.GetArrayElementAtIndex(i);
         EditorGUILayout.PropertyField(viewDataProperty, new GUIContent(label), true);
      }

      serializedObject.ApplyModifiedProperties();
   }

   int IndexOf(SerializedProperty array, string typeName)
   {
      var size = array.arraySize;
      for (var i = 0; i < size; i++)
      {
         var element = array.GetArrayElementAtIndex(i);
         if (element.stringValue == typeName)
            return i;
      }
      return -1;
   }
}
