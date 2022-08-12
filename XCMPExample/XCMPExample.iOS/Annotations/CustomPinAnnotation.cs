using CoreLocation;
using Foundation;
using MapKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms.Maps;

namespace XCMPExample.iOS.Annotations
{
    public class CustomPinAnnotation : MKPointAnnotation
    {
        public CustomPin SharedPin { get; }

        public CustomPinAnnotation(CustomPin pin)
        {
            SharedPin = pin;

            Title = pin.Label;
            Subtitle = pin.Address;
            Coordinate = ToLocationCoordinate(pin.Position);
        }

        public override string Title
        {
            get => base.Title;
            set
            {
                if (Title != value)
                {
                    string titleKey = nameof(Title).ToLower();

                    WillChangeValue(titleKey);
                    base.Title = value;
                    DidChangeValue(titleKey);
                }
            }
        }

        public override string Subtitle
        {
            get => base.Subtitle;
            set
            {
                if (Subtitle != value)
                {
                    string subtitleKey = nameof(Subtitle).ToLower();

                    WillChangeValue(subtitleKey);
                    base.Subtitle = value;
                    DidChangeValue(subtitleKey);
                }
            }
        }

        public override CLLocationCoordinate2D Coordinate
        {
            get => base.Coordinate;
            set
            {
                if (Coordinate.Latitude != value.Latitude ||
                    Coordinate.Longitude != value.Longitude)
                {
                    string coordinateKey = nameof(Coordinate).ToLower();

                    WillChangeValue(coordinateKey);
                    base.Coordinate = value;
                    DidChangeValue(coordinateKey);
                }
            }
        }

        private CLLocationCoordinate2D ToLocationCoordinate(Position self)
        {
            return new CLLocationCoordinate2D(self.Latitude, self.Longitude);
        }
    }
}