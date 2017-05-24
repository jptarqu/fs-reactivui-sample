using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using ReactiveUI;
using Xamarin.Forms;
using FsViewModelsPcl;

namespace CSLibView
{
    public partial class LoginPage : ContentPage, IViewFor<FsLoginViewModel>
    {
        public LoginPage()
        {
            InitializeComponent();
            // We'll initialize our viewmodel
            ViewModel = new FsLoginViewModel();
            // We'll add the bindings
            this.Bind(ViewModel, vm => vm.Email, v => v.Email.Text);
            this.Bind(ViewModel, vm => vm.Password, v => v.Password.Text);
            this.BindCommand(ViewModel, vm => vm.Login, v => v.Login);
            this.WhenAnyValue(x => x.ViewModel.IsLoading)
                  .ObserveOn(RxApp.MainThreadScheduler)
                  .Subscribe(busy =>
                  {
                      Email.IsEnabled = !busy;
                      Password.IsEnabled = !busy;
                      Loading.IsVisible = busy;
                  });
        }


        //The rest of the code below is plumbing:

        public static readonly BindableProperty ViewModelProperty =
            BindableProperty.Create(nameof(ViewModel), typeof(FsLoginViewModel),
                                    typeof(LoginPage), null, BindingMode.OneWay);

        public FsLoginViewModel ViewModel
        {
            get
            {
                return (FsLoginViewModel)GetValue(ViewModelProperty);
            }
            set
            {
                SetValue(ViewModelProperty, value);
            }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (FsLoginViewModel)value; }
        }
    }
}
