using UnityEngine;
using UnityEngine.Playables;
using Utils;

public class IntroSkip : Singleton<IntroSkip>
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private float timeToSkipTo;

    public bool Skipped;

    private void Update()
    {
        if (Input.anyKeyDown && !Skipped)
        {
            Skipped = true;
            playableDirector.time = timeToSkipTo;
        }
    }
}