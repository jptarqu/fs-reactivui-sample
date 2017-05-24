namespace FsViewModelsPcl

open System
open ReactiveUI
open Splat
open System.Threading
open System.Threading.Tasks
open Microsoft.FSharp.Linq.RuntimeHelpers
open System.Linq.Expressions

module RxUtils = 
    let inline startAsPlainTask (work : Async<unit>) = Task.Factory.StartNew(fun () -> work |> Async.RunSynchronously)
    
    let toExpression (``f# lambda`` : Quotations.Expr<'a>) = 
        ``f# lambda``
        |> LeafExpressionConverter.QuotationToExpression
        |> unbox<Expression<'a>>
    
    let mustBeLarge (s : string) = s <> null && s.Length > 3
    let toLExpT<'a, 'r> (exp : 'a -> 'r) = toExpression <@ Func<'a, 'r>(exp) @>
    
    let inline toLExp (exp : 'a -> 'r) = 
        <@ Func<'a, 'r>(exp) @> //it gives the index error
        |> LeafExpressionConverter.QuotationToExpression
        |> unbox<Expression<Func<'a, 'r>>>
    
    let inline toF (exp : 'a -> 'r) = Func<'a, 'r>(exp)

type FsLoginViewModel() as this = 
    inherit ReactiveObject()
    let mutable login : ReactiveCommand = null
    let mutable _isLoading : ObservableAsPropertyHelper<bool> = null
    let mutable _email = ""
    let mutable _password = ""
    
    do 
        let quot = fun (x : FsLoginViewModel) -> x.Email
        let prop1a = quot |> RxUtils.toLExpT<FsLoginViewModel, string>
        let prop1Bad = (fun (x : FsLoginViewModel) -> x.Email) |> RxUtils.toLExp
        let prop1Badstr = prop1Bad.ToString()
        do System.Diagnostics.Debug.WriteLine(prop1Badstr)
        let prop1 = <@ Func<FsLoginViewModel, _>(fun x -> x.Email) @> |> RxUtils.toExpression
        let prop1str = prop1.ToString()
        do System.Diagnostics.Debug.WriteLine(prop1)
        let prop2 = <@ Func<FsLoginViewModel, _>(fun x -> x.Password) @> |> RxUtils.toExpression
        let isLoadingExpr = <@ Func<FsLoginViewModel, _>(fun x -> x.IsLoading) @> |> RxUtils.toExpression
        //let isLoadingExpr = (fun (x : FsLoginViewModel) -> x.IsLoading) |> RxUtils.toLExp
        let canLogin = this.WhenAnyValue(prop1, prop2, fun (p1 : string) (ps2 : string) -> RxUtils.mustBeLarge p1)
        let task = 
            Func<CancellationToken, Task>
                (fun arg -> async { do System.Diagnostics.Debug.WriteLine("Hi") } |> RxUtils.startAsPlainTask)
        login <- ReactiveCommand.CreateFromTask(task, canLogin)
        login.IsExecuting.ToProperty(this, isLoadingExpr, &_isLoading) |> ignore
    
    //Results from toString in prop1Badstr and prop1str 
    //        delegateArg => value(<StartupCode$FsViewModelsPcl>.$FsLoginViewModel+prop1Bad@39).Invoke(delegateArg)
    //x => x.Email
    member x.Login 
        with get () = login
        and set value = login <- value
    
    //[<ValidatesViaMethod(AllowBlanks = false, AllowNull = false, Name = "IsAgeValid", ErrorMessage = "Please enter a valid age 0..120")>]
    member __.Email 
        with get () = _email
        and set value = this.RaiseAndSetIfChanged(&_email, value, "Email") |> ignore
    
    member __.Password 
        with get () = _password
        and set value = this.RaiseAndSetIfChanged(&_password, value, "Password") |> ignore
    
    member __.IsLoading = _isLoading.Value