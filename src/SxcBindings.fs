module Sxc

// generated with ts2fable
// npm "@2sic.com/2sxc-typings"

open System.Collections.Generic
open Fable.Core
open Fable.Core.JS
open Browser.Types

/// helper API to run ajax / REST calls to the server
/// it will ensure that the headers etc. are set correctly
/// and that urls are rewritten
type SxcWebApi =
    /// <summary>returns an http-get promise</summary>
    /// <param name="settingsOrUrl">the url to get</param>
    /// <param name="params">jQuery style ajax parameters</param>
    /// <param name="data">jQuery style data for post/put requests</param>
    /// <param name="preventAutoFail"></param>
    abstract get : settingsOrUrl: string * ?``params``: obj * ?data: obj * ?preventAutoFail: bool -> Promise<_>
    /// <summary>returns an http-post promise</summary>
    /// <param name="settingsOrUrl">the url to get</param>
    /// <param name="params">jQuery style ajax parameters</param>
    /// <param name="data">jQuery style data for post/put requests</param>
    /// <param name="preventAutoFail"></param>
    abstract post : settingsOrUrl: string * ?``params``: obj * ?data: obj * ?preventAutoFail: bool -> Promise<_>
    /// <summary>returns an http-delete promise</summary>
    /// <param name="settingsOrUrl">the url to talk to</param>
    /// <param name="params">jQuery style ajax parameters</param>
    /// <param name="data">jQuery style data for post/put requests</param>
    /// <param name="preventAutoFail"></param>
    abstract delete : settingsOrUrl: string * ?``params``: obj * ?data: obj * ?preventAutoFail: bool -> Promise<_>
    /// <summary>returns an http-put promise</summary>
    /// <param name="settingsOrUrl">the url to put</param>
    /// <param name="params">jQuery style ajax parameters</param>
    /// <param name="data">jQuery style data for post/put requests</param>
    /// <param name="preventAutoFail"></param>
    abstract put : settingsOrUrl: string * ?``params``: obj * ?data: obj * ?preventAutoFail: bool -> Promise<_>
    /// <summary>Generic http request</summary>
    /// <param name="params">jQuery style ajax parameters</param>
    /// <param name="data">jQuery style data for post/put requests</param>
    /// <param name="preventAutoFail"></param>
    /// <param name="method">the http verb name</param>
    /// <param name="settings">settings</param>
    abstract request :
        settings: string * ``params``: obj option * data: obj option * preventAutoFail: bool * method: string ->
        Promise<_>
    /// All the headers which are needed in an ajax call for this to work reliably.
    /// Use this if you need to get a list of headers in another system
    abstract headers : unit -> Dictionary<string, string>


/// SxcWebApi's methods are returning a JQueryPromise, not a real JSPromise
/// WebApi wraps all these methods in a Promise.resolve
type WebApi(webapi: SxcWebApi) =
    interface SxcWebApi with
        member this.delete(url: string, ?``params``: obj, ?data: obj, ?preventAutoFail: bool) : Promise<'a> =
            promise {
                return! webapi.delete (url, ?``params`` = ``params``, ?data = data, ?preventAutoFail = preventAutoFail)
            }

        member this.get(url: string, ?``params``: obj, ?data: obj, ?preventAutoFail: bool) : Promise<'a> =
            promise {
                return! webapi.get (url, ?``params`` = ``params``, ?data = data, ?preventAutoFail = preventAutoFail) }

        member this.headers() : Dictionary<string, string> = webapi.headers ()

        member this.post(url: string, ?``params``: obj, ?data: obj, ?preventAutoFail: bool) : Promise<'a> =
            promise {
                return! webapi.post (url, ?``params`` = ``params``, ?data = data, ?preventAutoFail = preventAutoFail) }

        member this.put(url: string, ?``params``: obj, ?data: obj, ?preventAutoFail: bool) : Promise<'a> =
            promise {
                return! webapi.put (url, ?``params`` = ``params``, ?data = data, ?preventAutoFail = preventAutoFail) }

        member this.request
            (
                settings: string,
                ``params``: obj option,
                data: obj option,
                preventAutoFail: bool,
                method: string
            ) : Promise<'a> =
            webapi.request (settings, ``params``, data, preventAutoFail, method)


/// The typical sxc-instance object for a specific DNN module or content-block
type SxcInstance =
    /// the sxc-instance ID, which is usually the DNN Module Id
    abstract id : int with get, set
    /// content-block ID, which is either the module ID, or the content-block definition entity ID
    /// this is an advanced concept you usually don't care about, otherwise you should research it
    abstract cbid : int with get, set
    /// checks if we're currently in edit mode
    abstract isEditMode : unit -> bool
    /// helpers for ajax calls
    abstract webApi : SxcWebApi with get, set


