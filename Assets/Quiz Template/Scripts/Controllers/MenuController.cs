using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
    public static GameManager gameManager;

    // Заполнять ручками требуется только переменные ниже
    public MenuConfig config;

    [Space]
    public GameObject buyWindow;
    public TextMeshProUGUI buyError, starCounter;
    public Transform buttonContainer;

    public void Init(int quizIndex)
    {
        if (config == null)
        {
            throw new NullReferenceException("No menu config!");
        }

        starCounter.text = YandexGame.savesData.stars.ToString();

        if (config.easyButtonImage != null)
        {
            buttonContainer.GetChild(0).GetComponent<Image>().sprite = config.easyButtonImage;
            buttonContainer.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().color = config.easyTextColor;
        }

        bool isOpen = gameManager.IsLevelWasOpened(quizIndex, 1);
        Sprite sprite = isOpen ? config.mediumButtonImage : config.lockImage;
        Color color = isOpen ? config.mediumTextColor : config.lockColor;
        buttonContainer.GetChild(1).GetComponent<Image>().sprite = sprite;
        buttonContainer.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().color = color;

        isOpen = gameManager.IsLevelWasOpened(quizIndex, 2);
        sprite = isOpen ? config.hardButtonImage : config.lockImage;
        color = isOpen ? config.hardTextColor : config.lockColor;
        buttonContainer.GetChild(2).GetComponent<Image>().sprite = sprite;
        buttonContainer.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().color = color;
    }

    // Функция для обработки нажатия кнопки уровня. На вход принимает номер уровня начиная с 0
    // Метод отбрасывает нажатия кнопок уровней, которые не должны запускаться
    public void LevelButtonPressed(int levelNumber)
    {
        if (!gameManager.IsLevelWasOpened(gameManager.chosenQuizIndex, levelNumber) && levelNumber != 0)
        {
            buyWindow.SetActive(true);
            int price = levelNumber == 1 ? config.mediumOpenPrice : config.hardOpenPrice;
            buyWindow.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"Хотите открыть этот уровень за {price} звезд?";
            return;
        }

        gameManager.NextStep(levelNumber, transform);
    }

    public void BuyButtonPressed()
    {
        MatchCollection matches = Regex.Matches(buyWindow.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text, @"\d+");
        int num = int.Parse(matches[0].Value);
        if (YandexGame.savesData.stars < num)
        {
            DoBuyError("У тебя недостаточно звезд!");
            buyWindow.SetActive(false);
            return;
        }
        else
        {
            int index = num == config.mediumOpenPrice ? 1 : 2;
            if (index == 2 && !gameManager.IsLevelWasOpened(gameManager.chosenQuizIndex, 1))
            {
                DoBuyError("Сначала нужно открыть средний уровень сложности!");
                buyWindow.SetActive(false);
                return;
            }

            YandexGame.savesData.stars -= num;
            gameManager.openedLevels.Add(new DoubleInt(gameManager.chosenQuizIndex, index));
            YandexGame.SaveProgress();
        }

        buyWindow.SetActive(false);
        Init(gameManager.chosenQuizIndex);
    }

    private void DoBuyError(string text)
    {
        DOTween.Kill(0); // Не оптимально использовать это
        buyError.text = text;
        buyError.GetComponent<RectTransform>().position = new Vector2(Screen.width / 2f, Screen.height / 2.5f);
        buyError.color = Color.white;
        DOTween.Sequence()
            .Append(buyError.GetComponent<RectTransform>().DOAnchorPosY(buyError.transform.position.y + 1f, 2f))    
            .Join(buyError.DOFade(0f, 2f))
            .SetId(0);
    }

    public void BackButtonPressed()
    {
        buyWindow.SetActive(false);
    }

    public void BackInMenuButtonPressed()
    {
        gameManager.BackInMenu(transform);
    }
}
