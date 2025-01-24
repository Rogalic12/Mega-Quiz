using UnityEngine;

[CreateAssetMenu(fileName = "New Question Card", menuName = "Quiz Objects/Questions/Question Card", order = 51)]
public class QuestionCard : ScriptableObject
{
    public string question;
    public Sprite image;
    public string rightAnswer;
    public string[] wrongAnswers = new string[3];
}