/// Enhanced sxc instance with additional editing functionality
/// Use this, if you intend to run content-management commands like "edit" from your JS directly
type SxcInstanceWithEditing =
    inherit SxcInstance
    /// manage object which provides access to additional content-management features
    /// it only exists if 2sxc is in edit mode (otherwise the JS are not included for these features)
    abstract manage : obj option with get, set

/// A log entry item
[<AllowNullLiteral>]
type LogEntry =
    abstract time : int with get, set
    abstract message : string with get, set

/// A log object which will collect log entries for another ojbect
type Log =
    /// The name of this log, for scenarios where multiple loggers are mixed
    abstract name : string with get, set
    /// List of all entries added to this log
    abstract entries : ResizeArray<LogEntry> with get, set
    /// Maximum amount of entries to add - to prevent memory hoging
    abstract maxEntries : int with get, set
    /// <summary>Add a simple message to the log</summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    abstract add : message: U2<(unit -> string), string> * ?data: obj -> unit


/// Any object that has an own log object
type HasLog =
    /// The logger for this object
    abstract log : Log with get, set

type JsInfo =
    /// Page ID
    abstract page : int with get, set
    /// Optional API key - optional if set from external, because it's auto derived from root
    abstract api : string with get, set
    /// Portal root path - used for various things incl. the API root
    abstract root : string with get, set
    /// Request verification token
    abstract rvt : string with get, set
    /// The root path for the UI
    abstract uiRoot : string with get, set

/// Provides environment information to $2sxc - usually page-id, api-root and stuff like that
type Environment =
    inherit HasLog
    abstract ready : bool with get, set
    abstract source : string with get, set
    /// <summary>Load a new jsInfo - must be public, as it's used in iframes where jquery is missing</summary>
    /// <param name="newJsInfo">new info to load</param>
    /// <param name="source"></param>
    abstract load : newJsInfo: JsInfo * ?source: string -> unit
    /// The API endpoint url from the environment
    abstract api : unit -> string
    /// The current page ID
    abstract page : unit -> int
    /// The Request Verification Token
    abstract rvt : unit -> string

type Http =
    /// All the headers which are needed in an ajax call for this to work reliably.
    /// Use this if you need to get a list of headers in another system
    abstract headers : ?id: int * ?cbid: int -> Dictionary<string, string>
    /// <summary>Get the API-Root path for a specific extension/endpoint</summary>
    /// <param name="endpointName"></param>
    abstract apiRoot : endpointName: string -> string
    /// <summary>Get the URL for a specific web API endpoint
    /// Will ignore urls which clearly already are the full url.</summary>
    /// <param name="url"></param>
    /// <param name="endpointName"></param>
    abstract apiUrl : url: string * ?endpointName: string -> string

type UrlParams =
    /// <summary>Get a param from the url, no matter if it's behind ? or #</summary>
    /// <param name="name"></param>
    abstract get : name: string -> string
    /// <summary>Get a required param from the url, no matter if it's behind ? or #
    /// Will throw an error if not found</summary>
    /// <param name="name"></param>
    abstract require : name: string -> string

/// This is the interface for the main $2sxc object on the window
type SxcRoot =
    /// <summary>Get's an SxcInstance</summary>
    /// <param name="id">number | HTMLElement</param>
    /// <param name="cbid">number</param>
    [<Emit "$0($1...)">]
    abstract Invoke : id: U2<int, HTMLElement> * ?cbid: int -> SxcInstance
    /// system information, mainly for checking which version of 2sxc is running
    /// note: it's not always updated reliably, but it helps when debugging
    abstract sysinfo : obj with get, set
    /// Environment information
    abstract env : Environment with get, set
    /// Http helper for API calls and such
    abstract http : Http with get, set
    /// Internal logger to better see what's happening
    abstract log : Log with get, set
    /// Helper to work with url parameters behind ? or #
    abstract urlParams : UrlParams with get, set
    /// sets up 2Sxc Toolbar for Element
    [<Emit "$0?._manage?._toolbarManager.build($1)">]
    abstract SetupToolbar : HTMLElement -> unit
    ///Attachs callback to the toolbar's refresh workflow
    [<Emit("""((event) => {
        event?.detail?.workflow?.add({
          command: 'refresh',           // only capture refresh requests
          code: (wfArgs) => {
            $1();  // emit event
            return false;               // prevent default refresh of the 2sxc API
          }
        })})($2)""")>]
    abstract OnRefresh : (unit -> unit) -> Browser.Types.Event -> unit
/// Gets the 2sxc root element ($2sxc)
[<Emit "window['$' + '2sxc']">]
let sxc : SxcRoot = jsNative
