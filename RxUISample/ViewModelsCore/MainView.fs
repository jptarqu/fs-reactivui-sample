namespace RxUISample

open Xamarin.Forms
open System;  
open System.Collections.Generic;  
open ReactiveUI;  
open System.Linq.Expressions
  

type MainView(a:int)  = 
    inherit ContentPage() 
    let stack = StackLayout(VerticalOptions = LayoutOptions.Center)
    let usernameLbl = Label(XAlign = TextAlignment.Center, Text = "Username")
    let usernameTxt = Entry( Text = "")
    let passwordLbl = Label(XAlign = TextAlignment.Center, Text = "Passowrd")
    let passwordTxt = Entry( Text = "")
    let button = Button(Text = "Click Me!",
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand)
    do                  
        button.Clicked
            |> Observable.subscribe (fun t -> 
                
                ()
             )
            |> ignore
            
        stack.Children.Add(usernameLbl)
        stack.Children.Add(usernameTxt)
        stack.Children.Add(passwordLbl)
        stack.Children.Add(passwordTxt)
        stack.Children.Add(button)
        
        base.Title <- "Login Page"
        base.Content <- stack
        
    static let mutable viewModelProperty = BindableProperty.Create("ViewModel", typeof<PersonViewModel> , typeof<MainView>, null, BindingMode.OneWay);
    static member ViewModelProperty = viewModelProperty
    member x.UsernameTxt = usernameTxt
    member x.ViewModel
            with get () = x.GetValue(MainView.ViewModelProperty) //:?> PersonViewModel
            and set (value:Object) = x.SetValue(MainView.ViewModelProperty, value) |> ignore
    member x.ViewModelT
            with get () = x.GetValue(MainView.ViewModelProperty) :?> PersonViewModel
            
    new() as this =
        MainView(0)
        then
            this.ViewModel <- new PersonViewModel()
            Console.Out.WriteLine("ftError person new")
            Console.Out.WriteLine(this.ViewModel)
            let ageExp:Expression<Func<PersonViewModel,string>> = Utility.toLinq <@ fun vm -> vm.Age @> //Utility.toLinq <@ fun vm -> vm.Age @>
            Console.Out.WriteLine("after age exp")
            let txtExp = Utility.toLinq <@ fun (v) -> this.UsernameTxt.Text @> //Utility.toLinqP<MainView,string> <@ fun (v) -> v.UsernameTxt.Text @> 
            Console.Out.WriteLine("after txtExp exp")
            this.Bind(viewModel = this.ViewModelT, vmProperty = ageExp, viewProperty = txtExp) |> ignore
        
               
    interface IViewFor<PersonViewModel> with
        member x.ViewModel
           with get () = x.ViewModel :?> PersonViewModel
           and set (value:PersonViewModel) = x.ViewModel <- (value ) |> ignore
        member x.ViewModel
           with get () = x.ViewModel 
           and set (value:Object) = x.ViewModel  |> ignore