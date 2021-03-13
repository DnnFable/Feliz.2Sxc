module Feliz.Router.BasePath

open Feliz
open Feliz.UseElmish
open Feliz.Router
open Elmish

type State =
    { BaseUrl: string list
      CurrentUrl: string list }

type Msg =
    | UrlChanged of string list
    | NavigateTo of string list

let init (basePath: string) =
    let baseUrl =
        Router.urlSegments basePath RouteMode.Path

    { BaseUrl = baseUrl
      CurrentUrl = Router.currentPath () |> List.trim baseUrl },
    Cmd.none

let update (msg: Msg) (state: State) =

    match msg with
    | UrlChanged url ->
        url
        |> List.trim state.BaseUrl
        |> fun url -> { state with CurrentUrl = url }, Cmd.none

    | NavigateTo url -> state, Cmd.navigatePath ((state.BaseUrl @ url) |> List.toArray)

[<RequireQualifiedAccess>]
module router =
    type BaseUrl =
        { /// Current Url without basePpath
          current: string list
          /// Navigates to the provided url
          /// * param url
          goto: string list -> Browser.Types.MouseEvent -> unit
          /// Creates a path including the base path
          /// * param url
          href: string list -> string }

    /// Sets up *Feliz.Router* to work against a different base path than "/"
    /// Returns
    /// * _BaseUrl_ - Use it to setup navigation
    /// * UrlChanged dispatch for *Feliz.Router*
    let useBasePath (basePath: string) =
        let (state, dispatch) =
            React.useElmish ((fun () -> init basePath), update, [||])

        { current = state.CurrentUrl
          href = fun url -> state.BaseUrl @ url |> Router.formatPath
          goto =
              fun url ev ->
                  url |> NavigateTo |> dispatch
                  ev.preventDefault () },
        UrlChanged >> dispatch
