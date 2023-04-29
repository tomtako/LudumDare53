using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SpriteCopier : MonoBehaviour
{
    public bool isSpriteRenderer=true;
    [ShowIf("isSpriteRenderer")]
    public SpriteRenderer m_rendererToCopy;
    [HideIf("isSpriteRenderer")]
    public Image m_ImageToCopy;

    private SpriteRenderer m_renderer;
    private Image m_Image;

    public SpriteRenderer spriteRenderer
    {
        get
        {
            if (m_renderer == null)
            {
                m_renderer = GetComponent<SpriteRenderer>();
            }

            return m_renderer;
        }
    }

    public Sprite spriteToCopy
    {
        get
        {
            if (m_rendererToCopy != null) return m_rendererToCopy.sprite;
            return m_ImageToCopy.sprite;
        }
    }

    public Sprite sprite
    {
        get
        {
            if (m_renderer != null) return m_renderer.sprite;
            return m_Image.sprite;
        }

        set
        {
            if (m_renderer != null) m_renderer.sprite = value;
            else m_Image.sprite = value;
        }
    }

    private void Awake()
    {
        m_renderer = GetComponent<SpriteRenderer>();
        m_Image = GetComponent<Image>();
    }

    public void Update()
    {
        if (spriteToCopy && spriteToCopy != sprite)
        {
            sprite = spriteToCopy;
        }
    }
}
