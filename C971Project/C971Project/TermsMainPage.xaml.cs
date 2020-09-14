using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class TermsMainPage : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private ObservableCollection<Term> _terms;

        //location of the sqlite db
        public static string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "MySQLite.db3");

        public TermsMainPage()
        {
            InitializeComponent();
      
        }

        protected override async void OnAppearing()
        {
            _connection = new SQLiteAsyncConnection(path);

            //creates Term and Course table in sqlite
            await _connection.CreateTableAsync<Term>();
            await _connection.CreateTableAsync<Course>();

            var terms = await _connection.Table<Term>().ToListAsync();

            //by setting ItemsSource as the observable collection, it allows for data to display immediately
            _terms = new ObservableCollection<Term>(terms);
            TermsListView.ItemsSource = _terms;

            base.OnAppearing();
        }

        async private void TermsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var term = e.SelectedItem as Term;
            await Navigation.PushAsync(new TermsDetailPage(term));
            TermsListView.SelectedItem = null;
        }

        async private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewTermForm());
        }

        //method to add sample data
        /*async void AddSampleData()
        {
            _connection = new SQLiteAsyncConnection(path);

            var term = new Term("Fall Term", Term.Status.InProgress, new DateTime(2020, 9, 1).Date, new DateTime(2020, 12, 31).Date);
            await _connection.InsertAsync(term);

            var course = new Course
            {
                Name = "Physics",
                TermId = term.Id,
                StartDate = new DateTime(2020, 9, 1).Date,
                EndDate = new DateTime(2020, 10, 1).Date,
                CurrentStatus = Course.Status.InProgress,
                InstructorName = "Aki Tanaka",
                InstructorEmail = "atanaka@wgu.edu",
                InstructorPhone = "714-227-9216",
                OAName = "Physics OA Project",
                OAStart = new DateTime(2020, 9, 20).Date,
                OAEnd = new DateTime(2020, 9, 25).Date,
                OAStatus = Course.Status.Upcoming,
                OANotes = "These are my notes in preparation for the Objective Assessment.",
                PAName = "Physics PA Exam",
                PAStart = new DateTime(2020, 9, 26).Date,
                PAEnd = new DateTime(2020, 9, 30).Date,
                PAStatus = Course.Status.Upcoming,
                PANotes = "These are my notes in preparation for the Performance Assessment."
            };
            await _connection.InsertAsync(course);

            //adding the new term to observable collection allows it to display immediately?
            _terms.Add(term);
        }*/


        //test button to add fake data for term and course. Uncomment the button in xaml if using.
        async private void Button_Clicked(object sender, EventArgs e)
        {
            _connection = new SQLiteAsyncConnection(path);

            var term = new Term("Fall Term", Term.Status.InProgress, new DateTime(2020, 9, 1).Date, new DateTime(2020, 12, 31).Date);
            await _connection.InsertAsync(term);

            var course = new Course
            {
                Name = "Physics",
                TermId = term.Id,
                StartDate = new DateTime(2020, 9, 1).Date,
                EndDate = new DateTime(2020, 10, 1).Date,
                CurrentStatus = Course.Status.InProgress,
                InstructorName = "Aki Tanaka",
                InstructorEmail = "atanaka@wgu.edu",
                InstructorPhone = "714-227-9216",
                OAName = "Physics OA Project",
                OAStart = new DateTime(2020, 9, 20).Date,
                OAEnd = new DateTime(2020, 9, 25).Date,
                OAStatus = Course.Status.Upcoming,
                OANotes = "These are my notes in preparation for the Objective Assessment.",
                PAName = "Physics PA Exam",
                PAStart = new DateTime(2020, 9, 26).Date,
                PAEnd = new DateTime(2020, 9, 30).Date,
                PAStatus = Course.Status.Upcoming,
                PANotes = "These are my notes in preparation for the Performance Assessment."
            };
            await _connection.InsertAsync(course);

            //adding the new term to observable collection allows it to display immediately?
            _terms.Add(term);
        }
    }
}