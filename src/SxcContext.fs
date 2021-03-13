module SxcContext

open Fable.Core.JsInterop
open Browser.Types
open Browser.Dom
open Sxc

[<RequireQualifiedAccess>]
module Sxc =
    type Context =
        { Root: Sxc.SxcRoot
          Instance: Sxc.SxcInstance
          Edition: string
          ApiEdition: string
          Api: SxcApi.Api
          ModuleId: int
          BasePath: string }

let getBasePath () : string =

    let anchor =
        document.createElement ("a") :?> HTMLAnchorElement

    anchor.href <- document.baseURI
    anchor.pathname

/// Connects to the current 2sxc instance and returns a Context
///
/// Param _container_ should be the same element which is also used for your Fable app.
///
/// The Context includes the common $2sxc interface, but also provides
/// * Async **API** to execute request against 2sxc
/// * The BasePath required for Routing

let configure (container: HTMLElement) =

    let instance = sxc.Invoke(!^container)
    //wraps all JQueryPromises in real Promises
    instance.webApi <- Sxc.WebApi(instance.webApi)

    { Sxc.Root = sxc
      Sxc.Api = SxcApi.Api(instance, sxc)
      Sxc.Instance = instance
      Sxc.Edition = container.dataset.["edition"]
      Sxc.ApiEdition = container.dataset.["apiedition"]
      Sxc.ModuleId = instance.id
      Sxc.BasePath = getBasePath () }
