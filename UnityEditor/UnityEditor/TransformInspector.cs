using System;
using UnityEngine;

namespace UnityEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(Transform))]
	internal class TransformInspector : Editor
	{
		private class Contents
		{
			public GUIContent positionContent = EditorGUIUtility.TrTextContent("Position", "The local position of this GameObject relative to the parent.", null);

			public GUIContent scaleContent = EditorGUIUtility.TrTextContent("Scale", "The local scaling of this GameObject relative to the parent.", null);

			public string floatingPointWarning = LocalizationDatabase.GetLocalizedString("Due to floating-point precision limitations, it is recommended to bring the world coordinates of the GameObject within a smaller range.");
		}

		private SerializedProperty m_Position;

		private SerializedProperty m_Scale;

		private TransformRotationGUI m_RotationGUI;

		private static TransformInspector.Contents s_Contents;

		public void OnEnable()
		{
			this.m_Position = base.serializedObject.FindProperty("m_LocalPosition");
			this.m_Scale = base.serializedObject.FindProperty("m_LocalScale");
			if (this.m_RotationGUI == null)
			{
				this.m_RotationGUI = new TransformRotationGUI();
			}
			this.m_RotationGUI.OnEnable(base.serializedObject.FindProperty("m_LocalRotation"), EditorGUIUtility.TrTextContent("Rotation", "The local rotation of this GameObject relative to the parent.", null));
		}

		public override void OnInspectorGUI()
		{
			if (TransformInspector.s_Contents == null)
			{
				TransformInspector.s_Contents = new TransformInspector.Contents();
			}
			if (!EditorGUIUtility.wideMode)
			{
				EditorGUIUtility.wideMode = true;
				EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 212f;
			}
			base.serializedObject.Update();
			this.Inspector3D();
			Transform transform = base.target as Transform;
			Vector3 position = transform.position;
			if (Mathf.Abs(position.x) > 100000f || Mathf.Abs(position.y) > 100000f || Mathf.Abs(position.z) > 100000f)
			{
				EditorGUILayout.HelpBox(TransformInspector.s_Contents.floatingPointWarning, MessageType.Warning);
			}
			base.serializedObject.ApplyModifiedProperties();
		}

		private void Inspector3D()
		{
			EditorGUILayout.PropertyField(this.m_Position, TransformInspector.s_Contents.positionContent, new GUILayoutOption[0]);
			this.m_RotationGUI.RotationField();
			EditorGUILayout.PropertyField(this.m_Scale, TransformInspector.s_Contents.scaleContent, new GUILayoutOption[0]);
		}
	}
}
