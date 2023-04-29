using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaskAnimationCopier : MonoBehaviour
{
    public SpriteRenderer m_renderer;
    protected SpriteMask m_spriteMask;

    private void Awake()
    {
        m_spriteMask = GetComponent<SpriteMask>();
    }

    private void Update()
    {
        m_spriteMask.sprite = m_renderer.sprite;
    }
}
