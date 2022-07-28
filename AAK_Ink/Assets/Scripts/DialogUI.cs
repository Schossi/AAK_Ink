using Ink.Runtime;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// simple singleton dialog UI that displays an Ink Stories line by line and char by char and displays buttons for choices if they occur
/// </summary>
public class DialogUI : MonoBehaviour
{
    public static DialogUI Instance;

    [Tooltip("the text field that will contain the stories lines")]
    public TMPro.TMP_Text Text;
    [Tooltip("parent object for eventual choices, has to contain one button that is used as the template for choices")]
    public GameObject Options;
    [Tooltip("delay between displaying characters, increase to slow text down")]
    public float CharacterDelay = 0.01f;

    private Button _optionPrefab;

    private Story _story;
    private Action _continued;
    private Action _finished;

    private string _currentLine;
    private Coroutine _currentShowLine;
    private bool _isEnding;

    private void Awake()
    {
        Instance = this;

        _optionPrefab = Options.GetComponentInChildren<Button>();
        _optionPrefab.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    public void Show(Story story, Action continued, Action finished)
    {
        gameObject.SetActive(true);

        _story = story;
        _continued = continued;
        _finished = finished;

        Continue();
    }

    public void Hide()
    {
        StopAllCoroutines();

        _story = null;
        _finished = null;

        _currentLine = null;
        _currentShowLine = null;
        _isEnding = false;

        gameObject.SetActive(false);
    }

    public void Continue()
    {
        if (_story.currentChoices.Count > 0)
            return;

        if (_currentShowLine != null)
        {
            StopCoroutine(_currentShowLine);
            _currentShowLine = null;
            finishLine();
            return;
        }

        if (_story.canContinue && !_isEnding)
        {
            _currentLine = _story.Continue();
            _isEnding = _story.currentTags.Contains("EndAction");
            _currentShowLine = StartCoroutine(showLine());
            _continued();
        }
        else
        {
            _finished.Invoke();

            Hide();
        }
    }
    private IEnumerator showLine()
    {
        Text.text = _currentLine;
        Text.maxVisibleCharacters = 0;

        var isTag=false;

        for (int i = 0; i < _currentLine.Length; i++)
        {
            if (isTag)
            {
                if (_currentLine[i] == '>')
                    isTag = false;
            }
            else if (_currentLine[i] == '<')
            {
                isTag = true;
            }
            else
            {
                yield return new WaitForSeconds(CharacterDelay);
            }

            Text.maxVisibleCharacters++;
        }

        finishLine();
        _currentShowLine = null;
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
