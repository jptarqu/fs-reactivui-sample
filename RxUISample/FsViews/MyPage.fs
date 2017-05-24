namespace FsViews

open System
open Xamarin.Forms

type BunchBtns() = 
    inherit ContentPage()
    let stack = StackLayout(VerticalOptions = LayoutOptions.Center)
    
    let addBtn idx = 
        let button = 
            Button
                (Text = "Click Me!", VerticalOptions = LayoutOptions.CenterAndExpand, 
                 HorizontalOptions = LayoutOptions.CenterAndExpand)
        button.Clicked
        |> Observable.subscribe (fun t -> 
               do System.Diagnostics.Debug.WriteLine("Hi " + idx.ToString())
               ())
        |> ignore
        stack.Children.Add button
    
    do 
        let t1 = DateTime.Now
        for idx in 0..1000 do
            addBtn idx
        base.Title <- "Login Page"
        let scroller = new ScrollView()
        scroller.Content <- stack
        base.Content <- scroller
        let diff = (new TimeSpan(DateTime.Now.Ticks - t1.Ticks)).TotalSeconds
        do System.Diagnostics.Debug.WriteLine("End " + diff.ToString())

type App() = 
    inherit Application()
    do base.MainPage <- BunchBtns()
