using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardsContainer))]
public class FileSelectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CardsContainer script = (CardsContainer) target;

        if (GUILayout.Button("������� .xlsx ����"))
        {
            string filePath = EditorUtility.OpenFilePanel("�������� .xlsx ����", "", "xlsx");
            if (!string.IsNullOrEmpty(filePath))
            {
                string relativePath = filePath.Replace(Application.dataPath, "Assets");
                script.FilePath = relativePath;
            }
        }

        if (!string.IsNullOrEmpty(script.FilePath))
        {
            EditorGUILayout.LabelField($"��������� ����: {script.FilePath}");
        }
    }
}
