using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DeviceInputEventDemo
{
    /// <summary>
    /// 设备点击事件转换类
    /// <para>统一转换成事件的按压、移动、抬起三种类型</para>
    /// </summary>
    public class DeviceEventTransformer
    {
        private readonly FrameworkElement _element;

        /// <summary>
        /// 提供<see cref="DeviceEventTransformer"/>类实例的初始化
        /// </summary>
        /// <param name="element"></param>
        public DeviceEventTransformer(FrameworkElement element)
        {
            _element = element;
        }

        #region 订阅/取消

        #region 订阅

        /// <summary>
        /// 订阅点击事件
        /// </summary>
        public void Register()
        {
            RegisterMouse();
            RegisterStylus();
            RegisterTouch();
        }

        private void RegisterTouch()
        {
            //冒泡事件
            _element.TouchDown += Element_TouchDown;
            _element.TouchMove += Element_TouchMove;
            _element.TouchUp += Element_TouchUp;

            //隧道事件
            _element.PreviewTouchDown += Element_PreviewTouchDown;
            _element.PreviewTouchMove += Element_PreviewTouchMove;
            _element.PreviewTouchUp += Element_PreviewTouchUp;
        }

        private void RegisterStylus()
        {
            //触笔-冒泡事件
            _element.StylusDown += Element_StylusDown;
            _element.StylusMove += Element_StylusMove;
            _element.StylusUp += Element_StylusUp;

            //触笔-隧道事件
            _element.PreviewStylusDown += Element_PreviewStylusDown;
            _element.PreviewStylusMove += Element_PreviewStylusMove;
            _element.PreviewStylusUp += Element_PreviewStylusUp;
        }

        private void RegisterMouse()
        {
            //鼠标-冒泡
            if (_element is Button button)
            {
                //Button类型的冒泡事件，被内部消化处理了，所以需要重新添加路由事件订阅
                button.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(Button_MouseLeftButtonDown), true);
                button.AddHandler(UIElement.MouseRightButtonDownEvent, new MouseButtonEventHandler(Button_MouseRightButtonDown), true);
                button.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Button_MouseLeftButtonUp), true);
                button.AddHandler(UIElement.MouseRightButtonUpEvent, new MouseButtonEventHandler(Button_MouseRightButtonUp), true);
            }
            else
            {
                _element.MouseLeftButtonDown += Button_MouseLeftButtonDown;
                _element.MouseRightButtonDown += Button_MouseRightButtonDown;
                _element.MouseLeftButtonUp += Button_MouseLeftButtonUp;
                _element.MouseRightButtonUp += Button_MouseRightButtonUp;
            }
            _element.MouseMove += Element_MouseMove;

            //鼠标-隧道事件
            _element.PreviewMouseLeftButtonDown += Button_PreviewMouseLeftButtonDown;
            _element.PreviewMouseRightButtonDown += Button_PreviewMouseRightButtonDown;
            _element.PreviewMouseLeftButtonUp += Button_PreviewMouseLeftButtonUp;
            _element.PreviewMouseRightButtonUp += Button_PreviewMouseRightButtonUp;
            _element.PreviewMouseMove += Element_PreviewMouseMove;
        }

        #endregion

        #region 注销

        /// <summary>
        /// 注销点击事件
        /// </summary>
        public void UnRegister()
        {
            UnRegisterMouse();
            UnRegisterStylus();
            UnRegisterTouch();
        }

        private void UnRegisterTouch()
        {
            //触摸-冒泡
            _element.TouchDown -= Element_TouchDown;
            _element.TouchMove -= Element_TouchMove;
            _element.TouchUp -= Element_TouchUp;

            //触摸-隧道事件
            _element.PreviewTouchDown -= Element_PreviewTouchDown;
            _element.PreviewTouchMove -= Element_PreviewTouchMove;
            _element.PreviewTouchUp -= Element_PreviewTouchUp;
        }

        private void UnRegisterStylus()
        {
            //触笔-冒泡
            _element.StylusDown -= Element_StylusDown;
            _element.StylusMove -= Element_StylusMove;
            _element.StylusUp -= Element_StylusUp;

            //触笔-隧道事件
            _element.PreviewStylusDown -= Element_PreviewStylusDown;
            _element.PreviewStylusMove -= Element_PreviewStylusMove;
            _element.PreviewStylusUp -= Element_PreviewStylusUp;
        }

        private void UnRegisterMouse()
        {
            //鼠标-冒泡
            if (_element is Button button)
            {
                button.RemoveHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(Button_MouseLeftButtonDown));
                button.RemoveHandler(UIElement.MouseRightButtonDownEvent, new MouseButtonEventHandler(Button_MouseRightButtonDown));
                button.RemoveHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Button_MouseLeftButtonUp));
                button.RemoveHandler(UIElement.MouseRightButtonUpEvent, new MouseButtonEventHandler(Button_MouseRightButtonUp));
            }
            else
            {
                _element.MouseLeftButtonDown -= Button_MouseLeftButtonDown;
                _element.MouseRightButtonDown -= Button_MouseRightButtonDown;
                _element.MouseLeftButtonUp -= Button_MouseLeftButtonUp;
                _element.MouseRightButtonUp -= Button_MouseRightButtonUp;
            }
            _element.MouseMove -= Element_MouseMove;
            //鼠标-隧道
            _element.PreviewMouseLeftButtonDown -= Button_PreviewMouseLeftButtonDown;
            _element.PreviewMouseRightButtonDown -= Button_PreviewMouseRightButtonDown;
            _element.PreviewMouseLeftButtonUp -= Button_PreviewMouseLeftButtonUp;
            _element.PreviewMouseRightButtonUp -= Button_PreviewMouseRightButtonUp;
            _element.PreviewMouseMove -= Element_PreviewMouseMove;
        }

        #endregion

        #endregion

        #region 内部方法

        #region 隧道-触摸事件

        private void Element_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            PreviewDeviceUp?.Invoke(_element, CreateTouchInputArgs(e));
        }
        private void Element_PreviewTouchMove(object sender, TouchEventArgs e)
        {
            PreviewDeviceMove?.Invoke(_element, CreateTouchInputArgs(e));
        }

        private void Element_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            PreviewDeviceDown?.Invoke(_element, CreateTouchInputArgs(e, true));
        }

        #endregion

        #region 隧道-触笔事件

        private void Element_PreviewStylusUp(object sender, StylusEventArgs e)
        {
            //如果支持触摸事件，则对Stylus事件内的触摸输入数据屏蔽掉。
            if (e.StylusDevice.TabletDevice.Type != TabletDeviceType.Stylus)
            {
                return;
            }
            PreviewDeviceUp?.Invoke(_element, CreateStylusInputArgs(e));
        }
        private void Element_PreviewStylusMove(object sender, StylusEventArgs e)
        {
            //如果支持触摸事件，则对Stylus事件内的触摸输入数据屏蔽掉。
            if (e.StylusDevice.TabletDevice.Type != TabletDeviceType.Stylus)
            {
                return;
            }
            PreviewDeviceMove?.Invoke(_element, CreateStylusInputArgs(e));
        }

        private void Element_PreviewStylusDown(object sender, StylusDownEventArgs e)
        {
            //如果支持触摸事件，则对Stylus事件内的触摸输入数据屏蔽掉。
            if (e.StylusDevice.TabletDevice.Type != TabletDeviceType.Stylus)
            {
                return;
            }
            PreviewDeviceDown?.Invoke(_element, CreateStylusInputArgs(e));
        }
        #endregion

        #region 隧道-鼠标事件

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="e"></param>
        /// <param name="deviceId">-1 左键；-2 右键</param>
        private void Element_PreviewMouseDown(MouseEventArgs e, int deviceId)
        {
            if (e.StylusDevice != null) return;
            PreviewDeviceDown?.Invoke(_element, CreateMouseInputArgs(e, deviceId));
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="e"></param>
        /// <param name="deviceId">-1 左键；-2 右键</param>
        private void Element_PreviewMouseUp(MouseEventArgs e, int deviceId)
        {
            if (e.StylusDevice != null) return;
            PreviewDeviceUp?.Invoke(_element, CreateMouseInputArgs(e, deviceId));
        }
        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Element_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.StylusDevice != null) return;
            PreviewDeviceMove?.Invoke(_element, CreateMouseInputArgs(e, -1));
        }
        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Element_PreviewMouseDown(e, (int)MouseDeviceType.LeftButton);
        }
        private void Button_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Element_PreviewMouseDown(e, (int)MouseDeviceType.RightButton);
        }
        private void Button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Element_PreviewMouseUp(e, (int)MouseDeviceType.LeftButton);
        }
        private void Button_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Element_PreviewMouseUp(e, (int)MouseDeviceType.RightButton);
        }

        #endregion

        #region 冒泡-触摸事件

        /// <summary>
        /// 触笔按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Element_TouchUp(object sender, TouchEventArgs e)
        {
            DeviceUp?.Invoke(_element, CreateTouchInputArgs(e));
        }
        /// <summary>
        /// 触笔移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Element_TouchMove(object sender, TouchEventArgs e)
        {
            DeviceMove?.Invoke(_element, CreateTouchInputArgs(e));
        }
        /// <summary>
        /// 触笔抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Element_TouchDown(object sender, TouchEventArgs e)
        {
            DeviceDown?.Invoke(_element, CreateTouchInputArgs(e, true));
        }

        #endregion

        #region 冒泡-触笔事件

        /// <summary>
        /// 触笔按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Element_StylusUp(object sender, StylusEventArgs e)
        {
            //如果支持触摸事件，则对Stylus事件内的触摸输入数据屏蔽掉。
            if (e.StylusDevice.TabletDevice.Type != TabletDeviceType.Stylus)
            {
                return;
            }
            DeviceUp?.Invoke(_element, CreateStylusInputArgs(e));
        }
        /// <summary>
        /// 触笔移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Element_StylusMove(object sender, StylusEventArgs e)
        {
            //如果支持触摸事件，则对Stylus事件内的触摸输入数据屏蔽掉。
            if (e.StylusDevice.TabletDevice.Type != TabletDeviceType.Stylus)
            {
                return;
            }
            DeviceMove?.Invoke(_element, CreateStylusInputArgs(e));
        }
        /// <summary>
        /// 触笔抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Element_StylusDown(object sender, StylusEventArgs e)
        {
            //如果支持触摸事件，则对Stylus事件内的触摸输入数据屏蔽掉。
            if (
                e.StylusDevice.TabletDevice.Type != TabletDeviceType.Stylus)
            {
                return;
            }
            DeviceDown?.Invoke(_element, CreateStylusInputArgs(e));
        }

        #endregion

        #region 冒泡-鼠标事件
        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="e"></param>
        /// <param name="deviceId">-1 左键；-2 右键</param>
        private void Element_MouseDown(MouseEventArgs e, int deviceId)
        {
            if (e.StylusDevice != null) return;
            DeviceDown?.Invoke(_element, CreateMouseInputArgs(e, deviceId));
        }
        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="e"></param>
        /// <param name="deviceId">-1 左键；-2 右键</param>
        private void Element_MouseUp(MouseEventArgs e, int deviceId)
        {
            if (e.StylusDevice != null) return;
            DeviceUp?.Invoke(_element, CreateMouseInputArgs(e, deviceId));
        }
        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.StylusDevice != null) return;
            DeviceMove?.Invoke(_element, CreateMouseInputArgs(e, -1));
        }
        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Element_MouseDown(e, (int)MouseDeviceType.LeftButton);
        }
        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Element_MouseDown(e, (int)MouseDeviceType.RightButton);
        }
        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Element_MouseUp(e, (int)MouseDeviceType.LeftButton);
        }
        private void Button_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Element_MouseUp(e, (int)MouseDeviceType.RightButton);
        }

        #endregion

        #region 参数

        /// <summary>
        /// 创建触摸事件的设备参数
        /// </summary>
        /// <param name="e"></param>
        /// <param name="initTouchArea"></param>
        /// <returns></returns>
        private DeviceInputArgs CreateTouchInputArgs(TouchEventArgs e, bool initTouchArea = false)
        {
            var element = _element;
            var inputArgs = new DeviceInputArgs
            {
                DeviceId = e.TouchDevice.Id,
                DeviceType = DeviceType.Touch,
                PositionLazy = new Lazy<Point>(() => e.GetTouchPoint(element).Position),
                PointsLazy = new Lazy<StylusPointCollection>(() => GetTouchPoints(e, element)),
                GetPositionFunc = (source, args) => e.GetTouchPoint(source).Position,
                SourceArgs = e,
            };
            if (initTouchArea)
            {
                inputArgs.TouchAreaLazy = new Lazy<Rect>(() => TouchArea.GetTouchPointArea(e, element));
            }
            return inputArgs;
        }

        /// <summary>
        /// 获取触摸点集
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private StylusPointCollection GetTouchPoints(TouchEventArgs eventArgs, FrameworkElement element)
        {
            // 临时去除description
            var pointCollection = new StylusPointCollection();
            var points = eventArgs.GetIntermediateTouchPoints(element);
            foreach (var stylusPoint in points)
            {
                var point = stylusPoint.Position;
                pointCollection.Add(new StylusPoint(point.X, point.Y, 0.5f));
            }
            return pointCollection;
        }
        /// <summary>
        /// 创建触摸事件的设备参数
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private DeviceInputArgs CreateStylusInputArgs(StylusEventArgs e)
        {
            var deviceType = e.StylusDevice.TabletDevice.Type == TabletDeviceType.Stylus
                ? DeviceType.Pen
                : DeviceType.Touch;
            var inputArgs = new DeviceInputArgs()
            {
                DeviceId = e.StylusDevice.Id,
                DeviceType = deviceType,
                PositionLazy = new Lazy<Point>(() => e.GetPosition(_element)),
                PointsLazy = new Lazy<StylusPointCollection>(() => GetStylusPoints(e, _element)),
                GetPositionFunc = (element, args) => e.GetPosition(element),
                SourceArgs = e
            };
            return inputArgs;
        }
        /// <summary>
        /// 创建鼠标事件的设备参数
        /// </summary>
        /// <param name="e"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        private DeviceInputArgs CreateMouseInputArgs(MouseEventArgs e, int deviceId)
        {
            var positionLazy = new Lazy<Point>(() => e.GetPosition(_element));
            return new DeviceInputArgs()
            {
                DeviceId = deviceId,
                DeviceType = DeviceType.Mouse,
                PositionLazy = positionLazy,
                PointsLazy = new Lazy<StylusPointCollection>(() =>
                {
                    var point = positionLazy.Value;
                    return new StylusPointCollection(new List<StylusPoint>() { new StylusPoint(point.X, point.Y) });
                }),
                GetPositionFunc = (element, args) => e.GetPosition(element),
                SourceArgs = e
            };
        }
        /// <summary>
        /// 获取触摸点集
        /// </summary>
        /// <param name="stylusEventArgs"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private StylusPointCollection GetStylusPoints(StylusEventArgs stylusEventArgs, FrameworkElement element)
        {
            // 临时去除description
            var pointCollection = new StylusPointCollection();
            var stylusPointCollection = stylusEventArgs.GetStylusPoints(element);
            foreach (var stylusPoint in stylusPointCollection)
            {
                pointCollection.Add(new StylusPoint(stylusPoint.X, stylusPoint.Y, stylusPoint.PressureFactor));
            }
            return pointCollection;
        }

        #endregion

        #endregion

        #region 对外事件

        /// <summary>
        /// 设备按下
        /// </summary>
        public event EventHandler<DeviceInputArgs> DeviceDown;

        /// <summary>
        /// 设备移动
        /// </summary>
        public event EventHandler<DeviceInputArgs> DeviceMove;

        /// <summary>
        /// 设备抬起
        /// </summary>
        public event EventHandler<DeviceInputArgs> DeviceUp;

        /// <summary>
        /// 设备按下
        /// </summary>
        public event EventHandler<DeviceInputArgs> PreviewDeviceDown;

        /// <summary>
        /// 设备移动
        /// </summary>
        public event EventHandler<DeviceInputArgs> PreviewDeviceMove;

        /// <summary>
        /// 设备抬起
        /// </summary>
        public event EventHandler<DeviceInputArgs> PreviewDeviceUp;

        #endregion
    }
}
