using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using MyCoffeeApp.Models;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace MyCoffeeApp.ViewModels {
    public class CoffeeEquipmentViewModel : ViewModelBase {

        public ObservableRangeCollection<Coffee> Coffee { get; set; }
        public ObservableRangeCollection<Grouping<string, Coffee>> CoffeeGroups { get; }

        public AsyncCommand RefreshCommand { get; }

        public AsyncCommand<Coffee> FavoriteCommand { get; }

        public AsyncCommand<Coffee> SelectedCommand { get; }

        public Command LoadMoreCommand { get; }
        public Command DelayLoadMoreCommand { get; }
        public Command ClearCommand { get; }

        public CoffeeEquipmentViewModel() {

            Title = "Coffee Equipment";

            Coffee = new ObservableRangeCollection<Coffee>();
            CoffeeGroups = new ObservableRangeCollection<Grouping<string, Coffee>>();

            LoadMore();

            RefreshCommand = new AsyncCommand(Refresh);
            FavoriteCommand = new AsyncCommand<Coffee>(Favorite);
            SelectedCommand = new AsyncCommand<Coffee>(Selected);
            LoadMoreCommand = new Command(LoadMore);
            ClearCommand = new Command(Clear);
            DelayLoadMoreCommand = new Command(DelayLoadMore);
        }

        async Task Favorite(Coffee coffee) {
            if (coffee == null) {
                return;
            }
            await Application.Current.MainPage.DisplayAlert("Favorited", coffee.Name, "OK");
        }

        Coffee previouslySelected;
        Coffee selectedCoffee;
        public Coffee SelectedCoffee {
            get => selectedCoffee;
            set => SetProperty(ref selectedCoffee, value);
        }

        async Task Selected(Coffee coffee) {
            //var coffee = args as Coffee;

            if (coffee == null) 
                return;

            SelectedCoffee = null;

            await Application.Current.MainPage.DisplayAlert("Selected", coffee.Name, "OK");
        }

        async Task Refresh() {
            IsBusy = true;

            await Task.Delay(2000);

            Coffee.Clear();
            LoadMore();

            IsBusy = false;
        }

        void LoadMore() {
            if (Coffee.Count >= 20)
                return;

            var image = "https://www.yesplz.coffee/app/uploads/2020/11/emptybag-min.png";
            Coffee.Add(new Coffee { Roaster = "Yes Plz", Name = "Sip of Sunshine", Image = image });
            Coffee.Add(new Coffee { Roaster = "Yes Plz", Name = "Potent Potable", Image = image });
            Coffee.Add(new Coffee { Roaster = "Yes Plz", Name = "Potent Potable", Image = image });
            Coffee.Add(new Coffee { Roaster = "Blue Bottle", Name = "Kenya Kiambu Handege", Image = image });
            Coffee.Add(new Coffee { Roaster = "Blue Bottle", Name = "Kenya Kiambu Handege", Image = image });

            CoffeeGroups.Clear();

            CoffeeGroups.Add(new Grouping<string, Coffee>("Blue Bottle", new[] { Coffee[2] }));
            CoffeeGroups.Add(new Grouping<string, Coffee>("Yes Plz", Coffee));
        }

        void DelayLoadMore() {
            if (Coffee.Count <= 10)
                return;

            LoadMore();
        }


        void Clear() {
            Coffee.Clear();
            CoffeeGroups.Clear();
        }
    }
}
