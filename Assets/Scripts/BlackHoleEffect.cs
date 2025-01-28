using UnityEngine;
using UnityEngine.Rendering;

public class BlackHoleEffect : MonoBehaviour
{
    public Material blackHoleMaterial;
    private Camera mainCamera;
    private RenderTexture renderTexture;
    private CommandBuffer commandBuffer;

    void Start()
    {
        mainCamera = Camera.main;
        renderTexture = new RenderTexture(Screen.width, Screen.height, 16);
        blackHoleMaterial.SetTexture("_MainTex", renderTexture);

        commandBuffer = new CommandBuffer();
        commandBuffer.name = "Black Hole Capture";

        commandBuffer.Blit(BuiltinRenderTextureType.CurrentActive, renderTexture);
        mainCamera.AddCommandBuffer(CameraEvent.AfterEverything, commandBuffer);
    }

    void Update()
    {
        if (renderTexture.width != Screen.width || renderTexture.height != Screen.height)
        {
            renderTexture.Release();
            renderTexture = new RenderTexture(Screen.width, Screen.height, 16);
            blackHoleMaterial.SetTexture("_MainTex", renderTexture);
        }

        // Capture the scene
        Graphics.Blit(null, renderTexture);
    }


    void OnDestroy()
    {
        if (commandBuffer != null)
        {
            mainCamera.RemoveCommandBuffer(CameraEvent.AfterEverything, commandBuffer);
        }
    }
}
