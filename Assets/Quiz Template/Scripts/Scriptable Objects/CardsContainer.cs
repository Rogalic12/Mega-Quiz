using UnityEngine;

[CreateAssetMenu(fileName = "New Question Container", menuName = "Quiz Objects/Questions/Question Container", order = 51)]
public class CardsContainer : ScriptableObject
{
    public bool shouldCutCardPool; // Следует ли ограничивать количество вопросов по numberOfQuestions. Если включено то советую использовать shouldShuffle в GameManager
    public int numberOfQuestions = 0;

    [Space]
    public QuestionCard[] questionCards;

    private void OnValidate()
    {
        // Теоритически, тут можно автоматом заполнять questionCards 
    }
}
