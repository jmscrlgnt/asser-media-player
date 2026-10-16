using System;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;

namespace AsserPlayer.Services
{
    internal sealed class PlaybackService : IDisposable
    {
        private readonly LibVLC _libVlc;
        private readonly MediaPlayer _player;
        private readonly VideoView _videoView;
        private bool _disposed;

        public PlaybackService(VideoView videoView)
        {
            if (videoView == null) throw new ArgumentNullException("videoView");

            Core.Initialize();
            _videoView = videoView;
            _libVlc = new LibVLC("--no-video-title-show", "--quiet");
            _player = new MediaPlayer(_libVlc);
            _videoView.MediaPlayer = _player;

            _player.Playing += delegate { Raise(Playing); };
            _player.Paused += delegate { Raise(Paused); };
            _player.Stopped += delegate { Raise(Stopped); };
            _player.EndReached += delegate { Raise(EndReached); };
            _player.EncounteredError += delegate { Raise(PlaybackError); };
            _player.TimeChanged += delegate { RaisePosition(); };
            _player.LengthChanged += delegate { RaisePosition(); };
        }

        public event Action Playing;
        public event Action Paused;
        public event Action Stopped;
        public event Action EndReached;
        public event Action PlaybackError;
        public event Action<long, long> PositionChanged;

        public bool IsPlaying { get { return !_disposed && _player.IsPlaying; } }
        public long Time { get { return _disposed ? 0 : Math.Max(0, _player.Time); } }
        public long Length { get { return _disposed ? 0 : Math.Max(0, _player.Length); } }
        public bool IsMuted { get { return !_disposed && _player.Mute; } }
        public int Volume { get { return _disposed ? 0 : _player.Volume; } }

        public void Play(string filePath)
        {
            if (_disposed) return;
            using (var media = new Media(_libVlc, new Uri(filePath)))
            {
                if (!_player.Play(media))
                    throw new InvalidOperationException("LibVLC could not start this media file.");
            }
        }

        public void TogglePause()
        {
            if (_disposed) return;
            if (_player.IsPlaying) _player.Pause();
            else _player.Play();
        }

        public void Pause()
        {
            if (!_disposed && _player.IsPlaying) _player.Pause();
        }

        public void Stop()
        {
            if (!_disposed) _player.Stop();
        }

        public void SetVolume(int value)
        {
            if (_disposed) return;
            _player.Volume = Math.Max(0, Math.Min(100, value));
        }

        public bool ToggleMute()
        {
            if (_disposed) return false;
            _player.Mute = !_player.Mute;
            return _player.Mute;
        }

        public void SetMute(bool muted)
        {
            if (!_disposed) _player.Mute = muted;
        }

        public void SeekToRatio(double ratio)
        {
            if (_disposed || _player.Length <= 0) return;
            ratio = Math.Max(0, Math.Min(1, ratio));
            _player.Time = (long)(_player.Length * ratio);
        }

        public void SeekBySeconds(int seconds)
        {
            if (_disposed || _player.Length <= 0) return;
            var target = _player.Time + (seconds * 1000L);
            _player.Time = Math.Max(0, Math.Min(_player.Length, target));
        }

        public void SetRate(float rate)
        {
            if (_disposed) return;
            rate = Math.Max(0.25F, Math.Min(4F, rate));
            _player.SetRate(rate);
        }

        private void Raise(Action handler)
        {
            if (handler != null) handler();
        }

        private void RaisePosition()
        {
            var handler = PositionChanged;
            if (handler != null) handler(Time, Length);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _videoView.MediaPlayer = null;
            _player.Dispose();
            _libVlc.Dispose();
        }
    }
}
