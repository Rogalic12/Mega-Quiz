using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionController : MonoBehaviour
{
    public static GameManager gameManager;

    private int rightIndex;
    private List<QuestionCard> cards = new();

    public int currentQuestion;
    public int rightAnswers;

    [Space]
    public GameObject inMenuWindow;
    public TextMeshProUGUI questionText, counterText;
    public Image image;
    public GameObject answers;

    // Инициализация контроллера. Обязательно вызвать ПЕРЕД началом каждого ответа на вопросы
    // Принимает на вход контейнер с вопросами, на которые потребуется отвечать пользователю и переменную, следует ли рандомизировать порядок вопросов 
    public void Init(CardsContainer container, bool isShuffle)
    {
        List<QuestionCard> allPool = new List<QuestionCard>(container.questionCards);
        if (isShuffle)
        {
            int n = allPool.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                QuestionCard temp = allPool[i];
                allPool[i] = allPool[j];
                allPool[j] = temp;
            }
        }
        cards = container.shouldCutCardPool ? new List<QuestionCard>(allPool.Take(container.numberOfQuestions)) : allPool;

        currentQuestion = 1;
        rightAnswers = 0;
        NextQuestion(cards[currentQuestion - 1]);
    }

    // Функция используется для занесения данных из входной переменной card в интерфейс
    public void NextQuestion(QuestionCard card)
    {
        questionText.text = card.question;
        image.sprite = card.image;
        counterText.text = $"{currentQuestion}/{cards.Count}";

        if (card.wrongAnswers.Length == 3)
        {
            List<TextMeshProUGUI> textContainers = new List<TextMeshProUGUI>(answers.GetComponentsInChildren<TextMeshProUGUI>());
            List<string> allAnswers = new List<string>(card.wrongAnswers) { card.rightAnswer };
            List<string> editableAnswers = new List<string>(allAnswers);
            for (int i = 0; i < allAnswers.Count; i++)
            {
                TextMeshProUGUI answer = textContainers[i];
                int rand = Random.Range(0, editableAnswers.Count);
                answer.text = editableAnswers[rand];
                if (editableAnswers[rand].Equals(allAnswers[allAnswers.Count - 1]))
                {
                    rightIndex = i;
                    print("Right index was set");
                }

                editableAnswers.Remove(editableAnswers[rand]);
            }
        }
    }

    // Нажатие кнопки ответа. На вход подается номер кнопки начиная с 0
    public void AnswerButtonPressed(int buttonIndex)
    {
        if (buttonIndex == rightIndex)
        {
            rightAnswers++;
            print("Right!");
        }
        else
        {
            print("Incorrect!");
        }

        if (currentQuestion != cards.Count) // Если вопрос был не последний
        {
            currentQuestion++;
            NextQuestion(cards[currentQuestion - 1]);
        }
        else
        {
            gameManager.NextStep(rightAnswers, transform);
        }
    }

    public void MenuButtonPressed()
    {
        inMenuWindow.SetActive(!inMenuWindow.activeSelf);
    }

    public void BackInMenu()
    {
        inMenuWindow.SetActive(false);
        gameManager.BackInMenu(transform);
    }
}
