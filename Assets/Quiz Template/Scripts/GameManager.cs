using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using YG;

// TODO: ����� ������� �� ������ ������, ������ ���� ���������, ����������� ���� �������
public class GameManager : MonoBehaviour
{
    public Material[] mats;


    public string Language => YandexGame.lang;
    public HashSet<DoubleInt> OpenedLevels => YandexGame.savesData.openedLevels;

    public enum GameState
    {
        ChoosingQuiz,
        InLevelMenu,
        SolvingQuestions,
        GettingResults
    }
    private GameState state;

    [Header("Music settings")]
    public GameObject musicButton;
    public Sprite[] musicSprites = new Sprite[2];

    [Header("Choose settings")]
    public QuizCard[] quizzes;
    [HideInInspector] public int chosenQuizIndex = -1;

    [Header("Questions settings")]
    public bool shouldShuffle; // ������� �� ��������������� ������� ��������
    [HideInInspector] public int chosenLevelIndex = -1;

    [Header("Controllers")]
    public ChooseController chooseController;
    public MenuController menuController;
    public QuestionController questionController;
    public ResultController resultController;

    // Bootstrap ��� ���� ����. �� ������ ��������� ������������� ���� 
    public void Awake()
    {
        OpenedLevels.Clear(); // ������
        YandexGame.savesData.passedLevels = 0; // ! ������ ����� ������� ��� ������
        YandexGame.SaveProgress(); // ! ������ ��� ������

        ChooseController.gameManager = this;
        MenuController.gameManager = this;
        QuestionController.gameManager = this;
        ResultController.gameManager = this;
        chooseController.OnGameStart(quizzes);

        if (!YandexGame.savesData.isMusicPlaying)
        {
            GetComponent<AudioSource>().Stop();
            musicButton.GetComponent<Image>().sprite = musicSprites[0];
            musicButton.transform.localScale = new Vector3(0.94f, 0.94f, 0.94f);
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = musicSprites[1];
            musicButton.transform.localScale = Vector3.one;
        }




        ExcelReader reader = new("Assets/Quiz Template/Example/Sheets/MegaQuiz.xlsx");
        List<Dictionary<string, string>> data = reader.ReadSheet("��������");
        for (int i = 0; i < data.Count; i++)
        {
            Texture2D texture = new Texture2D(2, 2);
            texture.filterMode = FilterMode.Point;
            texture.LoadImage(Convert.FromBase64String(data[i]["�����������"]));
            mats[i].mainTexture = texture;
        }
    }

    public GameState GetGameState() { return state; }

    private void LoadNewWindow(Transform oldWindow, Transform newWindow)
    {
        oldWindow.parent.gameObject.SetActive(false);
        newWindow.parent.gameObject.SetActive(true);
    }

    public bool IsLevelWasOpened(int quizIndex, int levelIndex)
    {
        return OpenedLevels.Any(obj => obj.quiz == quizIndex && obj.difficult == levelIndex);
    }

    // ����� ������� ��� ��������� ������ ������������ ����
    // �������� �����: ����� ����� -> ����� ��������� -> ��� ���� -> ��������� -> ��� ����...
    public void NextStep(int requiredInt, Transform currentWindow)
    {
        if (currentWindow.TryGetComponent(out ChooseController _))
        {
            chosenQuizIndex = requiredInt;
            state = GameState.InLevelMenu;
            LoadNewWindow(currentWindow, menuController.transform);
            menuController.Init();
        }
        else if (currentWindow.TryGetComponent(out MenuController _))
        {
            chosenLevelIndex = requiredInt;
            state = GameState.SolvingQuestions;
            LoadNewWindow(currentWindow, questionController.transform);
            questionController.Init(quizzes[chosenQuizIndex].testContainer);
        }
        else if (currentWindow.TryGetComponent(out QuestionController _))
        {
            YandexGame.savesData.stars += requiredInt;
            YandexGame.SaveProgress();

            state = GameState.GettingResults;
            LoadNewWindow(currentWindow, resultController.transform);
            resultController.Init(requiredInt);
        }
        else if (currentWindow.TryGetComponent(out ResultController _))
        {
            state = GameState.SolvingQuestions;
            LoadNewWindow(currentWindow, questionController.transform);
            questionController.Init(quizzes[chosenQuizIndex].testContainer);
        }
    }

    public void RestartLastQuiz(Transform window) { NextStep(chosenLevelIndex, window); }

    // ����� ������� ��� ���� ������ ����������� � ����
    public void BackInMenu(Transform lastWindow)
    {
        if (lastWindow.TryGetComponent(out QuestionController controller)) // ��������� ����� �� �����
        {
            YandexGame.savesData.stars += controller.rightAnswers;
            YandexGame.SaveProgress();
        }

        state = GameState.ChoosingQuiz;
        LoadNewWindow(lastWindow, chooseController.transform);
    }

    // ������� ��� ��������� ������� ������ ���������/���������� ������
    public void MusicButtonPressed()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource.isPlaying && YandexGame.savesData.isMusicPlaying)
        {
            YandexGame.savesData.isMusicPlaying = false;
            YandexGame.SaveProgress();

            audioSource.Stop();
            musicButton.GetComponent<Image>().sprite = musicSprites[0];
            musicButton.transform.localScale = new Vector3(0.94f, 0.94f, 0.94f);
        }
        else
        {
            YandexGame.savesData.isMusicPlaying = true;
            YandexGame.SaveProgress();

            audioSource.Play();
            musicButton.GetComponent<Image>().sprite = musicSprites[1];
            musicButton.transform.localScale = Vector3.one;
        }
    }
}