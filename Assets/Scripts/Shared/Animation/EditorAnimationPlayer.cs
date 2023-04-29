using Sirenix.OdinInspector;
using UnityEngine;

namespace CrossBlitz.Shared.Animation
{
    public class EditorAnimationPlayer : MonoBehaviour
    {
        public string animationToTestName;

        [Button]
        public void PlayAnimation()
        {
            GetComponent<SpriteAnimation>().Play(animationToTestName);
        }
    }
}