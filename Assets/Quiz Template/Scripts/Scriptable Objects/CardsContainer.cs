using UnityEngine;

[CreateAssetMenu(fileName = "New Question Container", menuName = "Quiz Objects/Questions/Question Container", order = 51)]
public class CardsContainer : ScriptableObject
{
    public bool shouldCutCardPool; // ������� �� ������������ ���������� �������� �� numberOfQuestions. ���� �������� �� ������� ������������ shouldShuffle � GameManager
    public int numberOfQuestions = 0;

    public QuestionCard[] QuestionCards
    {
        get { return new ExcelDataParser().ParseQuestions(filePath, sheetName).ToArray(); }
    }

    [Header("Excel file settings")]
    public string sheetName;

    private string filePath = "";
    public string FilePath
    {
        get { return filePath; }
        set { filePath = value; }
    }
}
