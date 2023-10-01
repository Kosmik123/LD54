using Bipolar.LoopedRooms;
using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DeathSequence : MonoBehaviour
{
    [SerializeField]
    private Movement playerMovement;

    [Header("Looking At Angel")]
    [SerializeField]
    private float lookAtAngelTransitionDuration;

    [Header("Screen Fade Out")]
    [SerializeField]
    private VolumeProfile postprocessingProfile;
    private Vignette vignetteEffect;
    private ColorAdjustments colorAdjustments;
    
    [SerializeField]
    private float fadeOutDuration;
    [SerializeField]
    private AnimationCurve vignetteIntensityFadeOutCurve;
    [SerializeField]
    private AnimationCurve screenExposureFadeOutCurve;

    [Header("Respawn In Random Room")]
    [SerializeField]
    private RoomsManager roomsManager;
    [SerializeField]
    private VisitedRoomsTracker visitedRoomsTracker;
    [SerializeField]
    private float teleportingDuration;

    [Header("Screen Fade In")]
    [SerializeField]
    private CinemachineVirtualCamera playerDownCamera;
    [SerializeField]
    private float fadeInDuration;
    [SerializeField]
    private AnimationCurve vignetteIntensityFadeInCurve;
    [SerializeField]
    private AnimationCurve screenExposureFadeInCurve;

    [Header("Player Standin Up")]
    [SerializeField]
    private float standUpDelay;
    [SerializeField]
    private float standingUpDuration;
    [SerializeField]
    private CinemachineVirtualCamera playerCamera;

    private Coroutine currentSequence;

    private const string angelGrabTag = "Angel Grab";

    private void Awake()
    {
        postprocessingProfile.TryGet(out vignetteEffect);
        postprocessingProfile.TryGet(out colorAdjustments);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(angelGrabTag) && currentSequence == null)
        {
            var angel = other.GetComponentInParent<SynchronizedTransformController>();
            currentSequence = StartCoroutine(DeathSequenceCo(angel));
        }
    }

    private IEnumerator DeathSequenceCo(SynchronizedTransformController angel)
    {
        playerMovement.enabled = false;

        var angelLookingCamera = angel.GetComponentInChildren<CinemachineVirtualCamera>();
        angelLookingCamera.Priority = 100;
        playerCamera.Priority = 0;
        var lookAtAngelWait = new WaitForSeconds(lookAtAngelTransitionDuration);
        yield return lookAtAngelWait;

        float progress = 0;
        float fadeSpeed = 1f / fadeOutDuration;
        while (progress < 1.1f)
        {
            progress += fadeSpeed * Time.deltaTime;
            colorAdjustments.postExposure.value = screenExposureFadeOutCurve.Evaluate(progress);
            vignetteEffect.intensity.value = vignetteIntensityFadeOutCurve.Evaluate(progress);
            yield return null;
        }

        roomsManager.enabled = false;
        var teleportingWait = new WaitForSeconds(teleportingDuration / 2);
        yield return teleportingWait;
        var visitedRooms = visitedRoomsTracker.VisitedRooms;
        Room respawnRoom;
        while (true)
        {
            yield return null;
            int randomIndex = Random.Range(0, visitedRooms.Count);
            respawnRoom = visitedRooms[randomIndex];
            if (respawnRoom.GetComponentInChildren<ChaseTarget>() == null)
            {
                angel.SynchronizedTransform.LocalPosition = Vector3.zero;
                roomsManager.TeleportToRoom(respawnRoom);
                break;
            }
        }
        playerDownCamera.Priority = 100;
        angelLookingCamera.Priority = 0;
        roomsManager.enabled = true;
        yield return teleportingWait;

        progress = 0;
        fadeSpeed = 1f / fadeInDuration;
        while (progress < 1.1f)
        {
            progress += fadeSpeed * Time.deltaTime;
            colorAdjustments.postExposure.value = screenExposureFadeInCurve.Evaluate(progress);
            vignetteEffect.intensity.value = vignetteIntensityFadeInCurve.Evaluate(progress);
            yield return null;
        }
        var standUpDelayWait = new WaitForSeconds(standUpDelay);
        yield return standUpDelayWait;
        
        playerCamera.transform.localRotation = Quaternion.identity;
        playerCamera.Priority = 100;
        playerDownCamera.Priority = 0;
        var standingUpWait = new WaitForSeconds(standingUpDuration);
        yield return standingUpWait;

        playerMovement.enabled = true;
        currentSequence = null;
    }
}