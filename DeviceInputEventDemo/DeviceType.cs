using System.ComponentModel;

namespace DeviceInputEventDemo
{
    /// <summary>
    /// 硬件设备类型
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// 鼠标
        /// </summary>
        Mouse,
        /// <summary>
        /// 触笔
        /// </summary>
        Pen,
        /// <summary>
        /// 触摸
        /// </summary>
        Touch,
    }
    /// <summary>
    /// 鼠标类型
    /// </summary>
    public enum MouseDeviceType 
    {
        /// <summary>
        /// 鼠标左键
        /// </summary>
        [Description("鼠标左键")]
        LeftButton = -1,
        /// <summary>
        /// 鼠标右键
        /// </summary>
        [Description("鼠标右键")]
        RightButton = -2
    }
}
