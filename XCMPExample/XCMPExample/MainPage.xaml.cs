using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace XCMPExample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                string universalFillData = i.ToString();
                customMap.Pins.Add(new CirclePin(universalFillData)
                {
                    Label = universalFillData,
                    Address = universalFillData,
                    Position = new Position(
                        random.NextDouble() + 55,
                        random.NextDouble() + 37)
                });
            }
        }
    }
}
