using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Android.Widget;
using Android.Content;
using Android.Util;
using System.IO;
using Google.Android.Material.TextField;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Android.Graphics;
using Android.Views;

namespace Weather_App
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {

        Button btn;
        TextView Degree, Sky, City, temp_max, temp_min, pressure, Feels_Like, humidity, visibility,
            speed, degree, main_des;
        AutoCompleteTextView search;
        string type_measure;
        List<string> List_Cities = new List<string>();
        ImageView img;
        TextView[] list_txt;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);


            btn = FindViewById<Button>(Resource.Id.btn_weather);

            Degree = FindViewById<TextView>(Resource.Id.txt_degree);
            Sky = FindViewById<TextView>(Resource.Id.txt_condition);
            City = FindViewById<TextView>(Resource.Id.txt_city);
            search = FindViewById<AutoCompleteTextView>(Resource.Id.edit_search);


            img = FindViewById<ImageView>(Resource.Id.img_weather);

            temp_max = FindViewById<TextView>(Resource.Id.temp_max);
            temp_min = FindViewById<TextView>(Resource.Id.temp_min);
            pressure = FindViewById<TextView>(Resource.Id.pressure);
            Feels_Like = FindViewById<TextView>(Resource.Id.Feels_Like);
            humidity = FindViewById<TextView>(Resource.Id.humidity);
            main_des = FindViewById<TextView>(Resource.Id.main_des);

            visibility = FindViewById<TextView>(Resource.Id.visibility);
            speed = FindViewById<TextView>(Resource.Id.speed);
            degree = FindViewById<TextView>(Resource.Id.degree);


            btn.Click += Btn_Click;


            list_txt = new TextView[] { Degree , Sky , City , temp_max , temp_min ,
            pressure,Feels_Like, humidity, main_des, visibility, speed, degree };


            foreach(TextView txt in list_txt)
            {


                TextFonts("CherryCreamSoda-Regular.ttf", txt);


            }


       


            TextAdapter();

            type_measure = Intent.GetStringExtra("type" ?? "not recv");

            
        }

        public void TextFonts(string FontType, TextView txt_fonts)
        {

            Typeface txt = Typeface.CreateFromAsset(Assets, FontType);
            txt_fonts.SetTypeface(txt, TypefaceStyle.Normal);

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

  
        
        public float MeasureType(float temperature)
        {

         
            
            switch (type_measure)
            {

                
                case "Celsius":

                    temperature = temperature - 273.15f;

                

                    break;
                case "Fahrenheit":


                    temperature = (temperature - 273.15f) * 9 / 5 + 32;
                 

                    break;


            }


            return temperature;



        }



        private void Cities()
        {
            Stream data = Assets.Open(@"cities.txt");

            using (StreamReader reader = new StreamReader(data))
            {

                string line;
                int count = 0;
                while ((line = reader.ReadLine()) != null)
                {

                    List_Cities.Add(line);
                    count += 1;

                }

                Log.Info("Lenght: ", count.ToString());

            }


        }

        private void TextAdapter()
        {

            

            Cities();
            ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.DrownListText, Resource.Id.city_txt_dropdown, List_Cities);

            search.Adapter = adapter;

        }



        private void Btn_Click(object sender, System.EventArgs e)
        {


            GetWeatherAsync(search.Text);

        }

        public async Task GetWeatherAsync(string placeInput)
        {

            string apikey = "be176a3c53a3d2a280bbc6e43022a398";
            string apiBase = "https://api.openweathermap.org/data/2.5/weather?q=";
            string unit = "metric";

            if (string.IsNullOrEmpty(placeInput))
            {

                Toast.MakeText(this, "type", ToastLength.Short).Show();


            }
            else
            {
                string url = apiBase + placeInput + "&appid=" + apikey + "&units" + unit;

                var handler = new HttpClientHandler();

                HttpClient client = new HttpClient(handler);

                try
                {

                    string result = await client.GetStringAsync(url);

                    var resultObject = JObject.Parse(result);

                    string Description = resultObject["weather"][0]["description"].ToString();
                    string Icon = resultObject["weather"][0]["icon"].ToString();


                    WeatherSystem w = new WeatherSystem();

                    int Weather_Icon_Number = w.Show_Weather(Icon);


                    img.SetImageResource(Weather_Icon_Number);
                    img.SetBackgroundResource(0);



                    float Kelvin = (float)resultObject["main"]["temp"];
                    string Name = resultObject["name"].ToString();

                    string city = resultObject["sys"]["country"].ToString();

                    float max_temp = (float)resultObject["main"]["temp_max"];
                    float min_temp = (float)resultObject["main"]["temp_min"];
                    float feels_fike = (float)resultObject["main"]["feels_like"];

                    humidity.Text = $"Humidity: {resultObject["main"]["humidity"]}%";
                    pressure.Text = $"Pressure: {resultObject["main"]["pressure"]} hPa";
                    Feels_Like.Text = $"Feels Like: {(int)MeasureType(feels_fike)}°";
                    temp_max.Text = $"Max Temp: {(int)MeasureType(max_temp)}°";
                    temp_min.Text = $"Min Temp: {(int)MeasureType(min_temp)}°";


                    visibility.Text = $"visibility: {(int)resultObject["visibility"]/1000} KM";
                    speed.Text = $"wind speed: {resultObject["wind"]["speed"]}m/s";
                    degree.Text = $"wind degree: {resultObject["wind"]["deg"]}°";


                    main_des.Text = $"{resultObject["weather"][0]["main"]}";

                    Degree.Text = $"{(int)MeasureType(Kelvin)}°";
                    Sky.Text = Description;
                    City.Text = $"{search.Text}, {city}";

                    search.Text = "";


                    
                }
                catch
                {
                   

                    Degree.Text = $"None";
                    Sky.Text = "None";
                    City.Text = $"None";

                }

                
            }
        }
    }
}