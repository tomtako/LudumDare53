using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects, CustomEditor(typeof(Transform))]
public class TransformInspector : Editor
{
    private const float PIXEL_PER_UNIT = 32;
    private const float PIXEL_WORLD_SIZE = 1F / PIXEL_PER_UNIT;
    private const float FIELD_WIDTH = 212.0f;
    private const bool WIDE_MODE = true;

    private const float POSITION_MAX = 100000.0f;

    private static GUIContent positionGUIContent = new GUIContent(LocalString("Position")
                                                                 , LocalString("The local position of this Game Object relative to the parent."));
    private static GUIContent rotationGUIContent = new GUIContent(LocalString("Rotation")
                                                                 , LocalString("The local rotation of this Game Object relative to the parent."));
    private static GUIContent scaleGUIContent = new GUIContent(LocalString("Scale")
                                                                 , LocalString("The local scaling of this Game Object relative to the parent."));

    private static string positionWarningText = LocalString("Due to floating-point precision limitations, it is recommended to bring the world coordinates of the GameObject within a smaller range.");

    private SerializedProperty positionProperty;
    private SerializedProperty rotationProperty;
    private SerializedProperty scaleProperty;

    private static string LocalString(string text)
    {
        return text;
    }

    public void OnEnable()
    {
        positionProperty = serializedObject.FindProperty("m_LocalPosition");
        rotationProperty = serializedObject.FindProperty("m_LocalRotation");
        scaleProperty = serializedObject.FindProperty("m_LocalScale");
    }

    public override void OnInspectorGUI()
    {
        EditorGUIUtility.wideMode = WIDE_MODE;
        //EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - FIELD_WIDTH;

        serializedObject.Update();

        EditorGUILayout.PropertyField(positionProperty, positionGUIContent);
        RotationPropertyField(rotationProperty, rotationGUIContent);
        EditorGUILayout.PropertyField(scaleProperty, scaleGUIContent);

        if (!ValidatePosition(((Transform)target).position))
        {
            EditorGUILayout.HelpBox(positionWarningText, MessageType.Warning);
        }

        char rightArrow = '\u25B6';
        char leftArrow = '\u25C0';
        char upArrow = '\u25B2';
        char downArrow = '\u25BC';

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button(leftArrow.ToString()))
        {
            Vector3 v = positionProperty.vector3Value;
            v.x -= PIXEL_WORLD_SIZE;
            positionProperty.vector3Value = v;
        }

        if (GUILayout.Button(rightArrow.ToString()))
        {
            Vector3 v = positionProperty.vector3Value;
            v.x += PIXEL_WORLD_SIZE;
            positionProperty.vector3Value = v;
        }
        if (GUILayout.Button(upArrow.ToString()))
        {
            Vector3 v = positionProperty.vector3Value;
            v.y += PIXEL_WORLD_SIZE;
            positionProperty.vector3Value = v;
        }
        if (GUILayout.Button(downArrow.ToString()))
        {
            Vector3 v = positionProperty.vector3Value;
            v.y -= PIXEL_WORLD_SIZE;
            positionProperty.vector3Value = v;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Floor 1"))
        {
            Vector3 v = positionProperty.vector3Value;
            v.y = -2.53125f;
            positionProperty.vector3Value = v;
        }

        if (GUILayout.Button("Floor 2"))
        {
            Vector3 v = positionProperty.vector3Value;
            v.y = -1.03125f;
            positionProperty.vector3Value = v;
        }
        if (GUILayout.Button("Floor 3"))
        {
            Vector3 v = positionProperty.vector3Value;
            v.y = 0.46875f;
            positionProperty.vector3Value = v;
        }
        if (GUILayout.Button("Floor 4"))
        {
            Vector3 v = positionProperty.vector3Value;
            v.y = 1.96875f;
            positionProperty.vector3Value = v;
        }

        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

    private bool ValidatePosition(Vector3 position)
    {
        if (Mathf.Abs(position.x) > POSITION_MAX) return false;
        if (Mathf.Abs(position.y) > POSITION_MAX) return false;
        if (Mathf.Abs(position.z) > POSITION_MAX) return false;
        return true;
    }

    private void RotationPropertyField(SerializedProperty p_rotationProperty, GUIContent p_content)
    {
        Transform transform = (Transform)targets[0];
        Quaternion localRotation = transform.localRotation;
        foreach (Object t in targets)
        {
            if (!SameRotation(localRotation, ((Transform)t).localRotation))
            {
                EditorGUI.showMixedValue = true;
                break;
            }
        }

        EditorGUI.BeginChangeCheck();

        Vector3 eulerAngles = EditorGUILayout.Vector3Field(p_content, localRotation.eulerAngles);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObjects(targets, "Rotation Changed");
            foreach (Object obj in targets)
            {
                Transform t = (Transform)obj;
                t.localEulerAngles = eulerAngles;
            }
            p_rotationProperty.serializedObject.SetIsDifferentCacheDirty();
        }

        EditorGUI.showMixedValue = false;
    }

    private bool SameRotation(Quaternion rot1, Quaternion rot2)
    {
        if (System.Math.Abs(rot1.x - rot2.x) > float.Epsilon) return false;
        if (System.Math.Abs(rot1.y - rot2.y) > float.Epsilon) return false;
        if (System.Math.Abs(rot1.z - rot2.z) > float.Epsilon) return false;
        if (System.Math.Abs(rot1.w - rot2.w) > float.Epsilon) return false;
        return true;
    }
}