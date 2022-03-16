using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace ServiceLocatorPath
{
    public class ShowVideo : MonoBehaviour, IShowVideo
    {
        [SerializeField] private GameObject father;
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private Button closeVideo;
        
        public delegate void OnPlayVideo();
        
        private OnPlayVideo callbackToFinish;

        private void Start()
        {
            closeVideo.onClick.AddListener(StopVideo);
            StopVideo();
        }

        private void PlayVideo(VideoClip clip)
        {
            father.SetActive(true);
            videoPlayer.clip = clip;
            videoPlayer.Play();
        }

        private void StopVideo()
        {
            videoPlayer.Stop();
            videoPlayer.clip = null;
            father.SetActive(false);
            callbackToFinish?.Invoke();
            callbackToFinish = null;
        }

        public async void Play(VideoClip clip, OnPlayVideo callbackToPlay, OnPlayVideo callbackToFinish)
        {
            this.callbackToFinish = callbackToFinish;
            callbackToPlay?.Invoke();
            PlayVideo(clip);
            await UniTask.Delay(TimeSpan.FromSeconds(clip.length));
            StopVideo();
        }

        public void Stop()
        {
            StopVideo();
        }
    }
}