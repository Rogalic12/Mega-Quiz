using UnityEngine;


[CreateAssetMenu(fileName = "New Quiz Card", menuName = "Quiz Objects/Quiz Card", order = 51)]
public class QuizCard : ScriptableObject
{
    public Sprite image;
    public string quizName;
    public CardsContainer[] testsContainers = new CardsContainer[3];
}
