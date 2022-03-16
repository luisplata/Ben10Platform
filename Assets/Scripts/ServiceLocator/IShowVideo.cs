using UnityEngine.Video;

namespace ServiceLocatorPath
{
    public interface IShowVideo
    {
        void Play(VideoClip clip, ShowVideo.OnPlayVideo callbackToPlay, ShowVideo.OnPlayVideo callbackToFinish);
        void Stop();
    }
}