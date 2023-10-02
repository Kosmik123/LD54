using Bipolar.LoopedRooms;
using Bipolar.RaycastSystem;
using System.Collections;
using UnityEngine;

public class Door : InteractiveObject
{
    public event System.Action<Door> OnDoorOpened;

    [SerializeField]
    private RaycastTarget raycastTarget;
    [SerializeField]
    private Key.Type color;

    [SerializeField, Multiline(2)]
    private string closedHint;
    [SerializeField, Multiline(2)]
    private string openHint;

    [SerializeField]
    private bool isOpen = false;
    public bool IsOpen => isOpen;

    [Header("Open Animation")]
    [SerializeField]
    private SynchronizedTransformController rotatingTranform;
    public SynchronizedTransform RotatingTransform => rotatingTranform.SynchronizedTransform;

    [SerializeField]
    private float openingDuration;
    [SerializeField]
    private float openAngle;

    public override string Hint => KeysManager.instance.HasKey(color) ? openHint : closedHint;

    public void OpenDoor()
    {
        if (isOpen)
            return;

        if (KeysManager.instance.HasKey(color))
        {
            KeysManager.instance.ChangeKeys(color, -1);
            raycastTarget.enabled = false;
            isOpen = true;
            StartCoroutine(OpeningDoorCo());
            OnDoorOpened?.Invoke(this);
        }
    }

    private IEnumerator OpeningDoorCo()
    {
        Quaternion closedRotation = rotatingTranform.SynchronizedTransform.LocalRotation;
        Quaternion openRotation = Quaternion.AngleAxis(openAngle, Vector3.up);
        float progress = 0;
        float speed = 1f / openingDuration;
        while (progress < 1)
        {
            progress += Time.deltaTime * speed;
            rotatingTranform.SynchronizedTransform.LocalRotation = Quaternion.Lerp(closedRotation, openRotation, progress);
            yield return null;
        }
        rotatingTranform.SynchronizedTransform.LocalRotation = openRotation;
    }
}
