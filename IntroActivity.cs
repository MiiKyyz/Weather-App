using Android.Animation;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Weather_App
{
    [Activity(Label = "Weather App", MainLauncher = true, Theme = "@style/Theme.AppCompat.NoActionBar", Icon = "@drawable/logo_weather")]
    public class IntroActivity : AppCompatActivity
    {

        ImageView img;
        TextView txt;

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_intro);
  


            img = FindViewById<ImageView>(Resource.Id.image_anim);
            txt = FindViewById<TextView>(Resource.Id.txt_anim);



           

            IntroPanel();
            Animations();
            TextFonts("TradeWinds-Regular.ttf", txt);
            
        }


        public void TextFonts(string FontType, TextView txt_fonts)
        {

            Typeface txt = Typeface.CreateFromAsset(Assets, FontType);
            txt_fonts.SetTypeface(txt, TypefaceStyle.Normal);

        }



        public void Animations()
        {



            ObjectAnimator Background = ObjectAnimator.OfFloat(img, "alpha", 0f, 1f);
            Background.SetDuration(2000);
            Background.Start();


            ObjectAnimator Title = ObjectAnimator.OfFloat(txt, "alpha", 0f, 1f);
            Title.SetDuration(2000);

            ObjectAnimator Title_translation = ObjectAnimator.OfFloat(txt, "y", -50,450);
            Title_translation.SetDuration(2000);

            AnimatorSet set = new AnimatorSet();


            set.PlayTogether(Title, Title_translation);
            set.Start();
        }


        private void IntroPanel()
        {
            Task splash = new Task(() =>
            {


                Thread.Sleep(4000);

            });

            splash.ContinueWith(t =>
            {

                StartActivity(new Intent(Application.Context, typeof(TemperatureMeasurement)));

            }, TaskScheduler.FromCurrentSynchronizationContext());

            splash.Start();

        }

      
    }
}