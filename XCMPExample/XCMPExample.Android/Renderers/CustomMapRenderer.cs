using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SkiaSharp;
using SkiaSharp.Views.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using XCMPExample;
using XCMPExample.Droid.Renderers;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace XCMPExample.Droid.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        public CustomMapRenderer(Context context) : base(context){}

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var opts = base.CreateMarker(pin);

            if(pin is CustomPin cpin)
            {
                SKPixmap markerBitmap = DrawMarker(cpin);

                opts.SetIcon(BitmapDescriptorFactory.FromBitmap(markerBitmap.ToBitmap()))
                       .Visible(cpin.IsVisible);
                opts.Anchor((float)cpin.AnchorX, (float)cpin.AnchorY);
            }

            return opts;
        }

        private SKPixmap DrawMarker(CustomPin skPin)
        {
            double bitmapWidth = skPin.Width * Context.Resources.DisplayMetrics.Density;
            double bitmapHeight = skPin.Height * Context.Resources.DisplayMetrics.Density;
            SKSurface surface = SKSurface.Create(new SKImageInfo((int)bitmapWidth, (int)bitmapHeight, SKColorType.Rgba8888, SKAlphaType.Premul));

            surface.Canvas.Clear(SKColor.Empty);
            skPin.DrawPin(surface);

            return surface.PeekPixels();
        }
    }
}