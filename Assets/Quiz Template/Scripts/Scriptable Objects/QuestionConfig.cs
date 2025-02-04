using UnityEngine;

[CreateAssetMenu(fileName = "New Question Config", menuName = "Quiz Objects/Question Config", order = 51)]
public class QuestionConfig : ScriptableObject
{
    public Sprite wrongAnswerSprite;
    public Color wrongButtonColor = new Color(0f, 0f, 0f, 1f);

    [Space]
    public Sprite rightAnswerSprite;
    public Color rightButtonColor = new Color(0f, 0f, 0f, 1f);
}
