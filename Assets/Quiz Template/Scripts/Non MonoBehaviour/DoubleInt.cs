// Класс, требуемый для сохранения информации о купленных уровнях 
public class DoubleInt
{
    public int quiz = -1;
    public int difficult = -1;

    public DoubleInt(int quizIndex, int levelIndex)
    {
        quiz = quizIndex;
        difficult = levelIndex;
    }

    public float GetBothAsFloat()
    {
        return quiz + (difficult / 10f);
    }

    public string GetBothAsString() 
    {
        return $"{quiz} {difficult}";
    }

}
