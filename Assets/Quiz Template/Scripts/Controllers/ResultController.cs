using DG.Tweening;
using TMPro;
using UnityEngine;

public class ResultController : MonoBehaviour
{
    public static GameManager gameManager;

    public TextMeshProUGUI resultText;

    public void Init(int rightAnswers)
    {
        if (gameManager.language == "ru")
        {
            resultText.text = $"Твой результат:\n{rightAnswers}!";
        }
        else
        {
            resultText.text = $"You scored:\n{rightAnswers}\npoints!";
        }

        resultText.transform.localScale = new Vector3(9f, 9f, 9f);
        resultText.transform.rotation = Quaternion.Euler(0f, 0f, -10f);

        resultText.transform.DOScale(12f, .75f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        resultText.transform.DOLocalRotate(new Vector3(0f, 0f, 10f), 5f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo).SetDelay(0.1f);
    }

    public void RestartButtonPressed()
    {
        DOTween.KillAll();
        gameManager.RestartLastQuiz(transform);
    }

    public void BackButtonPressed()
    {
        DOTween.KillAll();
        gameManager.BackInMenu(transform);
    }
}
