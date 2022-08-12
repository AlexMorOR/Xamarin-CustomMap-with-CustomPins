using Foundation;
using MapKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using XCMPExample;
using XCMPExample.iOS.Annotations;
using XCMPExample.iOS.Renderers;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace XCMPExample.iOS.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        protected override IMKAnnotation CreateAnnotation(Pin pin)
        {
            if (pin is CustomPin skPin)
            {
                //Если мы обрабатываем наш кастомный пин, то создаем ему специальную аннотацию.
                IMKAnnotation result = new CustomPinAnnotation(skPin);

                skPin.MarkerId = result;

                return result;
            }
            else
                return base.CreateAnnotation(pin);
        }

        protected override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            if (annotation is CustomPinAnnotation skiaAnnotation)
            {
                // Если мы обрабатываем нашу кастомную аннотацию, то получаем из нее наш пин
                CustomPin skPin = skiaAnnotation.SharedPin;

                // Проверяем на кэшированные аннотации, по совету Xamarin
                CustomPinAnnotationView pinView = mapView.DequeueReusableAnnotation(CustomPinAnnotationView.ViewIdentifier) as CustomPinAnnotationView
                                                    ?? new CustomPinAnnotationView(skiaAnnotation);

                // Добавляем жесты к пину
                base.AttachGestureToPin(pinView, annotation);

                pinView.Annotation = skiaAnnotation;
                // Отрисовываем пин
                pinView.UpdateImage();
                // Обновляем якорь
                pinView.UpdateAnchor();
                pinView.Hidden = !skPin.IsVisible;
                pinView.Enabled = skPin.Clickable;

                return pinView;
            }
            else
                return base.GetViewForAnnotation(mapView, annotation);
        }
    }
}