using UnityEngine;

[RequireComponent(typeof(SpriteAnimation))]
public class DestroyAfterAnimation : MonoBehaviour
{
    public string m_animation;
    private SpriteAnimation m_animator;

    public void Start()
    {
        m_animator = GetComponent<SpriteAnimation>();
        m_animator.Play(m_animation, OnAnimationComplete);
    }

    public void OnAnimationComplete()
    {
        Destroy(gameObject);
    }
}
