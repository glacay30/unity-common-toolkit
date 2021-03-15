using UnityEngine;

public class ImageFillAmountSetter : MonoBehaviour
{
    [SerializeField] private FloatVariable Value = null;

    /// <summary>
    /// Should the output be opposite of the input (i.e. in 1 = out 0)?
    /// </summary>
    [SerializeField] private bool Invert = false;


    private UnityEngine.UI.Image Image = null;

    private void Start()
    {
        Image = GetComponent<UnityEngine.UI.Image>();
    }

    private void Update()
    {
        if (!Value || !Image) {
            return;
        }

        if (Image.type == UnityEngine.UI.Image.Type.Filled) {
            float fill = Mathf.Clamp01(Value.Value);
            if (Invert) {
                fill = Mathf.Lerp(1.0f, 0.0f, Value.Value);
            }
            Image.fillAmount = fill;
        }
    }
}
