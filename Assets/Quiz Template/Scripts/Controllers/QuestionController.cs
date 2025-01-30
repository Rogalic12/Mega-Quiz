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
    public GameObject answers, answerButtonPrefab;

    // ������������� �����������. ����������� ������� ����� ������� ������� ������ �� �������
    // ��������� �� ���� ��������� � ���������, �� ������� ����������� �������� ������������ � ����������, ������� �� ��������������� ������� �������� 
    public void Init(CardsContainer container)
    {
        List<QuestionCard> allPool = new List<QuestionCard>(container.QuestionCards);
        if (gameManager.shouldShuffle)
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

    // ������� ������������ ��� ��������� ������ �� ������� ���������� card � ���������
    public void NextQuestion(QuestionCard card)
    {
        for (int i = 0; i < answers.transform.childCount; i++)
        {
            Destroy(answers.transform.GetChild(i).gameObject);
        }

        questionText.text = card.question;
        image.sprite = card.image;
        counterText.text = $"{currentQuestion}/{cards.Count}";

        List<string> wrongs = new(card.wrongAnswers.OrderBy(_ => Random.value).Take(gameManager.chosenLevelIndex + 1)); // ��������� ������������ ������, ���������� �� ��������� ������
        List<string> allAnswers = new(wrongs.Append(card.rightAnswer).OrderBy(_ => Random.value)); // ��������� �������� �������
        for (int i = 0; i < allAnswers.Count; i++)
        {
            GameObject button = Instantiate(answerButtonPrefab, answers.transform);
            int index = i;
            button.GetComponent<Button>().onClick.AddListener(() => AnswerButtonPressed(index));
            button.GetComponentInChildren<TextMeshProUGUI>().text = allAnswers[i];

            if (allAnswers[i] == card.rightAnswer)
            {
                rightIndex = i;
                print("Right index was set: " + i);
            }
        }
    }

    // ������� ������ ������. �� ���� �������� ����� ������ ������� � 0
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

        if (currentQuestion != cards.Count) // ���� ������ ��� �� ���������
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
