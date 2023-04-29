using UnityEngine;
using UnityEngine.UI;


public class FlashWhiteEffect : MonoBehaviour
{
    public float flashWhiteTime = 0.06F;
    public Material normalMat;
    public Material whiteMat;

    private bool flashing;
    private float timer;
    private new SpriteRenderer renderer;
    private Image image;

    public Material mat
    {
        get
        {
            if (renderer != null) return renderer.material;
            return image.material;
        }

        set
        {
            if (renderer != null) renderer.material = value;
            else if (image != null) image.material = value;
        }
    }

    public void Start()
    {
        image = GetComponent<Image>();
        renderer = GetComponent<SpriteRenderer>();
    }

    public void Flash()
    {
        flashing = true;
        timer = flashWhiteTime;
        mat = whiteMat;
    }

    private void Update()
    {
        if (!flashing)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            mat = normalMat;
        }
    }
}
