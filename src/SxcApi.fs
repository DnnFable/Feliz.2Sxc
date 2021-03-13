module SxcApi

open Fable.SimpleHttp
open Sxc
open Fable.Core
open Fable.Core.JsInterop
open Thoth.Json

type Api(instance: SxcInstance, root: SxcRoot) =
    /// GET Request against current 2sxc instance
    member __.get(query: string, [<Inject>] ?responseResolver: ITypeResolver<'Response>) : Async<'Response> =
        async {
            let! response =
                Http.request (root.http.apiUrl (query))
                |> Http.method GET
                |> Http.header (Headers.contentType "application/json")
                |> Http.header (Headers.create "ModuleId" (unbox instance.id))
                |> Http.header (Headers.create "PageId" (unbox (root.env.page ())))
                |> Http.header (Headers.create "RequestVerificationToken" (root.env.rvt ()))
                |> Http.withCredentials true
                |> Http.send

            return
                match response.statusCode with
                | code when code < 400 ->
                    match Decode.Auto.fromString<'Response> (response.responseText, ?resolver = responseResolver) with
                    | Ok result -> result
                    | Error message -> failwith message
                | _ -> failwith response.responseText
        }
    /// GET Request against current 2sxc instance, expecting to retrieve **Data Entitities**
    member __.query(query: string, [<Inject>] ?responseResolver: ITypeResolver<'Response>) : Async<'Response> =
        __.get<'Response> ("app/auto/query/" + query, ?responseResolver = responseResolver)

    /// GET Request against current 2sxc instance, expecting to execute an **2sxc Query**
    member __.content(content: string, [<Inject>] ?responseResolver: ITypeResolver<'Response>) : Async<'Response> =
        __.get<'Response> ("app/auto/content/" + content, ?responseResolver = responseResolver)
