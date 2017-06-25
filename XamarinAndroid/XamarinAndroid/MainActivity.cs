using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Java.Util;
using System.Globalization;
using Android.Content.PM;

using Android.Media;

namespace XamarinAndroid
{
	[Activity(Label = "XamarinAndroid", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		//int count = 1;

		public Task timer;
		private string S_Time_H, S_Time_M, S_Time_S,S_Time_Interval_S,S_Time_Interval_M;
		private int time,time_S,time_M,time_H,time_Interval_S,time_Interval_M, IntervalTime;
		private int time2, time_S2, time_M2, time_H2, time_Interval_S2,time_Interval_M2, IntervalTime2;


		private string State = "SELECT";
		private TextView text_H;
		private TextView text_M;
		private TextView text_S;

		private TextView Backtext_H,Backtext_M,Backtext_S,Backtext_Interval_S,Backtext_Interval_M;

		private Button button_Start;
		private Button button_Cancel;

		private Spinner spinner_H;
		private Spinner spinner_M;
		private Spinner spinner_S;
		private Spinner spinner_Interval_S,spinner_Interval_M;

		private MediaPlayer player;










		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);


			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);





			// Get our button from the layout resource,
			// and attach an event to it


			/*
			 * Button button = FindViewById<Button>(Resource.Id.myButton);
			button.Click += delegate
			{
				button.Text = $"{count++} clicks!";
			};*/

			text_H = FindViewById<TextView>(Resource.Id.text_H);
			text_M = FindViewById<TextView>(Resource.Id.text_M);
			text_S = FindViewById<TextView>(Resource.Id.text_S);

			spinner_H = FindViewById<Spinner>(Resource.Id.spinner_H);
			spinner_M = FindViewById<Spinner>(Resource.Id.spinner_M);
			spinner_S = FindViewById<Spinner>(Resource.Id.spinner_S);
			spinner_Interval_S = FindViewById<Spinner>(Resource.Id.spinner_Interval_S);
			spinner_Interval_M = FindViewById<Spinner>(Resource.Id.spinner_Interval_M);


			Backtext_H = FindViewById<TextView>(Resource.Id.Backtext_H);
			Backtext_M = FindViewById<TextView>(Resource.Id.Backtext_M);
			Backtext_S = FindViewById<TextView>(Resource.Id.Backtext_S);
			Backtext_Interval_S = FindViewById<TextView>(Resource.Id.Backtext_Interval_S);
			Backtext_Interval_M = FindViewById<TextView>(Resource.Id.Backtext_Interval_M);

			button_Start = FindViewById<Button>(Resource.Id.button_Start);
			button_Cancel = FindViewById<Button>(Resource.Id.button_Cancel);






