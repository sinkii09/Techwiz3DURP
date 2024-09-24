using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture;
    public RawImage screenImage;
    public VideoClip videoSource;
    void Start()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        // Assign the Render Texture to the Video Player
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.clip = videoSource;
        screenImage.texture = renderTexture;
        // Play the video
        videoPlayer.Play();
        videoPlayer.isLooping = true;
    }
}