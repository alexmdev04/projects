using UnityEngine;

public class uiRadarCamera : MonoBehaviour
{ // this script handles the position and rotation of the radar camera
    [SerializeField] float 
        yPos = 7.5f,
        lerpSpeed = 20f;
    [SerializeField] bool
		rotateWithPlayer = true;
    [SerializeField] GameObject
        playerIcon;
    void Update()
    {
        // lerps the radar camera to the players position, y value is set by yPos
        // if playerRotate is false the camera will not rotate with the player, if true it will rotate with the players y axis rotation flipped
        transform.SetPositionAndRotation(
            Vector3.Lerp(transform.position, new Vector3(Player.instance.transform.position.x, yPos, Player.instance.transform.position.z), Time.deltaTime * lerpSpeed),
            Quaternion.Euler(90f, 0f, rotateWithPlayer ? -Player.instance.transform.eulerAngles.y : 0f));

        // the player icon is always in the center of the radar, if rotateWithPlayer is false the icon will rotate with the players y axis rotation flipped, if true the icon is static
        playerIcon.transform.rotation = 
            Quaternion.Euler(0f, 0f, rotateWithPlayer ? 0 : -Player.instance.transform.eulerAngles.y);
    }
}