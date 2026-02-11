using UnityEngine;

public class Colorizer : MonoBehaviour
{
    [SerializeField] private Cube _cube;
    
    private void OnEnable()
    {
        _cube.OnTouched += SetColor;
    }

    private void OnDisable()
    {
        _cube.OnTouched -= SetColor;
    }

    private void SetColor(Cube cube)
    {
        float saturation = 1f;
        float brightness = 1f;
        float maximalHueValue = 1f;
     
        Renderer renderer = cube.GetComponent<Renderer>();
        
        renderer.material.color = Color.HSVToRGB(Random.Range(0.0f, maximalHueValue), saturation, brightness);
    }
}
