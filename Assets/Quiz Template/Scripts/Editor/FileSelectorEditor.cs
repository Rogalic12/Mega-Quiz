using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardsContainer))]
public class FileSelectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CardsContainer script = (CardsContainer) target;

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField("Выбранная таблица", script.FileName);
        EditorGUI.EndDisabledGroup();

        if (GUILayout.Button("Выбрать .xlsx файл"))
        {
            string filePath = EditorUtility.OpenFilePanel("Выберите .xlsx файл", "", "xlsx");
            if (!string.IsNullOrEmpty(filePath))
            {
                string relativePath = filePath.Replace(Application.dataPath, "Assets");
                script.FilePath = relativePath;
                EditorUtility.SetDirty(script);
            }
        }

        if (!string.IsNullOrEmpty(script.FilePath))
        {
            EditorGUILayout.LabelField($"Путь до файла: {script.FilePath}");
        }
    }
}
