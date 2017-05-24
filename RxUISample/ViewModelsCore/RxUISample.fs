namespace RxUISample

open Xamarin.Forms

type App() = 
    inherit Application()
    
    do 
        base.MainPage <- MainView()
