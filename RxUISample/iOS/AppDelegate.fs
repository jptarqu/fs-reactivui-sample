namespace RxUISample.iOS

open System
open UIKit
open Foundation
open Xamarin.Forms
open Xamarin.Forms.Platform.iOS

[<Register("AppDelegate")>]
type AppDelegate() = 
    inherit FormsApplicationDelegate()
    override this.FinishedLaunching(app, options) = 
        Forms.Init()
        this.LoadApplication(new CSLibView.App())
        //this.LoadApplication(new FsViews.App()) //(new CSLibView.App())
        base.FinishedLaunching(app, options)

module Main = 
    [<EntryPoint>]
    let main args = 
        UIApplication.Main(args, null, "AppDelegate")
        0
