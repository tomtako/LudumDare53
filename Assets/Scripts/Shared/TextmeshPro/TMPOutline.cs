using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Shared.TextmeshPro
{
    [ExecuteInEditMode]
    public class TMPOutline : MonoBehaviour
    {
        public enum OutlineMode
        {
            OutlineAndShadow,
            OutlineOnly,
            ShadowOnly
        }

        public int distance=1;
        public OutlineMode outlineMode;
        public Color outlineColor = Color.black;


        private string _text;

        private TextMeshProUGUI _master;
        public TextMeshProUGUI Master
        {
            get
            {
                if (_master == null)
                {
                    _master = GetComponent<TextMeshProUGUI>();
                }

                return _master;
            }
        }

        private TextMeshProUGUI _copy;
        public TextMeshProUGUI Copy
        {
            get
            {
                if (_copy == null)
                {
                    if (transform.childCount <= 0)
                    {
                        _copy = null;
                    }
                    else
                    {
                        _copy = transform.GetChild(transform.childCount-1).GetComponent<TextMeshProUGUI>();
                    }
                }

                return _copy;
            }
        }

        private List<TextMeshProUGUI> _outlines;

        private void Start()
        {
            if (_outlines == null)
            {
                _outlines = GetComponentsInChildren<TextMeshProUGUI>().ToList();
                _outlines.Remove(GetComponent<TextMeshProUGUI>());
            }
        }

        public void SetTextWithColor(string text, Color color)
        {
            if (_outlines == null)
            {
                Start();
            }

            _text = text;
            _outlines.ForEach(x=>x.text = _text);

            Master.text = _text;
            Copy.text = _text;

            Copy.color = color;
        }

        public void SetTextForFormatting(string nonFormatted, string formatted)
        {
            if (_outlines == null)
            {
                Start();
            }

            _text = nonFormatted;
            _outlines.ForEach(x=>x.text = _text);

            Master.text = _text;
            Copy.text = formatted;
        }

        private void Update()
        {
            if (_outlines==null || !Master || !Copy )
            {
                return;
            }

            if (_text == null)
            {
                _text = string.Empty;
            }

            if (Master.text != _text)
            {
                _text = Master.text;
                Copy.text = _text;
                _outlines.ForEach(x=>x.text = Copy.text);
            }
        }

        public void ForceUpdate()
        {
            Update();
        }

        [Button("Force Generate Outline")]
        private void GenerateOutlines()
        {
            if (_outlines != null)
            {
                for (var i = _outlines.Count - 1; i > -1; i--)
                {
                    if (_outlines[i] != null)
                    {
                        DestroyImmediate(_outlines[i].gameObject);
                    }
                }
            }

            if (Copy == null)
            {
                Debug.LogError("Please make a copy of the original tmp object and place it as a child.");
                return;
            }

            _outlines = new List<TextMeshProUGUI>();

            switch (outlineMode)
            {
                case OutlineMode.OutlineAndShadow:
                case OutlineMode.OutlineOnly:
                    OutlineTmp();
                    break;
                case OutlineMode.ShadowOnly:
                    ShadowOnlyTmp();
                    break;
            }
        }

        private void OutlineTmp()
        {
            var offset = Vector2.zero;

            for (var i = 0; i < 8; i++)
            {
                if (i == 0)
                    offset = new Vector2(0,distance);
                else if (i == 1)
                    offset = new Vector2(0,-distance);
                else if (i == 2)
                    offset = new Vector2(distance,0);
                else if (i == 3)
                    offset = new Vector2(-distance,0);
                else if (i == 4)
                    offset = new Vector2(distance,distance);
                else if (i == 5)
                    offset = new Vector2(-distance,-distance);
                else if (i == 6)
                    offset = new Vector2(distance,-distance);
                else if (i == 7)
                    offset = new Vector2(-distance,distance);

                var t = Instantiate(Copy, transform, false);
                t.name = $"Outline-{i}";
                t.rectTransform.anchoredPosition = offset;
                t.color = outlineColor;
                t.transform.SetSiblingIndex(0);
                _outlines.Add(t);
            }

            if (outlineMode == OutlineMode.OutlineAndShadow)
            {
                for (var i = 0; i < 5; i++)
                {
                    if (i == 0)
                        offset = new Vector2(distance+1, 0);
                    else if (i == 1)
                        offset = new Vector2(distance+1, -distance);
                    else if (i == 2)
                        offset = new Vector2(distance+1, -distance-1);
                    else if (i == 3)
                        offset = new Vector2(0, -distance-1);
                    else if (i == 4)
                        offset = new Vector2(distance, -distance-1);

                    var t = Instantiate(Copy, transform, false);
                    t.name = $"Shadow-{i}";
                    t.rectTransform.anchoredPosition = offset;
                    t.color = outlineColor;
                    t.transform.SetSiblingIndex(0);
                    _outlines.Add(t);
                }
            }
        }

        private void ShadowOnlyTmp()
        {
            var t = Instantiate(Copy, transform, false);
            t.name = $"Shadow";
            t.rectTransform.anchoredPosition = new Vector2(distance,-distance);
            t.color = outlineColor;
            t.transform.SetSiblingIndex(0);
            _outlines.Add(t);
        }
    }
}
