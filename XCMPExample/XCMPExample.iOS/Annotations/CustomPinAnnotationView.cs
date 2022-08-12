using CoreGraphics;
using Foundation;
using MapKit;
using SkiaSharp;
using SkiaSharp.Views.iOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

namespace XCMPExample.iOS.Annotations
{
    public class CustomPinAnnotationView : MKAnnotationView
    {
        public const string ViewIdentifier = nameof(CustomPinAnnotationView);

        private CustomPinAnnotation _SkiaAnnotation => base.Annotation as CustomPinAnnotation;
        private CancellationTokenSource _imageUpdateCts;
        private nfloat _screenDensity;

        public CustomPinAnnotationView(CustomPinAnnotation annotation) : base(annotation, ViewIdentifier)
        {
            _screenDensity = UIScreen.MainScreen.Scale;
        }

        internal async void UpdateImage()
        {
            CustomPin pin = _SkiaAnnotation?.SharedPin;
            UIImage image;
            CancellationTokenSource renderCts = new CancellationTokenSource();

            _imageUpdateCts?.Cancel();
            _imageUpdateCts = renderCts;

            try
            {
                image = await RenderPinAsync(pin, renderCts.Token).ConfigureAwait(false);

                renderCts.Token.ThrowIfCancellationRequested();

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (!renderCts.IsCancellationRequested)
                    {
                        Image = image;
                        Bounds = new CGRect(CGPoint.Empty, new CGSize(pin.Width, pin.Height));
                    }
                });
            }
            catch (OperationCanceledException)
            {
                // Ignore
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to render pin annotation: \n" + e);
            }
        }

        private Task<UIImage> RenderPinAsync(CustomPin pin, CancellationToken token = default(CancellationToken))
        {
            return Task.Run(() =>
            {
                double bitmapWidth = pin.Width * _screenDensity;
                double bitmapHeight = pin.Height * _screenDensity;

                using (SKSurface surface = SKSurface.Create(new SKImageInfo((int)bitmapWidth, (int)bitmapHeight, SKColorType.Rgba8888, SKAlphaType.Premul)))
                {
                    surface.Canvas.Clear(SKColor.Empty);
                    pin.DrawPin(surface);

                    return surface.PeekPixels().ToUIImage();
                }
            }, token);
        }

        public void UpdateAnchor()
        {
            CenterOffset = new CGPoint(Bounds.Width * (0.5 - _SkiaAnnotation.SharedPin.AnchorX),
                                       Bounds.Height * (0.5 - _SkiaAnnotation.SharedPin.AnchorY));
        }
    }
}