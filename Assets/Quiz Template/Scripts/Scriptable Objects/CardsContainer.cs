using UnityEngine;

[CreateAssetMenu(fileName = "New Question Container", menuName = "Quiz Objects/Question Container", order = 51)]
public class CardsContainer : ScriptableObject
{
    public bool shouldCutCardPool; // ������� �� ������������ ���������� �������� �� numberOfQuestions. ���� �������� �� ������� ������������ shouldShuffle � GameManager
    public int numberOfQuestions = 0;

    public QuestionCard[] QuestionCards
    {
        get { return new ExcelDataParser().ParseQuestions(filePath, sheetName).ToArray(); }
    }

    [Header("Import File Settings")]
    public string sheetName;

    [HideInInspector] public string FileName
    { 
        get { return filePath.Substring(filePath.LastIndexOf("/") + 1); }
        set { FileName = value; }
    }

    [HideInInspector] public string filePath = "";
    public string FilePath
    {
        get { return filePath; }
        set { filePath = value; }
    }
}
