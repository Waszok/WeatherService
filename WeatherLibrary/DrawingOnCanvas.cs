using System;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using Point = System.Windows.Point;
using Brushes = System.Windows.Media.Brushes;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using System.Collections.ObjectModel;

namespace WeatherLibrary
{
    public static class DrawingOnCanvas
    {
        #region Draw base of graph (a coordinate system)

        /// <summary>
        /// The method draw the base of target graph (a coordinate system) 
        /// and set the scale values on the Y axis.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="startScaleNumber">Beginning of the scale on Y axis</param>
        /// <param name="scaleUnit">Define how precise is the scale</param>
        public static void DrawGraphBase(Canvas graph, int startScaleNumber, int scaleUnit)
        {
            const double marginX = 50;
            const double marginY = 30;
            double xmin = marginX;
            double xmax = graph.Width - marginX;
            double ymax = graph.Height - marginY;
            const double stepX = 60;
            const double stepY = 10;

            //Draw the X axis and small strokes to define a scale.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(
                new Point(50, ymax), new Point(graph.Width, ymax)));
            for (double x = xmin + stepX;
                x <= graph.Width - stepX; x += stepX)
            {
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x, ymax - marginY / 4),
                    new Point(x, ymax + marginY / 4)));
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            graph.Children.Add(xaxis_path);

            //Draw the Y axis and small strokes to define a scale. 
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, 0), new Point(xmin, graph.Height - 30)));
            for (double y = stepY; y <= graph.Height - marginY; y += stepY)
            {
                double point = xmin - marginX / 5 - 30;
                yaxis_geom.Children.Add(new LineGeometry(
                    new Point(xmin - marginX / 5 + 5, y),
                    new Point(xmin + marginX / 5 - 5, y)));
                
                //Display scale values on the canvas (actual graph)
                DisplayTextOnCanvas(point + xmax, y - stepY, startScaleNumber.ToString(), Color.FromRgb(0, 0, 0), graph);
                startScaleNumber -= scaleUnit;
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            graph.Children.Add(yaxis_path);
        }

        /// <summary>
        /// [Helper method] Display scale data (units) on graphs
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <param name="graph"></param>
        public static void DisplayTextOnCanvas(double x, double y, string text, Color color, Canvas graph)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = new SolidColorBrush(color);
            textBlock.FontSize = 12;
            textBlock.FontWeight = FontWeights.DemiBold;
            Canvas.SetRight(textBlock, x);
            Canvas.SetTop(textBlock, y);
            graph.Children.Add(textBlock);
        }

        #endregion

        #region Appropriate coordinate scaling

        /// <summary>
        /// Scale single coordinate (double number) to match to the canvas (temp chart).
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double ScaleYCoordinate(double y)
        {
            y = 129.05 - y * 3.396;
            return y;
        }

        /// <summary>
        /// Scale array of coordinates (double numbers) to match to the canvas (temp chart).
        /// </summary>
        /// <param name="yList"></param>
        /// <returns></returns>
        public static List<double> ScaleYCoordinates(ObservableCollection<double> yList)
        {
            List<double> newYList = new List<double>();
            foreach (double y in yList)
            {
                newYList.Add(129.05 - y * 3.396);
            }
            return newYList;
        }

        /// <summary>
        /// Scale array of coordinates (double numbers) to match to the canvas (precipitation chart).
        /// </summary>
        /// <param name="yList"></param>
        /// <returns></returns>
        public static List<double> ScaleRainYCoordinates(ObservableCollection<double> yList)
        {
            List<double> newYList = new List<double>();
            foreach (double y in yList)
            {
                newYList.Add(5 * y);
            }
            return newYList;
        }

        #endregion

        #region Helper draw methods 

        /// <summary>
        /// [Helper method] Draw red points on canvas.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width">Horizontal ellipse radius</param>
        /// <param name="height">Vertical ellipse radius</param>
        /// <param name="cv"></param>
        public static void DrawCircle(double x, double y, int width, int height, Canvas cv)
        {
            Ellipse circle = new Ellipse()
            {
                Width = width,
                Height = height,
                Stroke = Brushes.Red,
                StrokeThickness = 6
            };

            cv.Children.Add(circle);

            circle.SetValue(Canvas.LeftProperty, x - width / 2);
            circle.SetValue(Canvas.TopProperty, y - height / 2);
        }

        /// <summary>
        /// [Helper method] Draw points on canvas.
        /// </summary>
        /// <param name="xList"></param>
        /// <param name="yList"></param>
        /// <param name="graph"></param>
        public static void DrawChartPoints(List<double> xList, List<double> yList, Canvas graph)
        {
            if (xList.Count.Equals(yList.Count))
            {
                for (int i = 0; i < xList.Count; i++)
                {
                    DrawCircle(xList[i], yList[i], 8, 8, graph);
                }
            }
            else
            {
                //throw new System.Exception("The lenght of lists are not the same!");
            }
        }

        /// <summary>
        /// [Helper method] Draw line between two given points.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="graph"></param>
        /// <param name="color"></param>
        public static void DrawLine(Point a, Point b, Canvas graph, Brush color)
        {
            Line line = new Line();
            line.Stroke = color;

            line.X1 = a.X;
            line.X2 = b.X;
            line.Y1 = a.Y;
            line.Y2 = b.Y;

            line.StrokeThickness = 2;
            graph.Children.Add(line);
        }

        /// <summary>
        /// [Helper method] Remove all ellipses from canvas.
        /// </summary>
        /// <param name="cv"></param>
        public static void ClearPoint(Canvas cv)
        {
            foreach (Ellipse ellipse in cv.Children.OfType<Ellipse>().ToList()) cv.Children.Remove(ellipse);
        }

        /// <summary>
        /// [Helper method] Remove all lines from canvas.
        /// </summary>
        /// <param name="cv"></param>
        public static void ClearLine(Canvas cv)
        {
            foreach (Line line in cv.Children.OfType<Line>().ToList()) cv.Children.Remove(line);
        }

        /// <summary>
        /// [Helper method] Remove all rectangles from canvas.
        /// </summary>
        /// <param name="cv"></param>
        public static void ClearRectangle(Canvas cv)
        {
            foreach (Rectangle rectangle in cv.Children.OfType<Rectangle>().ToList()) cv.Children.Remove(rectangle);
        }

        #endregion

        #region Draw graphs

        /// <summary>
        /// Draw line graph on canvas according to the given points.
        /// </summary>
        /// <param name="xPoints"></param>
        /// <param name="yPoints"></param>
        /// <param name="cv"></param>
        public static void DrawLineGraph(List<double> xPoints, List<double> yPoints, Canvas cv, Brush color)
        {
            ClearLine(cv);
            ClearPoint(cv);

            if (xPoints.Count.Equals(yPoints.Count))
            {
                for (int i = 0; i < xPoints.Count - 1; i++)
                {
                    Point p1 = new Point(xPoints[i], yPoints[i]);
                    Point p2 = new Point(xPoints[i + 1], yPoints[i + 1]);
                    DrawLine(p1, p2, cv, color);
                }
            }
            DrawChartPoints(xPoints, yPoints, cv);
        }

        /// <summary>
        /// Draw bar graph on the canvas according to the given lists of points.
        /// Actually we draw rectangles of appropriate height.
        /// </summary>
        /// <param name="xPoints">Number of bars (rectangles) and their X position</param>
        /// <param name="yPoints">Bars (rectangles) height</param>
        /// <param name="cv"></param>
        /// <param name="color">Rectangle fill color</param>
        public static void DrawBarGraph(List<double> xPoints, List<double> yPoints, Canvas cv, Brush color)
        {
            ClearRectangle(cv);

            if (xPoints.Count.Equals(yPoints.Count))
            {
                for (int i = 0; i < xPoints.Count; i++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Stroke = new SolidColorBrush(Colors.Black);
                    rect.Fill = color;
                    rect.StrokeThickness = 1;
                    rect.Width = 30;
                    rect.Height = yPoints[i];
                    Canvas.SetLeft(rect, xPoints[i] - rect.Width / 2);
                    Canvas.SetTop(rect, 180 - rect.Height);
                    cv.Children.Add(rect);
                }
            }
        }

        #endregion
    }
}
