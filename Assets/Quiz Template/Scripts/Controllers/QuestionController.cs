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
    public GameObject showRightButton, nextButton;
    public TextMeshProUGUI questionText, counterText;
    public Image image;
    public RectTransform answersParent;
    public GameObject answerButtonPrefab;

    // ������������� �����������. ����������� ������� ����� ������� ������� ������ �� �������
    // ��������� �� ���� ��������� � ���������, �� ������� ����������� �������� ������������
    public void Init(CardsContainer container)
    {
        List<QuestionCard> allPool = new List<QuestionCard>(container.QuestionCards);
        if (gameManager.shouldShuffle)
        {
            // allPool.OrderBy(_ => Random.value).Reverse(); 

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
        LoadNextQuestion(cards[currentQuestion - 1]);
    }

    // ������� ������������ ��� ��������� ������ �� ������� ���������� card � ���������
    public void LoadNextQuestion(QuestionCard card)
    {
        questionText.text = card.question;
        image.sprite = card.image;
        counterText.text = $"{currentQuestion}/{cards.Count}";

        List<string> wrongs = new(card.wrongAnswers.OrderBy(_ => Random.value).Take(gameManager.chosenLevelIndex + 1)); // ��������� ������������ ������, ���������� �� ��������� ������
        List<string> allAnswers = new(wrongs.Append(card.rightAnswer).OrderBy(_ => Random.value)); // ��������� �������� �������

        for (int i = 0; i < answersParent.childCount; i++)
        {
            Destroy(answersParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < allAnswers.Count; i++)
        { 
            GameObject button = Instantiate(answerButtonPrefab, answersParent);
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
        if (nextButton.activeSelf) return;

        nextButton.SetActive(true);
        Image pressedImage = answersParent.GetChild(buttonIndex).GetComponent<Image>();
        if (buttonIndex == rightIndex)
        {
            rightAnswers++;
            pressedImage.sprite = gameManager.questionConfig.rightAnswerSprite;
            pressedImage.color = gameManager.questionConfig.rightButtonColor;
            print("Right!");
        }
        else
        {
            showRightButton.SetActive(true);
            pressedImage.sprite = gameManager.questionConfig.wrongAnswerSprite;
            pressedImage.color = gameManager.questionConfig.wrongButtonColor;

            print("Incorrect!");
        }
    }

    public void NextButtonPressed()
    {
        nextButton.SetActive(false);
        showRightButton.SetActive(false);
        if (currentQuestion != cards.Count) // ���� ������ ��� �� ���������
        {
            currentQuestion++;
            LoadNextQuestion(cards[currentQuestion - 1]);
        }
        else
        {
            gameManager.NextStep(rightAnswers, transform);
        }
    }

    public void ShowRightAnswer()
    {
        Image rightButton = answersParent.GetChild(rightIndex).GetComponent<Image>();
        rightButton.sprite = gameManager.questionConfig.rightAnswerSprite;
        rightButton.color = gameManager.questionConfig.rightButtonColor;
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
