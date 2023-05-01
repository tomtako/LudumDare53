using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameHud : MonoBehaviour
    {
        [System.Serializable]
        public class TextQueueItem
        {
            public int portraitIndex;
            public string text;
        }

        public CanvasGroup canvasGroup;
        public SpriteAnimation villainAnimator;
        public RectTransform dialogueContainer;
        public TextMeshProUGUI dialogue;
        public float lingerTextDelay = 3;
        public float dialogueMoveDuration = .5f;
        public Vector2 targetOutAnchorPosition = new Vector2(248, 8);
        public GameObject servedStamp;
        public GameObject escapedStamp;

        private float m_lingerTextTimer;
        public List<TextQueueItem> m_textItems;
        private Tweener m_moveDialogueOutAnim;

        private void Start()
        {
            m_lingerTextTimer = 0;
            dialogueContainer.anchoredPosition = targetOutAnchorPosition;
        }

        public void AddText(int portraitIndex, string text)
        {
            if (m_textItems == null)
            {
                m_textItems = new List<TextQueueItem>();
            }
            m_textItems.Add( new TextQueueItem { portraitIndex = portraitIndex, text = text });
        }

        public void OnServed()
        {
            servedStamp.SetActive(true);
            servedStamp.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            servedStamp.transform.DOScale(1, 0.5f).SetEase(Ease.OutElastic);
        }

        public void OnEscaped()
        {
            escapedStamp.SetActive(true);
            escapedStamp.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            escapedStamp.transform.DOScale(1, 0.5f).SetEase(Ease.OutElastic);
        }

        private void Update()
        {
            if (m_moveDialogueOutAnim.IsActive())
            {
                return;
            }

            if (m_textItems.Count > 0)
            {
                m_lingerTextTimer -= Time.deltaTime;

                if (m_lingerTextTimer <= 0)
                {
                    if (Vector2.Distance(targetOutAnchorPosition, dialogueContainer.anchoredPosition) > 1)
                    {
                        m_moveDialogueOutAnim = dialogueContainer.
                            DOAnchorPosX(targetOutAnchorPosition.x, dialogueMoveDuration).
                            SetEase(Ease.InBack);

                        return;
                    }

                    escapedStamp.SetActive(false);
                    servedStamp.SetActive(false);

                    m_lingerTextTimer = lingerTextDelay;

                    var item = m_textItems[0];
                    m_textItems.RemoveAt(0);

                    if (item.portraitIndex >= 0)
                    {
                        villainAnimator.SetActive(true);
                        villainAnimator.SetFrame("portraits", item.portraitIndex);
                    }
                    else
                    {
                        villainAnimator.SetActive(false);
                    }

                    dialogue.text = item.text;

                    dialogueContainer.DOAnchorPosX(0, dialogueMoveDuration).SetEase(Ease.OutBack);
                }
            }
        }
    }
}