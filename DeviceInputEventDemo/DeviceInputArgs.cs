using System.Windows;
using System.Windows.Input;

namespace DeviceInputEventDemo
{
    /// <summary>
    /// 设备事件参数
    /// </summary>
    public class DeviceInputArgs : RoutedEventArgs
    {
        /// <summary>
        /// 初始化 <see cref="DeviceInputArgs" /> 类的新实例。
        /// <para>默认设备为鼠标左键</para>
        /// </summary>
        public DeviceInputArgs() : this(-1, new StylusPointCollection())
        {
        }
        /// <summary>
        /// 初始化 <see cref="DeviceInputArgs" /> 类的新实例。
        /// </summary>
        /// <param name="deviceId">设备id</param>
        /// <param name="pointCollection">点集</param>
        public DeviceInputArgs(int deviceId, StylusPointCollection pointCollection) : this(deviceId, pointCollection, 0)
        {
        }
        /// <summary>
        /// 初始化 <see cref="DeviceInputArgs" /> 类的新实例。
        /// </summary>
        /// <param name="deviceId">设备id</param>
        /// <param name="pointCollection">点集</param>
        /// <param name="timestamp">发生输入的时间</param>
        public DeviceInputArgs(int deviceId, StylusPointCollection pointCollection, int timestamp)
        {
            DeviceId = deviceId;
            Points = pointCollection;
            TimeStamp = timestamp;
        }

        /// <summary>
        /// 设备ID
        /// <para>默认为鼠标设备，鼠标左键-1，鼠标右键-2 </para>
        /// </summary>
        public int DeviceId { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public DeviceType DeviceType { get; set; }

        /// <summary>
        /// 位置-延迟获取(用于提升设备事件参数初始化时的性能)
        /// </summary>
        public Lazy<Point> PositionLazy { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public Point Position
        {
            get => PositionLazy?.Value ?? default;
            set => PositionLazy = new Lazy<Point>(() => value);
        }

        /// <summary>
        /// 触笔点集-延迟获取(用于提升设备事件参数初始化时的性能)
        /// </summary>
        public Lazy<StylusPointCollection> PointsLazy { get; set; }

        /// <summary>
        /// 触笔点集
        /// </summary>
        public StylusPointCollection Points
        {
            get => PointsLazy?.Value;
            set => PointsLazy = new Lazy<StylusPointCollection>(() => value);
        }

        /// <summary>
        /// 获取相对元素位置的函数(用于提升设备事件参数初始化时的性能)
        /// </summary>
        internal Func<FrameworkElement, InputEventArgs, Point> GetPositionFunc { get; set; }
        /// <summary>
        /// 获取相对元素的位置
        /// </summary>
        public Func<FrameworkElement, Point> GetPosition
        {
            get => relativeElement => GetPositionFunc(relativeElement, SourceArgs);
            set => GetPositionFunc = (relativeElement, args) => value(relativeElement);
        }

        /// <summary>
        /// 事件触发源数据
        /// </summary>
        public InputEventArgs SourceArgs { get; set; }
        /// <summary>
        /// 事件触发时的时间
        /// </summary>
        public int TimeStamp { get; }
        /// <summary>
        /// 触摸面积
        /// </summary>
        public Rect TouchArea => TouchAreaLazy?.Value ?? Rect.Empty;
        /// <summary>
        /// 触摸面积
        /// </summary>
        public Lazy<Rect> TouchAreaLazy { get; set; }
    }
}
