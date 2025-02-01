using UnityEngine;

[CreateAssetMenu(fileName = "New Menu Config", menuName = "Quiz Objects/Menu Config", order = 51)]
public class MenuConfig : ScriptableObject
{
    public Sprite cashSprite;

    [Space]
    public Sprite lockImage;
    public Color lockColor = new Color(0f, 0f, 0f, 1f);

    [Space]
    public Sprite easyButtonImage;
    public Color easyTextColor = new Color(0f, 0f, 0f, 1f);

    [Space]
    public Sprite mediumButtonImage;
    public Color mediumTextColor = new Color(0f, 0f, 0f, 1f);
    public int mediumOpenPrice;

    [Space]
    public Sprite hardButtonImage;
    public Color hardTextColor = new Color(0f, 0f, 0f, 1f);
    public int hardOpenPrice;
}