			spinner_H.ItemSelected += new System.EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected_H);
			spinner_M.ItemSelected += new System.EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected_M);
			spinner_S.ItemSelected += new System.EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected_S);
			spinner_Interval_S.ItemSelected += new System.EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected_Interval_S);
			spinner_Interval_M.ItemSelected += new System.EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected_Interval_M);

			var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.time, Android.Resource.Layout.SimpleSpinnerItem);

			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner_H.Adapter = adapter;
			spinner_M.Adapter = adapter;
			spinner_S.Adapter = adapter;







			Backtext_S.Enabled = false;
			Backtext_M.Enabled = false;
			Backtext_H.Enabled = false;
			Backtext_Interval_S.Enabled = false;
			Backtext_Interval_M.Enabled = false;

			button_Start.Click +=delegate {
				State = "TIMER";
				Start();

			};

			button_Cancel.Click += delegate {
				State = "SELECT";
				Cancel();
			};






		}




		protected override void OnResume(){
			base.OnResume();




			player = MediaPlayer.Create(this, Resource.Raw.alerm);



			timer = new Task(async () =>
			{
				while (true)
				{
					RunOnUiThread(() =>
					{

						switch (State)
						{
							case "SELECT":
								{


									spinner_H.ItemSelected += new System.EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected_H);
									spinner_M.ItemSelected += new System.EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected_M);
									spinner_S.ItemSelected += new System.EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected_S);
									spinner_M.ItemSelected += new System.EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected_M);


									try
									{
										time_S = time_S2 = int.Parse(S_Time_S, NumberStyles.AllowThousands);
										time_M = time_M2 = int.Parse(S_Time_M, NumberStyles.AllowThousands);
										time_H = time_H2 = int.Parse(S_Time_H, NumberStyles.AllowThousands);
										time_Interval_S = time_Interval_S2 = time_Interval_S2 = int.Parse(S_Time_Interval_S, NumberStyles.AllowThousands);
										time_Interval_M = time_Interval_M2 = time_Interval_M = int.Parse(S_Time_Interval_M, NumberStyles.AllowThousands);

									}
									catch (System.ArgumentNullException ae)
									{
										Android.Util.Log.Error("MyError", ae.ToString());

									}
									time = time2 = time_H * 3600 + time_M * 60 + time_S;
									IntervalTime = IntervalTime2 = time_Interval_M2 * 60 + time_Interval_S2;









									break;
								}

							case "TIMER":
								{


									time2 -= 1;

									time_H2 = time2 / 3600;
									time2 %= 3600;

									time_M2 = time2 / 60;
									time2 %= 60;

									time_S2 = time2;

									text_H.Text = time_H2.ToString();
									text_M.Text = time_M2.ToString();
									text_S.Text = time_S2.ToString();


									if (time2 <= 0)
									{
										State = "INTERVAL";
									}


									break;
								}
							case "INTERVAL":
								{




									player.Start();




									IntervalTime2--;


									if (IntervalTime2 <= 0)
									{
										State = "TIMER";
										time2 = time;
										time_S2 = time_S;
										time_M2 = time_M;
										time_H2 = time_H;

										IntervalTime2 = IntervalTime;
									}

									break;
								}


						}




					});
					await Task.Delay(1000);
				}
			});

			timer.Start();
		}





		private void spinner_ItemSelected_H(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			var spinner = sender as Spinner;

			//Toast.MakeText(this, "Choose: " + spinner.GetItemAtPosition(e.Position), ToastLength.Long).Show();
			Backtext_H.Text = text_H.Text = spinner.GetItemAtPosition(e.Position).ToString();
			S_Time_H = spinner.GetItemAtPosition(e.Position).ToString();

		}
		private void spinner_ItemSelected_M(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			var spinner = sender as Spinner;

			//Toast.MakeText(this, "Choose: " + spinner.GetItemAtPosition(e.Position), ToastLength.Long).Show();
			Backtext_M.Text = text_M.Text = spinner.GetItemAtPosition(e.Position).ToString();
			S_Time_M = spinner.GetItemAtPosition(e.Position).ToString();
		}
		private void spinner_ItemSelected_S(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			var spinner = sender as Spinner;

			//Toast.MakeText(this, "Choose: " + spinner.GetItemAtPosition(e.Position), ToastLength.Long).Show();
			Backtext_S.Text = text_S.Text = spinner.GetItemAtPosition(e.Position).ToString();
			S_Time_S = spinner.GetItemAtPosition(e.Position).ToString();
		}
		private void spinner_ItemSelected_Interval_S(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			var spinner = sender as Spinner;

			//Toast.MakeText(this, "Choose: " + spinner.GetItemAtPosition(e.Position), ToastLength.Long).Show();
			Backtext_Interval_S.Text = spinner.GetItemAtPosition(e.Position).ToString();
			S_Time_Interval_S = spinner.GetItemAtPosition(e.Position).ToString();
		}
		private void spinner_ItemSelected_Interval_M(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			var spinner = sender as Spinner;

			//Toast.MakeText(this, "Choose: " + spinner.GetItemAtPosition(e.Position), ToastLength.Long).Show();
			Backtext_Interval_M.Text = spinner.GetItemAtPosition(e.Position).ToString();
			S_Time_Interval_M = spinner.GetItemAtPosition(e.Position).ToString();
		}


		public void Start(){
			spinner_H.Enabled = false;
			spinner_M.Enabled = false;
			spinner_S.Enabled = false;
			spinner_Interval_M.Enabled = false;
			spinner_Interval_S.Enabled = false;

			Backtext_S.Enabled = true;
			Backtext_M.Enabled = true;
			Backtext_H.Enabled = true;
			Backtext_Interval_S.Enabled = true;
			Backtext_Interval_M.Enabled = true;


		}
		public void Cancel(){
			spinner_H.Enabled = true;
			spinner_M.Enabled = true;
			spinner_S.Enabled = true;
			spinner_Interval_S.Enabled = true;
			spinner_Interval_M.Enabled = true;

			Backtext_S.Enabled = false;
			Backtext_M.Enabled = false;
			Backtext_H.Enabled = false;
			Backtext_Interval_S.Enabled = false;
			Backtext_Interval_M.Enabled = false;

			time = time2 = time_H * 3600 + time_M * 60 + time_S;
			IntervalTime = IntervalTime2 = time_Interval_M2 * 60 + time_Interval_S2;



		}


		protected void OnStop(Bundle savedInstanceState){
			base.OnStop();



		}

		protected void OnDestroy(Bundle savedInstanceState){
			base.OnDestroy();


			player.Release();
		}



	}







}

