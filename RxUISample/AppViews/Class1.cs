using System;
using Xamarin.Forms;

namespace AppViews
{
    public class LoginPage : ContentPage
    {
    public LoginPage()
    {
    
      // We'll initialize our viewmodel
      ViewModel = new LoginViewModel();
      // We'll add the bindings
      this.Bind(ViewModel, vm => vm.Email, v => v.Email.Text);
      this.Bind(ViewModel, vm => vm.Password, v => v.Password.Text);
      this.BindCommand(ViewModel, vm => vm.Login, v => v.Login);
    }


    //The rest of the code below is plumbing:

    public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(LoginViewModel), typeof(LoginPage), null, BindingMode.OneWay);

    public LoginViewModel ViewModel
    {
      get { return (LoginViewModel)GetValue(ViewModelProperty); }
      set { SetValue(ViewModelProperty, value); }
    }

    object IViewFor.ViewModel
    {
      get { return ViewModel; }
      set { ViewModel = (LoginViewModel)value; }
    }
    
    }
}
