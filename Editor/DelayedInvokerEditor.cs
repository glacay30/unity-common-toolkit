/* Created by user DerHugo
 * 
 * Reference: https://stackoverflow.com/a/54303138
 */

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(DelayedInvoker))]
public class ExampleInspector : Editor
{
    private SerializedProperty InvokeOnStart;
    private SerializedProperty EventDelayPairs;
    private ReorderableList list;

    private DelayedInvoker _exampleScript;

    private void OnEnable()
    {
        _exampleScript = (DelayedInvoker)target;

        // Invoke on Start field *************************

        InvokeOnStart = serializedObject.FindProperty("InvokeOnStart");

        // EventDelayPairs field *************************

        EventDelayPairs = serializedObject.FindProperty("EventDelayPairs");

        list = new ReorderableList(serializedObject, EventDelayPairs) {
            draggable = true,
            displayAdd = true,
            displayRemove = true,
            drawHeaderCallback = rect => {
                EditorGUI.LabelField(rect, "DelayedEvents");
            },
            drawElementCallback = (rect, index, sel, act) => {
                var element = EventDelayPairs.GetArrayElementAtIndex(index);

                var unityEvent = element.FindPropertyRelative("unityEvent");
                var delay = element.FindPropertyRelative("Delay");


                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), delay);

                rect.y += EditorGUIUtility.singleLineHeight;

                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(unityEvent)), unityEvent);


            },
            elementHeightCallback = index => {
                var element = EventDelayPairs.GetArrayElementAtIndex(index);

                var unityEvent = element.FindPropertyRelative("unityEvent");

                var height = EditorGUI.GetPropertyHeight(unityEvent) + EditorGUIUtility.singleLineHeight;

                return height;
            }
        };
    }

    public override void OnInspectorGUI()
    {
        DrawScriptField();

        serializedObject.Update();

        EditorGUILayout.PropertyField(InvokeOnStart, new GUIContent(nameof(InvokeOnStart)), GUILayout.Height(25f));
        list.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawScriptField()
    {
        // Disable editing
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(_exampleScript), typeof(DelayedInvoker), false);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
    }
}

