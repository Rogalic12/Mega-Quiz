using UnityEngine;

[CreateAssetMenu(fileName = "New Question Container", menuName = "Quiz Objects/Questions/Question Container", order = 51)]
public class CardsContainer : ScriptableObject
{
    public bool shouldCutCardPool; // ������� �� ������������ ���������� �������� �� numberOfQuestions. ���� �������� �� ������� ������������ shouldShuffle � GameManager
    public int numberOfQuestions = 0;

    [Space]
    public QuestionCard[] questionCards;

    private void OnValidate()
    {
        // ������������, ��� ����� ��������� ��������� questionCards 
    }
}
