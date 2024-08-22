using System.Windows;
using System.Windows.Input;

namespace DeviceInputEventDemo
{
    internal class TouchArea
    {
        /// <summary>
        /// 获取含面积的触摸点集合
        /// </summary>
        /// <param name="stylusEventArgs"></param>
        /// <param name="uiElement"></param>
        /// <returns></returns>
        public static Rect GetTouchPointArea(TouchEventArgs stylusEventArgs, UIElement uiElement)
        {
            Rect touchArea = Rect.Empty;
            var touchPoints = stylusEventArgs.GetIntermediateTouchPoints(uiElement);
            foreach (var stylusPoint in touchPoints)
            {
                var stylusPointArea = stylusPoint.Bounds;
                touchArea.Union(stylusPointArea);
            }
            return touchArea;
        }
        /// <summary>
        /// 获取含面积的触摸点集合
        /// </summary>
        /// <param name="stylusEventArgs"></param>
        /// <param name="uiElement"></param>
        /// <returns></returns>
        public static Rect GetStylusPointArea(StylusEventArgs stylusEventArgs, UIElement uiElement)
        {
            Rect touchArea = Rect.Empty;
            var stylusPointCollection = stylusEventArgs.GetStylusPoints(uiElement);
            foreach (var stylusPoint in stylusPointCollection)
            {
                var stylusPointArea = GetStylusPointArea(stylusPoint);
                touchArea.Union(stylusPointArea);
            }
            return touchArea;
        }

        private static Rect GetStylusPointArea(StylusPoint stylusPoint)
        {
            var width = GetStylusPointProperty(stylusPoint, StylusPointProperties.Width);
            var height = GetStylusPointProperty(stylusPoint, StylusPointProperties.Height);
            var rect = new Rect(stylusPoint.X, stylusPoint.Y, width, height);
            return rect;
        }

        /// <summary>
        /// 获取触摸点的面积
        /// </summary>
        /// <param name="stylusPoint"></param>
        /// <param name="stylusPointProperty">StylusPointProperties.Height 或者 StylusPointProperties.Width</param>
        /// <returns></returns>
        private static double GetStylusPointProperty(StylusPoint stylusPoint, StylusPointProperty stylusPointProperty)
        {
            if (stylusPointProperty != StylusPointProperties.Height && stylusPointProperty != StylusPointProperties.Width)
                throw new ArgumentException($"计算属性非Height或Width");

            double value = 0.0d;
            if (!stylusPoint.HasProperty(stylusPointProperty)) return value;
            value = stylusPoint.GetPropertyValue(stylusPointProperty);
            StylusPointPropertyInfo propertyInfo = stylusPoint.Description.GetPropertyInfo(stylusPointProperty);
            if (Math.Abs(propertyInfo.Resolution - 0.0d) > 0.001d)
                value /= propertyInfo.Resolution;
            else
                value = 0.0d;

            //属性值的单位
            if (propertyInfo.Unit == StylusPointPropertyUnit.Centimeters)
                value /= 2.54d;
            value *= 96.0d;
            return value;
        }
    }
}
