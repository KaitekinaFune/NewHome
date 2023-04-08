using UnityEngine;
using Utils;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private AudioSource defaultAudioListener;

    [SerializeField] private AudioClip moneyGainedSound;
    [SerializeField] private AudioClip buildingCreatedSound;
    [SerializeField] private AudioClip buildingMinorUpgradedSound;
    [SerializeField] private AudioClip buildingMajorUpgradedSound;
    [SerializeField] private AudioClip buildingDestroyedSound;
    [SerializeField] private AudioClip oreDigSound;
    [SerializeField] private AudioClip oreBreakSound;

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

    private void PlayTargeted(Transform target, AudioClip clip)
    {
        if (!target.TryGetComponent(out AudioSource s))
        {
            s = target.gameObject.AddComponent<AudioSource>();
        }

        s.PlayOneShot(clip);
    }

    private void PlayNonTargeted(AudioClip clip)
    {
        defaultAudioListener.PlayOneShot(clip);
    }
}