using SkiaSharp;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XCMPExample
{
    internal class CirclePin : CustomPin
    {
        // Сохраненный Bitmap
        SKBitmap pinBitmap;

        // Конструктор принимает string - это текст внутри круга
        public CirclePin(string text)
        {
            // Отступ текста от краев круга
            int circleOffset = 10;

            // Минимальный размер круга, при маленьком тексте
            int minSize = 40;

            // Размер шрифта текста
            int textSize = 18;

            // Задание цвета текста
            Color tempColor = Color.White;
            // Перевод из Color в SKColor
            SKColor textColor = new SKColor((byte)(tempColor.R * 255), (byte)(tempColor.G * 255), (byte)(tempColor.B * 255));

            // Задание цвета круга
            tempColor = Color.Black;
            // Перевод из Color в SKColor
            SKColor circleColor = new SKColor((byte)(tempColor.R * 255), (byte)(tempColor.G * 255), (byte)(tempColor.B * 255));

            PrepareBitmap(circleOffset, circleColor, text, textSize, textColor, minSize);
        }

        private void PrepareBitmap(int circleOffset, SKColor circleColor, string text, float textSize, SKColor textColor, int minSize, int iconSize = 28)
        {
            int width;
            float den = (float)DeviceDisplay.MainDisplayInfo.Density;

            // Удваиваем отступ, т.к. он будет с 2-х сторон одинаковый
            circleOffset *= 2;

            using (var font = SKTypeface.FromFamilyName("Arial"))
            using (var textBrush = new SKPaint
            {
                Typeface = font,
                TextSize = textSize * den,
                IsAntialias = true,
                Color = textColor,
                TextAlign = SKTextAlign.Center,
            })
            {
                // Высчитывание размера текста
                SKRect textRect = new SKRect();
                textBrush.MeasureText(text, ref textRect);

                // Ширина текста в dip
                width = Math.Max((int)(Math.Ceiling(textRect.Width) / den) + circleOffset, minSize);

                // Задаем размер пина согласно ширине в dip
                Width = Height = width;

                // Ширина текста в пикселях
                width = (int)Math.Floor(width * den);

                // Создаем Bitmap для отрисовки
                pinBitmap = new SKBitmap(width, width, SKColorType.Rgba8888, SKAlphaType.Premul);

                using (var canvas = new SKCanvas(pinBitmap))
                {
                    using (var circleBrush = new SKPaint
                    {
                        IsAntialias = true,
                        Color = circleColor
                    })
                    {
                        //Отрисовка круга
                        canvas.DrawRoundRect(new SKRoundRect(new SKRect(0, width, width, 0), width / 2f), circleBrush);

                        //Отрисовка текста
                        canvas.DrawText(text, width * 0.5f, width * 0.5f - textRect.MidY, textBrush);

                        canvas.Flush();
                    }
                }
            }

        }

        public override void DrawPin(SKSurface surface)
        {
            // Получаем канвас из сурфейса, для отрисовки
            SKCanvas canvas = surface.Canvas;

            // Отрисовываем на канвас наш сохраненный Bitmap
            canvas.DrawBitmap(pinBitmap, canvas.LocalClipBounds.MidX - pinBitmap.Width / 2f, canvas.LocalClipBounds.MidY - pinBitmap.Height / 2f);
        }
    }
}
