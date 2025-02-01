using System;
using System.Collections.Generic;
using UnityEngine;

public class ExcelDataParser
{
    public List<QuestionCard> ParseQuestions(List<Dictionary<string, string>> sheetData)
    {
        List<QuestionCard> questions = new List<QuestionCard>();

        foreach (Dictionary<string, string> row in sheetData)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(Convert.FromBase64String(row["Изображение"]));

            QuestionCard card = new()
            {
                question = row["Вопрос"],
                rightAnswer = row["Правильный ответ"],
                wrongAnswers = new string[3] { row["Неправильный ответ 1"], row["Неправильный ответ 2"], row["Неправильный ответ 3"] },
                image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f),         
            };
            questions.Add(card);
        }
        return questions;
    }

    public List<QuestionCard> ParseQuestions(string path, string sheet)
    {
        return ParseQuestions(new ExcelReader(path).ReadSheet(sheet));
    }
}
