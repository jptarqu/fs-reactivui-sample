
open System

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
        let a = Func<'a, 'r>(exp)
        <@ a @> //it gives the index error
        |> LeafExpressionConverter.QuotationToExpression
        |> unbox<Expression<Func<'a, 'r>>>
    
    let inline toF (exp : 'a -> 'r) = Func<'a, 'r>(exp)

type FsLoginViewModel() as this = 
    let mutable _isLoading = false
    let mutable _email = ""
    let mutable _password = ""
    
    do 
        let quot = fun (x : FsLoginViewModel) -> x.Email
        let prop1a = quot |> RxUtils.toLExpT<FsLoginViewModel, string>
        let prop1Bad = <@ fun (x : FsLoginViewModel) -> x.Email @> |> RxUtils.toExpression //(fun (x : FsLoginViewModel) -> x.Email) |> RxUtils.toLExp
        let prop1Badstr = prop1Bad.ToString()
        do System.Console.WriteLine(prop1Badstr)
        let prop1 = <@ Func<FsLoginViewModel, _>(fun x -> x.Email) @> |> RxUtils.toExpression
        let prop1str = prop1.ToString()
        do System.Console.WriteLine(prop1)
        
    member __.Email 
        with get () = _email
        and set (value:string) =  ()
    
    member __.Password 
        with get () = _password
        and set (value:string) = ()
    
    member __.IsLoading = _isLoading

let a = FsLoginViewModel()
