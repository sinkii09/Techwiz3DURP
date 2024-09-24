using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GuideController : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip jumpVideoClip;
    [SerializeField] VideoClip doubleJumpVideoClip;
    [SerializeField] VideoClip dashOnGroundVideoClip;
    [SerializeField] VideoClip dashOnAirVideoClip;
    [SerializeField] VideoClip checkPointVideoClip;
    [SerializeField] VideoClip gemCollectVideoClip;
    [SerializeField] VideoClip trapHurtVideoClip;
    [SerializeField] VideoClip movingForward;
    [SerializeField] VideoClip movingBackward;

    private List<VideoClip> videoClips;
    private int currentClipIndex = 0;

    public void PlayVideo(VideoClip clip)
    {
        if (clip != null && videoPlayer != null)
        {
            videoPlayer.clip = clip;
            videoPlayer.Play();
        }
    }

    public void PlayJumpVideoClip()
    {
        PlayVideo(jumpVideoClip);
    }

    public void PlayDoubleJumpVideoClip()
    {
        PlayVideo(doubleJumpVideoClip);
    }

    public void PlayDashOnGroundVideoClip()
    {
        PlayVideo(dashOnGroundVideoClip);
    }

    public void PlayDashOnAirVideoClip()
    {
        PlayVideo(dashOnAirVideoClip);
    }

    public void PlayCheckPointVideoClip()
    {
        PlayVideo(checkPointVideoClip);
    }

    public void PlayGemCollectVideoClip()
    {
        PlayVideo(gemCollectVideoClip);
    }

    public void PlayTrapHurtVideoClip()
    {
        PlayVideo(trapHurtVideoClip);
    }
    public void PlayMoveForwardVideoClip()
    {
        PlayVideo(movingForward);
    }
    public void PlayMoveBackWardVideoClip()
    {
        PlayVideo(movingBackward);
    }
    void Start()
    {
        // Initialize the list of video clips
        videoClips = new List<VideoClip>
        {
            jumpVideoClip,
            doubleJumpVideoClip,
            dashOnGroundVideoClip,
            dashOnAirVideoClip,
            checkPointVideoClip,
            gemCollectVideoClip,
            trapHurtVideoClip
        };
    }

    void Update()
    {
        // Check for input to play the next clip
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayNextVideoClip();
        }
    }
    public void PlayVideos(VideoClip clip)
    {
        if (clip != null && videoPlayer != null)
        {
            videoPlayer.clip = clip;
            videoPlayer.Play();
            videoPlayer.isLooping = true;
        }
    }

    public void PlayNextVideoClip()
    {
        if (videoClips.Count == 0) return; // No clips available

        PlayVideos(videoClips[currentClipIndex]);

        // Move to the next clip, wrap around if necessary
        currentClipIndex = (currentClipIndex + 1) % videoClips.Count;
    }
}
