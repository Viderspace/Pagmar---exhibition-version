using TMPro;
using UnityEngine;

public class AnimatedFont : MonoBehaviour
{
    [SerializeField]private TMP_Text textMaterial;
    [SerializeField][Range(0f,.2f)]private float dialationRange = .2f;
    [SerializeField][Range(0f,1f)]private float minDialation = .5f;
    [SerializeField] private float dialationSpeed = 0.1f;

    private void Awake()
    {
        textMaterial = GetComponent<TMP_Text>();
    }

    private void FixedUpdate()
    {
        // Changing the dialation of the font material
        textMaterial.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, Mathf.PingPong(Time.time * dialationSpeed, dialationRange) + minDialation);
    }
}
