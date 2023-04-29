using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class TMPSpriteFont : MonoBehaviour
{
    public const string TAG_BUILDER = "<sprite=\"{0}\" name=\"{1}\">";
    public const string TAG_BUILDER_SPRITE_ASSET = "<sprite name={0}_{1}>";
    public bool spriteAsset;
    public string resourceName;
    public string text;

    private TextMeshProUGUI label;
    private string _lastText;

    public TextMeshProUGUI Label => label;

    public void Update()
    {
        if (string.IsNullOrEmpty(resourceName))
        {
            return;
        }

        if (label == null)
        {
            label = GetComponent<TextMeshProUGUI>();
        }

        if (label == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(text) && !text.Equals(_lastText))
        {
            _lastText = text;

            label.text = string.Empty;

            for (int i = 0; i < text.Length; i++)
            {
                if (spriteAsset)
                {
                    label.text += string.Format(TAG_BUILDER_SPRITE_ASSET, resourceName, text[i]);
                }
                else
                {
                    label.text += string.Format(TAG_BUILDER, resourceName, text[i]);
                }
            }
        }
    }

    public void ForceRefresh(TMP_SpriteAsset newSpriteAsset = null)
    {
        if (label == null)
        {
            label = GetComponent<TextMeshProUGUI>();
        }

        _lastText = text;

        label.text = string.Empty;

        for (int i = 0; i < text.Length; i++)
        {
            if (spriteAsset)
            {
                label.text += string.Format(TAG_BUILDER_SPRITE_ASSET, resourceName, text[i]);
            }
            else
            {
                label.text += string.Format(TAG_BUILDER, resourceName, text[i]);
            }
        }

        if (newSpriteAsset != null)
        {
            label.spriteAsset = newSpriteAsset;
        }
    }
}
