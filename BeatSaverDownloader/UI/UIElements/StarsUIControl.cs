﻿using BeatSaverDownloader.Misc;
using CustomUI.BeatSaber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeatSaverDownloader.UI.UIElements
{
    class StarsUIControl : MonoBehaviour
    {
        public int value { get { return _currentValue;  } set { HandleStarPressedEvent(value, false); } }
        public event Action<int> starPressed;

        private int _currentValue;

        Button[] _starButtons = new Button[5];

        public void Init(RectTransform parent, Vector2 position)
        {
            transform.SetParent(parent, false);

            (transform as RectTransform).anchorMin = new Vector2(0.5f, 0.5f);
            (transform as RectTransform).anchorMax = new Vector2(0.5f, 0.5f);
            (transform as RectTransform).sizeDelta = new Vector2(50f, 10f);
            (transform as RectTransform).anchoredPosition = position;

            for (int i = 0; i < 5; i++)
                TransformButton(i);
        }

        private void HandleStarPressedEvent(int index, bool callbackAction = true)
        {
            if(_currentValue == index && _currentValue == 1)
            {
                _currentValue = 0;
            }
            else
            {
                _currentValue = index;
            }

            if(_currentValue > 5)
            {
                _currentValue = 5;
            }
            else if(_currentValue < 0)
            {
                _currentValue = 0;
            }

            for (int i = 0; i < _currentValue; i++)
            {
                if (_starButtons.Length > i && i >= 0)
                    _starButtons[i].SetButtonIcon(Base64Sprites.StarFull);
                else
                    Misc.Logger.Log("Index out of bounds! (1) Items: " + _starButtons.Length + ", Index: "+i);
            }

            for (int i = _currentValue; i < _starButtons.Length; i++)
            {
                if (_starButtons.Length > i && i >= 0)
                    _starButtons[i].SetButtonIcon(Base64Sprites.StarEmpty);
                else
                    Misc.Logger.Log("Index out of bounds! (2) Items: " + _starButtons.Length + ", Index: " + i);
            }

            if(callbackAction)
                starPressed?.Invoke(_currentValue);
        }

        private void TransformButton(int index)
        {
            _starButtons[index] = BeatSaberUI.CreateUIButton(transform as RectTransform, "PracticeButton", new Vector2(10f * index, 0f), new Vector2(10f, 10f), () => { HandleStarPressedEvent(index + 1); }, "", Base64Sprites.StarEmpty);

            RectTransform iconTransform = _starButtons[index].GetComponentsInChildren<RectTransform>(true).First(x => x.name == "Icon");
            iconTransform.gameObject.SetActive(true);
            Destroy(iconTransform.parent.GetComponent<HorizontalLayoutGroup>());
            iconTransform.sizeDelta = new Vector2(8f, 8f);
            iconTransform.anchoredPosition = new Vector2(5f, -4.8f);

            _starButtons[index].GetComponentsInChildren<Image>().First(x => x.name == "Stroke").enabled = false;
        }

    }
}