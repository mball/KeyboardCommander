using System;
using System.Collections.Generic;
using KeyboardCommander.Engine.States;
using Microsoft.Xna.Framework.Audio;

namespace KeyboardCommander.Engine.Sound
{
    public class SoundManager
    {
        private int _soundTrackIndex = -1;
        private List<SoundEffectInstance> _soundTracks = new List<SoundEffectInstance>();
        private Dictionary<Type, SoundBankItem> _soundBank = new Dictionary<Type, SoundBankItem>();

        public void SetSoundtrack(List<SoundEffectInstance> tracks)
        {
            _soundTracks = tracks;
            _soundTrackIndex = _soundTracks.Count - 1;
        }

        public void OnNotify(BaseGameStateEvent gameEvent)
        {
            if (_soundBank.ContainsKey(gameEvent.GetType()))
            {
                var sound = _soundBank[gameEvent.GetType()];
                sound.Sound.Play(sound.Attributes.Volume, sound.Attributes.Pitch, sound.Attributes.Pan);
            }
        }

        public void PlaySoundtrack()
        {
            var nbTracks = _soundTracks.Count;

            if (nbTracks <= 0)
            {
                return;
            }

            var currentTrack = _soundTracks[_soundTrackIndex];
            var nextTrack = _soundTracks[(_soundTrackIndex + 1) % nbTracks];

            if (currentTrack.State == SoundState.Stopped)
            {
                nextTrack.Play();
                _soundTrackIndex++;

                if (_soundTrackIndex >= _soundTracks.Count)
                {
                    _soundTrackIndex = 0;
                }
            }
        }

        public void RegisterSound(BaseGameStateEvent gameEvent, SoundEffect sound)
        {
            RegisterSound(gameEvent, sound, 1.0f, 0.0f, 0.0f);
        }

        internal void RegisterSound(BaseGameStateEvent gameEvent, SoundEffect sound,
                                    float volume, float pitch, float pan)
        {
            _soundBank.Add(gameEvent.GetType(), new SoundBankItem(sound, new SoundAttributes(volume, pitch, pan)));
        }
    }
}
