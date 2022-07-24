using Ink.Runtime;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    public static DialogUI Instance;

    public Image Portrait;
    public TMPro.TMP_Text Text;
    public GameObject Options;
    public float CharacterDelay = 0.01f;

    private Button _optionPrefab;

    private Story _story;
    private Action _finished;

    private string _currentLine;
    private Coroutine _currentShowLine;

    private void Awake()
    {
        Instance = this;

        _optionPrefab = Options.GetComponentInChildren<Button>();
        _optionPrefab.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    public void Show(Story story, Action finished)
    {
        gameObject.SetActive(true);

        _story = story;
        _finished = finished;

        Continue();
    }

    public void Continue()
    {
        if (_story.currentChoices.Count > 0)
        {
            return;
        }

        if (_currentShowLine != null)
        {
            StopCoroutine(_currentShowLine);
            _currentShowLine = null;
            finishLine();
            return;
        }

        if (_story.canContinue)
        {
            _currentLine = _story.Continue();
            StartCoroutine(showLine());
        }
        else
        {
            _finished?.Invoke();

            _story = null;
            _finished = null;

            gameObject.SetActive(false);
        }
    }

    private IEnumerator showLine()
    {
        Text.text = _currentLine;
        Text.maxVisibleCharacters = 0;

        for (int i = 0; i < _currentLine.Length; i++)
        {
            yield return new WaitForSeconds(CharacterDelay);
            Text.maxVisibleCharacters++;
        }

        finishLine();
    }
    private void finishLine()
    {
        Text.maxVisibleCharacters = _currentLine.Length;

        foreach (var choice in _story.currentChoices)
        {
            var option = Instantiate(_optionPrefab, Options.transform);
            option.onClick.AddListener(new UnityAction(() => choose(choice)));
            option.GetComponentInChildren<TMPro.TMP_Text>().text = choice.text;
            option.gameObject.SetActive(true);
        }
    }

    private void choose(Choice choice)
    {
        for (int i = 1; i < Options.transform.childCount; i++)
        {
            Destroy(Options.transform.GetChild(i).gameObject);
        }

        _story.ChooseChoiceIndex(choice.index);
        Continue();
    }
}
