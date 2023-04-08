using System;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

public class AudioManager : Singleton<AudioManager>
{
    [FormerlySerializedAs("defaultAudioListener")] [SerializeField]
    private AudioSource defaultAudioSource;

    [SerializeField] private ClipData moneyGainedSound;
    [SerializeField] private ClipData buildingCreatedSound;
    [SerializeField] private ClipData buildingMinorUpgradedSound;
    [SerializeField] private ClipData buildingMajorUpgradedSound;
    [SerializeField] private ClipData buildingDestroyedSound;
    [SerializeField] private ClipData oreDigSound;
    [SerializeField] private ClipData oreBreakSound;
    [SerializeField] private ClipData uiClick1;
    [SerializeField] private ClipData wooshSound;
    [SerializeField] private ClipData shipArrivedSound;
    [SerializeField] private ClipData shipShootSound;
    [SerializeField] private ClipData shipDestroyedSound;
    [SerializeField] private ClipData turretShootSound;

    public void PlayMoneyGainedSound()
    {
        PlayNonTargeted(moneyGainedSound);
    }

    public void PlayBuildingMinorUpgrade(Transform target)
    {
        PlayTargeted(target, buildingMinorUpgradedSound);
    }

    public void PlayBuildingMajorUpgrade(Transform target)
    {
        PlayTargeted(target, buildingMajorUpgradedSound);
    }

    public void PlayOreDigSound(Transform target)
    {
        PlayTargeted(target, oreDigSound);
    }

    public void PlayOreBreakSound(Transform target)
    {
        PlayTargeted(target, oreBreakSound);
    }

    public void PlayBuildingDestroyedSound(Transform target)
    {
        PlayTargeted(target, buildingDestroyedSound);
    }

    public void PlayBuildingCreatedSound(Transform target)
    {
        PlayTargeted(target, buildingCreatedSound);
    }

    public void PlayUI1()
    {
        PlayNonTargeted(uiClick1);
    }

    public void PlayWhoosh()
    {
        PlayNonTargeted(wooshSound);
    }

    public void PlayShipArrivedSound(Transform target)
    {
        PlayTargeted(target, shipArrivedSound);
    }

    public void PlayShipShootSound(Transform target)
    {
        PlayTargeted(target, shipShootSound);
    }

    public void PlayShipDestroyedSound(Transform target)
    {
        PlayTargeted(target, shipDestroyedSound);
    }

    public void PlayTurretShootSound(Transform target)
    {
        PlayTargeted(target, turretShootSound);
    }

    private void PlayTargeted(Transform target, ClipData clipData)
    {
        if (!target.TryGetComponent(out AudioSource s))
        {
            s = target.gameObject.AddComponent<AudioSource>();
        }

        s.PlayOneShot(clipData.Clip, clipData.Volume);
    }

    private void PlayNonTargeted(ClipData clipData)
    {
        defaultAudioSource.PlayOneShot(clipData.Clip, clipData.Volume);
    }

    [Serializable]
    public class ClipData
    {
        public AudioClip Clip;
        [Range(0, 1f)] public float Volume = 1f;
    }
}