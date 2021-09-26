namespace ItaCursor.ToolWindow.ViewModel
{
    class VolumeControlWindowViewModel
    {

        /// <summary>
        /// 音量をバインディングするのでー
        /// 
        /// StringFormat=N0 をつけると 整数の値になります？
        /// </summary>
        public float MasterVolume
        {
            get => WindowsAPITool.WindowsCoreAudioAPITool.GetMasterSoundVolume() * 100;
            set => WindowsAPITool.WindowsCoreAudioAPITool.SetMasterSoundVolume(value / 100f);
        }

    }
}
