using UnityEngine;

public class MiniCameraManager : MonoBehaviour {
    public new Camera camera;
    public Vector3 fixPosition;
    public GameObject character;
    public float maxSize;
    public float minSize;
    public float dSize;

	// Use this for initialization
	void Start ()
    {
        camera = GetComponent<Camera>();
        FixOnCharacter();
    }
	
    public void FixOnCharacter()
    {
        transform.position = character.transform.position + fixPosition;
    }
    public void OnDownButtonClick()
    {
        if (camera.orthographicSize > minSize)
            camera.orthographicSize -= dSize;
    }
    public void OnUpButtonClick()
    {
        if (camera.orthographicSize < maxSize)
            camera.orthographicSize += dSize;
    }
}
