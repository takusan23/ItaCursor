using System;
using System.Runtime.InteropServices;
using ItaCursor.WindowsAPI;

namespace ItaCursor.WindowsAPITool
{
    class WindowsCoreAudioAPITool
    {
        private static IAudioEndpointVolume Vol()
        {
            var enumerator = new MMDeviceEnumeratorComObject() as IMMDeviceEnumerator;
            IMMDevice dev = null;
            Marshal.ThrowExceptionForHR(enumerator.GetDefaultAudioEndpoint(0, 1, out dev));
            IAudioEndpointVolume epv = null;
            var epvid = typeof(IAudioEndpointVolume).GUID;
            Marshal.ThrowExceptionForHR(dev.Activate(ref epvid, 23, 0, out epv));
            return epv;
        }

        /// <summary>
        /// マスター音量を設定する
        /// </summary>
        /// <param name="volume">0.0fから1.0f</param>
        public static void SetMasterSoundVolume(float volume) => Marshal.ThrowExceptionForHR(Vol().SetMasterVolumeLevelScalar(volume, Guid.Empty));

        /// <summary>
        /// 現在のマスター音量を取得する
        /// </summary>
        /// <returns>音量</returns>
        public static float GetMasterSoundVolume()
        {
            float volume;
            Marshal.ThrowExceptionForHR(Vol().GetMasterVolumeLevelScalar(out volume));
            return volume;
        }

    }
}
