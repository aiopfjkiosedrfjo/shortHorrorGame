using UnityEngine;

public class playerItems : MonoBehaviour
{
    private bool flashLightToggle = false;
    [SerializeField] private Light flashLightObject;
    [SerializeField] private AudioSource flashLightSFX;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) FlashLight();
    }
    private void FlashLight()
    {
        flashLightToggle = !flashLightToggle;
        if (flashLightToggle)
        {
            flashLightObject.enabled = true;
            flashLightSFX.pitch = 1f;
            flashLightSFX.Play();
        }
        else
        {
            flashLightObject.enabled = false;
            flashLightSFX.pitch = 0.5f;
            flashLightSFX.Play();
        }
    }
}
