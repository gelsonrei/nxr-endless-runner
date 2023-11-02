using UnityEngine;

[ExecuteInEditMode]
public class WorldCurverShaderControl : MonoBehaviour
{
	[Range(-0.1f, 0.1f)]
	public float curveStrength = 0.01f;
	public float speed = 0.01f;

    int m_PropertyID;

    private void OnEnable()
    {
        m_PropertyID = Shader.PropertyToID("_CurveStrength");
    }

	void Start()
	{
		Shader.SetGlobalFloat(m_PropertyID, curveStrength);
		RenderSettings.skybox.SetFloat("_Rotation", 0);
	}

	void Update()
	{
		// Get the current rotation of the skybox
		float rotation = RenderSettings.skybox.GetFloat("_Rotation");

		// Rotate the skybox by the specified amount
		rotation += Time.deltaTime * speed;

		// Set the rotation of the skybox
		RenderSettings.skybox.SetFloat("_Rotation", rotation);
	}

	private void OnApplicationQuit() 
    {
        RenderSettings.skybox.SetFloat("_Rotation", 0);
    }
}
