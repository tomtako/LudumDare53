using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameHud : MonoBehaviour
    {
        private class TextQueueItem
        {
            public int portraitIndex;
            public string text;
        }
        public SpriteAnimation villainAnimator;
        public RectTransform dialogueContainer;
        public TextMeshProUGUI dialogue;
        public float lingerTextDelay = 3;
        public float dialogueMoveDuration = .5f;
        public Vector2 targetOutAnchorPosition = new Vector2(248, 8);

        private float m_lingerTextTimer;
        private List<TextQueueItem> m_textItems;
        private Tweener m_moveDialogueOutAnim;

        private void Start()
        {
            m_textItems = new List<TextQueueItem>();
            m_lingerTextTimer = 0;
            dialogueContainer.anchoredPosition = targetOutAnchorPosition;
        }


        public void AddText(int portraitIndex, string text)
        {
            m_textItems ??= new List<TextQueueItem>();
            m_textItems.Add( new TextQueueItem { portraitIndex = portraitIndex, text = text });
        }

        private void Update()
        {
            //Debug.Log("help??");

            if (m_moveDialogueOutAnim.IsActive())
            {
                Debug.Log("is moving");
                return;
            }

            if (m_textItems.Count > 0)
            {
                Debug.Log($"doing? {m_lingerTextTimer}");
                m_lingerTextTimer -= Time.deltaTime;

                if (m_lingerTextTimer <= 0)
                {
                    if (Vector2.Distance(targetOutAnchorPosition, dialogueContainer.anchoredPosition) > 1)
                    {
                        Debug.Log("Started moving");
                        m_moveDialogueOutAnim = dialogueContainer.
                            DOAnchorPosX(targetOutAnchorPosition.x, dialogueMoveDuration).
                            SetEase(Ease.InBack);

                        // dialogueContainer.anchoredPosition = Vector3.MoveTowards(dialogueContainer.anchoredPosition,
                        //     targetAnchorPosition, dialogueMoveSpeed * Time.deltaTime);

                        return;
                    }

                    Debug.Log("working??");

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