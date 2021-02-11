using UnityEngine;

public class ImageFillAmountSetter : MonoBehaviour
{
    public FloatVariable Value = null;

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
            Image.fillAmount = Mathf.Clamp01(Value.Value);
        }
    }
}
